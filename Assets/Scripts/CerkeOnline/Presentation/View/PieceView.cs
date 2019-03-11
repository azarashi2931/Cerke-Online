﻿using System;
using UnityEngine;
using UniRx;
using Azarashi.CerkeOnline.Application;
using Azarashi.CerkeOnline.Data.DataStructure;
using Azarashi.CerkeOnline.Domain.Entities;

namespace Azarashi.CerkeOnline.Presentation.View
{
    public class PieceView : MonoBehaviour
    {
        [SerializeField] PieceMaterialsObject materials = default;
        IReadOnlyPiece piece;
        IBoard board;
        Vector3[,] columnMap = default;

        void Start()
        {
            if (materials == default)
                throw new NullReferenceException();

            board = GameController.Instance.Game.Board;
            board.OnEveruValueChanged.TakeUntilDestroy(this).Subscribe(UpdateView);
        }

        public void Initialize(IReadOnlyPiece piece, Vector3[,] columnMap)
        {
            this.piece = piece;
            this.columnMap = columnMap;

            //TODO 駒の色系統の処理がいろいろクソなので直す
            GetComponentInChildren<SpriteRenderer>().material = piece.Color == 0 ? materials.BlackMaterial : materials.RedMaterial;

            UpdateView(Unit.Default);
        }

        void UpdateView(Unit unit)
        {   
            Vector2Int position = piece.Position;
            if (position == new Vector2Int(-1, -1))
                return;

            //TODO マルチ対応
            float positionZ = transform.position.z;
            Vector3 columnPosition = columnMap[position.x, position.y];
            transform.position = new Vector3(columnPosition.x, columnPosition.y, positionZ);

            Quaternion quaternion = Quaternion.AngleAxis(piece.Owner == GameController.Instance.Game.FirstPlayer ? 0 : 180, Vector3.forward);
            transform.rotation = quaternion;
        }
    }
}