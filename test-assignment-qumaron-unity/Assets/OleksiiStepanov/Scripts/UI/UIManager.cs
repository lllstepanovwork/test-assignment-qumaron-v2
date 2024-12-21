using System;
using OleksiiStepanov.Utils;
using UnityEngine;

namespace OleksiiStepanov.UI
{
    public class UIManager : SingletonBehaviour<UIManager>
    {
        [Header("Content")] 
        [SerializeField] private SafeArea safeArea;
        [SerializeField] private GameplayPanel gameplayPanel;
        
        private readonly UIPanelOpener _uIPanelOpener = new UIPanelOpener();
        
        private UIPanel _currentUIPanel;

        public void Init(Action onComplete = null)
        {
            safeArea.Init();
            
            OpenGameplayPanel();
            
            onComplete?.Invoke();
        }

        private void OpenGameplayPanel()
        {
            _uIPanelOpener.OpenPanel(gameplayPanel);
        }
    }
}
