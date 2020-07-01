using System.Collections.Generic;
using UnityEngine;

namespace XMPro.Unity
{
    public class GroupTargetSwap
    {
        List<ITargetter> uiElements;
        // Use this for initialization
        public GroupTargetSwap(List<ITargetter> elements)
        {
            uiElements = elements;
        }

        public void ChangeTarget(GameObject target)
        {
            foreach (var go in uiElements)
            {
                go.SetTarget(target);
            }
        }
    }
}
