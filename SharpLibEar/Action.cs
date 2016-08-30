//


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace SharpLib.Ear
{
    [DataContract]
    public abstract class Action: Object
    {
        protected abstract Task DoExecute();

        /// <summary>
        /// Allows testing from generic edit dialog.
        /// </summary>
        public void Test()
        {
            Trace.WriteLine("Action test");
            Execute();
        }

        public async Task Execute()
        {
            Trace.WriteLine("Action executing: " + Brief());
            if (!IsValid())
            {
                Trace.WriteLine($"WARNING: action invalid, aborting execution.");
                return;
            }
            
            await DoExecute();
        }

    }


}