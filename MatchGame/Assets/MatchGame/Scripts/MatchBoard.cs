using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;
using IInitializable = Zenject.IInitializable;

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

            _matchGame.CurrentLevel.Cells.ForEach(cellConfig =>
            {
                (int X, int Y) coord = (cellConfig.PosX, cellConfig.PosY);
                var newCell = Instantiate(_cellsDictionary[cellConfig.CellType], _cellContainer);
                newCell.SetCellPosition(GetPositionByCoord(coord), coord);

                _inputManager.AddListeners(newCell);

                _grid[cellConfig.PosX, cellConfig.PosY] = newCell;
            });

            NormalizeBord();
        }

        public async UniTask<bool> MoveCell(CellObject cell, MoveDirectionType moveDirectionType)
        {
            _inputManager.IsInputBlocked = true;
            
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

            if (targetCellPos == currentCellPos) return false;

            if (IsValidCoord(targetCellPos))
            {
                var currentGridItem = _grid[currentCellPos.X, currentCellPos.Y];
                var targetGridItem = _grid[targetCellPos.X, targetCellPos.Y];

                if (targetGridItem == null && moveDirectionType == MoveDirectionType.Up) return false;

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

            NormalizeBord();
            return true;
        }

        private async UniTask<bool> MatchCells()
        {
            HashSet<CellObject> matchedCellObjects = new HashSet<CellObject>();

            _matchGame.CurrentLevel.AvailableCellsTypes.ForEach(cellType =>
            {
                var horizontalMatches = FindHorizontalMatches(cellType);
                var verticalMatches = FindVerticalMatches(cellType);

                matchedCellObjects.UnionWith(horizontalMatches);
                matchedCellObjects.UnionWith(verticalMatches);
            });

            List<UniTask> tasks = new List<UniTask>();
            foreach (var cellObject in matchedCellObjects)
            {
                _grid[cellObject.CellCoord.X, cellObject.CellCoord.Y] = null;
                _inputManager.RemoveListeners(cellObject);
                tasks.Add(cellObject.DestroyCell());
            }

            await UniTask.WhenAll(tasks);
            return matchedCellObjects.Count > 0;
        }

        private List<CellObject> FindVerticalMatches(CellType cellType)
        {
            List<CellObject> matchedCellObjects = new List<CellObject>();
            for (int column = 0; column < _boardSize.Columns; column++)
            {
                List<CellObject> matchSequence = new List<CellObject>();
                for (int row = 1; row < _boardSize.Rows; row++)
                {
                    if (_grid[column, row] != null && _grid[column, row].CellType == cellType)
                    {
                        matchSequence.Add(_grid[column, row]);
                    }
                    else
                    {
                        if (matchSequence.Count >= 3)
                        {
                            matchedCellObjects.AddRange(matchSequence);
                        }

                        matchSequence.Clear();
                    }
                }

                if (matchSequence.Count >= 3)
                {
                    matchedCellObjects.AddRange(matchSequence);
                }
            }

            return matchedCellObjects;
        }

        private List<CellObject> FindHorizontalMatches(CellType cellType)
        {
            List<CellObject> matchedCellObjects = new List<CellObject>();
            for (int row = 0; row < _boardSize.Rows; row++)
            {
                List<CellObject> matchSequence = new List<CellObject>();
                for (int column = 0; column < _boardSize.Columns; column++)
                {
                    if (_grid[column, row] != null && _grid[column, row].CellType == cellType)
                    {
                        matchSequence.Add(_grid[column, row]);
                    }
                    else
                    {
                        if (matchSequence.Count >= 3)
                        {
                            matchedCellObjects.AddRange(matchSequence);
                        }

                        matchSequence.Clear();
                    }
                }

                if (matchSequence.Count >= 3)
                {
                    matchedCellObjects.AddRange(matchSequence);
                }
            }

            return matchedCellObjects;
        }

        private async UniTask<bool> FallCells()
        {
            int fallingCells = 0;

            for (int row = 0; row < _boardSize.Rows; row++)
            {
                for (int column = 0; column < _boardSize.Columns; column++)
                {
                    if (_grid[column, row] == null) continue;

                    (int X, int Y) current = (column, row);
                    (int X, int Y) next = (column, row - 1);
                    (int X, int Y) target = current;

                    while (IsValidCoord(next) && _grid[next.X, next.Y] == null)
                    {
                        target = next;
                        next.Y--;
                    }

                    if (target != current)
                    {
                        var currentGridItem = _grid[current.X, current.Y];

                        _grid[target.X, target.Y] = currentGridItem;
                        _grid[current.X, current.Y] = null;

                        currentGridItem.SetCellPosition(GetPositionByCoord(target), target);
                        fallingCells++;
                    }
                }
            }

            return fallingCells > 0;
        }

        private async UniTask<bool> NormalizeBord()
        {
            bool fallResult;
            bool matchResult;

            _inputManager.IsInputBlocked = true;
            do
            {
                fallResult = await FallCells();
                matchResult = await MatchCells();
            } while (fallResult || matchResult);

            _inputManager.IsInputBlocked = false;
            return true;
        }

        private Vector3 GetPositionByCoord((int X, int Y) coord)
        {
            Vector3 positionOffset = transform.position - new Vector3(_boardSize.Columns * _cellSize.X / 2.5f, 11f, 0);
            return new Vector3(coord.X * _cellSize.X, coord.Y * _cellSize.Y, 0) + positionOffset;
        }

        private bool IsValidCoord((int X, int Y) coord)
        {
            return Enumerable.Range(0, _boardSize.Columns).Contains(coord.X) &&
                   Enumerable.Range(0, _boardSize.Rows).Contains(coord.Y);
        }
    }
}