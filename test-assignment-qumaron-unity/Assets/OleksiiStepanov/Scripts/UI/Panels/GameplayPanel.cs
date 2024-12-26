using TMPro;
using UnityEngine;

namespace OleksiiStepanov.UI
{
    public class GameplayPanel : UIPanel
    {
        [SerializeField] private GridManager gridManager;
        
        public void Init()
        {
            gridManager.Init();
        }

        public void OnBuildSmallBuildingButtonClicked()
        {
            
        }
        
        public void OnBuildMediumBuildingButtonClicked()
        {
            
        }
        
        public void OnBuildRoadButtonClicked()
        {
            if (!gridManager.IsGridActive)
            {
                gridManager.ActivateGrid();
            }
            else
            {
                gridManager.DeactivateGrid();
            }
        }
    }    
}

