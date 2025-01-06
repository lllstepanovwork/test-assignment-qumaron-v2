using UnityEngine;
using DG.Tweening;

namespace OleksiiStepanov.Gameplay
{
    public class BuildingPrefab : MonoBehaviour
    {
        [Header("Content")]
        [SerializeField] private BuildingDrag buildingDrag;
        [SerializeField] private SpriteRenderer buildingSpriteRenderer;
        
        private Tweener _startingTweener;
        private Vector3 _startingPosition;
        
        public BuildingSO BuildingSo { get; private set; }
        
        private GridManager _gridManager;
        
        public void Init(BuildingSO buildingSo, GridManager gridManager)
        {
            BuildingSo = buildingSo;
            
            _gridManager = gridManager;
            
            buildingSpriteRenderer.sprite = buildingSo.buildingSprite;
            
            _startingPosition = buildingSpriteRenderer.transform.localPosition;
            float targetPosition = _startingPosition.y + 0.2f;
            _startingTweener = buildingSpriteRenderer.transform.DOLocalMoveY(targetPosition, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
            
            gameObject.SetActive(true);
            
            buildingDrag.Init(buildingSo.size, gridManager);
        }

        public bool Place()
        {
            if (buildingDrag.IsPlaceable)
            {
                _startingTweener?.Kill();
                buildingSpriteRenderer.transform.localPosition = _startingPosition;

                buildingDrag.Deactivate();

                buildingSpriteRenderer.sortingOrder = buildingDrag.LastElement.SortingLayerOrder;
                
                _gridManager.OccupyGridElementWithPattern(buildingDrag.LastElement, BuildingSo.size);

                return true;
            }

            return false;
        }

        public void Hide()
        {
            _startingTweener?.Kill();
            buildingSpriteRenderer.transform.localPosition = _startingPosition;
            
            buildingDrag.Deactivate();
            gameObject.SetActive(false);
        }
    }    
}

