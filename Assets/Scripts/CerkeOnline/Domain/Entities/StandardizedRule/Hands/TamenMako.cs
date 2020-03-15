﻿using System.Collections.Generic;
using UnityEngine;
using UniRx;
using static Azarashi.CerkeOnline.Domain.Entities.Terminologies;

namespace Azarashi.CerkeOnline.Domain.Entities.StandardizedRule.Hands
{
    /// <summary>
    /// 皇再来
    /// </summary>
    public class TamenMako : IHand
    {
        public string Name { get; }
        public int Score { get; }

        readonly TamObserver tamObserver;

        public TamenMako(int score, TamObserver tamObserver)
        {
            Name = HandNameDictionary.PascalToJapanese[GetType().Name];
            Score = score;

            this.tamObserver = tamObserver;
        }

        public int GetNumberOfSuccesses(IReadOnlyList<IReadOnlyPiece> pieces)
        {
            return tamObserver.GetNumberOfTamenMako();
        }
    }
}