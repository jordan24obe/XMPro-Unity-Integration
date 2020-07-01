using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace XMPro.Unity.Api
{
    public abstract class EntityBase : MonoBehaviour, IApiObject
    {
        public static float minimumTimeBetweenApiRequests = 1.0f;
        public static Application application;

        [SerializeField]
        /// <summary>
        /// entity unique identifier.
        /// </summary>
        protected int _id;

        public abstract bool HasColors { get; }

        public string ShortName => !string.IsNullOrEmpty(alias) ? alias : this.name.Split('.')[1];
        public int Id
        {
            get => _id;
            set => _id = value;
        }
        [Tooltip("The alias as seen in the entity definitions")]
        public string alias = "";

        /// <summary>
        /// Data defined by the name of the variable and it's type.
        /// </summary>
        public abstract Dictionary<string, IoTTypes> DataDefinitions { get; }
        public FrameTimer ApiTimer { get; private set; }
        public bool Finished { get; set; }

        /// <summary>
        /// Method to set api timer and the associated function.
        /// </summary>
        public void InitializeApiTimer()
        {
            ApiTimer = new FrameTimer(application.timeBetweenRequests);
            ApiTimer.Update += GetData;
        }

        /// <summary>
        /// When an api call is performed, this function is called to parse it's return object.
        /// </summary>
        /// <param name="obj">The object received. </param>
        public virtual void Receive(JObject obj)
        {
            this._id = int.Parse(obj["id"].ToString());
        }
        /// <summary>
        /// Get request for this entity using the id.
        /// </summary>
        public void GetData()
        {
            StartCoroutine(WebRequest.GetRequest<IApiObject>(WebRequest.ApiBase + $"/entities/{this.Id}", this));
        }
        public string UpdateAsJson()
        {
            return new JObject
            {
                { "Name", !string.IsNullOrEmpty(this.alias) ? this.alias : this.name },
                { "AppId", GameObject.FindGameObjectWithTag("Manager").GetComponent<Application>().Id}

            }.ToString();

        }
        /// <summary>
        /// Return this entity as json.
        /// </summary>
        public string AsJson()
        {
            return new JObject
            {
                { "id", this.Id},
                { "Name", !string.IsNullOrEmpty(this.alias) ? this.alias : this.name },
                { "Data", ""},
                { "AppId", GameObject.FindGameObjectWithTag("Manager").GetComponent<Application>().Id}
            }.ToString();
        }
        /// <summary>
        /// Return the data of the entity, packaged as a jobject.
        /// </summary>
        public abstract string DataAsJson();
        /// <summary>
        /// Return the definitions packaged as a jobject.
        /// </summary>
        public string DefinitionsAsJson()
        {
            JArray array = new JArray();
            foreach (var def in DataDefinitions)
            {
                array.Add(new JObject { { "Name", def.Key }, { "Type", def.Value.ToString() } });
            }
            JObject json = new JObject
            {
                { "entityId", this.Id },
                { "dataDefinitions", array.ToString() }
            };
            return json.ToString();
        }

        public virtual void ReceiveSettings(JArray obj)
        {

        }

        /// <summary>
        /// Set colors for an entity.
        /// </summary>
        /// <param name="colors">Colors in order of primary,secondary,tertiary</param>
        public virtual void SetColors(List<Color> colors)
        {

        }
    }
}
