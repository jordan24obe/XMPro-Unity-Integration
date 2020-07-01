using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace XMPro.Unity.Api
{
    public class Settings
    {
        public static string JsonSettingName = "settingName";
        public static string JsonSettingValue = "settingValue";

        protected Dictionary<string, JObject> settings;

        public Settings(JArray settings)
        {
            ReplaceAll(settings);
        }


        public JObject GetSetting(string name)
        {
            if (Contains(name))
                return settings[name];
            return new JObject();
        }

        public bool Contains(string name)
        {
            if (settings.ContainsKey(name))
                return true;
            return false;
        }

        public void ReplaceAll(JArray settings)
        {
            this.settings = new Dictionary<string, JObject>();

            foreach (JObject setting in settings)
            {
                this.settings.Add(setting[JsonSettingName].ToString(), JObject.Parse(setting[JsonSettingValue].ToString()));
            }
        }

    }
}
