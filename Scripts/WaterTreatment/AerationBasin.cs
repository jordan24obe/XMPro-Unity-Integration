using Assets.Scripts.WaterTreatment;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XMPro.Unity.Api;

public class AerationBasin : EntityBase
{
    public override Dictionary<string, IoTTypes> DataDefinitions => new Dictionary<string, IoTTypes>()
    {
        { Generator1AlertLevel, IoTTypes.String },
        { Generator2AlertLevel, IoTTypes.String }
    };
    public override bool HasColors => false;

    public string Generator1AlertLevel => this.ShortName + "_gen1AlertLevel";
    public string Generator2AlertLevel => this.ShortName + "_gen2AlertLevel";

    public Color alertRed;
    public Color alertYellow;
    public Color alertNone;
    public string generator1MaterialName;
    public string generator2MaterialName;
    public RecommendationLevel gen1AlertLevel;
    public RecommendationLevel gen2AlertLevel;
    private RecommendationLevel gen1PrevLevel = RecommendationLevel.Initialization;
    private RecommendationLevel gen2PrevLevel = RecommendationLevel.Initialization;
    private new MeshRenderer renderer;
    private Material[] materials;

    // Start is called before the first frame update
    void Start()
    {
        this.renderer = this.GetComponent<MeshRenderer>();
        materials = renderer.materials;
        this.generator1MaterialName = generator1MaterialName + " (Instance)";
        this.generator2MaterialName = generator2MaterialName + " (Instance)";
    }

    // Update is called once per frame
    void Update()
    {
        if (gen1PrevLevel != gen1AlertLevel)
        {
            if (gen1AlertLevel == RecommendationLevel.Red)
            {
                materials.First(m => m.name == generator1MaterialName).color = alertRed;
            }
            else if (gen1AlertLevel == RecommendationLevel.Yellow)
            {
                materials.First(m => m.name == generator1MaterialName).color = alertYellow;
            }
            else
            {
                materials.First(m => m.name == generator1MaterialName).color = alertNone;
            }
            gen1PrevLevel = gen1AlertLevel;
        }
        if (gen2PrevLevel != gen2AlertLevel)
        {
            if (gen2AlertLevel == RecommendationLevel.Red)
            {
                materials.First(m => m.name == generator2MaterialName).color = alertRed;
            }
            else if (gen2AlertLevel == RecommendationLevel.Yellow)
            {
                materials.First(m => m.name == generator2MaterialName).color = alertYellow;
            }
            else
            {
                materials.First(m => m.name == generator2MaterialName).color = alertNone;
            }
            gen2PrevLevel = gen2AlertLevel;
        }
    }

    public override void Receive(JObject obj)
    {
        try
        {
            obj = JObject.Parse(obj["data"].ToString());
            Enum.TryParse(obj[Generator1AlertLevel].ToString(), out gen1AlertLevel);
            Enum.TryParse(obj[Generator2AlertLevel].ToString(), out gen2AlertLevel);
        }
        catch
        {

        }
    }

    public override string DataAsJson()
    {
        JObject json = new JObject
        {
            { Generator1AlertLevel, gen1AlertLevel.ToString() },
            { Generator2AlertLevel, gen2AlertLevel.ToString() }
        };
        return json.ToString();
    }
}
