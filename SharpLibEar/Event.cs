//


using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SharpLib.Ear
{
    [DataContract]
    [KnownType("DerivedTypes")]
    public abstract class Event
    {
        [DataMember]
        [AttributeObjectProperty
            (
                Id = "Event.Enabled",
                Name = "Enabled",
                Description = "When enabled an event instance can be triggered."
            )
        ]
        public bool Enabled { get; set; }

        [DataMember]
        public List<Action> Actions = new List<Action>();

        public string Name
        {
            //Get the name of this object attribute
            get { return Utils.Reflection.GetAttribute<AttributeObject>(GetType()).Name; }
            private set { }
        }

        public string Description
        {
            //Get the description of this object attribute
            get { return Utils.Reflection.GetAttribute<AttributeObject>(GetType()).Description; }
            private set { }
        }

        public virtual string Brief()
        {
            return Name;
        }

        protected Event()
        {
            Enabled = true;
        }


        /// <summary>
        /// Allows testing from generic edit dialog.
        /// </summary>
        public void Test()
        {
            Console.WriteLine("Event test");
            Trigger();
        }


        public void Trigger()
        {
            Console.WriteLine("Event triggered: " + Name);
            foreach (Action action in Actions)
            {
                action.Execute();
            }
        }

        /// <summary>
        /// So that data contract knows all our types.
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<Type> DerivedTypes()
        {
            return SharpLib.Utils.Reflection.GetDerivedTypes<Event>();
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