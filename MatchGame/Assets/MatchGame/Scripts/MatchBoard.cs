using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace MatchGame
{
    public class MatchBoard : MonoBehaviour, IInitializable
    {
        [SerializeField]
        private Transform _cellContainer;
        [SerializeField]
        public List<CellObject> _cells;
        
        private (float X, float Y) _cellSize = (2.1f, 2.1f);
        private (int Columns, int Rows) _boardSize = (7, 7);
        private Dictionary<CellType, CellObject> _cellsDictionary = new Dictionary<CellType, CellObject>();
        private MatchGame _matchGame;

        private CellObject[,] Grid;


        [Inject]
        void Construct(MatchGame matchGame)
        {
            _matchGame = matchGame;
        }
        
        public void Initialize()
        {
            Init();
        }
        
        private void Init()
        {
            _boardSize = (_matchGame.CurrentLevel.Columns, _matchGame.CurrentLevel.Rows);
            
            _cells.ForEach(_cell => _cellsDictionary.Add(_cell.CellType, _cell));
            
            CreateBoard();
        }

        private void CreateBoard()
        {
            Grid = new CellObject[_boardSize.Columns, _boardSize.Rows];
            Vector3 positionOffset = transform.position - new Vector3(_boardSize.Columns * _cellSize.X / 2.5f, 11f, 0);
            
            _matchGame.CurrentLevel._cells.ForEach(cellConfig =>
            {
                var newCell = Instantiate(_cellsDictionary[cellConfig.CellType], _cellContainer);

                newCell.transform.parent = transform;
                newCell.transform.position = new Vector3(cellConfig.PosX * _cellSize.X, cellConfig.PosY * _cellSize.Y, 0) + positionOffset;

                Grid[cellConfig.PosX, cellConfig.PosY] = newCell;
            });
        }
    }
}