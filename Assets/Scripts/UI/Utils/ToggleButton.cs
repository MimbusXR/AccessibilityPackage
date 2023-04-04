using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mimbus.SDK.UI;

namespace Mimbus.SDK.UI
{
    public class ToggleButton : MonoBehaviour
    {
        [SerializeField]
        UnityEngine.Events.UnityEvent<bool> m_Event;
        [SerializeField]
        Image m_Background;
        [SerializeField]
        Image m_Cursor;
        [SerializeField]
        float m_AnimationSpeed = 8;
        [SerializeField]
        Vector2 m_CursorXPositionDisabled = new Vector2(-128, 0);
        [SerializeField]
        Vector2 m_CursorXPositionEnabled = new Vector2(128, 0);

        Color m_DisabledColor;
        Color m_EnabledColor;

        float m_Lerp = 0;

        public Color disabledColor
        {
            get => m_DisabledColor;
            set
            {
                m_EnabledColor = value;
            }
        }

        public Color enabledColor
        {
            get => m_EnabledColor;
            set
            {
                m_EnabledColor = value;
            }
        }

        public Color cursorColor
        {
            get => m_Cursor.color;
            set => m_Cursor.color = value;
        }

        bool m_State;

        const string __STATE_PARAM_NAME = "State";

        public bool state
        {
            get => m_State;
            set
            {
                m_State = value;
            }
        }

        public void Toggle()
        {
            state = !state;
            m_Event?.Invoke(state);
        }

        void SetColorTheme()
        {
            //innerColorOn = Mimbus.SDK.UI.SkinSettings.Instance.UIColor;
            //m_EnabledColor = SkinSettings.Instance.ToggleEnabledColor;
            //m_DisabledColor = SkinSettings.Instance.ToggleDisabledColor;
            //cursorColor = SkinSettings.Instance.ToggleCursorColor;
        }

        void UpdateVisual()
        {
            if (m_State) // On / Enabled
                m_Lerp += m_AnimationSpeed * Time.deltaTime;
            else // Off / Disabled
                m_Lerp -= m_AnimationSpeed * Time.deltaTime;
            m_Lerp = Mathf.Clamp01(m_Lerp);
            m_Background.color = Color.Lerp(m_DisabledColor, m_EnabledColor, m_Lerp);
            m_Cursor.rectTransform.localPosition = Vector2.Lerp(m_CursorXPositionDisabled, m_CursorXPositionEnabled, m_Lerp);
        }

        public void Start()
        {
            //SkinSettings.OnSkinApply += SetColorTheme;
            //if (SkinSettings.Instance)
                SetColorTheme();
        }

        public void Update()
        {
            UpdateVisual();
        }
    }
}
