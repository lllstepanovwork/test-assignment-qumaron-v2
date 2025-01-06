using UnityEngine;
using UnityEngine.Serialization;

namespace OleksiiStepanov.Gameplay
{
    public class FXManager : MonoBehaviour
    {
        [SerializeField] private PopEffectManager popEffectManager;

        private void OnEnable()
        {
            CharacterManager.OnCharacterSpawn += OnCharacterSpawn;
        }

        private void OnDisable()
        {
            CharacterManager.OnCharacterSpawn -= OnCharacterSpawn;
        }
    
        private void OnCharacterSpawn(Character character)
        {
            CreatePopEffect(character);
        }
    
        private void CreatePopEffect(Character character)
        {
            popEffectManager.Create(character.transform.position, character.CurrentSortingOrder);
        }
    }
}

