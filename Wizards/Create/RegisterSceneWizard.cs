using System.Linq;
using System.Xml.Linq;
using UnityEditor;
using XMPro.Unity.Api;
#if(UNITY_EDITOR)

public class RegisterSceneWizard : ScriptableWizard
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
    
    [MenuItem("XMPro/Register/Application")]
    public static void CreateWizard()
    {
        var wiz = ScriptableWizard.DisplayWizard<RegisterSceneWizard>("Register Application", "Execute");
        wiz.maxSize = new UnityEngine.Vector2(400f, 200f);
    }

    public void OnWizardCreate()
    {
        if(!applicationExists)
        {
            var go = new UnityEngine.GameObject("Application")
            {
                tag = "Manager"
            };
            application = go.AddComponent<Application>();
        }

        var xmlPath = UnityEngine.Application.dataPath + "/Configuration/XMPro Configuration.xml";
        var xml = XDocument.Load(xmlPath);

        application.applicationName = this.ApplicationName;
        application.timeBetweenRequests = TimeBetweenApiRequests;
        application.token = xml.Element("Application").Element("Api").Element("Token").Value;
        application.baseApiAddress = xml.Element("Application").Element("Api").Element("ApiRoute").Value;

        var scenes = xml.Element("Application").Element("Scenes");
        var scene = scenes.Elements().FirstOrDefault(element => element.Name.LocalName == UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Replace(" ", "_"));
        if (scene != null)
        {
            application.unityId = scene.Attribute("Id").Value;
        }
        else
        {
            application.unityId = System.Guid.NewGuid().ToString();
            var newXElement = new XElement(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Replace(" ", "_"));
            newXElement.SetAttributeValue("Id", application.unityId);
            newXElement.SetAttributeValue("AppName", application.applicationName.Replace(" ", "_"));
            scenes.Add(newXElement);
            xml.Save(xmlPath);
            application.CreateApplication();
        }      
        application.GetAppId();
    }

    private void OnWizardOtherButton()
    {
        var xmlPath = UnityEngine.Application.dataPath + "/Configuration/XMPro Configuration.xml";
        var xml = XDocument.Load(xmlPath);
        application.applicationName = this.ApplicationName;
        application.timeBetweenRequests = TimeBetweenApiRequests;
        application.token = xml.Element("Application").Element("Api").Element("Token").Value;
        application.baseApiAddress = xml.Element("Application").Element("Api").Element("ApiRoute").Value;

        if (application.unityId == null)
        {
            var scenes = xml.Element("Application").Element("Scenes");
            var scene = scenes.Elements().FirstOrDefault(element => element.Name.LocalName == UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Replace(" ", "_"));
            if (scene != null)
            {
                application.unityId = scene.Attribute("Id").Value;
                scene.SetAttributeValue("AppName", application.applicationName.Replace(" ", "_"));
                xml.Save(xmlPath);
            }
            else
            {
                application.unityId = System.Guid.NewGuid().ToString();
                var newXElement = new XElement(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Replace(" ", "_"));
                newXElement.SetAttributeValue("Id", application.unityId);
                newXElement.SetAttributeValue("AppName", application.applicationName.Replace(" ", "_"));
                scenes.Add(newXElement);
                xml.Save(xmlPath);
            }
        }


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