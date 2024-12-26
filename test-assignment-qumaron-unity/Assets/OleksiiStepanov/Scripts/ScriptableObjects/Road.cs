using UnityEngine;

[CreateAssetMenu(fileName = "Road", menuName = "ScriptableObjects/Road", order = 1)]
public class Road : ScriptableObject
{
    public Sprite roadSprite;
    public RoadType roadType;
}

public enum RoadType
{
    Left,
    Right,
    CrossX,
    CrossTTop,
    CrossTRight,
    CrossTBottom,
    CrossTLeft,
    TurnTop,
    TurnBottom,
    TurnLeft,
    TurnRight,
}
