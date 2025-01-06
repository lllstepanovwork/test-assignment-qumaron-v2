using System;
using OleksiiStepanov.Gameplay;
using OleksiiStepanov.Utils;
using UnityEngine;

namespace OleksiiStepanov.UI
{
    public class UIManager : SingletonBehaviour<UIManager>
    {
        [Header("Content")] 
        [SerializeField] private SafeArea safeArea;
        [SerializeField] private GameplayPanel gameplayPanel;
        [SerializeField] private BuildingConfirmationDialog buildingConfirmationDialog;
        
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

        public void ShowBuildingConfirmationDialog(Func<bool> onConfirm, Action onDeny,Transform target)
        {
            buildingConfirmationDialog.gameObject.SetActive(true);
            buildingConfirmationDialog.Init(onConfirm, onDeny,target);
        }
        
        public void HideBuildingConfirmationDialog()
        {
            buildingConfirmationDialog.gameObject.SetActive(false);
        }
    }
}
