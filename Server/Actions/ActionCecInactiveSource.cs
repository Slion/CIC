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
    [AttributeAction(Id = "Cec.InactiveSource", Name = "CEC Inactive Source", Description = "Set this CEC device as inactive source.")]
    class ActionCecInactiveSource : SharpLib.Ear.Action
    {
        public override void Execute()
        {
            if (Cec.Client.Static == null)
            {
                Console.WriteLine("WARNING: No CEC client installed.");
            }

            Cec.Client.Static.Lib.SetInactiveView();
        }
    }
}
