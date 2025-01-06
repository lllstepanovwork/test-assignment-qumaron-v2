using System;
using OleksiiStepanov.Data;
using UnityEngine;

namespace OleksiiStepanov.Gameplay
{
    public class BuildingConfirmationDialog : MonoBehaviour
    {
        [SerializeField] private Transform contentTransform;
        
        private Transform _targetTransform;
        private readonly Vector3 _targetOffset = new Vector3(0, 2f, 0);
        
        private Func<bool> _confirmAction;
        private Action _denyAction;
        
        public void Init(Func<bool> onConfirm, Action onDeny, Transform target)
        {
            _confirmAction = onConfirm;
            _denyAction = onDeny;
            
            _targetTransform = target;
        }

        public void OnConfirmButtonClicked()
        {
            bool shouldClose = _confirmAction?.Invoke() ?? true;
    
            if (shouldClose)
            {
                _targetTransform = null;
                gameObject.SetActive(false);
                
                GameplayManager.Instance.SetCreationMode(CreationMode.None);
            }
        }
        
        public void OnDenyButtonClicked()
        {
            _targetTransform = null;
            gameObject.SetActive(false);
            
            _denyAction?.Invoke();
            
            GameplayManager.Instance.SetCreationMode(CreationMode.None);
        }
        
        private void Update()
        {
            if (_targetTransform == null) return;
            
            contentTransform.position = _targetTransform.position + _targetOffset;
        }
    }    
}

