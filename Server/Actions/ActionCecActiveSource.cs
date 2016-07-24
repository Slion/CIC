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
    class ActionCecActiveSource : SharpLib.Ear.Action
    {
        public override void Execute()
        {
            if (Cec.Client.Static == null)
            {
                Console.WriteLine("WARNING: No CEC client installed.");
            }

            Cec.Client.Static.Lib.SetActiveSource(CecDeviceType.PlaybackDevice);
        }
    }
}
