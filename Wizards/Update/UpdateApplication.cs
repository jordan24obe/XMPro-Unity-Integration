using XMPro.Unity.Api;
using UnityEditor;
using System.Xml.Linq;

#if (UNITY_EDITOR)
public class UpdateApplication : ScriptableWizard
{
    private Application application;
    private bool applicationExists = false;
    private bool checkedForApplication = false;
    [UnityEngine.Header("Application Settings")]
    [UnityEngine.Tooltip("Name of the application displayed in XMPro.")]
    public string ApplicationName;
    [UnityEngine.Range(1, 600)]
    [UnityEngine.Tooltip("How often to make api requests for objects in scene, in seconds.")]
    public int TimeBetweenApiRequests = 3;


    [MenuItem("XMPro/Update/Application")]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard<UpdateApplication>("Update Application", "Update");
    }

    void OnWizardCreate()
    {
        var xmlPath = UnityEngine.Application.dataPath + "/Configuration/XMPro Configuration.xml";
        var xml = XDocument.Load(xmlPath);

        var token = xml.Element("Application").Element("Api").Element("Token").Value;
        var baseApiAddress = xml.Element("Application").Element("Api").Element("ApiRoute").Value;

        application.token = token;
        application.baseApiAddress = baseApiAddress;
        application.InitializeApi();
        if(application.name != ApplicationName)
        {
            XMPro.Unity.Api.WebRequest.PutApplication(application.baseApiAddress + $"/application/{application.Id}", application); 
        }
        if (TimeBetweenApiRequests != application.timeBetweenRequests)
            application.timeBetweenRequests = TimeBetweenApiRequests;
    }

    void OnWizardUpdate()
    {
        if (!applicationExists && !checkedForApplication)
        {
            try
            {
                application = UnityEngine.GameObject.FindGameObjectWithTag("Manager").GetComponent<Application>();
                applicationExists = true;
                checkedForApplication = true;
                ApplicationName = application.name;
                TimeBetweenApiRequests = application.timeBetweenRequests;
            }
            catch
            {
                //do nothing. We create later.
            }
        }
    }

}

#endif