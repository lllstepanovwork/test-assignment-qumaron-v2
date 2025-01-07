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

        public async UniTask Init(Action onComplete = null)
        {
            await gridCreator.Init();
            await BuildStartingRoads();
            
            _gridElements = gridCreator.GetGridElements();
            _grid = gridCreator.GetGridArray();

            MiddleGridElement = gridCreator.GetElementAt(gridCreator.GetGridColumnCount() / 2, gridCreator.GetGridRowCount() / 2);
            
            _pathFinder.Init(MiddleGridElement);
            
            transform.Rotate(new Vector3(60, 0, 45));
            
            onComplete?.Invoke();
        }
        
        private void OnEnable()
        {
            CharacterManager.OnCharacterSpawn += OnCharacterSpawn; 
            Character.OnPathCompleted += OnCharacterPathCompleted;
        }

        private void OnDisable()
        {
            CharacterManager.OnCharacterSpawn -= OnCharacterSpawn;
            Character.OnPathCompleted -= OnCharacterPathCompleted;
        }
        
        private void OnCharacterSpawn(Character character)
        {
            _pathFinder.CreateStartingPath(character);
        }

        private void OnCharacterPathCompleted(Character character)
        {
            _pathFinder.CreateNewPath(character);
        }

        public async UniTask BuildStartingRoads()
        {
            roadManager.BuildRoad(gridCreator.GetElementAt(22,25));
            roadManager.BuildRoad(gridCreator.GetElementAt(23,25));
            roadManager.BuildRoad(gridCreator.GetElementAt(24,25));
            roadManager.BuildRoad(gridCreator.GetElementAt(25,25));
            roadManager.BuildRoad(gridCreator.GetElementAt(25,25));
            roadManager.BuildRoad(gridCreator.GetElementAt(26,25));
            
            await UniTask.Yield();
        }

        public void ActivateGrid(bool activate, bool withCollider = false)
        {
            foreach (var gridElement in _gridElements)
            {
                gridElement.Activate(activate, withCollider);
            }
        }

        public Vector2Int GetNearestGridPosition(Vector3 worldPosition)
        {
            float minDistance = float.MaxValue;
            Vector2Int nearestGridPos = Vector2Int.zero;

            ResetGridElementHighlight();
            
            foreach (var element in _gridElements)
            {
                float distance = Vector3.Distance(worldPosition, element.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestGridPos = element.GetGridPosition();
                }
            }
            
            return nearestGridPos;
        }

        public GridElement GetGridElementByPosition(Vector2Int gridPos)
        {
            return _grid[gridPos.x, gridPos.y];
        }

        public void HighlightSpaceByPattern(GridElement centralElement, Vector2Int pattern)
        {
            List<GridElement> elementsToHighlight = GetGridElementsByPattern(centralElement, pattern);

            foreach (var element in elementsToHighlight)
            {
                element.SetGridElementColor();
            }
        }
        
        public bool IsSpaceEnough(GridElement targetElement, Vector2Int pattern)
        {
            List<GridElement> elementsToCheck = GetGridElementsByPattern(targetElement, pattern);

            foreach (var element in elementsToCheck)
            {
                if (element.IsOccupied)
                {
                    return false;
                }
            }

            return true;
        }
        
        public void OccupyGridElementWithPattern(GridElement targetElement, Vector2Int pattern)
        {
            List<GridElement> elementsToOccupy = GetGridElementsByPattern(targetElement, pattern);

            foreach (var element in elementsToOccupy)
            {
                element.Occupy();
            }
        }

        private List<GridElement> GetGridElementsByPattern(GridElement centralElement, Vector2Int pattern)
        {
            List<GridElement> matchingElements = new List<GridElement>();

            int startCol = centralElement.GetGridPosition().x;
            int startRow = centralElement.GetGridPosition().y;

            int width = pattern.x;
            int height = pattern.y;

            for (int xOffset = 0; xOffset < width; xOffset++)
            {
                for (int yOffset = 0; yOffset < height; yOffset++)
                {
                    int targetCol = startCol + xOffset;
                    int targetRow = startRow + yOffset;

                    if (targetCol >= 0 && targetCol < gridCreator.GetGridColumnCount() &&
                        targetRow >= 0 && targetRow < gridCreator.GetGridRowCount())
                    {
                        GridElement gridElement = _grid[targetCol, targetRow];
                        if (gridElement != null)
                        {
                            matchingElements.Add(gridElement);
                        }
                    }
                }
            }
    
            return matchingElements;
        }

        public GridElement GetAvailablePositionForBuilding(Vector2Int pattern)
        {
            List<GridElement> emptyElementsByRoad = GetAvailableElementsByTheRoad();

            foreach (var element in emptyElementsByRoad)
            {
                if (IsSpaceEnough(element, pattern))
                {
                    return element;
                }
            }
            
            return null;
        }

        private List<GridElement> GetAvailableElementsByTheRoad()
        {
            List<GridElement> emptyElementsByRoad = new List<GridElement>();

            foreach (var element in _gridElements)
            {
                if (element.IsRoad)
                {
                    AddToListByOccupyStatus(element.Neighbors.TopElement, emptyElementsByRoad);
                    AddToListByOccupyStatus(element.Neighbors.BottomElement, emptyElementsByRoad);
                    AddToListByOccupyStatus(element.Neighbors.LeftElement, emptyElementsByRoad);
                    AddToListByOccupyStatus(element.Neighbors.RightElement, emptyElementsByRoad);
                }
            }

            return emptyElementsByRoad;
        }

        private void AddToListByOccupyStatus(GridElement element, List<GridElement> list)
        {
            if (element != null && !element.IsOccupied)
            {
                list.Add(element);
            }
        }

        public float GetGridBorderX()
        {
            //highest point in the grid (before rotation)
            int index = gridCreator.GetGridRowCount() - 1;
            
            return _gridElements[index].transform.position.x;
        }
        
        public float GetGridBorderY()
        {
            //lowest point in the grid
            return _gridElements[0].transform.position.y;
        }

        public void ResetGridElementHighlight()
        {
            foreach (var element in _gridElements)
            {
                element.ResetColor();
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
    }
}
