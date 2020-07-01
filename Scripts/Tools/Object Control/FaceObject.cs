using UnityEngine;

namespace XMPro.Unity
{
    /// <summary>
    /// This script uses the look rotation with angles to face an object's direction.
    /// </summary>
    [AddComponentMenu("XMPro/Object Controls/Face Object")]
    public class FaceObject : MonoBehaviour
    {
        [Tooltip("The game object to look at.")]
        public GameObject target;
        [Tooltip("How fast to snap to the target.")]
        public float rotSpeed = 250;
        [Tooltip("Option to lock the x axis when looking at target.")]
        public bool lockX;
        [Tooltip("Option to lock the y axis when looking at target.")]
        public bool lockY;
        [Tooltip("Option to lock the z axis when looking at target.")]
        public bool lockZ;
        // Use this for initialization
        void Start()
        {
            Face(target, lockX, lockY, lockZ);
        }

        // Update is called once per frame
        void Update()
        {
            Face(target, lockX, lockY, lockZ);
        }

        public void Face(GameObject target, bool x, bool y, bool z)
        {
            var previousRotation = transform.rotation.eulerAngles;
            this.transform.rotation = Quaternion.LookRotation(target.transform.position, Vector3.forward);
            this.transform.eulerAngles = new Vector3(x ? previousRotation.x : transform.eulerAngles.x, y ? previousRotation.y : transform.eulerAngles.y + 180, z ? previousRotation.z : transform.eulerAngles.z);
        }
    }
}