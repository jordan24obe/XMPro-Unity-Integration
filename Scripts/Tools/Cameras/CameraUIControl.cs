using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using static UnityEngine.UI.Button;
using static UnityEngine.UI.Toggle;

public class CameraUIControl : MonoBehaviour
{
    /*
        Map each button so the values can be accessed. 
    */
    public Button resetCameraButton;
    public Toggle lockCameraToggle;
    public InputField cameraSpeedField;
    public CameraBase cameraTarget;

    // Use this for initialization
    void Start()
    {
        cameraTarget = GameObject.Find("Main Camera").GetComponent<CameraBase>();
        lockCameraToggle.image.transform.Find("Checkmark").GetComponent<Image>().enabled = lockCameraToggle.isOn;
        cameraSpeedField.characterValidation = InputField.CharacterValidation.Integer;
        cameraSpeedField.onValueChanged.AddListener(OnSpeedChange);
        resetCameraButton.onClick.AddListener(OnResetClicked);
        lockCameraToggle.onValueChanged.AddListener(OnCameraLockToggle);
    }

    public void Update()
    {
        if(Input.GetKeyUp(KeyCode.L))
        {
            lockCameraToggle.onValueChanged.Invoke(!cameraTarget.Lock);
        }
        if(Input.GetKeyUp(KeyCode.R))
        {
            resetCameraButton.onClick.Invoke();
        }
    }

    private void OnSpeedChange(string value)
    {
        cameraTarget.Speed = int.Parse(value);
    }

    private void OnCameraLockToggle(bool value)
    {
        cameraTarget.Lock = value;
        lockCameraToggle.image.transform.Find("Checkmark").GetComponent<Image>().enabled = value;
    }
    private void OnResetClicked()
    {
        cameraTarget.ResetCamera();
    }
    
}
