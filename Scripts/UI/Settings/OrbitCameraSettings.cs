using Newtonsoft.Json.Linq;
using UnityEngine;

namespace XMPro.Unity.Settings
{
    /// <summary>
    /// Add this script when you want to use the settings api for the orbit camera.
    /// </summary>
    [AddComponentMenu("XMPro/Settings/UI/Orbit Camera Settings Display")]
    public class OrbitCameraSettings : MonoBehaviour, ISettingDisplay
    {
        public TMPro.TMP_InputField distanceFromTarget;
        public TMPro.TMP_InputField xAxisSpeed;
        public TMPro.TMP_InputField yAxisSpeed;
        public TMPro.TMP_InputField yMinimumLimit;
        public TMPro.TMP_InputField yMaximumLimit;
        public TMPro.TMP_InputField distanceMinimum;
        public TMPro.TMP_InputField distanceMaximum;
        public UIVectorDisplay offset;

        public void FindControls()
        {
            distanceFromTarget = transform.Find(nameof(distanceFromTarget)).GetChild(1).GetComponent<TMPro.TMP_InputField>();
            xAxisSpeed = transform.Find(nameof(xAxisSpeed)).GetChild(1).GetComponent<TMPro.TMP_InputField>();
            yAxisSpeed = transform.Find(nameof(yAxisSpeed)).GetChild(1).GetComponent<TMPro.TMP_InputField>();
            yMinimumLimit = transform.Find(nameof(yMinimumLimit)).GetChild(1).GetComponent<TMPro.TMP_InputField>();
            yMaximumLimit = transform.Find(nameof(yMaximumLimit)).GetChild(1).GetComponent<TMPro.TMP_InputField>();
            distanceMinimum = transform.Find(nameof(distanceMinimum)).GetChild(1).GetComponent<TMPro.TMP_InputField>();
            distanceMaximum = transform.Find(nameof(distanceMaximum)).GetChild(1).GetComponent<TMPro.TMP_InputField>();
            offset = transform.Find(nameof(offset)).GetComponent<UIVectorDisplay>();
        }
        public void MapSettings(JObject settings)
        {
            FindControls();
            settings = JObject.Parse(settings["settingValue"].ToString());
            distanceFromTarget.text = float.Parse(settings[OrbitCamera.JsonDistanceFromTarget].ToString()).ToString();
            xAxisSpeed.text = float.Parse(settings[OrbitCamera.JsonXAxisSpeed].ToString()).ToString();
            yAxisSpeed.text = float.Parse(settings[OrbitCamera.JsonYAxisSpeed].ToString()).ToString();
            yMinimumLimit.text = float.Parse(settings[OrbitCamera.JsonYMinimumLimit].ToString()).ToString();
            yMaximumLimit.text = float.Parse(settings[OrbitCamera.JsonYMaximumLimit].ToString()).ToString();
            distanceMinimum.text = float.Parse(settings[OrbitCamera.JsonDistanceMinimum].ToString()).ToString();
            distanceMaximum.text = float.Parse(settings[OrbitCamera.JsonDistanceMaximum].ToString()).ToString();
            offset.MapSettings(settings);
        }
    }
}