using System.Collections.Generic;
using UnityEngine;

namespace OleksiiStepanov.Gameplay
{
    public class PopEffectManager : MonoBehaviour
    {
        [SerializeField] private PopEffect popEffectPrefab;
        [SerializeField] private List<PopEffect> popEffects = new List<PopEffect>();
        
        public void Create(Vector3 position, int sortingLayerOrderPosition)
        {
            PopEffect popEffect = GetPopEffect();
            
            popEffect.gameObject.SetActive(true);
            popEffect.Init(position, sortingLayerOrderPosition);
        }

        private PopEffect GetPopEffect()
        {
            foreach (var popEffect in popEffects)
            {
                if (!popEffect.IsPlaying())
                {
                    return popEffect;
                }
            } 
            
            PopEffect newPopEffect = Instantiate(popEffectPrefab);
            popEffects.Add(newPopEffect);
            
            return newPopEffect;
        }
    }    
}

