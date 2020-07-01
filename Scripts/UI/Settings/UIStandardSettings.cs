using UnityEngine;
using XMPro.Unity.Api;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace XMPro.Unity.Settings
{
    /// <summary>
    /// use this on any 2d canvas object.
    /// </summary>
    [AddComponentMenu("XMPro/Settings/2D Object Settings")]
    public class UIStandardSettings : MonoBehaviour, IHasSettings
    {
        public static string JsonPosition = "position";
        public static string JsonScale = "scale";
        public static string JsonActive = "active";
        public static string JsonRotation = "rotation";
        public string JsonName => name.Replace(' ', '_');

        private RectTransform rectTransform;
        public RectTransform RectTransform
        {
            get
            {
                if (rectTransform == null)
                    rectTransform = GetComponent<RectTransform>();
                return rectTransform;
            }
        }

        public virtual Dictionary<string, object> SettingDefinitions => new Dictionary<string, object>()
    {
        { JsonPosition, RectTransform.position.ToJsonVector3() },
        { JsonRotation, RectTransform.eulerAngles.ToJsonVector3() },
        { JsonScale, RectTransform.localScale.ToJsonVector3() },
        { JsonActive, gameObject.activeSelf },
    };

        public virtual void ParseSettings(JObject obj)
        {
            RectTransform.position = obj[JsonPosition].ToString().ToVector3();
            RectTransform.eulerAngles = obj[JsonRotation].ToString().ToVector3();
            RectTransform.localScale = obj[JsonScale].ToString().ToVector3();
            this.gameObject.SetActive(bool.Parse(obj[JsonActive].ToString()));
        }
    }
}