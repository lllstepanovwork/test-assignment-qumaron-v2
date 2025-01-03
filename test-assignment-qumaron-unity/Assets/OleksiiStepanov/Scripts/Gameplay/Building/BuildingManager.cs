using System;
using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using OleksiiStepanov.Game;

namespace OleksiiStepanov.Gameplay
{
    public class BuildingManager : MonoBehaviour
    {
        [Header("Content")]
        [SerializeField] private GridManager gridManager;
        
        [Header("Settings")]
        [SerializeField] private List<Building> buildings;
        [SerializeField] private Transform buildingHolder;
        
        [SerializeField] private List<BuildingPrefab> _activeBuildings = new List<BuildingPrefab>();
        [SerializeField] private List<BuildingPrefab> _buildingPool = new List<BuildingPrefab>();

        private BuildingPrefab _activeBuildingPrefab;
        private BuildingType _activeBuildingType = BuildingType.None;
        
        public static event Action OnBuildingPlaced;

        private int _sortingOrder = 0;
        
        private void OnEnable()
        {
            BuildingConfirmationDialog.OnConfirm += OnBuildingConfirmed;
            BuildingConfirmationDialog.OnDeny += OnBuildingDenied;
        }

        private void OnDisable()
        {
            BuildingConfirmationDialog.OnConfirm -= OnBuildingConfirmed;
            BuildingConfirmationDialog.OnDeny -= OnBuildingDenied;
        }
        
        public void Activate(CreationMode creationMode)
        {
            switch (creationMode)
            {
                case CreationMode.Building2x2:
                    _activeBuildingType = BuildingType.Building2x2;
                    CreateBuilding(BuildingType.Building2x2);
                    break;
                case CreationMode.Building2x3:
                    _activeBuildingType = BuildingType.Building2x3;
                    CreateBuilding(BuildingType.Building2x3);
                    break;
            }
        }

        public void Deactivate()
        {
            if (_activeBuildingPrefab != null)
            {
                OnBuildingDenied();    
            }
        }

        private void CreateBuilding(BuildingType buildingType)
        {
            Building building = GetBuildingByType(buildingType);

            if (building == null)
            {
                Debug.LogError("Wrong building type!");
                return;
            }

            BuildingPrefab newBuilding = GetBuildingByTypeFromThePool(buildingType);
            
            if (newBuilding == null)
            {
                newBuilding = Instantiate(building.buildingPrefab, buildingHolder);
            }
            
            newBuilding.Init(building);
            newBuilding.transform.position = gridManager.MiddleGridElement.transform.position - gridManager.MiddleGridElement.Neighbors.BottomElement.transform.position;
            
            _activeBuildingPrefab = newBuilding;
        }

        private BuildingPrefab GetBuildingByTypeFromThePool(BuildingType buildingType)
        {
            foreach (var buildingPrefab in _buildingPool)
            {
                if (buildingPrefab.Building.buildingType == buildingType)
                {
                    _buildingPool.Remove(buildingPrefab);
                    return buildingPrefab;
                }
            }
            
            return null;
        }

        private Building GetBuildingByType(BuildingType buildingType)
        {
            return buildings.Find(building => building.buildingType == buildingType);
        }

        private void OnBuildingConfirmed()
        {
            _sortingOrder--;
            
            _activeBuildingPrefab.Place(_sortingOrder);
            _activeBuildings.Add(_activeBuildingPrefab);
            
            _activeBuildingPrefab = null;
            
            OnBuildingPlaced?.Invoke();
        }
        
        private void OnBuildingDenied()
        {
            _activeBuildingPrefab.Hide();
            _buildingPool.Add(_activeBuildingPrefab);
            
            _activeBuildingType = BuildingType.None;
            
            _activeBuildingPrefab = null;
        }

        public async UniTask ResetAll()
        {
            if (_activeBuildingPrefab != null)
            {
                _activeBuildingPrefab.Hide();
                _buildingPool.Add(_activeBuildingPrefab);
                
                _activeBuildingPrefab = null;
            }

            foreach (var building in _activeBuildings)
            {
                building.Hide();
                _buildingPool.Add(building);
            }
            
            await UniTask.Yield();
        }
    }
}
