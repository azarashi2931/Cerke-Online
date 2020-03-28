﻿using Azarashi.CerkeOnline.Domain.Entities.PublicDataType;

namespace Azarashi.CerkeOnline.Domain.Entities.StandardizedRule.PieceMoveAction.DataStructure
{
    internal struct ColumnData
    {
        public PublicDataType.IntVector2 Positin { get; }
        public IPiece Piece { get { return pieces.Read(Positin); } }

        readonly PositionArrayAccessor<IPiece> pieces;

        public ColumnData(PublicDataType.IntVector2 positin, PositionArrayAccessor<IPiece> pieces)
        {
            Positin = positin;
            this.pieces = pieces;
        }
    }
}