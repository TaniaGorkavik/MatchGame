using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace MatchGame
{
    public class MatchBoard : MonoBehaviour, IInitializable
    {
        [SerializeField] private Transform _cellContainer;
        [SerializeField] public List<CellObject> _cells;

        private (float X, float Y) _cellSize = (2.1f, 2.1f);
        private (int Columns, int Rows) _boardSize = (7, 7);
        private Dictionary<CellType, CellObject> _cellsDictionary = new Dictionary<CellType, CellObject>();

        private MatchGame _matchGame;
        private InputManager _inputManager;

        private CellObject[,] _grid;


        [Inject]
        void Construct(MatchGame matchGame, InputManager inputManager)
        {
            _matchGame = matchGame;
            _inputManager = inputManager;
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

        public void CreateBoard()
        {
            _grid = new CellObject[_boardSize.Columns, _boardSize.Rows];

            _matchGame.CurrentLevel._cells.ForEach(cellConfig =>
            {
                (int X, int Y) coord = (cellConfig.PosX, cellConfig.PosY);
                var newCell = Instantiate(_cellsDictionary[cellConfig.CellType], _cellContainer);
                newCell.SetCellPosition(GetPositionByCoord(coord), coord);
                
                _inputManager.AddListeners(newCell);

                _grid[cellConfig.PosX, cellConfig.PosY] = newCell;
            });
        }
        
        public void MoveCell(CellObject cell, MoveDirectionType moveDirectionType)
        {
            (int X, int Y) currentCellPos = cell.CellCoord;
            (int X, int Y) targetCellPos = currentCellPos;
            switch (moveDirectionType)
            {
                case MoveDirectionType.Left:
                    targetCellPos.X--;
                    break;
                case MoveDirectionType.Right:
                    targetCellPos.X++;
                    break;
                case MoveDirectionType.Up:
                    targetCellPos.Y++;
                    break;
                case MoveDirectionType.Down:
                    targetCellPos.Y--;
                    break;
            }

            if (targetCellPos == currentCellPos) return;

            if (Enumerable.Range(0, _boardSize.Columns).Contains(targetCellPos.X) &&
                Enumerable.Range(0, _boardSize.Rows).Contains(targetCellPos.Y))
            {
                var currentGridItem = _grid[currentCellPos.X, currentCellPos.Y];
                var targetGridItem = _grid[targetCellPos.X, targetCellPos.Y];
                
                if(targetGridItem == null && moveDirectionType == MoveDirectionType.Up) return;
                
                _grid[targetCellPos.X, targetCellPos.Y] = currentGridItem;
                _grid[currentCellPos.X, currentCellPos.Y] = targetGridItem;

                if (targetGridItem != null)
                {
                    currentGridItem.SetCellPosition(GetPositionByCoord(targetCellPos), targetCellPos);
                    targetGridItem.SetCellPosition(GetPositionByCoord(currentCellPos), currentCellPos);
                }
                else
                {
                    currentGridItem.SetCellPosition(GetPositionByCoord(targetCellPos), targetCellPos);
                }
            }
        }

        public void MatchCells()
        {
        }

        public void NormalizeBord()
        {
        }
        
        private Vector3 GetPositionByCoord((int X, int Y) coord)
        {
            Vector3 positionOffset = transform.position - new Vector3(_boardSize.Columns * _cellSize.X / 2.5f, 11f, 0);
            return new Vector3(coord.X * _cellSize.X, coord.Y * _cellSize.Y, 0) + positionOffset;
        }
    }
}