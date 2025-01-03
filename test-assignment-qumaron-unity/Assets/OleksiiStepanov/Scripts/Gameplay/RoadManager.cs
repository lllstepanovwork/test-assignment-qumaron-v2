using System;
using System.Collections.Generic;
using UnityEngine;
using OleksiiStepanov.Game;

namespace OleksiiStepanov.Gameplay
{
    public class RoadManager : MonoBehaviour
    {
        [SerializeField] private List<Road> roads;

        private bool _isActive = false;
        
        private void OnSelect(GridElement gridElement)
        {
            if (_isActive)
            {
                BuildRoad(gridElement);   
            }
        }
        
        private void OnEnable()
        {
            GridElement.OnSelect += OnSelect;
        }

        private void OnDisable()
        {
            GridElement.OnSelect -= OnSelect;
        }

        public void Activate(bool isActive)
        {
            _isActive = isActive;
        }

        public void BuildRoad(GridElement gridElement)
        {
            BuildRoad(gridElement, () =>
            {
                UpdateTile(gridElement.Neighbors.TopElement);
                UpdateTile(gridElement.Neighbors.BottomElement);
                UpdateTile(gridElement.Neighbors.LeftElement);
                UpdateTile(gridElement.Neighbors.RightElement); 
            });
        }

        private void BuildRoad(GridElement gridElement, Action onComplete)
        {
            if (gridElement.IsOccupied) return;
            
            bool top = gridElement.Neighbors.TopElement != null && gridElement.Neighbors.TopElement.IsOccupied;
            bool bottom = gridElement.Neighbors.BottomElement != null && gridElement.Neighbors.BottomElement.IsOccupied;
            bool left = gridElement.Neighbors.LeftElement != null && gridElement.Neighbors.LeftElement.IsOccupied;
            bool right = gridElement.Neighbors.RightElement != null && gridElement.Neighbors.RightElement.IsOccupied;

            RoadType newType = CalculateRoadType(top, bottom, left, right);

            Road road = GetRoadByRoadType(newType);
            
            gridElement.OccupyByRoad(road);
            
            onComplete?.Invoke();
        }
        
        private void UpdateTile(GridElement gridElement, Action onComplete = null)
        {
            if (gridElement == null || !gridElement.IsOccupied) return;
            
            bool top = gridElement.Neighbors.TopElement != null && gridElement.Neighbors.TopElement.IsOccupied;
            bool bottom = gridElement.Neighbors.BottomElement != null && gridElement.Neighbors.BottomElement.IsOccupied;
            bool left = gridElement.Neighbors.LeftElement != null && gridElement.Neighbors.LeftElement.IsOccupied;
            bool right = gridElement.Neighbors.RightElement != null && gridElement.Neighbors.RightElement.IsOccupied;

            RoadType newType = CalculateRoadType(top, bottom, left, right);
            
            gridElement.OccupyByRoad(GetRoadByRoadType(newType));
            
            onComplete?.Invoke();
        }
        
        private RoadType CalculateRoadType(bool top, bool bottom, bool left, bool right)
        {
            if (top && bottom && left && right) return RoadType.CrossX;
            
            if (top && left && right) return RoadType.CrossTTop;
            if (top && left && bottom) return RoadType.CrossTLeft;
            if (top && bottom && right) return RoadType.CrossTRight;
            if (bottom && left && right) return RoadType.CrossTBottom;
            
            if (top && right) return RoadType.TurnLeft;
            if (bottom && right) return RoadType.TurnTop;
            if (left && top) return RoadType.TurnBottom;
            if (left && bottom) return RoadType.TurnRight;
            
            if (left || right) return RoadType.Left;
            if (top || bottom) return RoadType.Right;

            return RoadType.CrossX;  
        }
        
        private Road GetRoadByRoadType(RoadType roadType)
        {
            foreach (var road in roads)
            {
                if (roadType == road.roadType)
                {
                    return road;
                }
            }

            return null;
        }
    }
}




