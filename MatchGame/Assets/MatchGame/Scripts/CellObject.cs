using UnityEngine;

namespace MatchGame
{
    public class CellObject : MonoBehaviour
    {
        [SerializeField] private CellType _cellType;

        public CellType CellType => _cellType;
    }
}