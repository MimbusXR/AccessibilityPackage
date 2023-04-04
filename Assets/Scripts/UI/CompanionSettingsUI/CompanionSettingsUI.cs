using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Rendering.PostProcessing;
using Mimbus.SDK.Rendering;
using System.Linq;
using Mimbus.SDK.UI;

namespace Accessibility
{
    public class CompanionSettingsUI : MonoBehaviour
    {
        const string __RING_STATE_PLAYERPREF = "ColorMemoEnabled";
        public static event System.Action<TMPro.TMP_FontAsset> OnGlobalFontOverride;
		static TMPro.TMP_FontAsset s_CurrentFont;
		public static TMPro.TMP_FontAsset currentFont => s_CurrentFont;

		[SerializeField]
        UnityEngine.Rendering.Volume m_Volume;
        //PostProcessVolume m_GlobalPPVolume;
        [SerializeField]
        GameObject m_ColorBlindnessCorrectionIntensitySetting;
        [SerializeField]
        UnityEngine.UI.Toggle m_ColorMemoHelperToggle;
        [SerializeField]
        bool m_DefaultMemoHelperState = true;
        [SerializeField]
        Dropdown m_ColorModeDropdown;

        [SerializeField]
        TMPro.TMP_FontAsset m_DyslexicFont;

		/// <summary>
		/// PPStack Color blind correction settings
		/// </summary>
		ColorBlindnessCorrection m_ColorBlindCorrection;
		/// <summary>
		/// List of colored memo rings
		/// </summary>
		List<Ring> m_Rings;

        private void Start()
        {
			GetColorBlindCorrectionSettings();
			InitRings();
            m_ColorModeDropdown.SetValue(PlayerPrefs.GetInt(SkinManager.C_PLAYERPREF_DARKMODE_KEY, 0));
        }

		#region Color Blindness Correction
		private void GetColorBlindCorrectionSettings()
		{
			ColorBlindnessCorrection colorBlindnessCorrection;
			if (m_Volume != null
				&& m_Volume.profile != null
				&& m_Volume.profile.TryGet(out colorBlindnessCorrection))
			{
				m_ColorBlindCorrection = colorBlindnessCorrection;
			}
		}

		public void SetColorBlindCorrection(ColorBlindness colorBlindnessType)
		{
			if (m_ColorBlindCorrection == null)
			{
				GetColorBlindCorrectionSettings();
				if (m_ColorBlindCorrection == null)
					return;
			}

			m_ColorBlindCorrection.colorBlindnessType.Override(colorBlindnessType);
		}

		public void DisableColorBlindCorrection() => SetColorBlindCorrection(ColorBlindness.None);
		public void SetProtanopiaCorrection() => SetColorBlindCorrection(ColorBlindness.Protanopia);
		public void SetDeutranopiaCorrection() => SetColorBlindCorrection(ColorBlindness.Deutranopia);
		public void SetTritanopiaCorrection() => SetColorBlindCorrection(ColorBlindness.Tritanopia);

		public void SetCorrectionIntensity(float intensity)
		{
			if (m_ColorBlindCorrection == null)
			{
				GetColorBlindCorrectionSettings();
				if (m_ColorBlindCorrection == null)
					return;
			}
			m_ColorBlindCorrection.intensity.Override(Mathf.Clamp01(intensity));
		}
		#endregion

		#region Color Memorisation Helper
		void InitRings()
        {
            m_Rings = FindObjectsOfType<Ring>().ToList();
            bool enabled = PlayerPrefs.GetInt(__RING_STATE_PLAYERPREF, m_DefaultMemoHelperState ? 1 : 0) == 1;
            m_ColorMemoHelperToggle.isOn = enabled;
        }

        public void SetActiveRings(bool active)
		{
            foreach (var ring in m_Rings)
                ring.gameObject.SetActive(active);
        }

        public void EnableRings() => SetActiveRings(true);

        public void DisableRings() => SetActiveRings(false);
		#endregion

		#region Font handler
        private void SetFont(TMPro.TMP_FontAsset font)
		{
			s_CurrentFont = font;
            OnGlobalFontOverride?.Invoke(font);
		}

        public void SetDyslexicFont() => SetFont(m_DyslexicFont);
        public void SetDefaultFont() => SetFont(null);
		#endregion
	}
}
