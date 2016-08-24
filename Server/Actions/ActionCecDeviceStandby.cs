using CecSharp;
using SharpLib.Ear;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SharpDisplayManager
{
    [DataContract]
    [AttributeObject(Id = "Cec.Device.Standby", Name = "CEC Device Standby", Description = "Puts on standby the specified CEC device on your HDMI bus.")]
    public class ActionCecDeviceStandby : ActionCecDevice
    {
        /// <summary>
        /// Build a user readable string to describe this action.
        /// </summary>
        /// <returns></returns>
        public override string Brief()
        {
            return "CEC Standby " + DeviceName;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void DoExecute()
        {
            if (Cec.Client.Static == null)
            {
                Trace.WriteLine("WARNING: No CEC client installed.");
                return;
            }

            Cec.Client.Static.Lib.StandbyDevices(Device);
        }

    }
}
