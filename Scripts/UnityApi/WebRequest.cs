using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;
using Newtonsoft.Json.Linq;
using System.Text;

namespace XMPro.Unity.Api
{
    public static class WebRequest
    {
        public static bool postRequestInProgess = false;
        private static string token;
        public static string Token
        {
            get => "bearer " + token;
            internal set => token = value;
        }
        public static string ApiBase { get; internal set; }

        public static void UpdateBaseRoute(string baseRoute)
        {
            ApiBase = baseRoute;
        }

        public static IEnumerator GetManyRequest(string uri, Application app)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                webRequest.SetRequestHeader("Authorization", Token);
                yield return webRequest.SendWebRequest();

                if (webRequest.isNetworkError)
                {
                    throw new Exception("Network error occurred.");
                }
                else
                {
                    app.ReceiveBatch(JArray.Parse(GetWebRequestText(webRequest)));
                }

            }
        }
        public static IEnumerator GetRequest<ApiObject>(string uri, ApiObject gameObject) where ApiObject : IApiObject
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                webRequest.SetRequestHeader("Authorization", Token);
                yield return webRequest.SendWebRequest();

                if (webRequest.isNetworkError)
                {
                    throw new Exception("Network error occurred.");
                }
                else
                {
                    gameObject.Receive(JObject.Parse(GetWebRequestText(webRequest)));
                }

            }
        }
        public static IEnumerator GetSettingsRequest<ApiObject>(string uri, ApiObject gameObject) where ApiObject : IReceivesSettings
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                webRequest.SetRequestHeader("Authorization", Token);
                yield return webRequest.SendWebRequest();

                if (webRequest.isNetworkError)
                {
                    throw new Exception("Network error occurred.");
                }
                else
                {
                    try
                    {
                        gameObject.ReceiveSettings(JArray.Parse(GetWebRequestText(webRequest)));
                    }
                    catch
                    {

                    }
                }
            }
        }
        public static IEnumerator PutRequest(string uri, IApiObject apiObject)
        {

            using (UnityWebRequest webRequest = UnityWebRequest.Put(uri + $"/{apiObject.Id}", apiObject.DataAsJson()))
            {
                webRequest.SetRequestHeader("Content-Type", "application/json");
                webRequest.SetRequestHeader("Authorization", Token);

                Debug.Log("Sending request");
                yield return webRequest.SendWebRequest();
                if (webRequest.isNetworkError || webRequest.isHttpError)
                {
                    Debug.Log(webRequest.error);
                }
                else
                {
                    //success
                }
            }
        }
        public static IEnumerator PutApplication(string uri, Application application)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Put(uri, application.UpdateAsJson()))
            {
                webRequest.SetRequestHeader("Content-Type", "application/json");
                webRequest.SetRequestHeader("Authorization", Token);
                webRequest.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(application.UpdateAsJson())); //upload the json to the server before sending request.

                yield return webRequest.SendWebRequest();
                if (webRequest.isNetworkError || webRequest.isHttpError)
                {
                    Debug.Log(webRequest.error);
                }
                else
                {

                }
            }
        }
        public static IEnumerator PutEntity(string uri, EntityBase entity)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Put(uri, entity.UpdateAsJson()))
            {
                webRequest.SetRequestHeader("Content-Type", "application/json");
                webRequest.SetRequestHeader("Authorization", Token);
                webRequest.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(entity.UpdateAsJson())); //upload the json to the server before sending request.

                yield return webRequest.SendWebRequest();
                if (webRequest.isNetworkError || webRequest.isHttpError)
                {
                    Debug.Log(webRequest.error);
                }
                else
                {

                }
            }
        }
        public static IEnumerator PutRequest(string uri, string message)
        {

            using (UnityWebRequest webRequest = UnityWebRequest.Put(uri, message))
            {
                webRequest.SetRequestHeader("Content-Type", "application/json");
                webRequest.SetRequestHeader("Authorization", Token);
                webRequest.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(message)); //upload the json to the server before sending request.

                yield return webRequest.SendWebRequest();
                if (webRequest.isNetworkError || webRequest.isHttpError)
                {
                    Debug.Log(webRequest.error);
                }
                else
                {
                    //success
                }
            }
        }
        public static IEnumerator PutDefinitions(string uri, IApiObject apiObject)
        {

            using (UnityWebRequest webRequest = UnityWebRequest.Put(uri + $"/{apiObject.Id}", apiObject.DefinitionsAsJson()))
            {
                webRequest.SetRequestHeader("Content-Type", "application/json");
                webRequest.SetRequestHeader("Authorization", Token);
                webRequest.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(apiObject.DefinitionsAsJson())); //upload the json to the server before sending request.

                Debug.Log("Sending request");
                yield return webRequest.SendWebRequest();
                if (webRequest.isNetworkError || webRequest.isHttpError)
                {
                    Debug.Log(webRequest.error);
                }
                else
                {
                    Debug.Log($"Entity Id {apiObject.Id} Definitions Created through API.");
                }
            }
        }
        public static IEnumerator PostEntity(string uri, IApiObject apiObject)
        {
            while (postRequestInProgess)
                yield return new WaitForSeconds(.5f);
            postRequestInProgess = true;
            using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, apiObject.DefinitionsAsJson()))
            {
                webRequest.SetRequestHeader("Content-Type", "application/json");
                webRequest.SetRequestHeader("Authorization", Token);
                webRequest.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(apiObject.DefinitionsAsJson())); //upload the json to the server before sending request.

                Debug.Log("Sending request");
                yield return webRequest.SendWebRequest();
                if (webRequest.isNetworkError || webRequest.isHttpError)
                {
                    Debug.Log(webRequest.error);
                }
                else
                {
                    ((EntityBase)apiObject).Receive(JObject.Parse(GetWebRequestText(webRequest))); //set id
                    PutDefinitions(uri, apiObject); //set definitions right after.
                    Debug.Log($"Entity Id {apiObject.Id} Created through API.");
                }
                postRequestInProgess = false;
            }
        }
        public static IEnumerator PostDefinitions(string uri, IApiObject apiObject)
        {
            while (postRequestInProgess)
                yield return new WaitForSeconds(.5f);
            postRequestInProgess = true;
            using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, apiObject.DefinitionsAsJson()))
            {
                webRequest.SetRequestHeader("Content-Type", "application/json");
                webRequest.SetRequestHeader("Authorization", Token);
                webRequest.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(apiObject.DefinitionsAsJson())); //upload the json to the server before sending request.

                Debug.Log("Sending request");
                yield return webRequest.SendWebRequest();
                if (webRequest.isNetworkError || webRequest.isHttpError)
                {
                    Debug.Log(webRequest.error);
                }
                else
                {
                    //success
                }
                postRequestInProgess = false;
            }
        }
        public static IEnumerator Authenticate(string uri, string username, string password)
        {
            var json = new JObject { { "username", username }, { "password", password } }.ToString();
            using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, json))
            {
                webRequest.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json)); //upload the json to the server before sending request.
                webRequest.SetRequestHeader("Content-Type", "application/json");
                webRequest.SetRequestHeader("Accept", "application/json");

                yield return webRequest.SendWebRequest();

                if (webRequest.isNetworkError)
                {
                    throw new Exception("Network error occurred. Please confirm the uri, username, and password are accurate.");
                }
                else
                {
                    //success
                    Token = "Bearer " + JObject.Parse(webRequest.downloadHandler.text)["token"].ToString();
                    yield return webRequest.downloadHandler.text;
                }

            }

        }
        public static IEnumerator PostRequest<ApiObject>(string uri, ApiObject gameObject) where ApiObject : IApiObject
        {
            while (postRequestInProgess)
                yield return new WaitForSeconds(.5f);
            postRequestInProgess = true;
            using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, gameObject.AsJson()))
            {
                webRequest.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(gameObject.AsJson())); //upload the json to the server before sending request.
                webRequest.SetRequestHeader("Content-Type", "application/json");
                webRequest.SetRequestHeader("Authorization", Token);
                yield return webRequest.SendWebRequest();
                if (webRequest.isNetworkError)
                {
                    throw new Exception("Network error occurred.");
                }
                else
                {
                    try
                    {
                        gameObject.Receive(JObject.Parse(GetWebRequestText(webRequest)));
                    }
                    catch
                    {
                        Debug.Log("Error parsing data");
                    }
                    //success
                }
                postRequestInProgess = false;
            }
        }
        public static IEnumerator PostRequest(string uri, string message)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, message))
            {
                webRequest.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(message)); //upload the json to the server before sending request.
                webRequest.SetRequestHeader("Content-Type", "application/json");
                webRequest.SetRequestHeader("Authorization", Token);
                yield return webRequest.SendWebRequest();

                if (webRequest.isNetworkError)
                {
                    throw new Exception("Network error occurred.");
                }
                else
                {
                    //success
                }

            }
        }

        private static string GetWebRequestText(UnityWebRequest request)
        {
            return request.downloadHandler.text;
        }
    }
}