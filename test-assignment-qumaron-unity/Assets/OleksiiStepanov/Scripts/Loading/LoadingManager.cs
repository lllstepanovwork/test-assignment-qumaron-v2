using System;
using System.Collections.Generic;
using OleksiiStepanov.Game;
using UnityEngine;

namespace OleksiiStepanov.Loading
{
    public class LoadingManager : MonoBehaviour
    {
        private List<LoadingStepBase> _loadingSteps;
        private LoadingStepBase CurrentLoadingStep { get; set; }

        public event Action<LoadingStep> LoadingStepCompleted;

        public static event Action LoadingCompleted;
        
        private int _currentLoadingStepIndex;

        private void Start()
        {
            Init();
        }

        public void Init()
        {
            _loadingSteps = new List<LoadingStepBase>
            {
                new LoadingStepAppInit(),
                new LoadingStepUIInit(),
                new LoadingStepComplete()
            };

            foreach (LoadingStepBase step in _loadingSteps)
            {
                step.Exited += GoToNextStep;
            }

            if (_loadingSteps != null && _loadingSteps.Count > 0)
            {
                SetCurrentLoadingStep(_loadingSteps[0]);
            }
            else
            {
                Debug.LogError("Loading steps list is null or empty");
                Application.Quit();
            }
        }

        public void GoToNextStep()
        {
            if (CurrentLoadingStep != null)
            {
                LoadingStepCompleted?.Invoke(CurrentLoadingStep.GetStepType());
            }

            _currentLoadingStepIndex++;

            if (_currentLoadingStepIndex < _loadingSteps.Count)
            {
                SetCurrentLoadingStep(_loadingSteps[_currentLoadingStepIndex]);
            }
            else
            {
                LoadingCompleted?.Invoke();
            }
        }

        private void SetCurrentLoadingStep(LoadingStepBase step)
        {
            Debug.Log($"{Constants.ConsoleMessageColorBlue}Loading: {step} step!{Constants.ConsoleMessageColorEnd}");
            CurrentLoadingStep = step;

            CurrentLoadingStep.Enter();
        }
    }
}