using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using OleksiiStepanov.Utils;
using OleksiiStepanov.Data;
using OleksiiStepanov.UI;

namespace OleksiiStepanov.Gameplay
{
    public class GameplayManager : SingletonBehaviour<GameplayManager>
    {   
        [Header("UI")]
        [SerializeField] private GameplayPanel gameplayPanel;
        
        [Header("Gameplay")]
        [SerializeField] private PanManager panManager;
        [SerializeField] private GridManager gridManager;
        [SerializeField] private RoadManager roadManager;
        [SerializeField] private BuildingManager buildingManager;
        [SerializeField] private CharacterManager characterManager;
        
        private CreationMode _currentCreationMode;
        
        public async void Init(Action onComplete)
        {
            await gridManager.Init(() =>
            {
                buildingManager.CreateStartingBuilding();

                InitPanManager();
                
                SetCreationMode(CreationMode.None);
                
                onComplete?.Invoke();   
            });
        }
        
        private void InitPanManager()
        {
            float gridBorderX = gridManager.GetGridBorderX();
            float gridBorderY = gridManager.GetGridBorderY();
                
            Vector2 worldBorder = new Vector2(gridBorderX, gridBorderY);
                
            panManager.Init(worldBorder);
        }

        public void SetCreationMode(CreationMode creationMode)
        {
            _currentCreationMode = creationMode;

            panManager.EnablePanning(false);
            buildingManager.ResetMode();
            roadManager.ConfirmRoads();
            
            switch (creationMode)
            {
                case CreationMode.None:
                    gridManager.ActivateGrid(false);
                    roadManager.Activate(false);
                    panManager.EnablePanning(true);
                    break;
                case CreationMode.Building2x2:
                    gridManager.ActivateGrid(true);
                    roadManager.Activate(false);
                    buildingManager.Activate(_currentCreationMode);
                    break;
                case CreationMode.Building2x3:
                    gridManager.ActivateGrid(true);
                    roadManager.Activate(false);
                    buildingManager.Activate(_currentCreationMode);
                    break;
                case CreationMode.Road:
                    gridManager.ActivateGrid(true, true);
                    roadManager.Activate(true);
                    break;
            }

            gameplayPanel.SetCreationMode(_currentCreationMode);
        }

        public async UniTask ResetAll()
        {
            roadManager.ResetAll();
            await gridManager.ResetAll();
            await buildingManager.ResetAll();
            await characterManager.ResetAll();
            await gridManager.BuildStartingRoads();
            
            buildingManager.CreateStartingBuilding();
            
            panManager.ResetPanning();
            
            _currentCreationMode = CreationMode.None;
            
            gameplayPanel.SetCreationMode(_currentCreationMode);
        }

        public void UndoLastRoadAction()
        {
            roadManager.UndoLastAction();
        }
        
        public void RedoLastRoadAction()
        {
            roadManager.RedoLastAction();
        }
    }    
}

