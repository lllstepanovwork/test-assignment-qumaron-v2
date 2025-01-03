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

                    if (row % 2 == 1)
                    {
                        xPos += gridSettings.HexWidth / 2;
                    }

                    xPos -= gridOffsetX;
                    yPos -= gridOffsetY;

                    Vector3 position = new Vector3(xPos, yPos, 0);
                    
                    GridElement gridElement = Instantiate(gridElementPrefab, position, Quaternion.identity, transform);

                    gridElement.SetGridPosition(col, row);
                    gridElement.gameObject.name = $"{col} | {row}";
                    
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
            (int col, int row) = gridElement.GetGridPosition();

            GridElement topElement = GetElementAt(col, row + 1);
            GridElement bottomElement = GetElementAt(col - 1, row - 1);
            GridElement leftElement = GetElementAt(col - 1, row + 1);
            GridElement rightElement = GetElementAt(col , row - 1);

            if (row % 2 != 1)
                return new GridElementNeighbors(
                    topElement,
                    bottomElement,
                    leftElement,
                    rightElement
                );
            
            topElement = GetElementAt(col + 1, row + 1);
            bottomElement = GetElementAt(col, row - 1);
            leftElement = GetElementAt(col, row + 1);
            rightElement = GetElementAt(col + 1, row - 1);

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

        public int GetRowByPosition(Vector3 position)
        {
            foreach (GridElement gridElement in _gridElements)
            {
                float height = gridElement.transform.position.y; 
                if (position.y >= height - gridSettings.HexHeight / 2 && position.y <= height + gridSettings.HexHeight / 2)
                {
                    return gridElement.GetGridPosition().Item2;
                }
            }

            return 25;
        }
        
        public int GetColumnByPosition(Vector3 position)
        {
            foreach (GridElement gridElement in _gridElements)
            {
                float width = gridElement.transform.position.x;
                
                if (position.x >= width - gridSettings.HexWidth / 2 && position.x <= width + gridSettings.HexWidth / 2)
                {
                    return gridElement.GetGridPosition().Item1;
                }
            }

            return 25;
        }
    }

    [Serializable]
    public class GridSettings
    {
        [SerializeField] private int columns = 50;
        [SerializeField] private int rows = 50;
        [SerializeField] private float hexWidth = 2.4f;
        [SerializeField] private float hexHeight = 0.6f;

        public int Columns => columns;
        public int Rows => rows;
        public float HexWidth => hexWidth;
        public float HexHeight => hexHeight;
    }
}
