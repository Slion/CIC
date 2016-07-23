using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpLib.Ear
{

    // Multiuse attribute.
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class AttributeAction : System.Attribute
    {
        public string Id;
        public string Name;
        public string Description;
    }

}
