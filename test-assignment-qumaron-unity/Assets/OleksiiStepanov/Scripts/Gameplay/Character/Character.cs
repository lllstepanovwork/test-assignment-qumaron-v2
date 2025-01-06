using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace OleksiiStepanov.Gameplay
{
    public class Character : MonoBehaviour
    {
        [Header("Content")]
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private CharacterAnimation characterAnimation;
        
        public bool Active { get; private set; }
        public GridElement CurrentElement { get; private set; }
        
        public int CurrentSortingOrder { get; private set; }
        public static event Action<Character> OnPathCompleted;
        
        private Stack<GridElement> _currentPath = new Stack<GridElement>();
        
        private Tweener _moveTweener;
        
        private Vector3 _spawnPosition;
        
        public void Init(GridElement startElement)
        {   
            gameObject.SetActive(true);
            
            Active = true;
            
            _spawnPosition = startElement.transform.position;
            transform.position = _spawnPosition;
            
            CurrentElement = startElement;

            CompletePath();
        }

        public void SetNewPath(Stack<GridElement> newPath)
        {
            _currentPath = newPath;

            if (_currentPath.Count > 0)
            {
                GridElement gridElement = _currentPath.Pop();
                
                MoveTo(gridElement);
            }
        }

        private void MoveTo(GridElement gridElement)
        {
            CurrentSortingOrder = gridElement.SortingLayerOrder + 1;
            
            spriteRenderer.sortingOrder = CurrentSortingOrder;
            
            characterAnimation.PlayWalkingAnimationByTargetPosition(gridElement.transform.position);
            
            _moveTweener = transform.DOMove(gridElement.transform.position, 2f).SetEase(Ease.Linear);
            _moveTweener.onComplete = () =>
                {
                    if (_currentPath.Count > 0)
                    {
                        GridElement nextGridElement = _currentPath.Pop();

                        MoveTo(nextGridElement);
                    }
                    else
                    {
                        CurrentElement = gridElement;
                        
                        characterAnimation.PlayIdleAnimation();

                        CompletePath();
                    }
                };
        }

        private void CompletePath()
        {   
            OnPathCompleted?.Invoke(this);
        }

        public void ResetCharacter()
        {
            _moveTweener?.Kill();
            
            transform.position = _spawnPosition;
            
            CurrentElement = null;
            
            Active = false;
            
            _currentPath.Clear();
            
            gameObject.SetActive(false);
        }
    }    
}


