
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
    [AttributeAction(Id = "Cec.SendKepressTvPowerOn", Name = "CEC Send Keypress TV Power On", Description = "Send Power On keypress to your TV.")]
    class ActionCecSendKeypressTvPowerOn : SharpLib.Ear.Action
    {
        public override void Execute()
        {
            if (Cec.Client.Static == null)
            {
                Console.WriteLine("WARNING: No CEC client installed.");
            }

            Cec.Client.Static.Lib.SendKeypress(CecLogicalAddress.Tv, CecUserControlCode.PowerOnFunction, true);
        }
    }
}
