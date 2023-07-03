using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MatchGame
{
    public class CellObject : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private static readonly int _destroyTrigger = Animator.StringToHash("Destroy");
        
        public event Action<CellObject, Vector2> OnPointerUpEvent;
        public event Action<CellObject, Vector2> OnPointerDownEvent;
        
        
        [SerializeField] private CellType _cellType;
        [SerializeField]
        private Animator _animator;

        private (int X, int Y) _cellCoord;

        public CellType CellType => _cellType;
        public (int X, int Y) CellCoord => _cellCoord;

        public void SetCellPosition(Vector3 cellPos, (int X, int Y) cellPosition)
        {
            transform.position = cellPos;
            _cellCoord = cellPosition;
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            OnPointerDownEvent?.Invoke(this, eventData.pressPosition);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnPointerUpEvent?.Invoke(this, eventData.position);
        }
    }
}