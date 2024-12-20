using System;

namespace OleksiiStepanov.UI
{
    public interface IUIViewAnimation
    {
        public void Animate(UIPanel uIPanel, bool open, Action onComplete = null);
    }
}