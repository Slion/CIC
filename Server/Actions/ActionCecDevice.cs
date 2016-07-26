using CecSharp;
using SharpLib.Ear;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SharpDisplayManager
{
    [DataContract]
    public abstract class ActionCecDevice: SharpLib.Ear.Action
    {
        [DataMember]
        [AttributeActionProperty
            (
            Id = "CEC.Device",
            Name = "Device",
            Description = "The logical address used by this action."
            )
        ]
        public CecLogicalAddress Device { get; set; }
    }
}
