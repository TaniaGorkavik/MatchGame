using System;
using UnityEngine;
using Zenject;

namespace MatchGame
{
    public class InputManager
    {
        private MatchBoard _matchBoard;

        public bool IsInputBlocked { get; set; }

        [Inject]
        void Construct(MatchBoard matchBoard)
        {
            _matchBoard = matchBoard;
        }

        public void AddListeners(CellObject cellObject)
        {
            cellObject.OnPointerDownEvent += OnCellClickDown;
            cellObject.OnPointerUpEvent += OnCellClickUp;
        }

        public void RemoveListeners(CellObject cellObject)
        {
            cellObject.OnPointerDownEvent -= OnCellClickDown;
            cellObject.OnPointerUpEvent -= OnCellClickUp;
        }

        private Vector2 _startPos;
        private CellObject _cellObject;

        private void OnCellClickDown(CellObject cellObject, Vector2 startPos)
        {
            _cellObject = cellObject;
            _startPos = startPos;
        }

        private void OnCellClickUp(CellObject cellObject, Vector2 endPos)
        {
            if (!IsInputBlocked)
            {
                Vector2 moveVector = endPos - _startPos;
                if (moveVector.magnitude > 10f)
                {
                    MoveDirectionType moveDirectionType = GetMoveDirectionType(moveVector);
                    _matchBoard.MoveCell(cellObject, moveDirectionType);
                }
            }
        }

        private MoveDirectionType GetMoveDirectionType(Vector2 moveVector)
        {
            MoveDirectionType moveDirectionType;
            float positiveX = Mathf.Abs(moveVector.x);
            float positiveY = Mathf.Abs(moveVector.y);

            if (positiveX > positiveY)
            {
                moveDirectionType = (moveVector.x > 0) ? MoveDirectionType.Right : MoveDirectionType.Left;
            }
            else
            {
                moveDirectionType = (moveVector.y > 0) ? MoveDirectionType.Up : MoveDirectionType.Down;
            }

            return moveDirectionType;
        }
    }

    public enum MoveDirectionType
    {
        None,
        Left,
        Right,
        Up,
        Down
    }
}