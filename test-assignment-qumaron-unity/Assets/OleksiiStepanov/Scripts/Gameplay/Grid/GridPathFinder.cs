using System.Collections.Generic;
using UnityEngine;
using OleksiiStepanov.Utils;

namespace OleksiiStepanov.Gameplay
{
    public class GridPathFinder
    {
        private GridElement _startGridElement;
        
        public void Init(GridElement startGridElement)
        {
            _startGridElement = startGridElement;
        }

        public void CreateStartingPath(Character character)
        {
            if (character == null) return;
            
            character.Init(_startGridElement);
        }

        public void CreateNewPath(Character character)
        {
            if (character == null) return;
            
            Stack<GridElement> path = GetCharacterMovingPath(character.CurrentElement);
            character.SetNewPath(path);
        }

        private Stack<GridElement> GetCharacterMovingPath(GridElement gridElement)
        {
            var path = new Stack<GridElement>();
            var lastGridElement = gridElement;
            var currentGridElement = gridElement;

            int randomPathLength = Random.Range(10, 20);
            
            for (int i = 0; i < randomPathLength; i++)
            {
                List<GridElement> validDirections = GetValidNeighbors(currentGridElement, lastGridElement);

                if (validDirections.Count == 0)
                {
                    break; 
                }

                int randomDirectionIndex = Random.Range(0, validDirections.Count);

                lastGridElement = currentGridElement;
                currentGridElement = validDirections[randomDirectionIndex];
                
                path.Push(currentGridElement);
            }

            return path.ReverseStack();
        }

        private List<GridElement> GetValidNeighbors(GridElement gridElement, GridElement lastGridElement)
        {
            List<GridElement> directions = new List<GridElement>();
            
            if (IsValidDirection(gridElement.Neighbors.TopElement, lastGridElement))
            {
                directions.Add(gridElement.Neighbors.TopElement);
            }
            if (IsValidDirection(gridElement.Neighbors.BottomElement, lastGridElement))
            {
                directions.Add(gridElement.Neighbors.BottomElement);
            }
            if (IsValidDirection(gridElement.Neighbors.LeftElement, lastGridElement))
            {
                directions.Add(gridElement.Neighbors.LeftElement);
            }
            if (IsValidDirection(gridElement.Neighbors.RightElement, lastGridElement))
            {
                directions.Add(gridElement.Neighbors.RightElement);
            }

            return directions;
        }

        private bool IsValidDirection(GridElement gridElement, GridElement lastGridElement)
        {
            if (gridElement == null) return false;
            if (gridElement == lastGridElement ) return false;
            if (!gridElement.IsOccupied) return false;
            if (!gridElement.IsRoad) return false;
            if (!gridElement.IsConfirmRoad) return false;
            
            return true;
        }
    }
}
