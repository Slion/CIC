using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpLib.Ear
{

    /// <summary>
    /// For objects such as Action and Event to define name and description.
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class AttributeObject : System.Attribute
    {
        /// <summary>
        /// Not actually used.
        /// </summary>
        public string Id;
        /// <summary>
        /// Displayed in object editor
        /// </summary>
        public string Name;
        /// <summary>
        /// Displayed in object editor
        /// </summary>
        public string Description;
        /// <summary>
        /// Allows us to hide objects from users in editor without making them abstract
        /// </summary>
        public bool Hidden = false;
    }


}
