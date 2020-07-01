using UnityEngine;

namespace XMPro.Unity
{
    /// <summary>
    /// This script rotates an object based on a key press.
    /// </summary>
    [AddComponentMenu("XMPro/Object Controls/Manual Rotate")]
    public class ObjectManualRotate : MonoBehaviour
    {
        [Tooltip("The axis to rotate on.")]
        public RotateAxis axis;
        [Tooltip("The key being used to rotate.")]
        public KeyCode rotateControl = KeyCode.Mouse1;
        [Tooltip("How fast to rotate in the x direction.")]
        public float horizontalSpeed = 2.0f;
        [Tooltip("How fast to rotate in the y direction.")]
        public float verticalSpeed = 2.0f;

        public void Update()
        {
            if (Input.GetKey(rotateControl))
            {
                if (axis == RotateAxis.Y)
                {
                    float v = ClampAngle(verticalSpeed * Input.GetAxis("Mouse Y"));
                    transform.Rotate(v, 0, 0);
                }
                else
                {
                    float h = horizontalSpeed * Input.GetAxis("Mouse X");
                    transform.Rotate(0, 0, h);

                }
            }
        }
        public float ClampAngle(float angle)
        {
            if (angle < -360F)
                angle += 360F;
            if (angle > 360F)
                angle -= 360F;
            return angle;
            //return Mathf.Clamp(angle, min, max);
        }
    }
}