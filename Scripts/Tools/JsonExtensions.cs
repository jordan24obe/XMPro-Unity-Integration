using UnityEngine;

namespace XMPro.Unity
{
    public static class JsonExtensions
    {
        public static Vector3 ToVector3(this string value)
        {
            var array = value.Replace("(","").Replace(")","").Split(',');
            return new Vector3(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2]));
        }

        public static string ToJsonVector3(this Vector3 value)
        {
            return $"({value.x},{value.y},{value.z})";
        }

        public static string ToJsonColor(this Color color)
        {
            return "#" + ColorUtility.ToHtmlStringRGBA(color);
        }

        public static Color ToColor(this string color)
        {
            ColorUtility.TryParseHtmlString(color, out Color returnColor);
            return returnColor;
        }
    }
}
