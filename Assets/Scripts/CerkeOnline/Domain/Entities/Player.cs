﻿using System;
using System.Collections.Generic;
using UniRx;
using static Azarashi.CerkeOnline.Domain.Entities.Terminologies;

namespace Azarashi.CerkeOnline.Domain.Entities
{
    internal class Player : IPlayer
    {
        readonly List<IPiece> pieces = new List<IPiece>();

        public IObservable<Unit> OnPieceStrageCahnged => onPieceStrageCahnged;
        readonly Subject<Unit> onPieceStrageCahnged = new Subject<Unit>();

        public Encampment Encampment { get; }

        internal Player(Encampment encampment)
        {
            Encampment = encampment;
        }

        public IEnumerable<IReadOnlyPiece> GetPieceList()
        {
            return pieces;
        }

        public void GivePiece(IPiece piece)
        {
            if (piece != null && !pieces.Contains(piece))
            {
                pieces.Add(piece);
                onPieceStrageCahnged.OnNext(Unit.Default);
            }
        }

        public void UseCapturedPiece(IPiece piece)
        {
            pieces.Remove(piece); 
            onPieceStrageCahnged.OnNext(Unit.Default);
        }
    }
}