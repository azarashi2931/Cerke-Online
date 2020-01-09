﻿namespace Azarashi.CerkeOnline.Domain.Entities
{
    public interface IScoreHolder
    {
        int MoveScore(IPlayer scorer, int score);
        bool TryGetScore(IPlayer player, out int score);
        bool Contains(IPlayer player);
        int GetScore(IPlayer player);
    }
}