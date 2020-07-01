using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using XMPro.Unity.Api;

namespace XMPro.Unity
{
    [AddComponentMenu("XMPro/Simulate/Generate Data")]
    public class SimulateData : MonoBehaviour
    {
        [Header("Simulated Floats")]
        [Tooltip("The minimum integer value for a variable.")]
        public float floatMinValue;
        [Tooltip("The maximum float value for a variable.")]
        public float floatMaxValue;
        [Header("Simulated Integers")]
        [Tooltip("The minimum integer value for a variable.")]
        public float intMinValue;
        [Tooltip("The maximum integer value for a variable.")]
        public float intMaxValue;
        [Header("Entity Configuration")]
        [Tooltip("The list of entities whom of which are being simulated.")]
        public List<EntityBase> entitiesToSimulate;
        [Tooltip("How often to generate simulated values.")]
        public int secondsBetweenSimulations;
        [Tooltip("The names of the variables that are being simulated. If the variable isn't found, an error will not be thrown.")]
        public List<string> variablesToSimulate;

        private FrameTimer frameTimer;
        protected readonly Random random = new Random();

        public void Start()
        {
            frameTimer = new FrameTimer(secondsBetweenSimulations);
            frameTimer.Update += Simulate;
            Debug.Log("Screen height: " + Screen.height);
            Debug.Log("Screen width: " + Screen.width);
        }
        public void Update()
        {
            frameTimer.Invoke();



        }
        public void Simulate()
        {
            foreach (var entity in entitiesToSimulate)
            {
                foreach (var value in variablesToSimulate)
                {
                    try
                    {
                        FieldInfo fi = entity.GetType().GetField(value);
                        if (fi.FieldType == typeof(float))
                        {
                            fi.SetValue(entity, UnityEngine.Random.value * floatMaxValue + floatMinValue);

                        }
                        else if (fi.FieldType == typeof(int))
                        {
                            fi.SetValue(entity, (int)UnityEngine.Random.Range(intMinValue, intMaxValue));
                        }
                        else if(fi.FieldType == typeof(bool))
                        {
                            fi.SetValue(entity, Random.value > .5f ? true : false);
                        }
                    }
                    catch
                    {

                    }
                }
            }
        }
    }
}
