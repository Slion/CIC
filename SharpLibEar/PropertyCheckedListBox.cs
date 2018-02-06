using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SharpLib.Ear
{
    /// <summary>
    /// CheckedListBox property
    /// </summary>
    [DataContract]
    [AttributeObject(Id = "Property.CheckedListBox", Name = "CheckedListBox", Description = "CheckedListBox property.")]
    public class PropertyCheckedListBox : PropertyList
    {       
        [DataMember]
        public IList<string> CheckedItems { get; set; } = new List<string>();
    }
}
