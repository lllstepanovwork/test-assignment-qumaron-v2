using DG.Tweening;
using OleksiiStepanov.Data;
using OleksiiStepanov.Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace OleksiiStepanov.UI
{
    public class GameplayPanel : UIPanel
    {
        [Header("Content")]
        [SerializeField] private Button redoButton;
        [SerializeField] private Button undoButton;

        [Header("TipPanel")]
        [SerializeField] private CanvasGroup tipPanelCanvasGroup;
        [SerializeField] private TMP_Text tipText;

        private CreationMode _creationMode;
        
        public void SetCreationMode(CreationMode creationMode)
        {
            _creationMode = creationMode;
            
            ShowUndoRedoButtons(false);
            
            switch (creationMode)
            {
                case CreationMode.None:
                    ShowTip(TipType.Pan);
                    break;
                case CreationMode.Building2x2:
                    ShowTip(TipType.Drag);
                    break;
                case CreationMode.Building2x3:
                    ShowTip(TipType.Drag);
                    break;
                case CreationMode.Road:
                    ShowTip(TipType.Tap);
                    ShowUndoRedoButtons(true);
                    break;
            }
        }

        private void ShowUndoRedoButtons(bool show)
        {
            redoButton.gameObject.SetActive(show);
            undoButton.gameObject.SetActive(show);
            
            redoButton.interactable = false;
            undoButton.interactable = false;
        }

        public void OnBuildSmallBuildingButtonClicked()
        {
            if (_creationMode == CreationMode.Building2x2)
            {
                GameplayManager.Instance.SetCreationMode(CreationMode.None);
            }
            else
            {
                GameplayManager.Instance.SetCreationMode(CreationMode.Building2x2);
            }
        }
        
        public void OnBuildMediumBuildingButtonClicked()
        {
            if (_creationMode == CreationMode.Building2x3)
            {
                GameplayManager.Instance.SetCreationMode(CreationMode.None);
            }
            else
            {
                GameplayManager.Instance.SetCreationMode(CreationMode.Building2x3);
            }
        }
        
        public void OnBuildRoadButtonClicked()
        {
            if (_creationMode == CreationMode.Road)
            {
                GameplayManager.Instance.SetCreationMode(CreationMode.None);
            }
            else
            {
                GameplayManager.Instance.SetCreationMode(CreationMode.Road);
            }
        }
        
        public async void OnResetButtonClicked()
        {
            await GameplayManager.Instance.ResetAll();
            
            ShowUndoRedoButtons(false);
        }
        
        public void OnUndoButtonClicked()
        {
            GameplayManager.Instance.UndoLastRoadAction();
        }
        
        public void OnRedoButtonClicked()
        {
            GameplayManager.Instance.RedoLastRoadAction();
        }
        
        private void OnRedoStatusChanged(bool redoStatus)
        {
            if (_creationMode == CreationMode.Road)
            {
                redoButton.interactable = redoStatus;
            }
        }
        
        private void OnUndoStatusChanged(bool undoStatus)
        {
            if (_creationMode == CreationMode.Road)
            {
                undoButton.interactable = undoStatus;
            }
        }

        private void ShowTip(TipType tipType)
        {
            tipPanelCanvasGroup.gameObject.SetActive(true);
            tipPanelCanvasGroup.alpha = 0;
            tipPanelCanvasGroup.DOFade(1, 0.5f);
            
            switch (tipType)
            {
                case TipType.Pan:
                    tipText.text = Constants.TmpTapPan + Constants.PanTip;
                    break;
                case TipType.Drag:
                    tipText.text = Constants.TmpTapDrag + Constants.DragTip;
                    break;
                case TipType.Tap:
                    tipText.text = Constants.TmpTapTap + Constants.TapTip;
                    break;
            }
        }

        private void OnEnable()
        {
            RoadCommander.OnRedoStatusChanged += OnRedoStatusChanged;
            RoadCommander.OnUndoStatusChanged += OnUndoStatusChanged;
        }
        
        private void OnDisable()
        {
            RoadCommander.OnRedoStatusChanged -= OnRedoStatusChanged;
            RoadCommander.OnUndoStatusChanged -= OnUndoStatusChanged;
        }
    }    
}

