using System;
using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace OleksiiStepanov.Gameplay
{
    public class GridManager : MonoBehaviour
    {
        [Header("Content")]
        [SerializeField] private RoadManager roadManager;
        [SerializeField] private GridCreator gridCreator;

        private readonly GridPathFinder _pathFinder = new GridPathFinder();
        private List<GridElement> _gridElements = new List<GridElement>();
        private GridElement[,] _grid;

        public GridElement MiddleGridElement { get; private set; }
        
        private bool _isGridActive;

        public async UniTask Init(Action onComplete = null)
        {
            await gridCreator.Init();
            await BuildStartingRoads();
            
            _gridElements = gridCreator.GetGridElements();
            _grid = gridCreator.GetGridArray();

            MiddleGridElement = gridCreator.GetElementAt(25, 25);
            
            _pathFinder.Init(MiddleGridElement);
            
            onComplete?.Invoke();
        }
        
        private void OnEnable()
        {
            CharacterManager.OnCharacterSpawned += OnCharacterSpawned; 
            CharacterMovement.OnPathCompleted += OnCharacterPathCompleted;
        }

        private void OnDisable()
        {
            CharacterManager.OnCharacterSpawned -= OnCharacterSpawned;
            CharacterMovement.OnPathCompleted -= OnCharacterPathCompleted;
        }
        
        private void OnCharacterSpawned(CharacterMovement character)
        {
            _pathFinder.CreateStartingPath(character);
        }

        private void OnCharacterPathCompleted(CharacterMovement characterMovement)
        {
            _pathFinder.CreateNewPath(characterMovement);
        }

        public async UniTask BuildStartingRoads()
        {
            roadManager.BuildRoad(gridCreator.GetElementAt(23,21));
            roadManager.BuildRoad(gridCreator.GetElementAt(24,22));
            roadManager.BuildRoad(gridCreator.GetElementAt(24,23));
            roadManager.BuildRoad(gridCreator.GetElementAt(25,24));
            roadManager.BuildRoad(gridCreator.GetElementAt(25,25));
            roadManager.BuildRoad(gridCreator.GetElementAt(26,26));
            
            await UniTask.Yield();
        }

        public void ActivateGrid(bool activate, bool withCollider = false)
        {
            if (activate != _isGridActive)
            {
                _isGridActive = activate;
            
                foreach (var gridElement in _gridElements)
                {
                    gridElement.Activate(activate, withCollider);
                }    
            }
        }

        public async UniTask ResetAll()
        {
            foreach (var gridElement in _gridElements)
            {
                gridElement.ResetAll();
            }
            
            await UniTask.Yield();
        }

        public void SetObjectPosition(BuildingDrag buildingDrag)
        {
            GridElement gridElement = gridCreator.GetElementAt(25, 25);
            
            buildingDrag.transform.parent.position = gridElement.transform.position;
        }

        public Vector3 GetGridPosition(Vector3 worldPosition)
        {
            int column = gridCreator.GetColumnByPosition(worldPosition);
            int row = gridCreator.GetRowByPosition(worldPosition);
            
            Debug.Log(column + ", " + row);
            
            GridElement gridElement = gridCreator.GetElementAt(column, row);
            
            return gridElement.transform.position;
        }

        public bool IsCellOccupied(int column, int row)
        {
            if (column < 0 || column >= 50 || row < 0 || row >= 50)
                return true;
            return _grid[column, row].IsOccupied;
        }

        public void HighlightArea(Vector2Int start, Vector2Int size, bool canPlace)
        {
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    int col = start.x + x;
                    int row = start.y + y;
                
                    if (col >= 0 && col < 50 && row >= 0 && row < 50)
                    {
                        _grid[col, row].SetGridElementColor();
                    }
                }
            }
        }
    }
}
