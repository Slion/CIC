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
    [AttributeAction(Id = "Cec.StandbyTv", Name = "CEC Standby TV", Description = "Standby TVs on your CEC HDMI network.")]
    class ActionCecStandbyTv : SharpLib.Ear.Action
    {
        public override void Execute()
        {
            if (Cec.Client.Static == null)
            {
                Console.WriteLine("WARNING: No CEC client installed.");
            }

            Cec.Client.Static.Lib.StandbyDevices(CecLogicalAddress.Tv);
        }
    }
}
