using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using XMPro.Unity.Api;
using Application = XMPro.Unity.Api.Application;

#if (UNITY_EDITOR)

public class UpdateEntities : ScriptableWizard
{
    [SerializeField]
    public List<EntityBase> entities = new List<EntityBase>();
    public int entityCount;
    private Application application = null;
    [MenuItem("XMPro/Update/Entities")]
    public static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard<RegisterEntitiesWizard>("Update Entities", "Execute");
    }

    public void OnWizardCreate()
    {
        application.InitializeApi();
        foreach (var entity in entities)
        {
            entity.StartCoroutine(WebRequest.PutEntity(WebRequest.ApiBase + $"/entities/{entity.Id}", entity)); 
        }
    }

    public void OnWizardUpdate()
    {
        if (application == null)
        {
            application = GameObject.FindGameObjectWithTag("Manager").GetComponent<Application>();
            GameObject.FindGameObjectsWithTag("Entity").ToList().ForEach(s => entities.Add(s.GetComponent<EntityBase>()));
            entityCount = entities.Count;
        }
    }

}
#endif
