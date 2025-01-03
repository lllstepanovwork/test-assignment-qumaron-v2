using System;
using UnityEngine;

namespace OleksiiStepanov.Gameplay
{
    public class BuildingConfirmationDialog : MonoBehaviour
    {
        public static event Action OnConfirm;
        public static event Action OnDeny;

        [SerializeField] private Transform contentTransform;
        
        private Transform _targetTransform;
        private readonly Vector3 _targetOffset = new Vector3(0, 2f, 0);
        
        public void Init(Transform target)
        {
            _targetTransform = target;
        }

        private void Update()
        {
            if (_targetTransform == null) return;
            
            contentTransform.position = _targetTransform.position + _targetOffset;
        }

        public void OnConfirmButtonClicked()
        {
            _targetTransform = null;
            
            gameObject.SetActive(false);
            
            OnConfirm?.Invoke();
        }
        
        public void OnDenyButtonClicked()
        {
            _targetTransform = null;
            gameObject.SetActive(false);

            OnDeny?.Invoke();
        }
    }    
}

