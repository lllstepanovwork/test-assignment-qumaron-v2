using UnityEngine;

namespace OleksiiStepanov.Gameplay
{
    public class PopEffect : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particles;
        [SerializeField] private ParticleSystemRenderer particlesRenderer;

        public void Init(Vector3 position, int sortingOrder)
        {
            transform.position = position;
            particlesRenderer.sortingOrder = sortingOrder;

            particles.Play();
        }

        public bool IsPlaying()
        {
            return particles.isPlaying;
        }
    }
}
