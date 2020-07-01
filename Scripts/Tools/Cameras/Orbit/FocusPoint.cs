using UnityEngine;

namespace XMPro.Unity
{
    [System.Serializable]
    public class FocusPoint
    {
        public GameObject focus;
        public KeyCode primaryHotKey;
        public KeyCode secondaryHoyKey;
        
        public float xOrbit;
        public float yOrbit;
    }
}