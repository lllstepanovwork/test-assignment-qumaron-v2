using UnityEngine;
using OleksiiStepanov.Gameplay;

public class BuildingDrag : MonoBehaviour
{
    [Header("Grid Settings")] 
    [SerializeField] private Transform dragTransform;

    public GridElement LastElement { get; private set; }
    public bool IsPlaceable {get; private set; }
    
    private bool _isDragging = false;
    
    private GridManager _gridManager;
    
    private Vector3 _offset;
    private Vector2Int _size;

    public void Init(Vector2Int size, GridManager gridManager)
    {
        _size = size;
        _gridManager = gridManager;
        
        LastElement = _gridManager.GetAvailablePositionForBuilding(size);

        if (LastElement == null)
        {
            LastElement = _gridManager.MiddleGridElement;
        }

        dragTransform.position = LastElement.transform.position;
        
        IsPlaceable = _gridManager.IsSpaceEnough(LastElement, _size);
        
        _gridManager.HighlightSpaceByPattern(LastElement, _size);

        gameObject.SetActive(true);
    }

    private void OnMouseDown()
    {
        _offset = dragTransform.position - GetMousePos();
        _isDragging = true;
    }

    private void OnMouseDrag()
    {
        if (_isDragging)
        {
            Vector3 targetPosition = GetMousePos() + _offset;
            
            dragTransform.position = targetPosition;
            
            SnapToGrid();
        }
    }

    private void OnMouseUp()
    {
        _isDragging = false;
    }
    
    private Vector3 GetMousePos()
    {
        Vector3 mousePoint = Input.mousePosition;
        
        if (Camera.main != null)
        {
            mousePoint.z = Camera.main.WorldToScreenPoint(dragTransform.position).z;
            return Camera.main.ScreenToWorldPoint(mousePoint);
        }
        
        return Vector3.zero;
    }
    
    private void SnapToGrid()
    {
        Vector2Int nearestGridPos = _gridManager.GetNearestGridPosition(dragTransform.position);
        LastElement = _gridManager.GetGridElementByPosition(nearestGridPos);
        
        Vector3 snapPosition = LastElement.transform.position;
        
        dragTransform.position = snapPosition;
        
        IsPlaceable = _gridManager.IsSpaceEnough(LastElement, _size);
        
        _gridManager.HighlightSpaceByPattern(LastElement, _size);
    }

    public void Deactivate()
    {
        _gridManager.ResetGridElementHighlight();
        
        gameObject.SetActive(false);
    }
}