//


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SharpLib.Ear
{
    [DataContract]
    [AttributeObject(Id = "Event", Name = "User Event", Description = "An event that can be triggered by users.")]
    public class Event : Object
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
        [AttributeObjectProperty
            (
                Id = "Event.Name",
                Name = "Name",
                Description = "Given event name. Can be used to trigger it."
            )
        ]
        public string Name { get; set; } = "";

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
        public async void Test()
        {
            Trace.WriteLine("Event test");
            await Trigger();
        }


        public async Task Trigger()
        {
            Trace.WriteLine("Event triggered: " + AttributeName);
            foreach (Action action in Actions)
            {
                await action.Execute();
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