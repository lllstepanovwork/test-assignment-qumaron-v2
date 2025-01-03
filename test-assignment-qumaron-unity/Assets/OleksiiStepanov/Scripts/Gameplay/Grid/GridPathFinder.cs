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

        public void CreateStartingPath(CharacterMovement character)
        {
            character.Init(_startGridElement);
            
            CreateNewPath(character);
        }

        public void CreateNewPath(CharacterMovement character)
        {
            Stack<GridElement> path = GetCharacterMovingPath(character.LastElement);
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

            if (gridElement.Neighbors.TopElement != null && gridElement.Neighbors.TopElement.IsOccupied && gridElement.Neighbors.TopElement != lastGridElement)
            {
                directions.Add(gridElement.Neighbors.TopElement);
            }
            if (gridElement.Neighbors.BottomElement != null && gridElement.Neighbors.BottomElement.IsOccupied && gridElement.Neighbors.BottomElement != lastGridElement)
            {
                directions.Add(gridElement.Neighbors.BottomElement);
            }
            if (gridElement.Neighbors.LeftElement != null && gridElement.Neighbors.LeftElement.IsOccupied && gridElement.Neighbors.LeftElement != lastGridElement)
            {
                directions.Add(gridElement.Neighbors.LeftElement);
            }
            if (gridElement.Neighbors.RightElement != null && gridElement.Neighbors.RightElement.IsOccupied && gridElement.Neighbors.RightElement != lastGridElement)
            {
                directions.Add(gridElement.Neighbors.RightElement);
            }

            return directions;
        }
    }
}
