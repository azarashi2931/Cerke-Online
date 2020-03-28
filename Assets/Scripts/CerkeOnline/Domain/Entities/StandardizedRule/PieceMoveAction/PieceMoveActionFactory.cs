﻿using System;
using UnityEngine;
using Azarashi.Utilities.Collections;
using Azarashi.CerkeOnline.Domain.Entities.StandardizedRule.PieceMoveAction.DataStructure;

namespace Azarashi.CerkeOnline.Domain.Entities.StandardizedRule.PieceMoveAction
{
    internal class PieceMoveActionFactory : IPieceMoveActionFactory
    {
        //Vector2YXArrayAccessor<IPiece> pieces, IFieldEffectChecker fieldEffectCheckerをコンストラクタの引数にすることも検討
        public IPieceMoveAction Create(IPlayer player, Vector2Int startPosition, Vector2Int viaPosition, Vector2Int endPosition,
            Vector2YXArrayAccessor<IPiece> pieces, IFieldEffectChecker fieldEffectChecker, IValueInputProvider<int> valueProvider,
            PieceMovement start2ViaPieceMovement, PieceMovement via2EndPieceMovement, 
            Action<PieceMoveResult> callback, Action onPiecesChanged, bool isTurnEnd)
        {
            var worldPath = PieceMovePathCalculator.CalculatePath(startPosition, viaPosition, endPosition, pieces, start2ViaPieceMovement, via2EndPieceMovement);
            var viaPositionNode = worldPath.Find(new ColumnData(viaPosition, pieces));
            
            //worldPathに開始地点は含まれない
            var moveActionData = new MoveActionData(pieces.Read(startPosition), player, worldPath, viaPositionNode);
            
            return new PieceMoveAction(moveActionData,
                pieces, fieldEffectChecker, valueProvider, start2ViaPieceMovement, via2EndPieceMovement,
                callback, onPiecesChanged, isTurnEnd);
        }
    }
}
