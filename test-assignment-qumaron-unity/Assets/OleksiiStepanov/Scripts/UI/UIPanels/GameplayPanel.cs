using OleksiiStepanov.Data;
using OleksiiStepanov.Gameplay;

namespace OleksiiStepanov.UI
{
    public class GameplayPanel : UIPanel
    {   
        public void OnBuildSmallBuildingButtonClicked()
        {
            GameplayManager.Instance.SetCreationMode(CreationMode.Building2x2);
        }
        
        public void OnBuildMediumBuildingButtonClicked()
        {
            GameplayManager.Instance.SetCreationMode(CreationMode.Building2x3);
        }
        
        public void OnBuildRoadButtonClicked()
        {
            GameplayManager.Instance.SetCreationMode(CreationMode.Road);
        }
        
        public async void OnResetButtonClicked()
        {
            await GameplayManager.Instance.ResetAll();
        }
    }    
}

