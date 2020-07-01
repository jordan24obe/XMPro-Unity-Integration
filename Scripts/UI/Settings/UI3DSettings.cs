using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;
using XMPro.Unity.Api;

namespace XMPro.Unity.Settings
{
    /// <summary>
    /// Use this on standard gameobjects.
    /// </summary>
    [AddComponentMenu("XMPro/Settings/3D Object Settings")]
    public class UI3DSettings : MonoBehaviour, IHasSettings
    {
        public static string JsonPosition = "position";
        public static string JsonScale = "scale";
        public static string JsonActive = "active";
        public static string JsonRotation = "rotation";
        public string JsonName => name.Replace(' ', '_');

        public Dictionary<string, object> SettingDefinitions => new Dictionary<string, object>()
    {
        { JsonPosition, transform.localPosition.ToJsonVector3() },
        { JsonRotation, transform.eulerAngles.ToJsonVector3() },
        { JsonScale, transform.localScale.ToJsonVector3() },
        { JsonActive, gameObject.activeSelf },
    };

        public void ParseSettings(JObject obj)
        {
            transform.localPosition = obj[JsonPosition].ToString().ToVector3();
            transform.eulerAngles = obj[JsonRotation].ToString().ToVector3();
            transform.localScale = obj[JsonScale].ToString().ToVector3();
            gameObject.SetActive(bool.Parse(obj[JsonActive].ToString()));
        }
    }
}
