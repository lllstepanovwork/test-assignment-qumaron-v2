using UnityEngine;
using UnityEngine.SceneManagement;
using OleksiiStepanov.Game;
using OleksiiStepanov.Loading;

namespace OleksiiStepanov.App
{
    public class GameStarter : MonoBehaviour
    {
        [SerializeField] private GameObject overlay;
        
        private void OnEnable()
        {
            LoadingManager.LoadingCompleted += OnLoadingCompleted;
        }

        private void OnDisable()
        {
            LoadingManager.LoadingCompleted -= OnLoadingCompleted;
        }

        public void Start()
        {
            overlay.SetActive(true);

            if (!SceneManager.GetSceneByName(Constants.MainSceneName).isLoaded)
            {
                SceneManager.LoadSceneAsync(Constants.MainSceneName, LoadSceneMode.Additive)!.completed += (obj) =>
                {
                    Debug.Log($"{Constants.ConsoleMessageColorBlue}Main Scene Loaded {Constants.ConsoleMessageColorEnd}");
                };
            }
        }

        private void OnLoadingCompleted()
        {
            overlay.SetActive(false);
        }
    }
}