﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using Azarashi.Utilities.Collections;
using static Azarashi.CerkeOnline.Domain.Entities.Terminologies;

namespace Azarashi.CerkeOnline.Domain.Entities.Official.PieceMoveAction
{
    /*命名に関する注意
     以下のクラスでは, 
     ・Surmountとその派生を巫の駒越え
     ・Semorkoを踏み越え
     という意味で使っています.
     */

    public class PieceMoveAction : IPieceMoveAction
    {
        readonly IPlayer player;
        readonly Vector2YXArrayAccessor<IPiece> pieces;
        readonly IFieldEffectChecker fieldEffectChecker;
        readonly IValueInputProvider<int> valueProvider;
        readonly IReadOnlyList<Vector2Int> relativePath;
        readonly IReadOnlyList<Vector2Int> worldPath;
        readonly PieceMovement pieceMovement;
        readonly Action<PieceMoveResult> callback;
        readonly Action onPiecesChanged;
        readonly bool isTurnEnd;
        bool surmounted = false;

        readonly Vector2Int startPosition;

        public PieceMoveAction(IPlayer player,Vector2Int startPosition, Vector2Int endPosition, Vector2YXArrayAccessor<IPiece> pieces, IFieldEffectChecker fieldEffectChecker, 
            IValueInputProvider<int> valueProvider, PieceMovement pieceMovement, Action<PieceMoveResult> callback, Action onPiecesChanged, bool isTurnEnd)
        {
            this.player = player ?? throw new ArgumentNullException("駒を操作するプレイヤーを指定してください.");
            this.pieces = pieces ?? throw new ArgumentNullException("盤面の情報を入力してください.");
            this.fieldEffectChecker = fieldEffectChecker ?? throw new ArgumentNullException("フィールド効果の情報を入力してください.");
            this.valueProvider = valueProvider ?? throw new ArgumentNullException("投げ棒の値を提供するインスタンスを指定してください.");

            this.startPosition = startPosition;
            bool isFrontPlayersPiece = pieces.Read(startPosition).Owner != null && pieces.Read(startPosition).Owner.Encampment == Encampment.Front;
            Vector2Int relativePosition = (endPosition - startPosition) * (isFrontPlayersPiece ? -1 : 1);
            this.relativePath = pieceMovement.GetPath(relativePosition) ?? throw new ArgumentException("移動不可能な移動先が指定されました.");
            this.worldPath = relativePath.Select(value => startPosition + value * (isFrontPlayersPiece ? -1 : 1)).ToArray();

            this.pieceMovement = pieceMovement;
            this.callback = callback;
            this.onPiecesChanged = onPiecesChanged;
            this.isTurnEnd = isTurnEnd;
        }

        IPiece PickUpPiece(IPiece movingPiece, Vector2Int endWorldPosition)
        {
            IPiece originalPiece = pieces.Read(endWorldPosition);     //命名が分かりにくい. 行先にある駒.
            if (!IsPickupable(movingPiece, originalPiece))
                return null;
            
            IPiece gottenPiece = originalPiece;
            if(!gottenPiece.PickUpFromBoard()) return null;
            gottenPiece.SetOwner(player);
            pieces.Write(endWorldPosition, null);
            return gottenPiece;
        }

        void ConfirmPiecePosition(IPiece movingPiece, Vector2Int endWorldPosition, bool isForceMove = false)
        {
            Vector2Int startWorldPosition = movingPiece.Position;
            movingPiece.MoveTo(endWorldPosition, isForceMove);

            //この順で書きまないと現在いる座標と同じ座標をendWorldPositionに指定されたとき盤上から駒の判定がなくなる
            pieces.Write(startWorldPosition, null);
            pieces.Write(endWorldPosition, movingPiece);
            
            onPiecesChanged();
        }

        void LastMove(IPiece movingPiece, Vector2Int endWorldPosition)
        {
            //移動先の駒を取る
            IPiece gottenPiece = PickUpPiece(movingPiece, endWorldPosition);
            ConfirmPiecePosition(movingPiece, endWorldPosition);
            callback(new PieceMoveResult(true, isTurnEnd, gottenPiece));
        }

