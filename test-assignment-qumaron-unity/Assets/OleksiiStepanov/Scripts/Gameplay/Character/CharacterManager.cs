using System;
using System.Collections.Generic;
using OleksiiStepanov.Gameplay;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class CharacterManager : MonoBehaviour
{
    [Header("Character Settings")]
    [SerializeField] private CharacterMovement characterPrefab;
    [SerializeField] private Transform characterHolder;

    public static event Action<CharacterMovement> OnCharacterSpawned;
    
    private readonly List<CharacterMovement> _currentCharacters = new List<CharacterMovement>();
    
    public void Init()
    {
        SpawnCharacter();
    }

    private void SpawnCharacter()
    {
        CharacterMovement character = Instantiate(characterPrefab, characterHolder);
        _currentCharacters.Add(character);
        
        OnCharacterSpawned?.Invoke(character);
    }
    
    public async UniTask ResetAll()
    {
        foreach (var character in _currentCharacters)
        {
            Destroy(character.gameObject);
        }
        
        await UniTask.Yield();
    }

    private void OnEnable()
    {
        BuildingManager.OnBuildingPlaced += SpawnCharacter;
    }
    
    private void OnDisable()
    {
        BuildingManager.OnBuildingPlaced -= SpawnCharacter;
    }
}
