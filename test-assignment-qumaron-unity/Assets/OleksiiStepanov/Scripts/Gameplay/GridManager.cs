using System;
using UnityEngine;
using System.Collections.Generic;

namespace OleksiiStepanov.UI
{
    public class GridManager : MonoBehaviour
    {
        private  List<GridElement> _gridElements = new List<GridElement>();

        [SerializeField] private GridElement gridElementPrefab;
        [SerializeField] private int gridColumns = 5;
        [SerializeField] private int gridRows = 4;
        [SerializeField] private float hexWidth = 2.4f;
        [SerializeField] private float hexHeight = 1.2f;

        private GridElement[,] grid;  // 2D array for element tracking

        public bool IsGridActive { get; private set; }

        public void Init()
        {
            GenerateGrid(BuildStartingRoads);
        }

        private void GenerateGrid(Action onComplete = null)
        {
            if (gridElementPrefab == null)
            {
                Debug.LogError("Prefab not assigned!");
                return;
            }

            grid = new GridElement[gridColumns, gridRows];

            float gridOffsetX = (gridColumns - 1) * hexWidth / 2;
            float gridOffsetY = (gridRows - 1) * hexHeight / 2;

            int counter = 0;
            
            for (int col = 0; col < gridColumns; col++)
            {
                counter++;
                
                for (int row = 0; row < gridRows; row++)
                {
                    float xPos = col * hexWidth;
                    float yPos = row * hexHeight;

                    // Stagger for odd rows (hex grid style)
                    if (row % 2 == 1)
                    {
                        xPos += hexWidth / 2;
                    }

                    xPos -= gridOffsetX;
                    yPos -= gridOffsetY;

                    Vector3 position = new Vector3(xPos, yPos, 0);
                    GridElement gridElement = Instantiate(gridElementPrefab, position, Quaternion.identity, transform);

                    // Assign grid position and store reference
                    gridElement.SetGridPosition(col, row);
                    grid[col, row] = gridElement;

                    gridElement.gameObject.name = $"{col} | {row}";
                    _gridElements.Add(gridElement);
                }
            }

            if (counter == gridColumns)
            {
                onComplete?.Invoke();    
            }
        }

        private void BuildStartingRoads()
        {
            GetElementAt(24,22).Select();
            GetElementAt(24,23).Select();
            GetElementAt(25,24).Select();
        }

        public GridElementNeighbors GetGridElementNeighbors(GridElement gridElement)
        {
            (int col, int row) = gridElement.GetGridPosition();

            GridElement topElement = GetElementAt(col, row + 1);
            GridElement bottomElement = GetElementAt(col - 1, row - 1);
            GridElement leftElement = GetElementAt(col - 1, row + 1);
            GridElement rightElement = GetElementAt(col , row - 1);
            
            if (row % 2 == 1)
            {
                topElement = GetElementAt(col + 1, row + 1);
                bottomElement = GetElementAt(col, row - 1);
                leftElement = GetElementAt(col, row + 1);
                rightElement = GetElementAt(col + 1, row - 1);
            }

            return new GridElementNeighbors(
                topElement,
                bottomElement,
                leftElement,
                rightElement
            );
        }

        private GridElement GetElementAt(int col, int row)
        {
            if (col >= 0 && col < gridColumns && row >= 0 && row < gridRows)
            {
                return grid[col, row];
            }
            return null;  // Out of bounds or empty spot
        }

            public void ActivateGrid()
            {
                for (int i = 0; i < _gridElements.Count; i++)
                {
                    _gridElements[i].Activate(true);
                }

                IsGridActive = true;
            }

            public void DeactivateGrid()
            {
                for (int i = 0; i < _gridElements.Count; i++)
                {
                    _gridElements[i].Activate(false);
                }

                IsGridActive = false;
            }
        }
        
        public class GridElementNeighbors
        {
            public readonly GridElement TopElement;
            public readonly GridElement BottomElement;
            public readonly GridElement LeftElement;
            public readonly GridElement RightElement;
            
            public GridElementNeighbors(
                GridElement topElement,
                GridElement bottomElement,
                GridElement leftElement,
                GridElement rightElement)
            {
                TopElement = topElement;
                BottomElement = bottomElement;
                LeftElement = leftElement;
                RightElement = rightElement;
            }
        }
}
