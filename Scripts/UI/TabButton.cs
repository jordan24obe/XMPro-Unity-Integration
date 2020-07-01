using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.UI.Extensions;

namespace XMPro.Unity.UI
{
    [AddComponentMenu("XMPro/UI/Tab Menu Button")]
    public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        public TabGroup tabGroup;
        public UIPolygon foreground;
        public UIPolygon background;
        public Transform page;
        // Use this for initialization
        void Start()
        {
            tabGroup.Subscribe(this);
        }

        public void TogglePage(bool visible)
        {
            page.gameObject.SetActive(visible);
        }
        public void OnPointerClick(PointerEventData data)
        {
            tabGroup.OnTabSelected(this);
        }
        public void OnPointerEnter(PointerEventData data)
        {
            tabGroup.OnTabEnter(this);
        }
        public void OnPointerExit(PointerEventData data)
        {
            tabGroup.OnTabExit(this);
        }
    }
}