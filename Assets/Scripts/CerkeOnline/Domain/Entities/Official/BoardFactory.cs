﻿using UnityEngine;
using static Azarashi.CerkeOnline.Domain.Entities.Terminologies;
using Azarashi.CerkeOnline.Domain.Entities.Official.Pieces;
using Azarashi.Utilities.Collections;



namespace Azarashi.CerkeOnline.Domain.Entities.Official
{
    public static class BoardFactory
    {
        public static IBoard Create(IPlayer frontPlayer, IPlayer backPlayer)
        {
            IPiece tam = new Tam(0, new Vector2Int(4, 4), null, null); //fieldEffectCheckerに渡すため別途生成

            FieldEffect[,] fieldEffectMap = new FieldEffect[,]
            {
                { FieldEffect.Normal, FieldEffect.Normal, FieldEffect.Normal, FieldEffect.Normal, FieldEffect.Normal, FieldEffect.Normal, FieldEffect.Normal, FieldEffect.Normal, FieldEffect.Normal },
                { FieldEffect.Normal, FieldEffect.Normal, FieldEffect.Normal, FieldEffect.Normal, FieldEffect.Normal, FieldEffect.Normal, FieldEffect.Normal, FieldEffect.Normal, FieldEffect.Normal },
                { FieldEffect.Normal, FieldEffect.Normal,  FieldEffect.Tarfe, FieldEffect.Normal, FieldEffect.Tammua, FieldEffect.Normal,  FieldEffect.Tarfe, FieldEffect.Normal, FieldEffect.Normal },
                { FieldEffect.Normal, FieldEffect.Normal, FieldEffect.Normal,  FieldEffect.Tarfe, FieldEffect.Tammua,  FieldEffect.Tarfe, FieldEffect.Normal, FieldEffect.Normal, FieldEffect.Normal },
                { FieldEffect.Normal, FieldEffect.Normal, FieldEffect.Tammua, FieldEffect.Tammua,  FieldEffect.Tanzo, FieldEffect.Tammua, FieldEffect.Tammua, FieldEffect.Normal, FieldEffect.Normal },
                { FieldEffect.Normal, FieldEffect.Normal, FieldEffect.Normal,  FieldEffect.Tarfe, FieldEffect.Tammua,  FieldEffect.Tarfe, FieldEffect.Normal, FieldEffect.Normal, FieldEffect.Normal },
                { FieldEffect.Normal, FieldEffect.Normal,  FieldEffect.Tarfe, FieldEffect.Normal, FieldEffect.Tammua, FieldEffect.Normal,  FieldEffect.Tarfe, FieldEffect.Normal, FieldEffect.Normal },
                { FieldEffect.Normal, FieldEffect.Normal, FieldEffect.Normal, FieldEffect.Normal, FieldEffect.Normal, FieldEffect.Normal, FieldEffect.Normal, FieldEffect.Normal, FieldEffect.Normal },
                { FieldEffect.Normal, FieldEffect.Normal, FieldEffect.Normal, FieldEffect.Normal, FieldEffect.Normal, FieldEffect.Normal, FieldEffect.Normal, FieldEffect.Normal, FieldEffect.Normal },
            };
            FieldEffectChecker fieldChecker = new Official.FieldEffectChecker(new Vector2YXArrayAccessor<FieldEffect>(fieldEffectMap), tam);

            IPiece[,] piece2DArray = new IPiece[,]
            {
                {    new Kua(0, new Vector2Int(0,0), backPlayer, fieldChecker),    new Dodor(0, new Vector2Int(1,0), backPlayer, fieldChecker),  new Vadyrd(0, new Vector2Int(2,0), backPlayer, fieldChecker),    new Varxle(0, new Vector2Int(3,0), backPlayer, fieldChecker),     new Ales(1, new Vector2Int(4,0), backPlayer, fieldChecker),    new Varxle(1, new Vector2Int(5,0), backPlayer, fieldChecker),  new Vadyrd(1, new Vector2Int(6,0), backPlayer, fieldChecker),    new Dodor(1, new Vector2Int(7,0), backPlayer, fieldChecker),     new Kua(1, new Vector2Int(8,0), backPlayer, fieldChecker) },
                { new Terlsk(1, new Vector2Int(0,1), backPlayer, fieldChecker),  new Gustuer(1, new Vector2Int(1,1), backPlayer, fieldChecker),                                                            null,  new Stistyst(1, new Vector2Int(3,1), backPlayer, fieldChecker),                                                             null,  new Stistyst(0, new Vector2Int(5,1), backPlayer, fieldChecker),                                                            null,  new Gustuer(1, new Vector2Int(7,1), backPlayer, fieldChecker),  new Terlsk(1, new Vector2Int(8,1), backPlayer, fieldChecker) },
                {  new Elmer(0, new Vector2Int(0,2), backPlayer, fieldChecker),    new Elmer(1, new Vector2Int(1,2), backPlayer, fieldChecker),   new Elmer(0, new Vector2Int(2,2), backPlayer, fieldChecker),     new Elmer(1, new Vector2Int(3,2), backPlayer, fieldChecker),  new Felkana(1, new Vector2Int(4,2), backPlayer, fieldChecker),     new Elmer(1, new Vector2Int(5,2), backPlayer, fieldChecker),   new Elmer(0, new Vector2Int(6,2), backPlayer, fieldChecker),    new Elmer(1, new Vector2Int(7,2), backPlayer, fieldChecker),   new Elmer(0, new Vector2Int(8,2), backPlayer, fieldChecker) },
                {                                                           null,                                                             null,                                                            null,                                                              null,                                                             null,                                                              null,                                                            null,                                                             null,                                                            null },
                {                                                           null,                                                             null,                                                            null,                                                              null,                                                              tam,                                                              null,                                                            null,                                                             null,                                                            null },
                {                                                           null,                                                             null,                                                            null,                                                              null,                                                             null,                                                              null,                                                            null,                                                             null,                                                            null },
                {   new Elmer(0, new Vector2Int(0,6), frontPlayer, fieldChecker),     new Elmer(1, new Vector2Int(1,6), frontPlayer, fieldChecker),    new Elmer(0, new Vector2Int(2,6), frontPlayer, fieldChecker),      new Elmer(1, new Vector2Int(3,6), frontPlayer, fieldChecker),   new Felkana(0, new Vector2Int(4,6), frontPlayer, fieldChecker),      new Elmer(1, new Vector2Int(5,6), frontPlayer, fieldChecker),    new Elmer(0, new Vector2Int(6,6), frontPlayer, fieldChecker),     new Elmer(1, new Vector2Int(7,6), frontPlayer, fieldChecker),    new Elmer(0, new Vector2Int(8,6), frontPlayer, fieldChecker) },
                {  new Terlsk(0, new Vector2Int(0,7), frontPlayer, fieldChecker),   new Gustuer(0, new Vector2Int(1,7), frontPlayer, fieldChecker),                                                            null,   new Stistyst(0, new Vector2Int(3,7), frontPlayer, fieldChecker),                                                             null,   new Stistyst(1, new Vector2Int(5,7), frontPlayer, fieldChecker),                                                            null,   new Gustuer(1, new Vector2Int(7,7), frontPlayer, fieldChecker),   new Terlsk(1, new Vector2Int(8,7), frontPlayer, fieldChecker) },
                {     new Kua(1, new Vector2Int(0,8), frontPlayer, fieldChecker),     new Dodor(1, new Vector2Int(1,8), frontPlayer, fieldChecker),   new Vadyrd(1, new Vector2Int(2,8), frontPlayer, fieldChecker),     new Varxle(1, new Vector2Int(3,8), frontPlayer, fieldChecker),      new Ales(0, new Vector2Int(4,8), frontPlayer, fieldChecker),     new Varxle(0, new Vector2Int(5,8), frontPlayer, fieldChecker),   new Vadyrd(0, new Vector2Int(6,8), frontPlayer, fieldChecker),     new Dodor(0, new Vector2Int(7,8), frontPlayer, fieldChecker),      new Kua(0, new Vector2Int(8,8), frontPlayer, fieldChecker) }
            };
            Vector2YXArrayAccessor<IPiece> pieceMap = new Vector2YXArrayAccessor<IPiece>(piece2DArray);

            Board board = new Board(pieceMap, fieldChecker);
            return board;
        }

    }
}