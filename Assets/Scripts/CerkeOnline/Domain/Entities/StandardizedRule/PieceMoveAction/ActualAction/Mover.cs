﻿using System;
using Azarashi.CerkeOnline.Domain.Entities.PublicDataType;

namespace Azarashi.CerkeOnline.Domain.Entities.StandardizedRule.PieceMoveAction.ActualAction
{
    internal class Mover
    {
        readonly PositionArrayAccessor<IPiece> pieces;
        readonly Action onPiecesChanged;

        public Mover(PositionArrayAccessor<IPiece> pieces, Action onPiecesChanged)
        {
            this.pieces = pieces;
            this.onPiecesChanged = onPiecesChanged;
        }

        public void MovePiece(IPiece movingPiece, PublicDataType.IntVector2 endWorldPosition, bool isForceMove = false)
        {
            PublicDataType.IntVector2 startWorldPosition = movingPiece.Position;
            movingPiece.MoveTo(endWorldPosition, isForceMove);

            //この順で書きまないと現在いる座標と同じ座標をendWorldPositionに指定されたとき盤上から駒の判定がなくなる
            pieces.Write(startWorldPosition, null);
            pieces.Write(endWorldPosition, movingPiece);

            onPiecesChanged();
        }
    }
}