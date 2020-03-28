﻿using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Azarashi.CerkeOnline.Domain.Entities.StandardizedRule.Pieces
{
    internal class Varxle : DefaultPiece
    {
        readonly PieceMovement[] normalPieceMovements;
        readonly PieceMovement[] expansionPieceMovements;

        internal Varxle(Terminologies.PieceColor color, PublicDataType.IntVector2 position, IPlayer owner, IExpandingMoveFieldChecker fieldChecker) : base(position, color, owner, Terminologies.PieceName.Varxle, fieldChecker)
        {
            normalPieceMovements = new PieceMovement[]
            {
                new PieceMovement(new PublicDataType.IntVector2(0, 1), 1),
                new PieceMovement(new PublicDataType.IntVector2(1, 0), 1), new PieceMovement(new PublicDataType.IntVector2(-1, 0), 1),
                new PieceMovement(new PublicDataType.IntVector2(1, 1), 1), new PieceMovement(new PublicDataType.IntVector2(1, -1), 1),
                new PieceMovement(new PublicDataType.IntVector2(-1, 1), 1), new PieceMovement(new PublicDataType.IntVector2(-1, -1), 1)
            };
            expansionPieceMovements = normalPieceMovements.Union(new PieceMovement[]
            {
                 new PieceMovement(new PublicDataType.IntVector2(0, -1), 1)
            }).ToArray();
        }

        public override IReadOnlyList<PieceMovement> GetMoveablePosition(bool isExpanded)
        {
            if (!isExpanded)
                return normalPieceMovements;

            return expansionPieceMovements;
        }
    }
}