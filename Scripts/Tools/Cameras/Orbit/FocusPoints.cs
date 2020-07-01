using System;
using System.Collections.Generic;
using UnityEngine;

namespace XMPro.Unity
{
    [Serializable]
    public class FocusPoints
    {
        public int currentFocus;
        public OrbitCamera parent;
        [SerializeField]
        public List<FocusPoint> focusPoints;

        public int Count => focusPoints.Count;
        // Update is called once per frame
        public void SetFocus(int focusIndex)
        {
            try
            {
                var focusPoint = focusPoints[focusIndex];

                parent.x = focusPoint.xOrbit;
                parent.y = focusPoint.yOrbit;
            }
            catch
            {

            }
        }
    }
}