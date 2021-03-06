﻿using System.Linq;
using System.Collections.Generic;

namespace Azarashi.CerkeOnline.Domain.Entities.StandardizedRule.Pieces
{
    internal class Chariot : DefaultPiece
    {
        protected readonly PieceMovement[] normalPieceMovements;
        protected readonly PieceMovement[] expansionPieceMovements;

        public Chariot(Terminologies.PieceColor color, PublicDataType.IntegerVector2 position, IPlayer owner, IExpandingMoveFieldChecker fieldChecker) 
            : base(position, color, owner, Terminologies.PieceName.Chariot, fieldChecker)
        {
            normalPieceMovements = new PieceMovement[]
            {
                new PieceMovement(new PublicDataType.IntegerVector2(2, 0), 1), new PieceMovement(new PublicDataType.IntegerVector2(-2, 0), 1),
                new PieceMovement(new PublicDataType.IntegerVector2(0, 2), 1), new PieceMovement(new PublicDataType.IntegerVector2(0, -2), -1),
            };
            expansionPieceMovements = new PieceMovement[]
            {
                new PieceMovement(new PublicDataType.IntegerVector2(2,2), 1), new PieceMovement(new PublicDataType.IntegerVector2(-2,2), 1),
                new PieceMovement(new PublicDataType.IntegerVector2(2,-2), 1), new PieceMovement(new PublicDataType.IntegerVector2(-2,-2), 1)
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