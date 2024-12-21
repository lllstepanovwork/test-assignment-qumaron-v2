using UnityEngine;

namespace OleksiiStepanov.UI
{
    public class SafeArea : MonoBehaviour
    {
        [Header("Content")]
        [SerializeField] private RectTransform rectTransform;
        
        private Rect _lastSafeArea = new Rect(0, 0, 0, 0);
        private Vector2 _lastScreenSize = Vector2.zero;

        private void OnRectTransformDimensionsChange()
        {
            Init();
        }

        public void Init()
        {
            if (rectTransform == null) return;

            // Get the current safe area and screen size
            Rect currentSafeArea = Screen.safeArea;
            Vector2 currentScreenSize = new Vector2(Screen.width, Screen.height);
            
            // Check if something has changed
            if (currentSafeArea == _lastSafeArea && currentScreenSize == _lastScreenSize) 
                return; // No changes, skip re-calculation

            _lastSafeArea = currentSafeArea;
            _lastScreenSize = currentScreenSize;

            // Convert Safe Area to local normalized coordinates (0 to 1)
            Vector2 anchorMin = _lastSafeArea.position;
            Vector2 anchorMax = _lastSafeArea.position + _lastSafeArea.size;

            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;

            // Apply Safe Area anchors to the RectTransform
            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
        }
    }
}

