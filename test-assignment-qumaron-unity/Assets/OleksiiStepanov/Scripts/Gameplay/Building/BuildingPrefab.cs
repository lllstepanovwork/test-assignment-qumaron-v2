using UnityEngine;
using DG.Tweening;
using OleksiiStepanov.UI;

namespace OleksiiStepanov.Gameplay
{
    public class BuildingPrefab : MonoBehaviour
    {
        [Header("Content")]
        [SerializeField] private BuildingDrag buildingDrag;
        [SerializeField] private GameObject dragCollider;
        [SerializeField] private SpriteRenderer buildingSpriteRenderer;
        
        private Tweener _startingTweener;
        private Vector3 _startingPosition;
        
        public Building Building { get; private set; }
        
        public void Init(Building building)
        {
            Building = building;
            
            //buildingSpriteRenderer.sprite = building.buildingSprite;
            
            _startingPosition = buildingSpriteRenderer.transform.localPosition;
            float targetPosition = _startingPosition.y + 0.2f;
            _startingTweener = buildingSpriteRenderer.transform.DOLocalMoveY(targetPosition, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
            
            gameObject.SetActive(true);

            UIManager.Instance.ShowBuildingConfirmationDialog(transform);

            buildingDrag.Init();
        }

        public void Place(int sortingOrder)
        {
            _startingTweener?.Kill();
            buildingSpriteRenderer.transform.localPosition = _startingPosition;

            buildingSpriteRenderer.sortingOrder = sortingOrder; 
            
            dragCollider.SetActive(false);
        }

        public void Hide()
        {
            _startingTweener?.Kill();
            buildingSpriteRenderer.transform.localPosition = _startingPosition;
            
            gameObject.SetActive(false);
        }
    }    
    
    public enum BuildingType
    {
        None,
        Building2x2,
        Building2x3,
    }
}

