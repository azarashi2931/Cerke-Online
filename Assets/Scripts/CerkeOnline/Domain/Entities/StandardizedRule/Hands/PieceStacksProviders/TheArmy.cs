﻿using static Azarashi.CerkeOnline.Domain.Entities.Terminologies;

namespace Azarashi.CerkeOnline.Domain.Entities.StandardizedRule.Hands.PieceStackProviders
{
    internal class TheArmy : DefaultPieceStacksProviderr
    {
        public TheArmy()
        {
            HandName = HandName.TheArmy;
            pieceStacks = new PieceStack[] { new PieceStack(PieceName.General, 1), new PieceStack(PieceName.Pawn, 2) };
        }
    }
}