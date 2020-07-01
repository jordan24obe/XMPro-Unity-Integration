using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMPro.Unity.Api
{
    public interface IHasSettings
    {
        string JsonName { get; }
        Dictionary<string,object> SettingDefinitions { get; }
        void ParseSettings(JObject obj);
    }
}
