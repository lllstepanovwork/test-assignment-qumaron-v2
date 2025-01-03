using OleksiiStepanov.Gameplay;
using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "ScriptableObjects/Building", order = 2)]
public class Building : ScriptableObject
{
    public Sprite buildingSprite;
    public BuildingType buildingType;
    public BuildingPrefab buildingPrefab;
}
