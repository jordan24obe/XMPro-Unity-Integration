using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace XMPro.Unity.Api
{
    [AddComponentMenu("XMPro/Application")]
    public class Application : MonoBehaviour, IApiObject, IHasSettings, IReceivesSettings
    {
        public static string JsonPrimaryColor = "primaryColor";
        public static string JsonSecondaryColor = "secondaryColor";
        public static string JsonTertiaryColor = "tertiaryColor";
        public static string JsonTimeBetweenRequests = "timeBetweenRequests";
        public static string JsonCameraBackgroundColor = "cameraBackgroundColor";
        public string ShortName => name;
        [Header("Application Details")]
        public string applicationName;
        [Header("Api Details")]
        [Tooltip("The base api address data is accessed from. (Given by XMPro)")]
        public string baseApiAddress;
        [Tooltip("How often to query for new data within the application.")]
        public int timeBetweenRequests;
        [Header("Application Settings")]
        public Color primaryColor;
        public Color secondaryColor;
        public Color tertiaryColor;

        [HideInInspector]
        public string token;

        public Settings applicationSettings;
        public Dictionary<int, Settings> entitySettings;

        public List<GameObject> settingsObjects;
        private List<EntityBase> entities;

        public List<EntityBase> Entities
        {
            get
            {
                if(entities == null)
                {
                    var gameObjects = GameObject.FindGameObjectsWithTag("Entity");
                    entities = new List<EntityBase>();
                    foreach (var entity in gameObjects)
                    {
                        entities.Add(entity.GetComponent<EntityBase>());
                    }
                }
                return entities;

            }
        }
        [HideInInspector]
        public string unityId;
        [HideInInspector]
        public int _id;
        public int Id => _id;

        public string JsonName => "ApplicationGlobal";

        public Dictionary<string, object> SettingDefinitions => new Dictionary<string, object>()
        {
            { JsonPrimaryColor, primaryColor.ToJsonColor()},
            { JsonSecondaryColor, secondaryColor.ToJsonColor() },
            { JsonTertiaryColor, tertiaryColor.ToJsonColor() },
            { JsonTimeBetweenRequests, timeBetweenRequests },
            { JsonCameraBackgroundColor, Camera.main.backgroundColor.ToJsonColor() }
        };

        void OnEnable()
        {
            if (baseApiAddress == "")
            {
                throw new System.Exception("base api address cannot be empty.");

            }
            if (token == "")
            {
                throw new System.Exception("api token cannot be empty.");
            }
            if (unityId == "")
            {
                this.unityId = System.Guid.NewGuid().ToString();
            }
            if (applicationName == "")
            {
                throw new System.Exception("Application name cannot be empty.");
            }
            InitializeApi();
            GetApplicationSettings();
            EntityBase.application = this;
           
        }

        public void InitializeApi()
        {
            WebRequest.ApiBase = baseApiAddress;
            WebRequest.Token = token;
        }

        public void GetEntities()
        {
            StartCoroutine(WebRequest.GetManyRequest(WebRequest.ApiBase + $"/applications/{this.Id}/data", this));
        }

        public void ReceiveBatch(JArray array)
        {
            foreach(JObject obj in array)
            {
                Entities.First(e => e.Id == int.Parse(obj["id"].ToString())).Receive(obj);
            }
        }
        public void CreateApplication()
        {
            InitializeApi();
            StartCoroutine(WebRequest.PostRequest(WebRequest.ApiBase + $"/applications", this));
        }
        public void UpdateApplication()
        {
            InitializeApi();
            StartCoroutine(WebRequest.PutRequest(WebRequest.ApiBase + $"/applications/{this.unityId}", this));
        }

        public void GetAppId()
        {
            StartCoroutine(WebRequest.GetRequest(WebRequest.ApiBase + $"/applications/{this.unityId}", this));

        }

        public void GetApplicationSettings()
        {
            StartCoroutine(WebRequest.GetSettingsRequest(WebRequest.ApiBase + $"/settings/application/{this.unityId}", this));
        }

        public void Receive(JObject obj)
        {
            this._id = int.Parse(obj["id"].ToString());
        }
        public void ReceiveSettings(JArray obj)
        {
            try
            {
                applicationSettings = new Settings(obj);
                foreach (var go in settingsObjects)
                {
                    go.GetComponent<IHasSettings>().ParseSettings(applicationSettings.GetSetting(go.GetComponent<IHasSettings>().JsonName));
                }
            }
            catch
            {
                //error parsing settings
            }
        }
        public string UpdateAsJson()
        {
            JObject jo = new JObject
            {
                { "Name", this.applicationName }
            };
            return jo.ToString();
        }
        public string AsJson()
        {
            JObject json = new JObject
            {
                { "UnityId", this.unityId },
                { "Name", this.applicationName },
                { "Id", this.Id },
                { "CreatedTime", null },
                { "CreatedUser", null },
                { "Published", null },
                { "PublishedTime", null }
            };
            return json.ToString();
        }
        public string DataAsJson()
        {
            JObject json = new JObject
            {
                { "Name", this.applicationName }
            };
            return json.ToString();
        }
        public string DefinitionsAsJson() //unneeded
        {
            throw new System.NotImplementedException();
        }

        public void ParseSettings(JObject obj)
        {
            ColorUtility.TryParseHtmlString(obj[JsonPrimaryColor].ToString(), out primaryColor);
            ColorUtility.TryParseHtmlString(obj[JsonSecondaryColor].ToString(), out secondaryColor);
            ColorUtility.TryParseHtmlString(obj[JsonTertiaryColor].ToString(), out tertiaryColor);
            timeBetweenRequests = int.Parse(obj[JsonTimeBetweenRequests].ToString());
            List<Color> colors = new List<Color>() { primaryColor, secondaryColor, tertiaryColor };
            ColorUtility.TryParseHtmlString(obj[JsonCameraBackgroundColor].ToString(), out Color bgColor);
            Camera.main.backgroundColor = bgColor;

            foreach (var item in Entities)
            {
                if (item.HasColors)
                    item.SetColors(colors);
                item.InitializeApiTimer();
            }
        }
    }
}