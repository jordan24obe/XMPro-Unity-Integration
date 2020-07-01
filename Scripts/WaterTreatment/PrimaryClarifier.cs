using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;
using XMPro.Unity.Api;

public class PrimaryClarifier : EntityBase
{
    
    public override Dictionary<string, IoTTypes> DataDefinitions => new Dictionary<string, IoTTypes>()
    {
        { WaterVelocity, IoTTypes.Float}
    };
    public override bool HasColors => false;

    public string WaterVelocity => this.ShortName + "_waterVelocity";

    [Header("Debugging variables")]
    public float waterVelocity;

    public override void Receive(JObject obj)
    {
        try
        {
            obj = JObject.Parse(obj["data"].ToString());
            waterVelocity = float.Parse(obj[WaterVelocity].ToString());
        }
        catch
        {

        }
    }

    public override string DataAsJson()
    {
        JObject json = new JObject
        {
            { WaterVelocity, waterVelocity }
        };
        return json.ToString();

    }
}
