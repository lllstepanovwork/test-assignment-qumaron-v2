using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace OleksiiStepanov.Gameplay
{
    public class CharacterMovement : MonoBehaviour
    {
        [Header("Content")]
        [SerializeField] private CharacterAnimation characterAnimation;
        
        public GridElement LastElement { get; private set; }

        public static event Action<CharacterMovement> OnPathCompleted;
        
        private Stack<GridElement> _currentPath;
        
        public void Init(GridElement startElement)
        {
            transform.position = startElement.transform.position;
            LastElement = startElement;
        }

        public void SetNewPath(Stack<GridElement> newPath)
        {
            _currentPath = newPath;

            if (newPath.Count > 0)
            {
                GridElement gridElement = _currentPath.Pop();
                
                MoveTo(gridElement.transform.position);
            }
        }

        private void MoveTo(Vector3 targetPosition)
        {
            characterAnimation.PlayWalkingAnimationByTargetPosition(targetPosition);
            
            transform.DOMove(targetPosition, 2f).SetEase(Ease.Linear).onComplete = (() =>
            {
                if (_currentPath.Count != 0)
                {
                    GridElement gridElement = _currentPath.Pop();
                    
                    LastElement = gridElement;
                
                    MoveTo(gridElement.transform.position);
                }
                else
                {
                    characterAnimation.PlayIdleAnimation();
         
                    Invoke(nameof(CompletePath), 2f);
                }
            });
        }

        private void CompletePath()
        {   
            OnPathCompleted?.Invoke(this);
        }
    }    
}


