using UnityEngine;
using System.Collections;
using XMPro.Unity.Settings;

namespace XMPro.Unity.UI
{
    [AddComponentMenu("XMPro/UI/3D Text Control")]
    [RequireComponent(typeof(UI3DTextSettings))]
    public class TextControl3D : MonoBehaviour
    {
        public GameObject target;
        public Color outlineColor = Color.white;
        public bool lookAtTarget = true;
        public bool hideAfterCertainDistance;
        public float hideDistance;
        [HideInInspector]
        public bool hidden = false;

        private void Start()
        {
            hidden = false;
            if (hideAfterCertainDistance)
                CheckDistance();
        }
        // Update is called once per frame
        void Update()
        {
            if(lookAtTarget)
            {
                transform.LookAt(target.transform);
                this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y, this.transform.eulerAngles.z);

            }
            if (hideAfterCertainDistance)
                CheckDistance();
                
        }

        public void CheckDistance()
        {
            if(Vector3.Distance(transform.position, target.transform.position) > hideDistance && !hidden)
            {
                this.transform.localScale = new Vector3(0, 0, 0);
                hidden = true;
            }
            else if(hidden == true)
            {
                if (Vector3.Distance(transform.position, target.transform.position) < hideDistance)
                {
                    this.transform.localScale = new Vector3(-1, 1, 1); //-1 to face camera.
                    hidden = false;
                }               
            }
        }
        public void SetHeight(float height)
        {
            this.transform.position = new Vector3(transform.position.x, height, transform.position.z);
        }

        public void Reset()
        {
            hidden = false;
            if (hideAfterCertainDistance)
                CheckDistance();
        }
    }
}

