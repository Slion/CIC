//


using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Slions.Ear
{
    [DataContract]
    abstract class MEvent
    {
        [DataMember]
        public string Name { get; protected set; }

        [DataMember]
        public string Description { get; protected set; }

        public abstract void Trigger();
    };

    [DataContract]
    abstract class Event : MEvent
    {
        List<Action> iActions;

        protected Event()
        {
            iActions = new List<Action>();
        }

        public override void Trigger()
        {
            Console.WriteLine("Event '" + Name + "' triggered.");
            foreach (Action action in iActions)
            {
                action.Execute();
            }
        }
    }

}