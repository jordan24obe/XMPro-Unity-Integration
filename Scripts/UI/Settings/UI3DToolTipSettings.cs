using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;
using XMPro.Unity.Api;
using XMPro.Unity.UI;

namespace XMPro.Unity.Settings
{
    [AddComponentMenu("XMPro/Settings/3D Tooltip Settings")]
    public class UI3DToolTipSettings : MonoBehaviour, IHasSettings
    {
        public static string JsonBackgroundColor = "backgroundColor";
        public string JsonName => name.Replace(' ', '_');


        public Dictionary<string, object> SettingDefinitions => new Dictionary<string, object>()
    {
        { JsonBackgroundColor, GetComponent<DisplayOnMouseover>().BackgroundColor.ToJsonColor()},
    };

        public void ParseSettings(JObject obj)
        {
            ColorUtility.TryParseHtmlString(obj[JsonBackgroundColor].ToString(), out Color value);
            GetComponent<DisplayOnMouseover>().background.GetComponent<MeshRenderer>().materials[0].SetColor("Color_5CE9BD30", value);
        }
    }
}