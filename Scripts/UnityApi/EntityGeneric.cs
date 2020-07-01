using System.Collections.Generic;
using XMPro.Unity.Api;
using UnityEngine;
using Newtonsoft.Json.Linq;

namespace XMPro.Unity.Api
{
    [AddComponentMenu("XMPro/Entity")]
    public class EntityGeneric : EntityBase
    {
        [Tooltip("The variables used for this entity. Fill this out before registering the entity.")]
        public List<EntityDefinition> dataDefinitions;
        private List<EntityData> data;
        public override Dictionary<string, IoTTypes> DataDefinitions
        {
            get
            {
                Dictionary<string, IoTTypes> dict = new Dictionary<string, IoTTypes>();
                foreach(var def in dataDefinitions)
                {
                    dict.Add(def.name, def.type);
                }
                return dict;
            }
        }
        public override bool HasColors => false;

        public string WaterVelocity => this.ShortName + "_waterVelocity";

        public override void Receive(JObject obj)
        {
            try
            {
                data = new List<EntityData>();
                obj = JObject.Parse(obj["data"].ToString());
                foreach(var definition in dataDefinitions)
                {
                    if (obj.ContainsKey(definition.name))
                    {
                        data.Add(ParseData(definition, obj[definition.name].ToString()));
                    }
                }
            }
            catch
            {

            }
        }
        public EntityData ParseData(EntityDefinition definition, string data)
        {
            switch (definition.type)
            {
                case IoTTypes.Int32:
                    return new EntityData(definition.name, data.ToInt());
                case IoTTypes.Int64:
                    return new EntityData(definition.name, data.ToLong());
                case IoTTypes.Float:
                    return new EntityData(definition.name, data.ToFloat());
                case IoTTypes.DateTime:
                    return new EntityData(definition.name, data.ToDateTime());
                default:
                    return new EntityData(definition.name, data);
            }
        }

        public override string DataAsJson()
        {
            JObject json = new JObject();
            foreach(var dat in data)
            {
                json.Add(dat.name, JToken.FromObject(dat.value));
            }
            return json.ToString();
        }
    }
}
