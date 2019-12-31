﻿using System;
using UniRx;
using UnityEngine;
using Azarashi.Utilities.Collections;
using Azarashi.CerkeOnline.Domain.Entities.Official.PieceMoveAction;
using static Azarashi.CerkeOnline.Domain.Entities.Terminologies;
using Azarashi.CerkeOnline.Domain.Entities.Official.PieceMoveAction.DataStructure;

namespace Azarashi.CerkeOnline.Domain.Entities.Official
{
    public class Board : IBoard
    {
        readonly Vector2YXArrayAccessor<IPiece> pieces;
        readonly IFieldEffectChecker fieldChecker;
        public IObservable<Unit> OnEveruValueChanged => onEveryValueChanged;
        readonly Subject<Unit> onEveryValueChanged = new Subject<Unit>();

        //これ、Boardが保持すべき情報ではない
        readonly OperationStatus operationStatus = new OperationStatus();

        public Board(Vector2YXArrayAccessor<IPiece> pieceMap, FieldEffectChecker fieldChecker)
        {
            this.pieces = pieceMap;
            this.fieldChecker = fieldChecker;
            
            onEveryValueChanged.OnNext(Unit.Default);
        }

        public IReadOnlyPiece GetPiece(Vector2Int position)
        {
            if (!IsOnBoard(position))
                return null;

            return pieces.Read(position);
        }

        public IReadOnlyPiece SearchPiece(PieceName pieceName)
        {
            for (int i = 0; i < pieces.Width; i++)
            {
                for (int j = 0; j < pieces.Height; j++)
                {
                    IReadOnlyPiece piece = pieces.Read(new Vector2Int(i, j));
                    if (piece != null && piece.PieceName == pieceName)
                        return piece;
                }
            }

            return null;
        }

        bool isLocked = false;
        public void MovePiece(Vector2Int startPosition, Vector2Int viaPosition, Vector2Int endPosition, IPlayer player, IValueInputProvider<int> valueProvider, Action<PieceMoveResult> callback)
        {
            if (isLocked) return;

            if (!IsOnBoard(startPosition) || !IsOnBoard(endPosition))
                throw new ArgumentException();

            bool areViaAndLastSame = viaPosition == endPosition;
            IPiece movingPiece = pieces.Read(startPosition);
            IPiece viaPiece = pieces.Read(viaPosition);
            IPiece originalPiece = pieces.Read(endPosition);     //元からある駒の意味で使っているが, 英語があってるか不明.
            bool isTargetNull = movingPiece == null;
            bool isViaPieceNull = viaPiece == null;//
            bool isOwner = !isTargetNull && movingPiece.IsOwner(player);
            bool isSameOwner = !isTargetNull && originalPiece != null && originalPiece.Owner == movingPiece.Owner;
            PieceMovement start2ViaPieceMovement = PieceMovement.Default;
            PieceMovement via2EndPieceMovement = PieceMovement.Default;
            bool isMoveableToVia = !isTargetNull && movingPiece.TryToGetPieceMovement(viaPosition, out start2ViaPieceMovement);//
            bool isMoveableToLast = !isTargetNull && (areViaAndLastSame || movingPiece.TryToGetPieceMovement(startPosition + endPosition - viaPosition, out via2EndPieceMovement));//
            if (isTargetNull || (!areViaAndLastSame && isViaPieceNull) || !isOwner || isSameOwner || !isMoveableToVia || ! isMoveableToLast)//
            {
                callback(new PieceMoveResult(false, false, null));
                return;
            }

            //1ターンに複数回動作する駒のためのロジック
            if (movingPiece == operationStatus.PreviousPiece)
            {
                operationStatus.AddCount();
            }
            else
            {
                if (operationStatus.PreviousPiece != null)
                {
                    callback(new PieceMoveResult(false, false, null));
                    return;
                }
                else
                {
                    operationStatus.Reset(movingPiece);
                }
            }
            bool isTurnEnd = operationStatus.Count >= movingPiece.NumberOfMoves;
            if (isTurnEnd)
                operationStatus.Reset(null);


            isLocked = true;
            callback += (result) => { isLocked = false; };
            var worldPath = new PieceMovePathCalculator().CalculatePath(startPosition, viaPosition, endPosition, pieces, start2ViaPieceMovement, via2EndPieceMovement);
            var viaPositionNode = worldPath.Find(new ColumnData(viaPosition, pieces));
            var moveActionData = new MoveActionData(worldPath.First.Value.Piece, player, worldPath, viaPositionNode);
            IPieceMoveAction pieceMoveAction = new PieceMoveAction.PieceMoveAction(moveActionData,
                pieces, fieldChecker, valueProvider, start2ViaPieceMovement, via2EndPieceMovement, 
                callback, () => onEveryValueChanged.OnNext(Unit.Default), isTurnEnd);
            pieceMoveAction.StartMove();
        }

        public void MovePiece(Vector2Int startPosition, Vector2Int lastPosition, IPlayer player, IValueInputProvider<int> valueProvider, Action<PieceMoveResult> callback)
            => MovePiece(startPosition, lastPosition, lastPosition, player, valueProvider, callback);
            
        
        public bool SetPiece(Vector2Int position, IPiece piece)
        {
            if (!IsOnBoard(position) || pieces.Read(position) != null)
                return false;

            if (piece.Position != new Vector2Int(-1, -1))
                return false;

            piece.SetOnBoard(position);
            pieces.Write(position, piece);
            onEveryValueChanged.OnNext(Unit.Default);
            return true;
        }

        public bool IsOnBoard(Vector2Int position)
        {
            return position.x >= 0 && position.y >= 0 && position.x < pieces.Width && position.y < pieces.Height;
        }
    }
}