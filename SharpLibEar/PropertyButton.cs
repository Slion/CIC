using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SharpLib.Ear
{
    /// <summary>
    /// Generic button property
    /// </summary>
    [DataContract]
    [AttributeObject(Id = "Property.Button", Name = "Button", Description = "Button property.")]
    public class PropertyButton : Object
    {
        public EventHandler ClickEventHandler;

        [DataMember]
        public string Text { get; set; }
    }
}
