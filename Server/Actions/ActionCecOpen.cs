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
    [AttributeObject(Id = "Cec.Open", Name = "CEC Open", Description = "Open CEC connection.")]
    class ActionCecOpen : SharpLib.Ear.Action
    {
        protected override void DoExecute()
        {
            if (Cec.Client.Static == null)
            {
                Trace.WriteLine("WARNING: No CEC client installed.");
                return;
            }

            Cec.Client.Static.Open(1000);
        }
    }
}
