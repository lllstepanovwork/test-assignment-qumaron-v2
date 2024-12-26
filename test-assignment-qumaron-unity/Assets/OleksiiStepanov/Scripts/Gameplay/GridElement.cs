using System;
using UnityEngine;

namespace OleksiiStepanov.UI
{
    public class GridElement : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer roadSpriteRenderer;
        [SerializeField] private SpriteRenderer boxSpriteRenderer;

        private Road _road;

        public bool IsOccupied { get; private set; }

        private bool _isActive;

        public static event Action<GridElement> OnSelect;
        
        private int _column;
        private int _row;

        public void Init()
        {
            
        }

        public void Activate(bool activate)
        {
            if (IsOccupied) return;

            boxSpriteRenderer.gameObject.SetActive(activate);

            _isActive = activate;
        }

        public void Occupy(Road road)
        {
            IsOccupied = true;
            
            _road = road;
            
            roadSpriteRenderer.gameObject.SetActive(true);
            roadSpriteRenderer.sprite = road.roadSprite;
        }

        private void OnMouseDown()
        {
            if (IsOccupied || !_isActive) return;
            
            Select();
        }

        public void SetGridPosition(int col, int r)
        {
            _column = col;
            _row = r;
        }

        public (int, int) GetGridPosition()
        {
            return (_column, _row);
        }

        public void Select()
        {
            OnSelect?.Invoke(this);
        }
    }
}
