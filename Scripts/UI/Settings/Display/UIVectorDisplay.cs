using Newtonsoft.Json.Linq;
using UnityEngine;

namespace XMPro.Unity.Settings
{
    [AddComponentMenu("XMPro/Settings/Vector Display")]
    public class UIVectorDisplay : MonoBehaviour, ISettingDisplay
    {
        public string vectorName;

        public TMPro.TMP_InputField x;
        public TMPro.TMP_InputField y;
        public TMPro.TMP_InputField z;

        public void MapSettings(JObject settings)
        {
            var vector = settings[vectorName].ToString().ToVector3();
            x.text = vector.x.ToString();
            y.text = vector.y.ToString();
            z.text = vector.z.ToString();

        }
    }
}