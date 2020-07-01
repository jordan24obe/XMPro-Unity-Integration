using UnityEngine;
using System.Collections;

public abstract class CameraBase : MonoBehaviour
{
    /// <summary>
    /// Whether to lock the camera to it's current position/rotation
    /// </summary>
    public abstract bool Lock { get; set; }
    /// <summary>
    /// The current camera speed.
    /// </summary>
    public abstract float Speed { get; set; }
    /// <summary>
    /// Reset the camera to its base values.
    /// </summary>
    public abstract void ResetCamera();
}
