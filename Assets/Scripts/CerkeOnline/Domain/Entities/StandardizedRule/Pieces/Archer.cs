﻿using System.Linq;
using System.Collections.Generic;

namespace Azarashi.CerkeOnline.Domain.Entities.StandardizedRule.Pieces
{
    internal class Archer : DefaultPiece
    {
        protected readonly PieceMovement[] normalPieceMovements;
        protected readonly PieceMovement[] expansionPieceMovements;

        public Archer(Terminologies.PieceColor color, PublicDataType.IntegerVector2 position, IPlayer owner, IExpandingMoveFieldChecker fieldChecker) 
            : base(position, color, owner, Terminologies.PieceName.Archer, fieldChecker)
        {
            normalPieceMovements = new PieceMovement[]
            {
                new PieceMovement(new PublicDataType.IntegerVector2(0, 1), -1), new PieceMovement(new PublicDataType.IntegerVector2(0, -1), -1),
                new PieceMovement(new PublicDataType.IntegerVector2(1, 0), -1), new PieceMovement(new PublicDataType.IntegerVector2(-1, 0), -1),
            };
            expansionPieceMovements = new PieceMovement[]
            {
                new PieceMovement(new PublicDataType.IntegerVector2(1,1), -1), new PieceMovement(new PublicDataType.IntegerVector2(1,-1), -1),
                new PieceMovement(new PublicDataType.IntegerVector2(-1,1), -1), new PieceMovement(new PublicDataType.IntegerVector2(-1,-1), -1)
            };
        }

        public override IEnumerable<PieceMovement> GetMoveablePosition(bool isExpanded)
        {
            if (!isExpanded)
                return normalPieceMovements;

            return expansionPieceMovements;
        }
    }
}