//


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace SharpLib.Ear
{
    [DataContract]
    [AttributeObject(Id = "Action", Name = "Action Group", Description = "Use it to group other actions together.")]
    public class Action: Object
    {
        [DataMember]
        [AttributeObjectProperty
            (
            Id = "Action.Iterations",
            Name = "Iterations",
            Description = "Specifies the number of time this action should execute.",
            Minimum = 0,
            Maximum = 10000,
            Increment = 1
            )
        ]
        public int Iterations { get; set; } = 1;

        /// <summary>
        /// 
        /// </summary>
        public bool Enabled
        {
            get { return Iterations > 0; }
        }


        public override bool IsValid()
        {            
            // We don't want to override this behaviour for derived classes
            if (GetType() == typeof(Action))
            {
                // Avoid having empty actions with no name
                return !string.IsNullOrEmpty(Name);
            }

            return base.IsValid();
        }


        /// <summary>
        /// Basic action just does nothing
        /// </summary>
        /// <returns></returns>
        protected virtual async Task DoExecute(Context aContext)
        {
            
        }

        /// <summary>
        /// Allows testing from generic edit dialog.
        /// </summary>
        public void Test()
        {
            Trace.WriteLine("Action test");
            Execute(new Context());
        }

        /// <summary>
        /// Execute our action N times.
        /// </summary>
        /// <returns></returns>
        public async Task Execute(Context aContext)
        {
            if (!IsValid())
            {
                Trace.WriteLine("EAR: Action.Execute: WARNING: Action invalid, aborting execution: " + Brief());
                return;
            }

            if (!Enabled)
            {
                Trace.WriteLine("EAR: Action.Execute: Action disabled: " + Brief());
                return;
            }

            for (int i = Iterations; i > 0; i--)
            {
                Trace.WriteLine($"EAR: Action.Execute: [{Iterations - i + 1}/{Iterations}] - {BriefBase()}");
                //For each iteration
                //We first execute ourselves
                await DoExecute(aContext);

                //Then our children
                foreach (Action a in Objects.OfType<Action>())
                {
                    await a.Execute(aContext);
                }
            }            
        }

        /// <summary>
        /// For inherited classes to override.
        /// </summary>
        /// <returns></returns>
        public virtual string BriefBase()
        {
            return string.IsNullOrEmpty(Name)? AttributeName : Name;
        }

        /// <summary>
        /// Dynamic object description.
        /// </summary>
        /// <returns></returns>
        public sealed override string Brief()
        {
            return Iterations > 1 ? $"{Iterations} x " + BriefBase():BriefBase();
        }






    }


}