using UnityEngine;

namespace XMPro.Unity
{
    /// <summary>
    /// Data representation of a fraction with basic operations.
    /// </summary>
    public struct Fraction
    {
        public int numerator;
        public int denominator;

        public Fraction(int n, int d)
        {
            this.numerator = n;
            this.denominator = d;
        }

        public float ToFloat() => (float)this.numerator / (float)this.denominator;
        public double ToDouble() => (double)this.numerator / (double)this.denominator;

        #region Operators
        public static bool operator ==(Fraction a, float b)
        {
            if(Mathf.Abs(a.numerator/a.denominator - b) < .01f)
                return true;
            return false;
        }
        public static bool operator !=(Fraction a, float b)
        {
            if (Mathf.Abs((float)a.numerator / (float)a.denominator - b) < .01f)
                return false;
            return true;
        }
        public static bool operator ==(Fraction a, Fraction b)
        {
            if (a.numerator == b.numerator && a.denominator == b.denominator)
                return true;
            return false;
        }
        public static bool operator !=(Fraction a, Fraction b)
        {
            if (a.numerator == b.numerator && a.denominator == b.denominator)
                return false;
            return true;
        }
        public static Fraction operator +(Fraction a, Fraction b)
        {
            var dec = a.ToFloat() + b.ToFloat();
            return dec.ToFraction();
        }
        public static Fraction operator -(Fraction a, Fraction b)
        {
            var dec = a.ToFloat() - b.ToFloat();
            return dec.ToFraction();
        }
        public static Fraction operator *(Fraction a, Fraction b)
        {
            return new Fraction((int)(a.numerator * b.numerator), (int)(a.denominator * b.denominator));
        }
        public static Fraction operator /(Fraction a, Fraction b)
        {
            return new Fraction((int)(a.numerator * b.denominator), (int)(a.denominator * b.numerator));
        }


        #endregion Operators
        #region Required Overrides
        public override bool Equals(object obj)
        {
            if(((Fraction)obj).denominator == this.denominator && ((Fraction)obj).numerator == this.numerator)
                return true;
            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = -671859081;
            hashCode = hashCode * -1521134295 + this.numerator.GetHashCode();
            hashCode = hashCode * -1521134295 + this.denominator.GetHashCode();
            return hashCode;
        }

        public override string ToString() => $"{numerator}/{denominator}";
        #endregion Required Overrides
    }
}
