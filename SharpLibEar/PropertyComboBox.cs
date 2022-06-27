using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SharpLib.Ear
{
    /// <summary>
    /// ComboBox property
    /// </summary>
    [DataContract]
    [AttributeObject(Id = "Property.ComboBox", Name = "ComboBox", Description = "ComboBox property.")]
    public class PropertyComboBox : PropertyList
    {
        //TODO: shall this be an object, but how would it get persisted then?
        [DataMember]
        public string CurrentItem { get; set; } = "";

        public bool CurrentItemNotFound { get; set; } = false;
    }
}
