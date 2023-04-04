using System.Collections.Generic;
using UnityEngine;

namespace Accessibility
{
    public class SkinManager : MonoBehaviour
    {
        public static string C_PLAYERPREF_DARKMODE_KEY = "DarkMode";

        public enum ColorMode
        {
            Normal,
            Dark,
            HighContrast
        }

        [System.Serializable]
        public class ColorModeConfig
        {
            public ColorMode colorMode;
            public SkinSettings settings;
        }

        /// <summary>
        /// Color configuration for each mode
        /// </summary>
        [SerializeField]
        List<ColorModeConfig> m_ColorConfigs = new List<ColorModeConfig>() {    new ColorModeConfig() { colorMode = ColorMode.Normal },
                                                                            new ColorModeConfig() { colorMode = ColorMode.Dark },
                                                                            new ColorModeConfig() { colorMode = ColorMode.HighContrast } };

        /// <summary>
        /// Active color mode
        /// </summary>
        public ColorMode CurrentColorMode { get; private set; }

        /// <summary>
        /// Color mode selector
        /// </summary>
        [SerializeField]
        Dropdown m_ColorModeSelect;

        // Start is called before the first frame update
        void Start()
        {
            // If no dark mode value in playerprefs, set default
            if (!PlayerPrefs.HasKey(C_PLAYERPREF_DARKMODE_KEY))
                PlayerPrefs.SetInt(C_PLAYERPREF_DARKMODE_KEY, 0);

            // Set dark mode from playerprefs value
            var colorMode = PlayerPrefs.GetInt(C_PLAYERPREF_DARKMODE_KEY);
            SetColorMode((ColorMode)colorMode);
            m_ColorModeSelect.SetValue(colorMode);
        }

        /// <summary>
        /// Set the color mode
        /// </summary>
        /// <param name="colorMode">True to enable dark mode</param>
        public void SetColorMode(ColorMode colorMode)
        {
            CurrentColorMode = colorMode;
            PlayerPrefs.SetInt(C_PLAYERPREF_DARKMODE_KEY, (int)colorMode);
            m_ColorConfigs.Find(p => p.colorMode == colorMode)?.settings.Apply();
        }

        /// <summary>
        /// Set light mode
        /// </summary>
        public void SetLightMode() => SetColorMode(ColorMode.Normal);

        /// <summary>
        /// Set dark mode
        /// </summary>
        public void SetDarkMode() => SetColorMode(ColorMode.Dark);

        /// <summary>
        /// Set high contrast mode
        /// </summary>
        public void SetHighContrastMode() => SetColorMode(ColorMode.HighContrast);
    }
}
