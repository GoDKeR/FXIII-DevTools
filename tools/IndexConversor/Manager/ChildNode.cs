using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatConverter
{
    class ChildNode
    {
        public KeyValuePair<string, string> kvp;

        public ChildNode(string Key, string Value)
        {
            kvp = new KeyValuePair<string, string>(Key, Value);
        }
    }
}
