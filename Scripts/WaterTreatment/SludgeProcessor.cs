using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMPro.Unity.Api;

public class SludgeProcessor : EntityBase
{
    public override Dictionary<string, IoTTypes> DataDefinitions => new Dictionary<string, IoTTypes>()
    {
        { Nitrogen, IoTTypes.Float },
        { Phosphorus, IoTTypes.Float }
    };

    public string Nitrogen => this.ShortName + "_nitrogenLevel";
    public string Phosphorus => this.ShortName + "_phosphorusLevel";

    public override bool HasColors => false;

    [Header("Debugging variables")]
    public float phosphorusLevel;
    public float nitrogenLevel;


    public override void Receive(JObject obj)
    {
        try
        {
            obj = JObject.Parse(obj["data"].ToString());
            phosphorusLevel = float.Parse(obj[Phosphorus].ToString());
            nitrogenLevel = float.Parse(obj[Nitrogen].ToString());
        }
        catch
        {

        }
    }

    public override string DataAsJson()
    {
        JObject json = new JObject
        {
            { Nitrogen, nitrogenLevel },
            { Phosphorus, phosphorusLevel }
        };
        return json.ToString();

    }
}
