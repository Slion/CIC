﻿using System;
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
    public class PropertyComboBox : Object
    {
        public IList<string> Items { get; set; } = new List<string>();

        [DataMember]
        public string CurrentItem { get; set; }
    }
}