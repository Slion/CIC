using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Windows.Forms;
using Ear = SharpLib.Ear;
using Hid = SharpLib.Hid;

namespace SharpDisplayManager.Events
{

    [DataContract]
    [Ear.AttributeObject(Id = "Event.Hid.Axis", Name = "HID Axis", Description = "Handle input from a controller axis.")]
    class EventHidAxis : EventHid
    {


        protected override bool IsSupportedDevice(Hid.Device.Input aDevice)
        {
            // We only want gamepads and joysticks here
            return aDevice.IsGamePad || aDevice.IsJoystick;
        }



    }
}
