using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Accessibility
{
	[CreateAssetMenu(fileName = "UISkin", menuName = "Mimbus/UI/Create New Skin Settings", order = 1)]
	public class SkinSettings : ScriptableObject
	{
		static SkinSettings s_Instance;
		public static SkinSettings Instance => s_Instance;

		public static event System.Action OnSkinApply;

		[SerializeField]
		Color m_UIColor = new Color(.9f, .3f, .15f, 1f);
		[SerializeField]
		Color m_TopBarTextColor = Color.white;
		[SerializeField]
		Color m_BackgroundColor = new Color(.95f, .95f, .95f, .8f);
		[SerializeField]
		Color m_TextColor = Color.black;
		[SerializeField]
		Color m_ButtonTextColor = Color.black;
		[SerializeField]
		Color m_ButtonBackgroundColor = new Color(1f, 1f, 1f, 0f);
		[SerializeField]
		Color m_ButtonOutlineColor = new Color(.9f, .3f, .15f, 1);
		[SerializeField]
		Color m_DisabledButtonTextColor = new Color(1, 1, 1, .5f);
		[SerializeField]
		Color m_DisabledButtonBackgroundColor = new Color(.4f, .4f, .4f, 1f);
		[SerializeField]
		Color m_DisabledButtonOutlineColor = new Color(.9f, .3f, .15f, .5f);
		[SerializeField]
		Color m_ToggleEnabledColor = new Color(.9f, .3f, .15f, 1f);
		[SerializeField]
		Color m_ToggleDisabledColor = new Color(.4f, .4f, .4f, 1f);
		[SerializeField]
		Color m_ToggleCursorColor = new Color(.9f, .3f, .15f, .5f);
		[SerializeField]
		Color m_DropdownBackgroundColor = Color.white;
		[SerializeField]
		Color m_DropdownCheckmarkColor = new Color(.9f, .3f, .15f, 1f);
		[SerializeField]
		Color m_DropdownHoverColor = new Color(.7f, .7f, .7f, 1);
		[SerializeField]
		Color m_DropdownTextColor = Color.black;
		[SerializeField]
		Color m_SeparatorColor = new Color(.7f, .7f, .7f, 1);

		public Color UIColor => m_UIColor;
		public Color TopBarTextColor => m_TopBarTextColor;
		public Color BackgroundColor => m_BackgroundColor;
		public Color TextColor => m_TextColor;
		public Color ButtonTextColor => m_ButtonTextColor;
		public Color ButtonBackgroundColor => m_ButtonBackgroundColor;
		public Color ButtonOutlineColor => m_ButtonOutlineColor;
		public Color DisabledButtonTextColor => m_DisabledButtonTextColor;
		public Color DisabledButtonBackgroundColor => m_DisabledButtonBackgroundColor;
		public Color DisabledButtonOutlineColor => m_DisabledButtonOutlineColor;
		public Color ToggleEnabledColor => m_ToggleEnabledColor;
		public Color ToggleDisabledColor => m_ToggleDisabledColor;
		public Color ToggleCursorColor => m_ToggleCursorColor;
		public Color DropdownBackgroundColor => m_DropdownBackgroundColor;
		public Color DropdownCheckmarkColor => m_DropdownCheckmarkColor;
		public Color DropdownHoverColor => m_DropdownHoverColor;
		public Color DropdownTextColor => m_DropdownTextColor;
		public Color SeparatorColor => m_SeparatorColor;

		public void Apply()
		{
			s_Instance = this;
			OnSkinApply?.Invoke();
		}
	}
}
