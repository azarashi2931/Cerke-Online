﻿using UnityEngine;
using Azarashi.Utilities.UnityEvents;
using Azarashi.CerkeOnline.Domain.Entities.PublicDataType;

namespace Azarashi.CerkeOnline.Presentation.Presenter.Columns
{
    public abstract class BaseColumnSelector : MonoBehaviour
    {
        [SerializeField] IReadOnlyPieceUnityEvent onPieceSelected = default;
        [SerializeField] IntVector2UnityEvent onViaPositionSelected = default;
        [SerializeField] IntVector2UnityEvent onTargetPositionSelected = default;

        static readonly IntVector2 NonePosition = new IntVector2(-1, -1);
        IntVector2 startPosition = NonePosition;
        IntVector2 viaPosition = NonePosition;
        protected bool isLockSelecting = false;

        public void OnClickColumn(IntVector2 position)
        {
            if (isLockSelecting || this.startPosition == NonePosition || position == NonePosition)
            {
                this.startPosition = position;
                this.viaPosition = NonePosition;

                CallPieceSelectedEvent(position);
                
                return;
            }

            if (this.viaPosition == NonePosition)
            {
                this.viaPosition = position;
                CallViaPositionSelectedEvent(position);
                return;
            }

            onTargetPositionSelected.Invoke(position);
            OnColumnSelected(this.startPosition, this.viaPosition, position);
            this.startPosition = NonePosition;
        }

        protected abstract void OnColumnSelected(IntVector2 start, IntVector2 via, IntVector2 last);

        void CallPieceSelectedEvent(IntVector2 position)
        {
            var game = Application.GameController.Instance.Game;
            var board = game.Board;
            var piece = board.GetPiece(position);
            onPieceSelected.Invoke(piece);
        }
        
        void CallViaPositionSelectedEvent(IntVector2 position)
        {
            var game = Application.GameController.Instance.Game;
            var board = game.Board;
            var piece = board.GetPiece(position);
            if (piece != null)  //駒が無ければ経由点ではないので呼ばない
                onViaPositionSelected.Invoke(viaPosition);
        }
    }
}