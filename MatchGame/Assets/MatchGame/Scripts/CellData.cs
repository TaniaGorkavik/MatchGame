using System;
using UnityEngine;

namespace MatchGame
{
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