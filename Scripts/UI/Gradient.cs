using UnityEngine;

namespace XMPro.Unity.UI
{
    /// <summary>
    /// Class to have a changing color based on a gradient.
    /// </summary>
    public class Gradient
    {
        GradientColorKey[] gck;
        GradientAlphaKey[] gak;
        UnityEngine.Gradient gradient;

        public Gradient(Color colorLow, Color colorHigh)
        {
            this.gradient = new UnityEngine.Gradient();
            GradientColorKey[] gck = new GradientColorKey[2];
            GradientAlphaKey[] gak = new GradientAlphaKey[2];
            gck[0].color = colorHigh;
            gck[0].time = 1.0F;
            gck[1].color = colorLow;
            gck[1].time = -1.0F;
            gak[0].alpha = 0.0F;
            gak[0].time = 1.0F;
            gak[1].alpha = 0.0F;
            gak[1].time = -1.0F;
            gradient.SetKeys(gck, gak);
        }

        public Color Evaluate(float gradientValue)
        {
            return gradient.Evaluate(gradientValue);
        }

    }
}