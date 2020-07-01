using UnityEngine;
using System.Collections;

namespace XMPro.Unity
{
    public enum ViewportSplitType
    {
        horizontal,
        vertical
    }
    public class SplitViewport : MonoBehaviour
    {
        public ViewportSplitType splitType;
        public UnityEngine.Camera topCam;
        public UnityEngine.Camera botCam;
        // Use this for initialization
        void Start()
        {
            topCam.rect = new Rect(.5f, 0, .5f, 1);
            botCam.rect = new Rect(0, 0, 0.5f, 1);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
