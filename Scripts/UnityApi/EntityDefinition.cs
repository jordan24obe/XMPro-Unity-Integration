using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XMPro.Unity.Api
{
    [Serializable]
    public struct EntityDefinition
    {
        [Tooltip("The name of the entity variable.")]
        public string name;
        [Tooltip("The type of the entity variable.")]
        public IoTTypes type;
    }
}
