using UnityEngine;

namespace OleksiiStepanov.Utils
{
    public static class ColorExtensions
    {
        /// <summary>
        /// Converts a Unity Color to a HEX string.
        /// </summary>
        /// <param name="color">The Unity Color to convert.</param>
        /// <param name="includeAlpha">Include alpha in the HEX string (default: true).</param>
        /// <returns>A HEX string representation of the color.</returns>
        public static string ToHex(this Color color, bool includeAlpha = true)
        {
            string r = Mathf.Clamp(Mathf.RoundToInt(color.r * 255), 0, 255).ToString("X2");
            string g = Mathf.Clamp(Mathf.RoundToInt(color.g * 255), 0, 255).ToString("X2");
            string b = Mathf.Clamp(Mathf.RoundToInt(color.b * 255), 0, 255).ToString("X2");

            if (includeAlpha)
            {
                string a = Mathf.Clamp(Mathf.RoundToInt(color.a * 255), 0, 255).ToString("X2");
                return $"#{r}{g}{b}{a}";
            }
        
            return $"#{r}{g}{b}";
        }
    }
}
