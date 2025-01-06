using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "ScriptableObjects/Building", order = 2)]
public class BuildingSO : ScriptableObject
{
    public Sprite buildingSprite;
    public Vector2Int size;
}
