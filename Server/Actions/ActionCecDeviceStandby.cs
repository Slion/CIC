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
    [AttributeAction(Id = "Cec.Device.Standby", Name = "CEC Device Standby", Description = "Puts on standby the specified CEC device on your HDMI bus.")]
    public class ActionCecDeviceStandby : ActionCecDevice
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string Brief()
        {
            if (Device == CecLogicalAddress.Broadcast)
            {
                //Because of duplicated values it would display Unregistered
                return "CEC Standby Broadcast";
            }

            return "CEC Standby " + Device.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Execute()
        {
            if (Cec.Client.Static == null)
            {
                Console.WriteLine("WARNING: No CEC client installed.");
                return;
            }

            Cec.Client.Static.Lib.StandbyDevices(Device);
        }

    }
}
