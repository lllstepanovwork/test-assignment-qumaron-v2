using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace OleksiiStepanov.Gameplay
{
    public class CharacterAnimation : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        
        private const string IdleAnimationKey = "Idle";
        private const string WalkTopAnimationKey = "WalkTop";
        private const string WalkBottomAnimationKey = "WalkBottom";

        public void PlayIdleAnimation()
        {
            animator.Play(IdleAnimationKey, 0, 0);
        }

        public void PlayWalkingAnimationByTargetPosition(Vector3 targetPosition)
        {
            if (targetPosition.y < transform.position.y)
            {
                if (targetPosition.x < transform.position.x)
                {
                    WalkBottomLeft();
                }
                else
                {
                    WalkBottomRight();
                }
            }
            else
            {
                if (targetPosition.x < transform.position.x)
                {
                    WalkTopLeft();
                }
                else
                {
                    WalkTopRight();
                }
            } 
        }

        private void WalkTopLeft()
        {
            animator.Play(WalkTopAnimationKey, 0, 0);
            transform.localScale = new Vector3(1, 1, 1);
        }
        
        private void WalkTopRight()
        {
            animator.Play(WalkTopAnimationKey, 0, 0);
            transform.localScale = new Vector3(-1, 1, 1);
        }
        
        private void WalkBottomLeft()
        {
            animator.Play(WalkBottomAnimationKey, 0, 0);
            transform.localScale = new Vector3(1, 1, 1);
        }
        
        private void WalkBottomRight()
        {
            animator.Play(WalkBottomAnimationKey, 0, 0);
            transform.localScale = new Vector3(-1, 1, 1);
        }

    }    
}


