using System;
using System.Collections.Generic;
using UnityEngine;

namespace XMPro.Unity
{
    /// <summary>
    /// Component attached to a camera to allow for free movement within a scene.
    /// <para>Default position is determined by the start position of the camera. </para>
    /// </summary>
    [AddComponentMenu("XMPro/Camera/Mouse Free Fly Camera")]
    public class FlyCamera : CameraBase
    {
        public bool cameraLockedAtStart;

        [Serializable]
        public class FocalPoint
        {
            public KeyCode focusKey;
            public GameObject gameObject;
            public GameObject display;
        }
        private FocalPoint currentFocus;
        public List<FocalPoint> focalPoints;

        /*
        wasd : basic movement
        shift : Makes camera accelerate
        space : Moves camera on X and Z axis only.  So camera doesn't gain any height*/
        #region ResetCache
        Vector3 defaultPosition;
        Quaternion defaultRotation;
        #endregion ResetCache

        #region Configuration
        public float minCameraHeight = 50f;
        public KeyCode forwardMovement = KeyCode.W;
        public KeyCode backwardMovement = KeyCode.S;
        public KeyCode leftMovement = KeyCode.A;
        public KeyCode rightMovment = KeyCode.D;
        public KeyCode turbo = KeyCode.LeftShift; //Fast camera movement.
        [SerializeField]
        private float movementSpeed = 1f; //Speed of the camera.
        public float mouseSensitivity = 0.25f;
        public float turboSpeedAdded = 250.0f;
        public float turboMaxSpeedAdded = 1000.0f;

        public override bool Lock { get; set; } = false;
        public override float Speed
        {
            get => movementSpeed;
            set => movementSpeed = value;
        }

        #endregion Configuration

        private Vector3 lastMouse = new Vector3(255, 255, 255); //kind of in the middle of the screen, rather than at the top (play)
        private float totalRun = 1.0f;

        private void Start()
        {
            this.defaultPosition = this.transform.position;
            this.defaultRotation = this.transform.rotation;
        }

        void Update()
        {
            foreach( var focal in focalPoints)
            {
                if(Input.GetKey(focal.focusKey))
                {
                    this.transform.position = focal.gameObject.transform.position;
                    this.transform.rotation = focal.gameObject.transform.rotation;

                    if (currentFocus != null)
                        if (currentFocus.display != null)
                            currentFocus.display.transform.localScale = new Vector3(0, 0, 0);
                    currentFocus = focal;
                    if (focal.display != null)
                        focal.display.transform.localScale = new Vector3(1, 1, 1);
                }
            }
            if (Input.GetKeyUp(KeyCode.L))
                cameraLockedAtStart = !cameraLockedAtStart;
            if (Input.GetKeyUp(KeyCode.R))
            {
                this.transform.position = defaultPosition;
                this.transform.rotation = defaultRotation;
            }
            if (Input.GetKeyUp(KeyCode.C))
                this.transform.rotation = defaultRotation;
            if (!cameraLockedAtStart)
            {
                lastMouse = Input.mousePosition - lastMouse;
                lastMouse = new Vector3(-lastMouse.y * mouseSensitivity, lastMouse.x * mouseSensitivity, 0);
                lastMouse = new Vector3(ClampMax(transform.eulerAngles.x + lastMouse.x, 89), transform.eulerAngles.y + lastMouse.y, 0);
                transform.eulerAngles = lastMouse;
                lastMouse = Input.mousePosition;
                //Mouse  camera angle done.  

                //Keyboard commands
                Vector3 p = GetBaseInput();
                if (Input.GetKey(turbo))
                {
                    totalRun += Time.deltaTime;
                    p = p * totalRun * turboSpeedAdded;
                    p.x = Mathf.Clamp(p.x, -turboMaxSpeedAdded, turboMaxSpeedAdded);
                    p.y = Mathf.Clamp(p.y, -turboMaxSpeedAdded, turboMaxSpeedAdded);
                    p.z = Mathf.Clamp(p.z, -turboMaxSpeedAdded, turboMaxSpeedAdded);
                }
                else
                {
                    totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
                    p *= movementSpeed;
                }

                p *= Time.deltaTime;
                Vector3 newPosition = transform.position;
                if (Input.GetKey(KeyCode.Space))
                { //If player wants to move on X and Z axis only
                    transform.Translate(p);
                    newPosition.x = transform.position.x;
                    newPosition.z = transform.position.z;
                    transform.position = newPosition;
                }
                else
                {
                    transform.Translate(p);
                }
            }
            if (this.transform.position.y < minCameraHeight)
                this.transform.position = new Vector3(transform.position.x, minCameraHeight, transform.position.z);
        }

        private float ClampMax(float currentAngle, float maxAngle)
        {
            if(currentAngle > maxAngle)
            {
                currentAngle = maxAngle;
            }
            return currentAngle;
        }
        private Vector3 GetBaseInput()
        { //returns the basic values, if it's 0 than it's not active.
            Vector3 p_Velocity = new Vector3();
            if (Input.GetKey(forwardMovement))
            {
                p_Velocity += new Vector3(0, 0, 1 * movementSpeed);
            }
            if (Input.GetKey(backwardMovement))
            {
                p_Velocity += new Vector3(0, 0, -1 * movementSpeed);
            }
            if (Input.GetKey(leftMovement))
            {
                p_Velocity += new Vector3(-1 * movementSpeed, 0, 0);
            }
            if (Input.GetKey(rightMovment))
            {
                p_Velocity += new Vector3(1 * movementSpeed, 0, 0);
            }
            return p_Velocity;
        }

        public override void ResetCamera()
        {
            this.transform.position = defaultPosition;
            this.transform.rotation = defaultRotation;
        }
    }
}