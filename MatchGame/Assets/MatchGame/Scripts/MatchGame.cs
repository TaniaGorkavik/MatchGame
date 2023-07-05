using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace MatchGame
{
    public class MatchGame : MonoBehaviour
    {
        [SerializeField] private List<LevelConfig> _levels = new List<LevelConfig>();

        private int _levelId = 0;

        private MatchBoard _matchBoard;

        public LevelConfig CurrentLevel => _levels[_levelId];

        [Inject]
        private void Construct(MatchBoard matchBoard)
        {
            _matchBoard = matchBoard;
        }

        private void Start()
        {
            _matchBoard.CreateBoard();
        }

        public void RestartLevel()
        {
            _matchBoard.CreateBoard();
        }

        public void NextLevel()
        {
            _levelId++;
            if (_levelId == _levels.Count)
            {
                _levelId = 0;
            }

            _matchBoard.CreateBoard();
        }
    }
}