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
        public IList<object> Items { get; set; } = new List<object>();

        /// <summary>
        /// The name of the property in our item object we should display.
        /// Not needed when items are just strings.
        /// </summary>
        public string DisplayMember { get; set; }
        /// <summary>
        /// The name of the property in our item object we should store and persist.
        /// Not needed when items are just strings
        /// </summary>
        public string ValueMember { get; set; }

        // Looks like that default does not always apply.
        public bool Sorted = true;
    }
}
