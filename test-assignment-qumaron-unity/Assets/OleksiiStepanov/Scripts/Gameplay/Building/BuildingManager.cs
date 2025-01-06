using System;
using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using OleksiiStepanov.Data;
using OleksiiStepanov.UI;

namespace OleksiiStepanov.Gameplay
{
    public class BuildingManager : MonoBehaviour
    {
        [Header("Content")]
        [SerializeField] private GridManager gridManager;
        
        [Header("Settings")]
        [SerializeField] private List<BuildingSO> buildings;
        [SerializeField] private Transform buildingHolder;
        
        [SerializeField] private BuildingPrefab buildingPrefab;
        
        private readonly List<BuildingPrefab> _activeBuildings = new List<BuildingPrefab>();
        private readonly List<BuildingPrefab> _buildingPool = new List<BuildingPrefab>();

        private BuildingPrefab _activeBuildingPrefab;
        
        public static event Action OnNewBuildingPlaced;

        public void CreateStartingBuilding()
        {
            CreateBuildingPrefab(new Vector2Int(2,2), false, () =>
            {
                TryToPlaceBuilding();                
            });
        }

        public void Activate(CreationMode creationMode)
        {
            Vector2Int size;
            
            switch (creationMode)
            {
                case CreationMode.Building2x2:
                    size = new Vector2Int(2,2);
                    CreateBuildingPrefab(size);
                    break;
                case CreationMode.Building2x3:
                    size = new Vector2Int(2,3);
                    CreateBuildingPrefab(size);
                    break;
            }
        }

        private void CreateBuildingPrefab(Vector2Int size, bool withConfirmation = true, Action onComplete = null)
        {
            BuildingSO buildingSo = GetBuildingBySize(size);

            if (buildingSo == null)
            {
                Debug.LogError("Wrong building type!");
                return;
            }

            BuildingPrefab newBuilding = GetBuildingBySizeFromThePool(size);
            
            newBuilding.Init(buildingSo, gridManager);
            
            _activeBuildingPrefab = newBuilding;

            if (withConfirmation)
            {
                UIManager.Instance.ShowBuildingConfirmationDialog(TryToPlaceBuilding, OnBuildingDenied, newBuilding.transform);    
            }
            
            onComplete?.Invoke();
        }

        private BuildingPrefab GetBuildingBySizeFromThePool(Vector2Int size)
        {
            foreach (var buildingInPool in _buildingPool)
            {
                if (buildingInPool.BuildingSo.size == size)
                {
                    _buildingPool.Remove(buildingInPool);
                    
                    return buildingInPool;
                }
            }
            
            BuildingPrefab newBuildingPrefab = Instantiate(buildingPrefab, buildingHolder);
            
            return newBuildingPrefab;
        }

        private BuildingSO GetBuildingBySize(Vector2Int size)
        {
            return buildings.Find(building => building.size == size);
        }

        private bool TryToPlaceBuilding()
        {
            if (!_activeBuildingPrefab.Place())
            {
                Debug.Log("Building placement failed.");
                return false;
            }
    
            _activeBuildings.Add(_activeBuildingPrefab);
            
            OnNewBuildingPlaced?.Invoke();
            
            _activeBuildingPrefab = null;

            return true;
        }
        
        private void OnBuildingDenied()
        {
            if (_activeBuildingPrefab == null) return;
            
            _activeBuildingPrefab.Hide();
            _buildingPool.Add(_activeBuildingPrefab);

            _activeBuildingPrefab = null;
        }

        public void ResetMode()
        {
            OnBuildingDenied();
            
            UIManager.Instance.HideBuildingConfirmationDialog();
        }

        public async UniTask ResetAll()
        {
            ResetMode();

            foreach (var building in _activeBuildings)
            {
                _buildingPool.Add(building);
                building.Hide();
            }
            
            _activeBuildings.Clear();
            
            await UniTask.Yield();
        }
    }
}
