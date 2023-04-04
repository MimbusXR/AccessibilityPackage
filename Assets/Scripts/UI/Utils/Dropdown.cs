using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;

namespace Accessibility
{
    public class Dropdown : MonoBehaviour
    {
        [System.Serializable]
        class DropdownElement
        {
            [SerializeField]
            string m_OptionKey;
            [SerializeField]
            UnityEngine.Events.UnityEvent m_Event;

            public string optionKey => m_OptionKey;
            public UnityEngine.Events.UnityEvent action => m_Event;
        }

        [SerializeField]
        TMP_Dropdown m_Dropdown;
        [SerializeField]
        List<DropdownElement> m_Options;

        [SerializeField]
        Image m_DropdownBackground;
        [SerializeField]
        Image m_DropdownListBackground;
        [SerializeField]
        Toggle m_ElemToggle;
        [SerializeField]
        Image m_Checkmark;
        [SerializeField]
        TMPro.TMP_Text m_LabelSelectedText;
        [SerializeField]
        TMPro.TMP_Text m_LabelText;

        // Start is called before the first frame update
        void Start()
        {
            m_Dropdown.onValueChanged.AddListener(OnValueChanged);
            UpdateOptions();
            SkinSettings.OnSkinApply += SetColors;
		}

        void SetColors()
		{
            m_DropdownBackground.color = SkinSettings.Instance.DropdownBackgroundColor;
            m_DropdownListBackground.color = SkinSettings.Instance.DropdownBackgroundColor;
            var colors = m_ElemToggle.colors;
            colors.normalColor = SkinSettings.Instance.DropdownBackgroundColor;
            m_ElemToggle.colors = colors;
            m_Checkmark.color = SkinSettings.Instance.DropdownCheckmarkColor;
            m_LabelText.color = SkinSettings.Instance.DropdownTextColor;
            m_LabelSelectedText.color = SkinSettings.Instance.DropdownTextColor;
        }

        void UpdateOptions()
		{
            m_Dropdown.ClearOptions();
            var options = m_Options.Select(p => p.optionKey).ToList();
            m_Dropdown.AddOptions(options);
        }

        public void SetValue(int i)
		{
            m_Dropdown.value = i;
		}

        void OnValueChanged(int i)
		{
            if (i >= 0 && i < m_Options.Count)
                m_Options[i].action?.Invoke();
		}
    }
}
