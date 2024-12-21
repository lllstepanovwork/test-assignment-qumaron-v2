using System;

namespace OleksiiStepanov.UI
{
    public class UIPanelOpener
    {
        public void OpenPanel(UIPanel uIPanel, Action onComplete = null)
        {
            switch (uIPanel.animationType)
            {
                case UIViewAnimationType.FromBottom:
                    IUIViewAnimation UIPanelAnimation = new FromBottomUIViewAnimation();

                    uIPanel.gameObject.SetActive(true);
                    UIPanelAnimation.Animate(uIPanel, true, () =>
                    {
                        onComplete?.Invoke();
                    });
                    break;
                case UIViewAnimationType.None:
                    uIPanel.gameObject.SetActive(true);
                    onComplete?.Invoke();
                    break;
            }
        }

        public void ClosePanel(UIPanel uIPanel, Action onComplete = null)
        {
            switch (uIPanel.animationType)
            {
                case UIViewAnimationType.FromBottom:
                    IUIViewAnimation UIPanelAnimation = new FromBottomUIViewAnimation();

                    UIPanelAnimation.Animate(uIPanel, false, () =>
                    {
                        uIPanel.gameObject.SetActive(false);
                        onComplete?.Invoke();
                    });
                    break;
                case UIViewAnimationType.None:
                    uIPanel.gameObject.SetActive(false);
                    onComplete?.Invoke();
                    break;
            }
        }
    }
}


