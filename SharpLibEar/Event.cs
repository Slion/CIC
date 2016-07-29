//


using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SharpLib.Ear
{
    [DataContract]
    public abstract class MEvent
    {
        public string Name { get; protected set; }
        public string Description { get; protected set; }

        public abstract void Trigger();
    };

    [DataContract]
    public abstract class Event : MEvent
    {
        [DataMember]
        public List<Action> Actions = new List<Action>();

        protected Event()
        {
           
        }

        public override void Trigger()
        {
            Console.WriteLine("Event triggered: " + Name);
            foreach (Action action in Actions)
            {
                action.Execute();
            }
        }
    }

}