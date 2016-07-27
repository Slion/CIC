using CecSharp;
using SharpLib.Ear;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SharpDisplayManager
{
    [DataContract]
    [AttributeAction(Id = "Cec.ActiveSource", Name = "CEC Active Source", Description = "Set this CEC device as active source.")]
    class ActionCecActiveSource : ActionCecDeviceType
    {
        /// <summary>
        /// Build a user readable string to describe this action.
        /// </summary>
        /// <returns></returns>
        public override string Brief()
        {
            return "CEC Active Source to " + DeviceType.ToString();
        }

        /// <summary>
        /// Set the defined device type as active source.
        /// </summary>
        public override void Execute()
        {
            if (Cec.Client.Static == null)
            {
                Console.WriteLine("WARNING: No CEC client installed.");
                return;
            }

            Cec.Client.Static.Lib.SetActiveSource(DeviceType);
        }
    }
}
