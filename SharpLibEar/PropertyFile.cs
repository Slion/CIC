using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SharpLib.Ear
{
    /// <summary>
    /// File property
    /// </summary>
    [DataContract]
    [AttributeObject(Id = "Property.File", Name = "File", Description = "Holds a full file path.")]
    public class PropertyFile : Object
    {
        [DataMember]
        public string FullPath { get; set; } = "Select a file";
    }
}
