using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XMPro.Unity.Api;
using Application = XMPro.Unity.Api.Application;

#if (UNITY_EDITOR)

public class RegisterEntitiesWizard : ScriptableWizard
{
    [SerializeField]
    public List<EntityBase> entities = new List<EntityBase>();
    public int entityCount;
    private Application application = null;
    [MenuItem("XMPro/Register/Entities")]
    public static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard<RegisterEntitiesWizard>("Register Entities", "Execute");
    }

    public void OnWizardCreate()
    {
        WebRequest.postRequestInProgess = false;
        application.InitializeApi();
        foreach (var entity in entities)
        {
            entity.StartCoroutine(WebRequest.PostRequest<IApiObject>(WebRequest.ApiBase + "/entities", entity));
           //entity.StartCoroutine(WebRequest.GetRequest<IApiObject>(WebRequest.ApiBase + $"/entities/{entity.name}", entity));
           // application.StartCoroutine(WebRequest.PutDefinitions(WebRequest.ApiBase + $"/definitions/{entity.Id}", entity));
        }
    }

    public void OnWizardUpdate()
    {
        if(application == null)
        {
            application = GameObject.FindGameObjectWithTag("Manager").GetComponent<Application>();
            GameObject.FindGameObjectsWithTag("Entity").ToList().ForEach(s => entities.Add(s.GetComponent<EntityBase>()));
            entityCount = entities.Count;
        }
    }

}
#endif