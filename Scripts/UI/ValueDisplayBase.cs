using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using XMPro.Unity.Api;

namespace XMPro.Unity.UI
{
    public class ValueDisplayBase<T> : MonoBehaviour, ITargetter where T : TMP_Text
    {
        [SerializeField]
        public EntityBase target;
        protected T display = null;
        public string propertyName;
        public string prefix;
        public string suffix;
        protected PropertyInfo info;
        [Tooltip("How many decimal places to round to.")]
        [Range(0, 4)]
        public int decimalPlaces = 2;
        public NumberType decimalType = NumberType.Decimal;

        public void SetTarget(GameObject target)
        {
            this.target = target.GetComponent<EntityBase>();
        }

        // Use this for initialization
        void Start()
        {
            display = (T)GetComponent<TMP_Text>();
            info = target.GetType().GetProperty(propertyName);
            UpdateDisplay();
        }
        // Update is called once per frame
        void Update()
        {
            UpdateDisplay();
        }

        public void UpdateDisplay()
        {
            if (info.PropertyType == typeof(float))
            {
                float value = (float)Math.Round(float.Parse(info.GetValue(target).ToString()), decimalPlaces);

                if (decimalType == NumberType.Decimal)
                {
                    display.text = prefix + value + suffix;
                }
                else
                {
                    Fraction fraction = (value - Mathf.Floor(value)).ToFraction();
                    value = Mathf.Floor(value);
                    if (fraction != 0f)
                        display.text = $"{prefix}{value}-{fraction.numerator}/{fraction.denominator}{suffix}";
                    else
                        display.text = $"{prefix}{value}{suffix}";
                }
            }
            else
                display.text = prefix + info.GetValue(target).ToString() + suffix;
        }
    }
}
