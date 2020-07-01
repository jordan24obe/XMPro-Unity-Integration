using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XMPro.Unity.Settings;

namespace XMPro.Unity.UI
{
    [AddComponentMenu("XMPro/UI/3D Tooltip")]
    [RequireComponent(typeof(UI3DToolTipSettings))]
    public class DisplayOnMouseover : MonoBehaviour
    {
        public class DisplayOnMouseoverObject
        {
            public readonly Vector3 offset;
            public readonly GameObject tooltipObject;
            public DisplayOnMouseoverObject(GameObject obj, Vector3 offset)
            {
                this.tooltipObject = obj;
                this.offset = offset;
            }
        }
        [Tooltip("The object that is displayed when no tooltip is needed. Ensure that this object has a collider.")]
        public GameObject normalDisplay;
        [Tooltip("The background object for the tooltip.")]
        public GameObject background;
        [Tooltip("The color of the background object. This only works if the tooltip is of type xray.")]
        public Color BackgroundColor;
        [Tooltip("The objects that are displayed when the normal display is moused over.")]
        public List<GameObject> displayOnMouseover = new List<GameObject>();
        protected List<DisplayOnMouseoverObject> displayOnMouseoverObjects;
        [Tooltip("The layer of the elements being checked for by the script.")]
        public LayerMask layerMask;
        [Tooltip("The max distance from the camera to object to check for collison.")]
        public float maxDistance;
        protected MeshRenderer render;
        protected bool tooltipsVisible;
        protected Ray ray;
        protected RaycastHit hit;

        // Start is called before the first frame update
        void Start()
        {
            tooltipsVisible = false;
            this.render = normalDisplay.GetComponent<MeshRenderer>();
            displayOnMouseoverObjects = new List<DisplayOnMouseoverObject>();
            background.GetComponent<MeshRenderer>().materials[0].SetColor("Color_5CE9BD30", BackgroundColor);
            foreach (var go in displayOnMouseover)
            {
                go.SetActive(false);
                displayOnMouseoverObjects.Add(new DisplayOnMouseoverObject(go, go.transform.localPosition));

            }
        }

        // Update is called once per frame
        void Update()
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, maxDistance, layerMask.value))
            {
                //if normal object is moused over.
                if (normalDisplay.GetInstanceID() == hit.collider.gameObject.GetInstanceID())
                {
                    if (!tooltipsVisible)
                    {
                        SetTooltipsActive(true);
                    }
                }
                else
                {
                    if (tooltipsVisible)
                        SetTooltipsActive(false);
                }
            }
            else
            {
                if (tooltipsVisible)
                {
                    SetTooltipsActive(false);
                }
            }
        }

        private void SetTooltipsActive(bool value)
        {
            tooltipsVisible = value;
            render.enabled = !value;
            foreach (var go in displayOnMouseoverObjects)
            {
                go.tooltipObject.transform.localPosition = normalDisplay.transform.localPosition + go.offset;
                go.tooltipObject.SetActive(value);
            }
        }
    }
}