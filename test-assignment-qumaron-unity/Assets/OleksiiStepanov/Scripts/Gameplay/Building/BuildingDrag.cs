using UnityEngine;
using OleksiiStepanov.Gameplay;

public class BuildingDrag : MonoBehaviour
{
    [Header("Grid Settings")] 
    private GridManager _gridManager;
    private bool _isDragging = false;
    private Vector3 _offset;

    public void Init()
    {
        _gridManager = FindObjectOfType<GridManager>();
        
        _gridManager.SetObjectPosition(this);
    }

    private void OnMouseDown()
    {
        _offset = transform.parent.position - GetMousePos();
        _isDragging = true;
    }

    private void OnMouseDrag()
    {
        if (_isDragging)
        {
            Vector3 targetPosition = GetMousePos() + _offset;
            Vector3 gridPosition = _gridManager.GetGridPosition(targetPosition);
            
            transform.parent.position = gridPosition;
        }
    }

    private void OnMouseUp()
    {
        _isDragging = false;
    }
    
    private Vector3 GetMousePos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}