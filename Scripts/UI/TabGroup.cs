using System.Collections.Generic;
using UnityEngine;

namespace XMPro.Unity.UI
{
    [AddComponentMenu("XMPro/UI/Tab Menu Control")]
    public class TabGroup : MonoBehaviour
    {
        public List<TabButton> buttons;
        public Color tabIdle;
        public Color tabHover;
        public Color tabBorder;
        public TabButton selectedTab;

        public void Subscribe(TabButton button)
        {
            if (buttons == null)
            {
                buttons = new List<TabButton>();
            }
            buttons.Add(button);
        }

        public void OnTabEnter(TabButton button)
        {
            ResetTabs();
            if (button != selectedTab)
                button.foreground.color = tabHover;
        }

        public void OnTabExit(TabButton button)
        {
            ResetTabs();
            if (button != selectedTab)
                button.foreground.color = tabIdle;
        }

        public void OnTabSelected(TabButton button)
        {
            selectedTab = button;
            ResetTabs();
            button.TogglePage(true);
        }

        public void ResetTabs()
        {
            foreach (var button in buttons)
            {
                if (selectedTab != null && button == selectedTab)
                {
                    continue;
                }
                button.foreground.color = tabIdle;
                button.background.color = tabBorder;
                button.TogglePage(false);
            }
        }

        // Use this for initialization
        void Start()
        {
            // selectedTab.background.color = tabBorder;
            ResetTabs();
        }
    }
}