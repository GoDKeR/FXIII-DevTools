using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatConverter
{
    class MainNode
    {
        public string Name;
        public List<ChildNode> values;
        public int numValues;

        public MainNode(string name)
        {
            Name = name;
            values = new List<ChildNode>();
        }
    }
}
