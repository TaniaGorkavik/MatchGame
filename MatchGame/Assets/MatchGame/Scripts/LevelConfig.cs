using System.Collections.Generic;
using UnityEngine;

namespace MatchGame
{
    [CreateAssetMenu(fileName = "level_config", menuName = "Configs/New Level config")]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField]
        public int _level;
        [SerializeField]
        public int _columns;
        [SerializeField]
        public int _rows;
        [SerializeField]
        public int _minSwipeCount;
        [SerializeField]
        public int _maxSwipeCount;
        [SerializeField] 
        public List<CellData> _cells;
        
        public int Level => _level;
        public int Columns => _columns;
        public int Rows => _rows;
        public int MinSwipeCount => _minSwipeCount;
        public int MaxSwipeCount => _maxSwipeCount;
        public List<CellData> Cells => _cells;

    }
}

// todo: заменить на генератор уровней