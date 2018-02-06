using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SharpLib.Ear
{
    /// <summary>
    /// Base class for list based properties
    /// </summary>
    [DataContract]
    public abstract class PropertyList : Object
    {
        public IList<string> Items { get; set; } = new List<string>();

        // Looks like that default does not always apply.
        public bool Sorted = true;
    }
}
