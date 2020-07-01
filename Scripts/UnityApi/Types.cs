using System;

namespace XMPro.Unity.Api
{
    public enum IoTTypes
    {
        Int32,
        Int64,
        Float,
        DateTime,
        String,
        Json
    }

    public static class Extensions
    { 
        public static float ToFloat(this string value)
        {
            return float.Parse(value);
        }
        public static int ToInt(this string value)
        {
            return int.Parse(value);
        }
        public static DateTime ToDateTime(this string value)
        {
            return DateTime.Parse(value);
        }
        public static long ToLong(this string value)
        {
            return long.Parse(value);
        }
    }

}