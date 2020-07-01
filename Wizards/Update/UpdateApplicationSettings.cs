using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;
using XMPro.Unity.Api;

#if (UNITY_EDITOR)
public class UpdateApplicationSettings : ScriptableWizard
{
    public XMPro.Unity.Api.Application application;

    [MenuItem("XMPro/Update/Application Settings")]
    public static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard<UpdateApplicationSettings>("Update Application Settings", "Update");
    }

    public void OnWizardCreate()
    {
        if (application == null)
            application = GameObject.FindWithTag("Manager").GetComponent<XMPro.Unity.Api.Application>();
        application.InitializeApi();

        foreach (var item in application.settingsObjects)
        {
            JObject jo = new JObject();
            Debug.Log(item.name);
            jo.Add("settingName", item.GetComponent<IHasSettings>().JsonName);
            jo.Add("settingValue", JObject.FromObject(item.GetComponent<IHasSettings>().SettingDefinitions).ToString());
            jo.Add("ApplicationId", application.Id);
            application.StartCoroutine(WebRequest.PutRequest(WebRequest.ApiBase + $"/settings/{application.Id}/{item.GetComponent<IHasSettings>().JsonName}", jo.ToString()));
        }
    }
}
#endif