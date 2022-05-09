using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JParser
{
    internal class JObject
    {
        private Dictionary<string, object> data = new Dictionary<string, object>();

        public void Add(string key, object value) => data.Add(key, value);

        public object Get(string key)
        {
            if (!data.ContainsKey(key))
                throw new ArgumentOutOfRangeException(nameof(key));

            return data[key];
        }

        public JObject GetJObject(string key)
        {
            object value = Get(key);

            if (!(value is JObject))
                throw new JSONTypeException("Type of value is not JsonObject");

            return (JObject)value;
        }

    }
}
