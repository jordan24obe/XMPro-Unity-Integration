using Newtonsoft.Json.Linq;
using UnityEngine.UI.Extensions.ColorPicker;
using XMPro.Unity.Api;

namespace XMPro.Unity.Settings
{
    [UnityEngine.AddComponentMenu("XMPro/Settings/UI/Application Settings Display")]
    public class ApplicationSettings : UnityEngine.MonoBehaviour, ISettingDisplay
    {
        public ColorPickerControl primaryColor;
        public ColorPickerControl secondaryColor;
        public ColorPickerControl tertiaryColor;
        public ColorPickerControl backgroundColor;
        public TMPro.TMP_InputField timeBetweenRequests;

        public void MapSettings(JObject settings)
        {
            settings = JObject.Parse(settings["settingValue"].ToString());
            primaryColor.CurrentColor = settings[Application.JsonPrimaryColor].ToString().ToColor();
            secondaryColor.CurrentColor = settings[Application.JsonSecondaryColor].ToString().ToColor();
            tertiaryColor.CurrentColor = settings[Application.JsonTertiaryColor].ToString().ToColor();
            backgroundColor.CurrentColor = settings[Application.JsonCameraBackgroundColor].ToString().ToColor();
            timeBetweenRequests.text = float.Parse(settings[Application.JsonTimeBetweenRequests].ToString()).ToString();
        }
    }
}