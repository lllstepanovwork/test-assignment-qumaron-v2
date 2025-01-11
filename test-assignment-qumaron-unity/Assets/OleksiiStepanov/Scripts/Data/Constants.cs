using UnityEngine;

namespace OleksiiStepanov.Data
{
    public class Constants : MonoBehaviour
    {
        #region UI

        public const string PanTip = "Tap and hold to pan the camera.";
        public const string TapTip = "Tap a grid cell to build a road.";
        public const string DragTip = "Tap and hold on a building to drag it.";

        #endregion
        
        #region TextMeshProSpriteShortcuts

        public const string TmpTapPan = "<sprite name=\"icon-touch-pan\">";
        public const string TmpTapDrag = "<sprite name=\"icon-touch-drag\">";
        public const string TmpTapTap = "<sprite name=\"icon-touch-tap\">";

        #endregion


        
        #region Scenes

        public const string LoaderSceneName = "Loader";
        public const string MainSceneName = "Main";

        #endregion

        #region ConsoleColors

        public const string ConsoleMessageColorPink = "<color=#EE77AE>";
        public const string ConsoleMessageColorGreen = "<color=#00B587>";
        public const string ConsoleMessageColorRed = "<color=#EF413B>";
        public const string ConsoleMessageColorBlue = "<color=#00AAFF>";
        public const string ConsoleMessageColorYellow = "<color=yellow>";
        public const string ConsoleMessageColorEnd = "</color>";

        #endregion
    }
}
