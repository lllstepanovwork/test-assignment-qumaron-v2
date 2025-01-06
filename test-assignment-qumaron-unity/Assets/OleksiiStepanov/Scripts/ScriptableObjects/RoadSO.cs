using UnityEngine;
using OleksiiStepanov.Data;

namespace OleksiiStepanov.Gameplay
{
    [CreateAssetMenu(fileName = "Road", menuName = "ScriptableObjects/Road", order = 1)]
    public class RoadSO : ScriptableObject
    {
        public Sprite roadSprite;
        public RoadType roadType;
    }   
}
