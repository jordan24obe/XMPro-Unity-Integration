using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;
using XMPro.Unity.Api;
using XMPro.Unity.Settings;

namespace XMPro.Unity
{
    /// <summary>
    /// Component attached to the camera to allow for orbiting around a target.
    /// </summary>
    [AddComponentMenu("XMPro/Camera/Mouse Orbit with zoom")]
    [RequireComponent(typeof(OrbitCameraSettings))]
    public class OrbitCamera : CameraBase, IHasSettings
    {
        public static string JsonDistanceFromTarget = "distanceFromTarget";
        public static string JsonXAxisSpeed = "xAxisSpeed";
        public static string JsonYAxisSpeed = "yAxisSpeed";
        public static string JsonYMinimumLimit = "yMinimumLimit";
        public static string JsonYMaximumLimit = "yMaximumLimit";
        public static string JsonDistanceMinimum = "distanceMinimum";
        public static string JsonDistanceMaximum = "distanceMaximum";
        public static string JsonCameraOffset = "cameraOffset";

        #region Reset Cache
        private Vector3 defaultPosition;
        private Quaternion defaultRotation;
        private Transform defaultTarget;
        private Vector3 previousPosition;
        private Quaternion previousRotation;
        #endregion Reset Cache

        [Header("Camera Settings")]
        [Tooltip("Focus points are set camera angles that show important features.")]
        public FocusPoints focusPoints;
        
        [Tooltip("The target that is being rotated around.")]
        public Transform target;
        [Tooltip("The camera offset at which the camera rotates around the target.")]
        public Vector3 offset = new Vector3(0,0,0);
        [Space]
        [Header("Control Settings")]
        public LayerMask layerMask;
        [Tooltip("The key that allows the camera to rotate.")]
        public KeyCode allowRotateControl = KeyCode.Mouse1;
        [HideInInspector]
        public float distanceFromTarget = 5.0f;
        [Tooltip("The speed of the camera as it rotates in the x direction.")]
        public float xAxisSpeed = 22f;
        [Tooltip("The speed of the camera as it rotates in the y direction.")]
        public float yAxisSpeed = 22f;
        [Tooltip("The minimum y distance from the target.")]
        public float yMininimumLimit = -20f;
        [Tooltip("The maximum y distance from the target.")]
        public float yMaximumLimit = 80f;
        [Tooltip("The minimum distance between the camera and the target.")]
        public float distanceMinimum = .5f;
        [Tooltip("The maximum distance between the camera and the target.")]
        public float distanceMaximum = 15f;

        private new Rigidbody rigidbody; //physics component of the camera.
        [HideInInspector]
        public int currentFocusIndex = 0;
        [HideInInspector]
        public GameObject currentFocusObject;
        [HideInInspector]
        public float x = 0.0f;
        [HideInInspector]
        public float y = 0.0f;

        public string JsonName => this.name.Replace(' ', '_');
        public override bool Lock { get; set; }
        public override float Speed
        {
            get => xAxisSpeed;
            set
            {
                xAxisSpeed = value;
                yAxisSpeed = value;
            }
        }

        public Dictionary<string, object> SettingDefinitions => new Dictionary<string, object>()
        {
            { JsonDistanceFromTarget, 12.5 },
            { JsonXAxisSpeed, 22 },
            { JsonYAxisSpeed, 60 },
            { JsonYMinimumLimit, 13 },
            { JsonYMaximumLimit, 40 },
            { JsonDistanceMinimum, 12.75 },
            { JsonDistanceMaximum, 12.75 },
            { JsonCameraOffset, offset.ToJsonVector3()}
        };


        // Use this for initialization
        void Start()
        { 
            this.defaultRotation = this.transform.rotation;
            this.defaultTarget = this.target;

            Vector3 angles = transform.eulerAngles;
            x = angles.y;
            y = angles.x;

            rigidbody = GetComponent<Rigidbody>();

            // Make the rigid body not change rotation
            if (rigidbody != null)
            {
                rigidbody.freezeRotation = true;
            }
            StandardRotateCamera();
        }

