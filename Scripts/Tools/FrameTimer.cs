using UnityEngine;

namespace XMPro.Unity
{
    /// <summary>
    /// Class to measure time within the update function.
    /// </summary>
    public class FrameTimer
    {
        public delegate void OnUpdate();

        public OnUpdate Update;

        public float TimeToWait { get; set; }

        private float timeRemaining;

        public FrameTimer(float timeToWait)
        {
            this.TimeToWait = timeToWait;
            timeRemaining = timeToWait;
        }

        public void Invoke()
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining < 0)
            {
                Update();
                Reset();
            }
        }
        public void Reset()
        {
            timeRemaining = TimeToWait;
        }
    }
}