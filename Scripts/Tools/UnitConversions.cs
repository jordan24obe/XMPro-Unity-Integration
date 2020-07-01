using UnityEngine;

namespace XMPro.Unity
{
    public static class UnitConversions
    {
        #region Imperial Conversions
        public static float FeetToMeters(float value)
        {
            return value / 3.281f;
        }
        public static float MetersToFeet(float value)
        {
            return value * 3.281f;
        }
        #endregion Imperial Conversions

        public static Fraction ToFraction(this float value, float error = 0.000001f)
        {
            int n = (int)Mathf.Floor(value);
            value -= n;
            if (value < error)
                return new Fraction(n, 1);
            else if (1 - error < value)
                return new Fraction(n + 1, 1);
            //The lower fraction is 0/1
            int lower_n = 0;
            int lower_d = 1;
            //The upper fraction is 1/1
            int upper_n = 1;
            int upper_d = 1;
            while (true)
            {
                //The middle fraction is (lower_n + upper_n) / (lower_d + upper_d)
                int middle_n = lower_n + upper_n;
                int middle_d = lower_d + upper_d;
                //If x + error < middle
                if (middle_d * (value + error) < middle_n)
                {
                    //middle is our new upper
                    upper_n = middle_n;
                    upper_d = middle_d;
                }
                //Else If middle < x - error
                else if(middle_n < (value - error) * middle_d)
                {
                    //middle is our new lower
                    lower_n = middle_n;
                    lower_d = middle_d;
                }
                //Else middle is our best fraction
                else
                {
                    return new Fraction(n * middle_d + middle_n, middle_d);
                }
            }
        }
    }
}
