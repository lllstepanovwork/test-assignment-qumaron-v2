using System;
using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace OleksiiStepanov.Gameplay
{
    public class GridCreator : MonoBehaviour
    {
        [Header("Content")]
        [SerializeField] private GridElement gridElementPrefab;
        
        [Header("Grid Settings")]
        [SerializeField] private GridSettings gridSettings;
        
        private readonly List<GridElement> _gridElements = new List<GridElement>();
        private GridElement[,] _grid;
        
        public async UniTask Init()
        {   
            await GenerateGridAsync();
            await SetGridNeighboursAsync();
        }

        private async UniTask GenerateGridAsync()
        {
            if (gridElementPrefab == null)
            {
                Debug.LogError("Prefab not assigned!");
                return;
            }

            _grid = new GridElement[gridSettings.Columns, gridSettings.Rows];

            float gridOffsetX = (gridSettings.Columns - 1) * gridSettings.HexWidth / 2;
            float gridOffsetY = (gridSettings.Rows - 1) * gridSettings.HexHeight / 2;

            for (int col = 0; col < gridSettings.Columns; col++)
            {
                for (int row = 0; row < gridSettings.Rows; row++)
                {
                    float xPos = col * gridSettings.HexWidth;
                    float yPos = row * gridSettings.HexHeight;

                    xPos -= gridOffsetX;
                    yPos -= gridOffsetY;

                    Vector3 position = new Vector3(xPos, yPos, 0);
                    
                    GridElement gridElement = Instantiate(gridElementPrefab, position, Quaternion.identity, transform);

                    gridElement.Init(col, row);
                    
                    _grid[col, row] = gridElement;
                    _gridElements.Add(gridElement);
                    
                    if (col % 25 == 0 && row % 25 == 0)
                    { 
                        await UniTask.Yield();
                    }
                }
            }
        }

        private async UniTask SetGridNeighboursAsync()
        {
            for (int i = 0; i < _gridElements.Count; i++)
            {   
                GridElementNeighbors gridElementNeighbors = GetGridElementNeighbors(_gridElements[i]);
                
                _gridElements[i].SetNeighbors(gridElementNeighbors);
                
                if (i % 50 == 0)
                {
                    await UniTask.Yield();
                }
            }
        }

        private GridElementNeighbors GetGridElementNeighbors(GridElement gridElement)
        {
            Vector2Int gridPosition = gridElement.GetGridPosition();

            GridElement topElement = GetElementAt(gridPosition.x + 1, gridPosition.y);
            GridElement bottomElement = GetElementAt(gridPosition.x - 1, gridPosition.y);
            GridElement leftElement = GetElementAt(gridPosition.x, gridPosition.y + 1);
            GridElement rightElement = GetElementAt(gridPosition.x, gridPosition.y - 1);
            
            return new GridElementNeighbors(
                topElement,
                bottomElement,
                leftElement,
                rightElement
            );
        }

        public GridElement GetElementAt(int col, int row)
        {
            if (col >= 0 && col < gridSettings.Columns && row >= 0 && row < gridSettings.Rows)
            {
                return _grid[col, row];
            }
            return null; 
        }

        public List<GridElement> GetGridElements()
        {
            return _gridElements;
        }
        
        public GridElement[,] GetGridArray()
        {
            return _grid;
        }

        public int GetGridRowCount()
        {
            return gridSettings.Rows;
        }

        public int GetGridColumnCount()
        {
            return gridSettings.Columns;
        }
    }

    [Serializable]
    public class GridSettings
    {
        [SerializeField] private int columns = 50;
        [SerializeField] private int rows = 50;
        [SerializeField] private float hexWidth = 1.28f;
        [SerializeField] private float hexHeight = 1.28f;

        public int Columns => columns;
        public int Rows => rows;
        public float HexWidth => hexWidth;
        public float HexHeight => hexHeight;
    }
}
