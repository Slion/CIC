using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Ear = SharpLib.Ear;
using Hid = SharpLib.Hid;


namespace SharpDisplayManager
{
    /// <summary>
    /// TODO: Consider an option to make it device specific.
    /// </summary>
    [DataContract]
    [Ear.AttributeObject(Id = "Event.Hid.Key", Name = "HID", Description = "Handle input from Keyboard, remote, joystick and gamepad buttons.")]
    class EventHidKey : EventHid
    {
    }
}
