using System;
using System.Collections.Generic;
using OleksiiStepanov.Gameplay;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class CharacterManager : MonoBehaviour
{
    [Header("Character Settings")]
    [SerializeField] private Character characterPrefab;
    [SerializeField] private Transform characterHolder;
    public static event Action<Character> OnCharacterSpawn;
    
    private readonly List<Character> _currentCharacters = new List<Character>();

    private void SpawnCharacter()
    {
        Character character = GetCharacter();
        
        OnCharacterSpawn?.Invoke(character);
    }

    private Character GetCharacter()
    {
        foreach (Character character in _currentCharacters)
        {
            if (!character.Active)
            {
                return character; 
            }
        }
        
        Character newCharacter = Instantiate(characterPrefab, characterHolder);
        _currentCharacters.Add(newCharacter);
        
        return newCharacter;
    }

    public async UniTask ResetAll()
    {
        foreach (var character in _currentCharacters)
        {
            character.ResetCharacter();
        }
        
        await UniTask.Yield();
    }

    private void OnEnable()
    {
        BuildingManager.OnNewBuildingPlaced += SpawnCharacter;
    }
    
    private void OnDisable()
    {
        BuildingManager.OnNewBuildingPlaced -= SpawnCharacter;
    }
}
