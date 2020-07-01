using UnityEngine;

namespace XMPro.Unity
{
    /// <summary>
    /// This script rotates an object around an axis. Only 1 rotation can be used at a time.
    /// </summary>
    [AddComponentMenu("XMPro/Object Controls/Auto Rotate")]
    public class ObjectAutoRotate : MonoBehaviour
    {
        [Tooltip("How fast to rotate the object each frame.")]
        public float rotationSpeed;
        [Tooltip("The axis to rotate on.")]
        public RotateAxis axis;

        // Update is called once per frame
        void Update()
        {
            Rotate();
        }

        /// <summary>
        /// Method to rotate in regards to rotation speed.
        /// </summary>
        public void Rotate()
        {
            switch(axis)
            {
                case RotateAxis.X:
                    transform.Rotate(new Vector3(rotationSpeed, 0f, 0f));
                    break;
                case RotateAxis.Y:
                    transform.Rotate(new Vector3(0f, rotationSpeed, 0f));
                    break;
                case RotateAxis.Z:
                    transform.Rotate(new Vector3(0f, 0f, rotationSpeed));
                    break;
            }
        }
    }
}
