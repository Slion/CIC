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
    /// <summary>
    /// Send a user key press event to the given CEC device.
    /// </summary>
    [DataContract]
    [AttributeObject(Id = "Cec.UserControlReleased", Name = "CEC User Control Released", Description = "Send user control release opcode to a given CEC device.")]
    public class ActionCecUserControlReleased : ActionCecDevice
    {

        public ActionCecUserControlReleased()
        {
            Wait = true;
        }

        [DataMember]
        [AttributeObjectProperty(
        Id = "Cec.UserControlPressed.Wait",
        Name = "Wait",
        Description = "Wait for that command."
        )]
        public bool Wait { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string BriefBase()
        {
            string brief = AttributeName + " to " + DeviceName;
            if (Wait)
            {
                brief += " (wait)";
            }
            return brief;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override async Task DoExecute()
        {
            if (Cec.Client.Static == null)
            {
                Trace.WriteLine("WARNING: No CEC client installed.");
                return;
            }

            Cec.Client.Static.Lib.SendKeyRelease(Device, Wait);
        }
    }
}
