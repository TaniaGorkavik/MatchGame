using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MatchGame
{
    [CreateAssetMenu(fileName = "cells_config", menuName = "Configs/Cells config")]
    public class CellsConfig : ScriptableObject
    {
        [SerializeField] private List<CellConfig> _cellConfigs = new List<CellConfig>();

        public CellObject GetCellObjectByType(CellType type)
        {
            return _cellConfigs.FirstOrDefault(_config => _config.CellType == type)?.CellObject;
        }
    }

    [Serializable]
    public class CellConfig
    {
        [SerializeField] private CellType _cellType;
        [SerializeField] private CellObject _cellObject;

        public CellType CellType => _cellType;
        public CellObject CellObject => _cellObject;
    }
}