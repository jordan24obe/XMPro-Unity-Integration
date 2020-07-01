using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XMPro.Unity.Api;

namespace XMPro.Unity.UI
{
    public class SlidingBar 
    {

        protected Slider slidingBar;
        protected float value;

        public SlidingBar(Slider bar, float startingValue = 0)
        {
            this.slidingBar = bar;
            this.value = startingValue;
        }

        public void Update(float value)
        {
            slidingBar.value = value;
        }
    }
}
