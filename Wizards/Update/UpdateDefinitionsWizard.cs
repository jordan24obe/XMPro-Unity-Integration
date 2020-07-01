using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using XMPro.Unity.Api;
using Application = XMPro.Unity.Api.Application;
#if (UNITY_EDITOR)

public class UpdateDefinitionsWizard : ScriptableWizard
{
    [SerializeField]
    public List<EntityBase> entities = new List<EntityBase>();
    public int entityCount;
    private Application application = null;
    [MenuItem("XMPro/Update/Entity Definitions")]
    public static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard<UpdateDefinitionsWizard>("Update Entity Definitions", "Execute");
    }

    void OnWizardCreate()
    {
        WebRequest.postRequestInProgess = false;
        application.InitializeApi();
        foreach (var entity in entities)
        {
            entity.StartCoroutine(WebRequest.PutDefinitions(WebRequest.ApiBase + $"/definitions", entity));
        }
    }

    void OnWizardUpdate()
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