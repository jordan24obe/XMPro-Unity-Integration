using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMPro.Unity.Api;

namespace XMPro.Unity.Settings
{
    [AddComponentMenu("XMPro/Settings/Dynamic Display")]
    public class UIDynamicSettingsControl : MonoBehaviour, IReceivesSettings
    {
        public int applicationId;
        public List<string> settingNames;
        public List<GameObject> settingDisplayObjects;
        public Canvas canvas;
        public GameObject container;
        public GameObject vectorDisplay;
        public GameObject colorPicker;
        void OnEnable()
        {
            GetApplicationSettings();
        }

        public void GetApplicationSettings()
        {
            StartCoroutine(WebRequest.GetSettingsRequest(WebRequest.ApiBase + $"/settings/application/{this.applicationId}", this));
        }

        public void ReceiveSettings(JArray obj)
        {
            foreach (JObject jObject in obj)
            {
                int index = 0;
                foreach (var item in settingNames)
                {

                    if (item == jObject["settingName"].ToString())
                    {
                        settingDisplayObjects[index].GetComponent<ISettingDisplay>().MapSettings(jObject);
                    }
                    else
                        index++;
                }
            }
        }
    }
}
