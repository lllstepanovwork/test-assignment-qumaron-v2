using UnityEngine;

namespace OleksiiStepanov.Gameplay
{
    public class PopEffect : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particleSystem;
        [SerializeField] private ParticleSystemRenderer particleSystemRenderer;

        public void Init(Vector3 position, int sortingOrder)
        {
            transform.position = position;
            particleSystemRenderer.sortingOrder = sortingOrder;

            particleSystem.Play();
        }

        public bool IsPlaying()
        {
            return particleSystem.isPlaying;
        }
    }
}
