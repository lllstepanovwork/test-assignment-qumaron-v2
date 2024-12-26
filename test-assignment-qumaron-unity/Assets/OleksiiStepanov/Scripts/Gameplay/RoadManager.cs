using System;
using System.Collections.Generic;
using OleksiiStepanov.UI;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    [SerializeField] private List<Road> roads;
    [SerializeField] private GridManager gridManager;

    public void Init()
    {
    }

    private void OnEnable()
    {
        GridElement.OnSelect += OnSelect;
    }

    private void OnDisable()
    {
        GridElement.OnSelect -= OnSelect;
    }

    private void OnSelect(GridElement gridElement)
    {
        OccupyTile(gridElement, () =>
        {
            GridElementNeighbors gridElementNeighbors = gridManager.GetGridElementNeighbors(gridElement);

            UpdateTile(gridElementNeighbors.TopElement);
            UpdateTile(gridElementNeighbors.BottomElement);
            UpdateTile(gridElementNeighbors.LeftElement);
            UpdateTile(gridElementNeighbors.RightElement); 
        });
    }

    private Road GetRoadByRoadType(RoadType roadType)
    {
        for (int i  = 0; i  < roads.Count; i ++)
        {
            if (roadType == roads[i].roadType)
            {
                return roads[i];
            }
        }

        return null;
    }
    
    private void OccupyTile(GridElement gridElement, Action onComplete = null)
    {
        if (gridElement.IsOccupied) return;
        
        GridElementNeighbors gridElementNeighbors = gridManager.GetGridElementNeighbors(gridElement);
        
        bool top = gridElementNeighbors.TopElement != null && gridElementNeighbors.TopElement.IsOccupied;
        bool bottom = gridElementNeighbors.BottomElement != null && gridElementNeighbors.BottomElement.IsOccupied;
        bool left = gridElementNeighbors.LeftElement != null && gridElementNeighbors.LeftElement.IsOccupied;
        bool right = gridElementNeighbors.RightElement != null && gridElementNeighbors.RightElement.IsOccupied;

        // Determine the correct road type
        RoadType newType = CalculateRoadType(top, bottom, left, right);
        
        //Debug.Log($"Top: {top}, Bottom: {bottom}, Left: {left}, Right: {right}, RoadType: {newType}");
        
        gridElement.Occupy(GetRoadByRoadType(newType));
        
        onComplete?.Invoke();
    }
    
    private void UpdateTile(GridElement gridElement, Action onComplete = null)
    {
        if (gridElement == null || !gridElement.IsOccupied) return;
        
        GridElementNeighbors gridElementNeighbors = gridManager.GetGridElementNeighbors(gridElement);
        
        bool top = gridElementNeighbors.TopElement != null && gridElementNeighbors.TopElement.IsOccupied;
        bool bottom = gridElementNeighbors.BottomElement != null && gridElementNeighbors.BottomElement.IsOccupied;
        bool left = gridElementNeighbors.LeftElement != null && gridElementNeighbors.LeftElement.IsOccupied;
        bool right = gridElementNeighbors.RightElement != null && gridElementNeighbors.RightElement.IsOccupied;

        // Determine the correct road type
        RoadType newType = CalculateRoadType(top, bottom, left, right);
        
        Debug.Log($"Top: {gridElementNeighbors.TopElement.gameObject.name}");
        Debug.Log($"Bottom: {gridElementNeighbors.BottomElement.gameObject.name}");
        Debug.Log($"Left: {gridElementNeighbors.LeftElement.gameObject.name}");
        Debug.Log($"Right: {gridElementNeighbors.RightElement.gameObject.name}");
        
        Debug.Log($"Element: {gridElement.gameObject.name},  Top: {top}, Bottom: {bottom}, Left: {left}, Right: {right}, RoadType: {newType}");
        
        gridElement.Occupy(GetRoadByRoadType(newType));
        
        onComplete?.Invoke();
    }

// Choose road type based on connected neighbors
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
}



