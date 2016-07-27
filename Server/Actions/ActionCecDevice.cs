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
    /// Abstract CEC action using a device logical address.
    /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public string DeviceName {
            get
            {
                if (Device == CecLogicalAddress.Broadcast)
                {
                    //Because of duplicate value in enumeration
                    return "Broadcast";
                }

                return Device.ToString();
            }
        }

    }
}
