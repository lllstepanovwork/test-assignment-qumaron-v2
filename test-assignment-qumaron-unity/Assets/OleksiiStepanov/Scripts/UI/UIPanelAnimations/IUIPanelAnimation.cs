using System;

namespace OleksiiStepanov.UI
{
    public interface IUIPanelAnimation
    {
        public void Animate(UIPanel uIPanel, bool open, Action onComplete = null);
    }
}