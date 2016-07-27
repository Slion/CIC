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
    /// <summary>
    /// Abstract CEC action using a device type.
    /// </summary>
    [DataContract]
    public abstract class ActionCecDeviceType : SharpLib.Ear.Action
    {
        [DataMember]
        [AttributeActionProperty
            (
            Id = "CEC.DeviceType",
            Name = "Device Type",
            Description = "The device type used by this action."
            )
        ]
        public CecDeviceType DeviceType { get; set; }
    }
}
