using System;
using DG.Tweening;
using UnityEngine;

namespace OleksiiStepanov.UI
{
    public class UIPanelAnimationFromBottom : IUIPanelAnimation
    {
        private const float OffScreenYPosition = -20f;
        private const float Duration = 0.5f;

        public void Animate(UIPanel uIPanel, bool open, Action onComplete = null)
        {
            float startY;
            float endY;

            if (open)
            {
                startY = OffScreenYPosition;
                endY = 0;
            }
            else
            {
                startY = 0;
                endY = OffScreenYPosition;
            }

            uIPanel.contentTransform.position = new Vector3(0, startY, 0);
            uIPanel.contentTransform.DOMoveY(endY, Duration).OnComplete(() =>
            {
                onComplete?.Invoke();
            });
        }
    }
}