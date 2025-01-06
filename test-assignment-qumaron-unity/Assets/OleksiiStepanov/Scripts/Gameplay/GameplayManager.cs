using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using OleksiiStepanov.Utils;
using OleksiiStepanov.Data;

namespace OleksiiStepanov.Gameplay
{
    public class GameplayManager : SingletonBehaviour<GameplayManager>
    {   
        [Header("Gameplay")]
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
                
                onComplete?.Invoke();   
            });
        }

        public void SetCreationMode(CreationMode creationMode)
        {
            if (_currentCreationMode == creationMode)
            {
                creationMode = CreationMode.None;
            }
            
            _currentCreationMode = creationMode;

            HandleCreationModeChange(creationMode);
        }

        private void HandleCreationModeChange(CreationMode creationMode)
        {
            buildingManager.ResetMode();
            
            switch (creationMode)
            {
                case CreationMode.None:
                    gridManager.ActivateGrid(false);
                    roadManager.Activate(false);
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
        }

        public async UniTask ResetAll()
        {
            await gridManager.ResetAll();
            await buildingManager.ResetAll();
            await characterManager.ResetAll();
            await gridManager.BuildStartingRoads();
            
            buildingManager.CreateStartingBuilding();
            
            _currentCreationMode = CreationMode.None;
        }
    }    
}

