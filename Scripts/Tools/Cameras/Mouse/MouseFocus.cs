using UnityEngine;

namespace XMPro.Unity
{
    [AddComponentMenu("XMPro/Camera/Mouse Focus")]
    /// <summary>
    /// This script is used to finding the current mouseover target.
    /// </summary>
    public class MouseFocus : MonoBehaviour
    {
        protected OrbitCamera orbitCamera;
        protected GameObject focusObject;
        protected RaycastHit hit;
        protected Ray ray;

        public void Start()
        {
            orbitCamera = Camera.main.GetComponent<OrbitCamera>();
        }
        public void Update()
        {
            
            if (Input.GetMouseButtonDown(0))
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    focusObject = hit.collider.gameObject;
                    SetFocus();
                }
            }
        }


        public void SetFocus()
        {
            orbitCamera.SetFocus(focusObject);
        }
    }
}
