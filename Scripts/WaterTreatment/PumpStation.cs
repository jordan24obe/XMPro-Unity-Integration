using Assets.Scripts.WaterTreatment;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XMPro.Unity.Api;

public class PumpStation : EntityBase
{
    public override Dictionary<string, IoTTypes> DataDefinitions => new Dictionary<string, IoTTypes>()
    {
        { AlertLevel, IoTTypes.String }
    };
    public override bool HasColors => false;

    public string AlertLevel => this.ShortName + "_alertLevel";
    public Color alertRed;
    public Color alertYellow;
    public Color alertNone;
    public string materialName;
    public RecommendationLevel recommendationLevel;
    private RecommendationLevel previousLevel = RecommendationLevel.None;
    private new MeshRenderer renderer;
    private Material[] materials;
    // Start is called before the first frame update
    void Start()
    {
        this.renderer = this.GetComponent<MeshRenderer>();
        materials = renderer.materials;
        this.materialName = materialName + " (Instance)";
    }

    // Update is called once per frame
    void Update()
    {
        if(previousLevel != recommendationLevel)
        {
            if(recommendationLevel == RecommendationLevel.Red)
            {
                materials.First(m => m.name == materialName).color = alertRed;
            }
            else if (recommendationLevel == RecommendationLevel.Yellow)
            {
                materials.First(m => m.name == materialName).color = alertYellow;
            }
            else
            {
                materials.First(m => m.name == materialName).color = alertNone;
            }
            previousLevel = recommendationLevel;
        }
    }
    public override void Receive(JObject obj)
    {
        try
        {
            obj = JObject.Parse(obj["data"].ToString());
            Enum.TryParse(obj[AlertLevel].ToString(), out recommendationLevel);
        }
        catch
        {
            
        }
    }

    public override string DataAsJson()
    {
        JObject json = new JObject
        {
            { AlertLevel, recommendationLevel.ToString() }
        };
        return json.ToString();

    }
}
