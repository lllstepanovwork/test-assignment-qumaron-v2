using UnityEngine;
using OleksiiStepanov.Game;

[CreateAssetMenu(fileName = "Road", menuName = "ScriptableObjects/Road", order = 1)]
public class Road : ScriptableObject
{
    public Sprite roadSprite;
    public RoadType roadType;
}
