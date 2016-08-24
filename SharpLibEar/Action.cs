//


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Threading;

namespace SharpLib.Ear
{
    [DataContract]
    public abstract class Action: Object
    {
        protected abstract void DoExecute();

        /// <summary>
        /// Allows testing from generic edit dialog.
        /// </summary>
        public void Test()
        {
            Trace.WriteLine("Action test");
            Execute();
        }

        public void Execute()
        {
            Trace.WriteLine("Action executing: " + Brief());
            if (!IsValid())
            {
                Trace.WriteLine($"WARNING: action invalid, aborting execution.");
                return;
            }
            
            DoExecute();
        }

    }


}