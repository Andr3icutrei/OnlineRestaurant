using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRestaurant.UI.Services
{
    public class CustomDynamicObject
    {
        private readonly Dictionary<string, object> _values;

        public CustomDynamicObject(Dictionary<string, object> values)
        {
            _values = values;
        }

        public object this[string key] => _values.TryGetValue(key, out var val) ? val : null;

        public object Get(string key) => this[key];
    }

}
