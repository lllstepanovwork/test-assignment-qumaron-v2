using System;
using DG.Tweening;
using UnityEngine;

namespace OleksiiStepanov.UI
{
    public class FromBottomUIViewAnimation : IUIViewAnimation
    {
        public void Animate(UIPanel uIPanel, bool open, Action onComplete = null)
        {
            float startY = 0;
            float endY = 0;

            if (open)
            {
                startY = -20;
                endY = 0;
            }
            else
            {
                startY = 0;
                endY = -20;
            }

            uIPanel.contentTransform.position = new Vector3(0, startY, 0);
            uIPanel.contentTransform.DOMoveY(endY, 0.5f).OnComplete(() =>
            {
                onComplete?.Invoke();
            });
        }
    }
}