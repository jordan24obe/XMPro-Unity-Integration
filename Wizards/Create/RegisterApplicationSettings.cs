using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;
using XMPro.Unity.Api;

#if (UNITY_EDITOR)
public class RegisterApplicationSettings : ScriptableWizard
{
    public XMPro.Unity.Api.Application application;

    [MenuItem("XMPro/Register/Application Settings")]
    public static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard<RegisterApplicationSettings>("Register Application Settings", "Register");
    }

    public void OnWizardCreate()
    {
        if (application == null)
            application = GameObject.FindWithTag("Manager").GetComponent<XMPro.Unity.Api.Application>();
        application.InitializeApi();

        foreach (var item in application.settingsObjects)
        {
            JObject jo = new JObject
            {
                { "settingName", item.GetComponent<IHasSettings>().JsonName },
                { "settingValue", JObject.FromObject(item.GetComponent<IHasSettings>().SettingDefinitions).ToString() },
                { "ApplicationId", application.Id }
            };
            application.StartCoroutine(WebRequest.PostRequest(WebRequest.ApiBase + $"/settings", jo.ToString()));
        }
    }

}
#endif