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
    [AttributeObject(Id = "Cec.UserControlPressed", Name = "CEC User Control Pressed", Description = "Send user control code to defined CEC device.")]
    public class ActionCecUserControlPressed : ActionCecDevice
    {

        public ActionCecUserControlPressed()
        {
            Wait = true;
        }

        [DataMember]
        [AttributeObjectProperty(
        Id = "Cec.UserControlPressed.Code",
        Name = "Code",
        Description = "The key code used by this action."
        )]
        public CecUserControlCode Code { get; set; }

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
        public override string Brief()
        {
            string brief = AttributeName + ": " + Code.ToString() + " to " + DeviceName;
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

            Cec.Client.Static.Lib.SendKeypress(Device, Code, Wait);
        }
    }
}