        void OnFailure(IPiece movingPiece)
        {
            ConfirmPiecePosition(movingPiece, startPosition, true);
            callback(new PieceMoveResult(isSuccess: false, isTurnEnd: false, gottenPiece: null));
        }

        bool IsNecessaryWaterEntryJudgment(IPiece movingPiece, int index)
        {
            Vector2Int start = movingPiece.Position;
            bool isInWater = (index > 0 && fieldEffectChecker.IsInTammua(worldPath[index - 1])) || (index == 0 && fieldEffectChecker.IsInTammua(start));
            bool isIntoWater = fieldEffectChecker.IsInTammua(worldPath[index]);
            bool canLittuaWithoutJudge = movingPiece.CanLittuaWithoutJudge();
            bool isNecessaryWaterEntryJudgment = !isInWater && isIntoWater && !canLittuaWithoutJudge;
            return isNecessaryWaterEntryJudgment;
        }

        bool IsPickupable(IPiece movingPiece, IPiece targetPiece)
        {
            if (targetPiece == null) return false;

            bool canMovingPieceTakePiece = movingPiece.CanTakePiece();
            bool isPiecePickupable = targetPiece.IsPickupable();
            bool isSameOwner = targetPiece.Owner == player;
            return canMovingPieceTakePiece && isPiecePickupable && !isSameOwner;
        }

        public void StartMove()
        {
            Move(true, startPosition, 0);
        }

        void Move(bool condition, Vector2Int start, int index)
        {
            IPiece movingPiece = pieces.Read(start);

            //再帰終了処理
            if (!condition)
            {
                if (index > 1)
                    LastMove(movingPiece, worldPath[index - 2]);
                if (index == 1)
                    callback(new PieceMoveResult(true, isTurnEnd, null));
                return;
            }
            if (index >= relativePath.Count)
            {
                LastMove(movingPiece, worldPath[index - 1]);
                return;
            }
            
            IPiece piece = pieces.Read(worldPath[index]);

            //入水判定の必要があるか
            if (IsNecessaryWaterEntryJudgment(movingPiece, index))
            {
                if (index > 0) ConfirmPiecePosition(movingPiece, worldPath[index - 1]);
                valueProvider.RequestValue(value => Move(value >= 3, movingPiece.Position, ++index));
                return;
            }

            //PieceMovementが踏み越えに対応しているか
            bool isSurmountable = piece != null && !surmounted && pieceMovement.surmountable && index < worldPath.Count - 1;
            if (isSurmountable)
            {
                Action surmountAction = () =>
                { 
                    surmounted = true;
                    if (pieces.Read(worldPath[index + 1]) == null)
                    {
                        ConfirmPiecePosition(movingPiece, worldPath[index + 1], isForceMove: true);
                        Move(true, worldPath[index + 1], index + 2);
                        return;
                    }

                    if (worldPath[index + 1] == worldPath.Last())
                    {
                        LastMove(movingPiece, worldPath[index + 1]);
                        return;
                    }

                    OnFailure(movingPiece);
                };

                //別の書き方にしたい
                if (IsNecessaryWaterEntryJudgment(movingPiece, index) || 
                    IsNecessaryWaterEntryJudgment(movingPiece, index + 1))
                {
                    if (index > 0) ConfirmPiecePosition(movingPiece, worldPath[index - 1]);
                    valueProvider.RequestValue(value =>
                    {
                        if (value < 3)
                        {
                            if (index > 0)
                                LastMove(movingPiece, worldPath[index - 1]);
                            if (index == 0)
                                callback(new PieceMoveResult(true, isTurnEnd, null));
                            return;
                        }

                        surmountAction();
                    });
                }
                else
                    surmountAction();

                return;
            }

            if (piece != null)
            {
                if (IsPickupable(movingPiece, piece) && worldPath[index] == worldPath.Last())
                {
                    LastMove(movingPiece, worldPath[index]);
                    return;
                }

                //取ることが出ない駒が移動ルート上にある場合は移動失敗として終了する
                OnFailure(movingPiece);
                return;
            }

            Move(true, start, ++index);
        }
    }
}