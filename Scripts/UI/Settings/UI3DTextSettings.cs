using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;
using XMPro.Unity.UI;

namespace XMPro.Unity.Settings
{
    /// <summary>
    /// use this on any textmeshpro 3d object.
    /// </summary>
    [AddComponentMenu("XMPro/Settings/3D Text Settings")]
    public class UI3DTextSettings : UIStandardSettings
    {
        public static string JsonFontSize = "fontSize";
        public static string JsonTextColor = "textColor";
        public static string JsonTextOutlineColor = "outlineColor";

        private TMPro.TextMeshPro tmp;
        public TMPro.TextMeshPro TMP
        {
            get
            {
                if (tmp == null)
                    tmp = GetComponent<TMPro.TextMeshPro>();
                return tmp;
            }
        }

        public override Dictionary<string, object> SettingDefinitions => new Dictionary<string, object>()
    {
        { JsonPosition, RectTransform.position.ToJsonVector3() },
        { JsonRotation, RectTransform.eulerAngles.ToJsonVector3() },
        { JsonScale, RectTransform.localScale.ToJsonVector3() },
        { JsonActive, gameObject.activeSelf },
        { JsonFontSize, TMP.fontSize },
        { JsonTextColor, TMP.color.ToJsonColor() },
        { JsonTextOutlineColor, GetComponent<TextControl3D>().outlineColor.ToJsonColor() }
    };

        public override void ParseSettings(JObject obj)
        {
            base.ParseSettings(obj);
            TMP.fontSize = float.Parse(obj[JsonFontSize].ToString());
            Color textColor;
            ColorUtility.TryParseHtmlString(obj[JsonTextColor].ToString(), out textColor);
            ColorUtility.TryParseHtmlString(obj[JsonTextOutlineColor].ToString(), out Color outlineColor);
            TMP.color = textColor;
            GetComponent<TextControl3D>().outlineColor = outlineColor;
            GetComponent<MeshRenderer>().materials[0].SetColor("_OutlineColor", outlineColor);
            GetComponent<TextControl3D>().Reset();
        }
    }
}