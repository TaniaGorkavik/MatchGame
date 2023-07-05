using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MatchGame
{
    public class CellObject : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private static readonly int DestroyTrigger = Animator.StringToHash("Destroy");
        private static readonly string DestroyAnimationName = "Destroy";

        [SerializeField] private CellType _cellType;
        [SerializeField] private Animator _animator;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private (int X, int Y) _cellCoord;

        public CellType CellType => _cellType;
        public (int X, int Y) CellCoord => _cellCoord;

        public event Action<CellObject, Vector2> OnPointerUpEvent;
        public event Action<CellObject, Vector2> OnPointerDownEvent;

        public void SetCellPosition(Vector3 cellPos, (int X, int Y) cellPosition)
        {
            transform.position = cellPos;
            _cellCoord = cellPosition;

            _spriteRenderer.sortingOrder = cellPosition.X + cellPosition.Y * 10;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnPointerDownEvent?.Invoke(this, eventData.pressPosition);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnPointerUpEvent?.Invoke(this, eventData.position);
        }

        public async UniTask DestroyCell()
        {
            _animator.SetTrigger(DestroyTrigger);
            await UniTask.WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).IsName(DestroyAnimationName));
            await UniTask.WaitWhile(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1);
            Destroy(gameObject);
        }
    }
}