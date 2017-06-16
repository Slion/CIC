using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SharpLib.Ear
{
    /// <summary>
    /// Text property introduced for multiline support.
    /// Single line could use explicit string property.
    /// </summary>
    [DataContract]
    [AttributeObject(Id = "Property.Text", Name = "Text", Description = "Text property.")]
    public class PropertyText : Object
    {
        protected override void DoConstruct()
        {
            base.DoConstruct();
        }

        public int HeightInLines = 3;

        public bool Multiline = true;

        [DataMember]
        public string Text { get; set; }
    }
}