        void LateUpdate()
        {
            previousPosition = this.transform.position;
            previousRotation = this.transform.rotation;
            if (target && Input.GetKey(allowRotateControl))
            {
                previousPosition = this.transform.position;
                previousRotation = this.transform.rotation;
                StandardRotateCamera();
            }
            else if (target)
            {
                for (int i = 0; i < focusPoints.focusPoints.Count; i++)
                {
                    if (Input.GetKey(focusPoints.focusPoints[i].primaryHotKey) || Input.GetKey(focusPoints.focusPoints[i].secondaryHoyKey))
                    {
                        currentFocusIndex = i;
                        currentFocusObject = focusPoints.focusPoints[i].focus;
                        focusPoints.SetFocus(i);
                        ForceRotateCamera();
                    }
                }
            }
        }

        public float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360F)
                angle += 360F;
            if (angle > 360F)
                angle -= 360F;
            return Mathf.Clamp(angle, min, max);
        }

        public override void ResetCamera()
        {
            this.transform.position = defaultPosition;
            this.transform.rotation = defaultRotation;
            this.target = defaultTarget;
        }

        public void ForceRotateCamera()
        {
            y = ClampAngle(y, yMininimumLimit, yMaximumLimit);

            Quaternion rotation = Quaternion.Euler(y, x, 0);

            distanceFromTarget = Mathf.Clamp(distanceFromTarget - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMinimum, distanceMinimum);
            if (Physics.Linecast(target.position, previousPosition, out RaycastHit hit, layerMask))
            {
                distanceFromTarget -= hit.distance;
            }
            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distanceFromTarget);
            Vector3 position = rotation * negDistance + target.position;

            transform.rotation = rotation;
            transform.position = position + offset;
        }

        public void StandardRotateCamera()
        {
            x += Input.GetAxis("Mouse X") * xAxisSpeed * distanceFromTarget * 0.02f;
            y -= Input.GetAxis("Mouse Y") * yAxisSpeed * 0.02f;

            y = ClampAngle(y, yMininimumLimit, yMaximumLimit);

            Quaternion rotation = Quaternion.Euler(y, x, 0);

            distanceFromTarget = Mathf.Clamp(distanceFromTarget - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMinimum, distanceMaximum);

            if (Physics.Linecast(target.position, previousPosition, out RaycastHit hit, layerMask))
            {
                distanceFromTarget -= hit.distance;
            }

            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distanceFromTarget);
            Vector3 position = rotation * negDistance + target.position;

            transform.rotation = rotation;
            transform.position = position + offset;
        }

        public void SetFocus(GameObject obj)
        {
            for (int i = 0; i < focusPoints.focusPoints.Count; i++)
            {
                if(focusPoints.focusPoints[i].focus == obj)
                { 
                    currentFocusIndex = i;
                    currentFocusObject = focusPoints.focusPoints[i].focus;
                    focusPoints.SetFocus(i);
                    ForceRotateCamera();
                }
            }
        }

        public void ParseSettings(JObject obj)
        {
            distanceFromTarget = float.Parse(obj[JsonDistanceFromTarget].ToString());
            xAxisSpeed = float.Parse(obj[JsonXAxisSpeed].ToString());
            yAxisSpeed = float.Parse(obj[JsonYAxisSpeed].ToString());
            yMininimumLimit = float.Parse(obj[JsonYMinimumLimit].ToString());
            yMaximumLimit = float.Parse(obj[JsonYMaximumLimit].ToString());
            distanceMaximum = float.Parse(obj[JsonDistanceMaximum].ToString());
            distanceMinimum = float.Parse(obj[JsonDistanceMinimum].ToString());
            offset = obj[JsonCameraOffset].ToString().ToVector3();
        }
    }

}