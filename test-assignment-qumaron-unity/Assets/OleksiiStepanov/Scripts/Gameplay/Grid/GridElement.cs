using System;
using UnityEngine;

namespace OleksiiStepanov.Gameplay
{
    public class GridElement : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer roadSpriteRenderer;
        [SerializeField] private SpriteRenderer gridElementSpriteRenderer;
        [SerializeField] private SpriteRenderer gridElementFillSpriteRenderer;
        
        [SerializeField] private GameObject gridElementCollider;

        public static event Action<GridElement> OnSelect;
        
        public GridElementNeighbors Neighbors { get; private set; }
        
        public bool IsRoad { get; private set; }
        public bool IsConfirmRoad { get; private set; }
        public bool IsOccupied { get; private set; }
        
        private bool _isGridActive;
        
        private int _column;
        private int _row;

        public int SortingLayerOrder { get; private set; }

        public void Init(int column, int row)
        {
            _column = column;
            _row = row;
            
            gameObject.name = $"{_column} | {_row}";
            
            SortingLayerOrder = -1 * (_column + _row);
        }

        public void SetNeighbors(GridElementNeighbors neighbors)
        {
            Neighbors = neighbors;
        }

        public void Activate(bool activate, bool withCollider = false)
        {
            if (IsOccupied) return;

            gridElementCollider.SetActive(withCollider);
            gridElementSpriteRenderer.gameObject.SetActive(activate);

            _isGridActive = activate;
        }

        public void Occupy()
        {
            IsOccupied = true;
            
            gridElementSpriteRenderer.gameObject.SetActive(false);
        }
        
        public void OccupyByRoad(RoadSO roadSo)
        {
            IsOccupied = true;
            IsRoad = true;
            
            roadSpriteRenderer.gameObject.SetActive(true);
            roadSpriteRenderer.sprite = roadSo.roadSprite;
            
            gridElementSpriteRenderer.gameObject.SetActive(false);
        }

        public void ResetRoadOccupation()
        {
            IsOccupied = false;
            IsRoad = false;
            
            roadSpriteRenderer.gameObject.SetActive(false);
            gridElementSpriteRenderer.gameObject.SetActive(true);
        }

        public void ConfirmRoadOccupation()
        {
            IsConfirmRoad = true;
        }

        public void Select()
        {
            if (IsOccupied || !_isGridActive) return;
            
            OnSelect?.Invoke(this);
        }

        public Vector2Int GetGridPosition()
        {
            return new Vector2Int(_column, _row); 
        }

        public void SetGridElementColor()
        {
            gridElementFillSpriteRenderer.gameObject.SetActive(true);
            
            if (IsOccupied)
            {
                gridElementFillSpriteRenderer.color = Color.red;
            }
            else
            {
                gridElementFillSpriteRenderer.color = Color.green;
            }
        }

        public void ResetColor()
        {
            gridElementFillSpriteRenderer.color = Color.clear;
            gridElementFillSpriteRenderer.gameObject.SetActive(false);
        }
        
        public void ResetAll()
        {
            IsRoad = false;
            IsOccupied = false;
            IsConfirmRoad = false;
            
            _isGridActive = false;
            
            roadSpriteRenderer.gameObject.SetActive(false);
            gridElementSpriteRenderer.gameObject.SetActive(false);
            gridElementFillSpriteRenderer.gameObject.SetActive(false);
            gridElementCollider.gameObject.SetActive(false);
            
            ResetColor();
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


