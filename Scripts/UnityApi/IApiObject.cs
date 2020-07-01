using UnityEngine;
using System.Collections;
using Newtonsoft.Json.Linq;

namespace XMPro.Unity.Api
{
    public interface IApiObject
    {
        int Id { get; }
        string ShortName { get; }
        void Receive(JObject obj);
        string AsJson();
        string DefinitionsAsJson();
        string DataAsJson();
    }
}