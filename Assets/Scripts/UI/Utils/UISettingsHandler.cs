using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Accessibility
{
	public class UISettingsHandler : MonoBehaviour
	{
		public enum ElementToChangeColor { Image, Text, Button, ButtonWithNormalColor, TMP, TMP_UI, ButtonWithSelectedColor }

		public enum TargetColor
		{
			UIColor,
			TopBarTextColor,
			BackgroundColor,
			TextColor,
			ButtonTextColor,
			ButtonBackgroundColor,
			ButtonOutlineColor,
			Separator
		}

		[SerializeField] ElementToChangeColor m_elementType = ElementToChangeColor.Button;

		[SerializeField]
		TargetColor m_TargetColor = TargetColor.UIColor;

		[SerializeField]
		bool m_UpdateColor = true;

		[SerializeField]
		float m_colorOffset = 0f;

		private bool m_StartDone;

		private System.Action OnNullGlobalFont;

		private Color GetTargetColor()
		{
			switch (m_TargetColor)
			{
				case TargetColor.UIColor:
					return SkinSettings.Instance.UIColor;
				case TargetColor.TopBarTextColor:
					return SkinSettings.Instance.TopBarTextColor;
				case TargetColor.BackgroundColor:
					return SkinSettings.Instance.BackgroundColor;
				case TargetColor.TextColor:
					return SkinSettings.Instance.TextColor;
				case TargetColor.ButtonTextColor:
					return SkinSettings.Instance.ButtonTextColor;
				case TargetColor.ButtonBackgroundColor:
					return SkinSettings.Instance.ButtonBackgroundColor;
				case TargetColor.ButtonOutlineColor:
					return SkinSettings.Instance.ButtonOutlineColor;
				case TargetColor.Separator:
					return SkinSettings.Instance.SeparatorColor;
				default:
					return SkinSettings.Instance.UIColor;
			}
		}

		[ContextMenu("SetColor")]
		public void SetColor()
		{
			if (!m_UpdateColor)
				return;

			Color lColorOffset = new Color(m_colorOffset, m_colorOffset, m_colorOffset, 0);
			Color lVulcanColorOffset = GetTargetColor() + lColorOffset;

			switch (m_elementType)
			{
				case ElementToChangeColor.Image:
					if (TryGetComponent(out Image image))
					{
						image.color = lVulcanColorOffset;
					}

					break;

				case ElementToChangeColor.Text:
					if (TryGetComponent(out Text text))
					{
						text.color = lVulcanColorOffset;
					}

					break;

				case ElementToChangeColor.Button:
					if (TryGetComponent(out Button lButton))
					{
						var colors = lButton.colors;
						colors.highlightedColor = lVulcanColorOffset;
						colors.pressedColor = lVulcanColorOffset;
						//	colors.selectedColor = lVulcanColorOffset;
						lButton.colors = colors;
					}
					break;

				case ElementToChangeColor.ButtonWithNormalColor:
					if (TryGetComponent(out Button lButton2))
					{
						var colors = lButton2.colors;
						colors.normalColor = lVulcanColorOffset;
						colors.highlightedColor = lVulcanColorOffset;
						colors.pressedColor = lVulcanColorOffset;
						//	colors.selectedColor = lVulcanColorOffset;
						lButton2.colors = colors;
					}
					break;

				case ElementToChangeColor.ButtonWithSelectedColor:
					if (TryGetComponent(out Button lButton3))
					{
						var colors = lButton3.colors;
						colors.highlightedColor = lVulcanColorOffset;
						colors.pressedColor = lVulcanColorOffset;
						colors.selectedColor = lVulcanColorOffset;
						lButton3.colors = colors;
					}
					break;

				case ElementToChangeColor.TMP:
				case ElementToChangeColor.TMP_UI:
					if (TryGetComponent(out TMP_Text tMP_Text))
					{
						tMP_Text.color = GetTargetColor();
					}
					break;

				default:
					break;
			}
		}

		void ApplyFont(TMPro.TMP_FontAsset font)
		{
			if (font == null)
			{
				OnNullGlobalFont?.Invoke();
				return;
			}
			switch (m_elementType)
			{
				case ElementToChangeColor.Text:
					if (TryGetComponent(out Text text))
						text.font = font.sourceFontFile;
					break;
				case ElementToChangeColor.TMP:
				case ElementToChangeColor.TMP_UI:
					if (TryGetComponent(out TMP_Text tMP_Text))
						tMP_Text.font = font;
					break;
			}
		}

		void InitFontOverride()
		{
			CompanionSettingsUI.OnGlobalFontOverride += ApplyFont;
			switch (m_elementType)
			{
				case ElementToChangeColor.Text:
					if (TryGetComponent(out Text text))
					{
						var defaultFont = text.font;
						OnNullGlobalFont = () =>
							text.font = defaultFont;
					}
					break;
				case ElementToChangeColor.TMP:
				case ElementToChangeColor.TMP_UI:
					if (TryGetComponent(out TMP_Text tMP_Text))
					{
						var defaultFont = tMP_Text.font;
						OnNullGlobalFont = () =>
							tMP_Text.font = defaultFont;
					}
					break;
			}
		}

		private void OnDestroy()
		{
			SkinSettings.OnSkinApply -= SetColor;
			CompanionSettingsUI.OnGlobalFontOverride -= ApplyFont;
		}

		private void Start()
		{
			InitFontOverride();
			ApplyFont(CompanionSettingsUI.currentFont);
			SkinSettings.OnSkinApply += SetColor;
			if (SkinSettings.Instance)
				SetColor();
			m_StartDone = true;
		}

		private void OnEnable()
		{
			if (!m_StartDone)
				return;
			SetColor();
		}
	}
}
