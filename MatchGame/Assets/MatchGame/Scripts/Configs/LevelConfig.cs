using System;
using System.Collections.Generic;
using UnityEngine;

namespace MatchGame
{
    [CreateAssetMenu(fileName = "level_config", menuName = "Configs/New Level config")]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField] private int _level;
        [SerializeField] private int _columns;
        [SerializeField] private int _rows;
        [SerializeField] private int _minSwipeCount;
        [SerializeField] private int _maxSwipeCount;
        [SerializeField] private List<CellType> _availableCellsTypes;
        [SerializeField] private List<CellData> _cells;

        public int Level => _level;
        public int Columns => _columns;
        public int Rows => _rows;
        public int MinSwipeCount => _minSwipeCount;
        public int MaxSwipeCount => _maxSwipeCount;
        public List<CellType> AvailableCellsTypes => _availableCellsTypes;
        public List<CellData> Cells => _cells;
    }

    [Serializable]
    public struct CellData
    {
        [SerializeField] private CellType _cellType;
        [SerializeField] private int _posX;
        [SerializeField] private int _posY;

        public CellType CellType => _cellType;
        public int PosX => _posX;
        public int PosY => _posY;
    }
}

// todo: заменить на генератор уровней, который генерирует джэйсончики с настройками