using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpLib.Ear
{
    /// <summary>
    /// To expose an action property thus enabling user to edit it.
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class AttributeActionProperty : System.Attribute
    {
        public string Id;
        public string Name;
        public string Description;
    }
}
