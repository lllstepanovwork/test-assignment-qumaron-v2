using UnityEngine;

namespace OleksiiStepanov.Gameplay
{
    public class PanManager : MonoBehaviour
    {
        [SerializeField] private Transform panTarget;
        [SerializeField] private float panSpeed = 0.5f;

        private Vector2 _panLimitMin = new Vector2(-10f, -10f);
        private Vector2 _panLimitMax = new Vector2(10f, 10f);

        private Vector3 _startPanPosition;
        private Vector3 _lastPanPosition;
        
        private bool _isEnabled;
        private bool _isPanning;
        
        public void Init(Vector2 worldLimit)
        {
            _panLimitMin = new Vector2(worldLimit.x, worldLimit.y);
            _panLimitMax = new Vector2(-worldLimit.x, -worldLimit.y);
            
            _startPanPosition = panTarget.transform.position;
        }

        public void EnablePanning(bool enabledStatus)
        {
            _isEnabled = enabledStatus;
        }

        private void Update()
        {
            if (!_isEnabled) return;
            if (panTarget == null)
            {
                Debug.LogWarning("Pan Target is not assigned!");
                return;
            }

            HandleMousePan();
        }

        private void HandleMousePan()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _lastPanPosition = Input.mousePosition;
                _isPanning = true;
            }

            if (Input.GetMouseButton(0) && _isPanning)
            {
                Vector3 delta = Input.mousePosition - _lastPanPosition;
                PanCamera(delta);
                _lastPanPosition = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(0))
            {
                _isPanning = false;
            }
        }

        private void PanCamera(Vector3 delta)
        {
            
            Vector3 panMovement = new Vector3(
                -delta.x / Screen.width,
                -delta.y / Screen.height,
                0
            ) * (panSpeed * Time.deltaTime * 100f);

            Vector3 newPosition = panTarget.position + panMovement;

            newPosition.x = Mathf.Clamp(newPosition.x, _panLimitMin.x, _panLimitMax.x);
            newPosition.y = Mathf.Clamp(newPosition.y, _panLimitMin.y, _panLimitMax.y);

            panTarget.position = newPosition;
        }

        public void ResetPanning()
        {
            panTarget.position = _startPanPosition;
        }
    }
}