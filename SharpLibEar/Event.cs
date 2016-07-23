//


using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SharpLib.Ear
{
    [DataContract]
    public abstract class MEvent
    {
        [DataMember]
        public string Name { get; protected set; }

        [DataMember]
        public string Description { get; protected set; }

        public abstract void Trigger();
    };

    [DataContract]
    public abstract class Event : MEvent
    {
        public List<Action> Actions;

        protected Event()
        {
           Actions = new List<Action>();
        }

        public override void Trigger()
        {
            Console.WriteLine("Event '" + Name + "' triggered.");
            foreach (Action action in Actions)
            {
                action.Execute();
            }
        }
    }

}