//


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace SharpLib.Ear
{
    [DataContract]
    public abstract class Event : Object
    {
        [DataMember]
        [AttributeObjectProperty
            (
                Id = "Event.Enabled",
                Name = "Enabled",
                Description = "When enabled an event instance can be triggered."
            )
        ]
        public bool Enabled { get; set; } = true;

        [DataMember]
        public List<Action> Actions = new List<Action>();


        protected override void DoConstruct()
        {
            base.DoConstruct();

            // TODO: Construct properties too
            foreach (Action a in Actions)
            {
                a.Construct();
            }

        }


        /// <summary>
        /// Allows testing from generic edit dialog.
        /// </summary>
        public void Test()
        {
            Trace.WriteLine("Event test");
            Trigger();
        }


        public void Trigger()
        {
            Trace.WriteLine("Event triggered: " + Name);
            foreach (Action action in Actions)
            {
                action.Execute();
            }
        }

        //
        public override bool Equals(object obj)
        {
            //Default implementation assumes event are the same if types are the same
            bool res=  obj.GetType() == GetType();
            return res;
        }
    };

}