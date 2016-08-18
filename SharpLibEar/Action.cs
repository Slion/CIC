//


using System;
using System.Collections.Generic;
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
            Console.WriteLine("Action test");
            Execute();
        }

        public void Execute()
        {
            Console.WriteLine("Action executing: " + Brief());
            if (!IsValid())
            {
                Console.WriteLine($"WARNING: action invalid, aborting execution.");
                return;
            }
            
            DoExecute();
        }

    }


}