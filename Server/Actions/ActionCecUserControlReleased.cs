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
    /// Send a user key press event to the given CEC device.
    /// </summary>
    [DataContract]
    [AttributeAction(Id = "Cec.UserControlReleased", Name = "CEC User Control Released", Description = "Send user control release opcode to a given CEC device.")]
    public class ActionCecUserControlReleased : ActionCecDevice
    {

        public ActionCecUserControlReleased()
        {
            Wait = true;
        }

        [DataMember]
        [AttributeActionProperty(
        Id = "Cec.UserControlPressed.Wait",
        Name = "Wait",
        Description = "Wait for that command."
        )]
        public bool Wait { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string Brief()
        {
            string brief = Name + " to " + DeviceName;
            if (Wait)
            {
                brief += " (wait)";
            }
            return brief;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void DoExecute()
        {
            if (Cec.Client.Static == null)
            {
                Console.WriteLine("WARNING: No CEC client installed.");
                return;
            }

            Cec.Client.Static.Lib.SendKeyRelease(Device, Wait);
        }
    }
}
