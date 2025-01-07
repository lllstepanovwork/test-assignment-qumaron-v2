using System;
using System.Collections.Generic;
using UnityEngine;
using OleksiiStepanov.Data;

namespace OleksiiStepanov.Gameplay
{
    public class RoadManager : MonoBehaviour
    {
        [SerializeField] private List<RoadSO> roads;

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
            if (gridElement == null || gridElement.IsOccupied) return;
            
            bool top = gridElement.Neighbors.TopElement != null && gridElement.Neighbors.TopElement.IsOccupied && gridElement.Neighbors.TopElement.IsRoad;
            bool bottom = gridElement.Neighbors.BottomElement != null && gridElement.Neighbors.BottomElement.IsOccupied && gridElement.Neighbors.BottomElement.IsRoad;
            bool left = gridElement.Neighbors.LeftElement != null && gridElement.Neighbors.LeftElement.IsOccupied && gridElement.Neighbors.LeftElement.IsRoad;
            bool right = gridElement.Neighbors.RightElement != null && gridElement.Neighbors.RightElement.IsOccupied && gridElement.Neighbors.RightElement.IsRoad;

            RoadType newType = CalculateRoadType(top, bottom, left, right);

            RoadSO roadSo = GetRoadByRoadType(newType);
            
            gridElement.OccupyByRoad(roadSo);
            
            onComplete?.Invoke();
        }
        
        private void UpdateTile(GridElement gridElement, Action onComplete = null)
        {
            if (gridElement == null || !gridElement.IsRoad) return;
            
            bool top = gridElement.Neighbors.TopElement != null && gridElement.Neighbors.TopElement.IsRoad;
            bool bottom = gridElement.Neighbors.BottomElement != null  && gridElement.Neighbors.BottomElement.IsRoad;
            bool left = gridElement.Neighbors.LeftElement != null &&  gridElement.Neighbors.LeftElement.IsRoad;
            bool right = gridElement.Neighbors.RightElement != null &&  gridElement.Neighbors.RightElement.IsRoad;

            RoadType newType = CalculateRoadType(top, bottom, left, right);
            
            RoadSO roadSo = GetRoadByRoadType(newType);
            
            gridElement.OccupyByRoad(roadSo);
            
            onComplete?.Invoke();
        }
        
        private RoadType CalculateRoadType(bool top, bool bottom, bool left, bool right)
        {
            if (top && bottom && left && right) return RoadType.CrossX;
            
            if (top && left && right) return RoadType.CrossTTop;
            if (top && left && bottom) return RoadType.CrossTLeft;
            if (top && bottom && right) return RoadType.CrossTRight;
            if (bottom && left && right) return RoadType.CrossTBottom;
            
            if (bottom && !(top || left || right)) return RoadType.TopRightEnd;
            if (top && !(left || right || bottom)) return RoadType.BottomLeftEnd;
            if (left && !(top || right || bottom)) return RoadType.BottomRightEnd;
            if (right && !(top || left || bottom)) return RoadType.TopLeftEnd;
            
            if (top && right) return RoadType.TurnLeft;
            if (bottom && right) return RoadType.TurnTop;
            if (left && top) return RoadType.TurnBottom;
            if (left && bottom) return RoadType.TurnRight;
            
            if (left || right) return RoadType.Left;
            if (top || bottom) return RoadType.Right;

            return RoadType.CrossX;  
        }
        
        private RoadSO GetRoadByRoadType(RoadType roadType)
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




