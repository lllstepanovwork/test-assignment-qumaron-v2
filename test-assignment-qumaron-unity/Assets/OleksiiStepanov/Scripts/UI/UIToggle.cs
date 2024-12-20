using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using System;

namespace OleksiiStepanov.UI
{
    public class UIToggle : MonoBehaviour
    {
        [Header("Colors")]
        [SerializeField] private Button toggleButton;
        [SerializeField] private Image toggleButtonImage;
        [SerializeField] private RectTransform circleImageRectTransform;

        [Header("Colors")]
        [SerializeField] private Color enabledColor;
        [SerializeField] private Color disabledColor;

        public event Action<bool> OnValueChanged;

        private const float LeftPositionX = -25;
        private const float RightPositionX = 25;

        private bool _active = true;

        public void OnButtonClick()
        {
            toggleButton.interactable = false;

            float xPosition = 0;
            Color color;

            if (_active)
            {
                _active = false;

                color = enabledColor;
                xPosition = LeftPositionX;
            }
            else
            {
                _active = true;

                color = enabledColor;
                xPosition = RightPositionX;
            }

            Sequence sequence = DOTween.Sequence();

            sequence
                .Append(circleImageRectTransform.DOLocalMoveX(xPosition, 0.2f))
                .Join(toggleButtonImage.DOColor(color, 0.2f))
                .AppendCallback(()=>
                {
                    toggleButton.interactable = true;

                    OnValueChanged?.Invoke(_active);
                });

        }
    }
}
