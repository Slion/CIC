//


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            foreach (Action action in Objects.OfType<Action>())
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