using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpLib.Ear
{

    /// <summary>
    /// For action class to define name and description.
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class AttributeObject : System.Attribute
    {
        public string Id;
        public string Name;
        public string Description;
    }


}
