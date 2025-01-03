using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using OleksiiStepanov.Utils;
using OleksiiStepanov.Game;
using OleksiiStepanov.UI;

namespace OleksiiStepanov.Gameplay
{
    public class GameplayManager : SingletonBehaviour<GameplayManager>
    {
        [Header("UI")]
        [SerializeField] private GameplayPanel gameplayPanel;
        
        [Header("Gameplay")]
        [SerializeField] private GridManager gridManager;
        [SerializeField] private RoadManager roadManager;
        [SerializeField] private BuildingManager buildingManager;
        [SerializeField] private CharacterManager characterManager;
        
        private CreationMode _currentCreationMode;
        
        public void Init(Action onComplete)
        {
            gridManager.Init(() =>
            {
                characterManager.Init();
                
                onComplete?.Invoke();   
            });
        }

        public void SetCreationType(CreationMode creationMode)
        {
            if (_currentCreationMode == creationMode)
            {
                creationMode = CreationMode.None;
            }
            
            _currentCreationMode = creationMode;

            HandleCreationModeChanged(creationMode);
        }

        private void HandleCreationModeChanged(CreationMode creationMode)
        {
            switch (creationMode)
            {
                case CreationMode.None:
                    gridManager.ActivateGrid(false);
                    buildingManager.Deactivate();
                    roadManager.Activate(false);
                    break;
                case CreationMode.Building2x2:
                    buildingManager.Activate(_currentCreationMode);
                    gridManager.ActivateGrid(true);
                    roadManager.Activate(false);
                    break;
                case CreationMode.Building2x3:
                    buildingManager.Activate(_currentCreationMode);
                    gridManager.ActivateGrid(true);
                    roadManager.Activate(false);
                    break;
                case CreationMode.Road:
                    buildingManager.Deactivate();
                    gridManager.ActivateGrid(true, true);
                    roadManager.Activate(true);
                    break;
            }
            
            gameplayPanel.SetCreationModeText(_currentCreationMode);
        }

        public async UniTask ResetAll()
        {
            await gridManager.ResetAll();
            await buildingManager.ResetAll();
            await characterManager.ResetAll();
            await gridManager.BuildStartingRoads();
        }

        private void OnEnable()
        {
            BuildingConfirmationDialog.OnConfirm += BuildingManagerOnOnBuildingPlaced;
            BuildingConfirmationDialog.OnDeny += BuildingManagerOnOnBuildingPlaced;
        }

        private void OnDisable()
        {
            BuildingConfirmationDialog.OnConfirm -= BuildingManagerOnOnBuildingPlaced;
            BuildingConfirmationDialog.OnDeny -= BuildingManagerOnOnBuildingPlaced;
        }

        private void BuildingManagerOnOnBuildingPlaced()
        {
            SetCreationType(CreationMode.None);
        }
    }    
}

