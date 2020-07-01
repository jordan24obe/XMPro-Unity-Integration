using UnityEditor;
using UnityEngine;

namespace XMPro.Unity
{
    /// <summary>
    /// This script uses the transform look at function to face an object.
    /// </summary>
    [AddComponentMenu("XMPro/Object Controls/Look At")]
    public class LookAtObject : MonoBehaviour
    {
        [Tooltip("Object to look at.")]
        public GameObject target;
        [Tooltip("Object being manipulated.")]
        public GameObject rotateObject;
        [Header("Lock Axis")]
        public bool lockX;
        public int lockXRotation;
        public bool lockY;
        public int lockYRotation;
        public bool lockZ;
        public int lockZRotation;

        private void Start()
        {
            rotateObject.transform.LookAt(target.transform);
            FaceTarget();
        }

        private void Update()
        {
            rotateObject.transform.LookAt(target.transform);
            FaceTarget();
        }
        /// <summary>
        /// Method to face the target object, with regards to locking.
        /// </summary>
        public void FaceTarget()
        {
            rotateObject.transform.LookAt(target.transform);
            rotateObject.transform.eulerAngles = new Vector3
            (
                !lockX ? rotateObject.transform.eulerAngles.x : lockXRotation,
                !lockY ? rotateObject.transform.eulerAngles.y : lockYRotation,
                !lockZ ? rotateObject.transform.eulerAngles.z : lockZRotation
            );
        }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(LookAtObject))]
    public class LookAtObjectEditor : Editor
    {
        override public void OnInspectorGUI()
        {
            var myScript = target as LookAtObject;

            myScript.lockX = GUILayout.Toggle(myScript.lockX, "Lock X");
            if (myScript.lockX)
                myScript.lockXRotation = EditorGUILayout.IntSlider("X Rotation: ", myScript.lockXRotation, 0, 359);
            myScript.lockY = GUILayout.Toggle(myScript.lockY, "Lock Y");
            if (myScript.lockY)
                myScript.lockYRotation = EditorGUILayout.IntSlider("Y Rotation: ", myScript.lockYRotation, 0, 359);
            myScript.lockZ = GUILayout.Toggle(myScript.lockZ, "Lock Z");
            if (myScript.lockZ)
                myScript.lockZRotation = EditorGUILayout.IntSlider("Z Rotation: ", myScript.lockZRotation, 0, 359);



        }
    }
#endif
}