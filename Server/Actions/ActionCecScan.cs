using CecSharp;
using SharpLib.Ear;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SharpDisplayManager
{

    [DataContract]
    [AttributeObject(Id = "Cec.Scan", Name = "CEC Scan", Description = "Scan devices on your CEC HDMI network.")]
    class ActionCecScan : SharpLib.Ear.Action
    {
        protected override async Task DoExecute()
        {
            if (Cec.Client.Static == null)
            {
                Trace.WriteLine("WARNING: No CEC client installed.");
                return;
            }

            Cec.Client.Static.Scan();
        }
    }
}
