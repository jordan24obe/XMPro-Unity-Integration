using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XMPro.Unity.Api;

namespace XMPro.Unity.UI
{
    public class ShortNameDisplay : MonoBehaviour, ITargetter
    {
        [Tooltip("The entity being displayed.")]
        public EntityBase displayObject;
        [Tooltip("The textmeshpro object being written to.")]
        public TMPro.TextMeshProUGUI textbox;
        [Tooltip("Add a prefix before the name.")]
        public string prefix;
        [Tooltip("Add a suffix after the name.")]
        public string suffix;

        public void SetTarget(GameObject target)
        {
            displayObject = target.GetComponent<EntityBase>();
        }

        // Use this for initialization
        void Start()
        {
            textbox.text = prefix + displayObject.ShortName + suffix;
        }

        // Update is called once per frame
        void Update()
        {
            if (textbox.text != displayObject.ShortName)
            {
                textbox.text = prefix + displayObject.ShortName + suffix;
            }
        }
    }
}
