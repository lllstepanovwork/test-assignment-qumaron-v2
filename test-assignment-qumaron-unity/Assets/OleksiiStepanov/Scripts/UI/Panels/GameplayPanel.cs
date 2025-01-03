using OleksiiStepanov.Game;
using OleksiiStepanov.Gameplay;
using TMPro;
using UnityEngine;

namespace OleksiiStepanov.UI
{
    public class GameplayPanel : UIPanel
    {
        [Header("Content")]
        [SerializeField] private TMP_Text buildTypeText; 
        
        public void OnBuildSmallBuildingButtonClicked()
        {
            GameplayManager.Instance.SetCreationType(CreationMode.Building2x2);
        }
        
        public void OnBuildMediumBuildingButtonClicked()
        {
            GameplayManager.Instance.SetCreationType(CreationMode.Building2x3);
        }
        
        public void OnBuildRoadButtonClicked()
        {
            GameplayManager.Instance.SetCreationType(CreationMode.Road);
        }
        
        public void OnResetButtonClicked()
        {
            GameplayManager.Instance.ResetAll();
        }

        public void SetCreationModeText(CreationMode creationMode)
        {
            buildTypeText.text = $"{Constants.BuildType} {creationMode}";
        }
    }    
}

