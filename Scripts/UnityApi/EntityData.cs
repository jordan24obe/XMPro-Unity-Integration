using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMPro.Unity.Api
{
    [Serializable]
    public struct EntityData
    {
        public string name;
        public object value;
        public Type type;

        public EntityData(string name, object value)
        {
            this.name = name;
            this.value = value;
            this.type = value.GetType();
        }
    }
}
