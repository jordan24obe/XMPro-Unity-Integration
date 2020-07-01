using UnityEngine;
using System.Collections;
using Newtonsoft.Json.Linq;

namespace XMPro.Unity.Settings
{
    public interface ISettingDisplay
    {
        void MapSettings(JObject settings);
    }
}
