using UnityEngine;

namespace OleksiiStepanov.UI
{
    public class SafeArea : MonoBehaviour
    {
        [Header("Content")]
        [SerializeField] private RectTransform rectTransform;
        
        private Rect _lastSafeArea = new Rect(0, 0, 0, 0);
        
        public void Init()
        {
            _lastSafeArea = Screen.safeArea;

            Vector2 anchorMin = _lastSafeArea.position;
            Vector2 anchorMax = _lastSafeArea.position + _lastSafeArea.size;

            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;

            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
        }
    }
}

