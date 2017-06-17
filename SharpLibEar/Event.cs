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
        public Context Context;

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

        /// <summary>
        /// Run the actions associated with this event.
        /// </summary>
        /// <returns></returns>
        public async Task Trigger()
        {
            Trace.WriteLine("Event triggered: " + AttributeName);
            foreach (Action action in Objects.OfType<Action>())
            {
                await action.Execute(Context);
            }
        }

        /// <summary>
        /// Check if this event matches the given one.
        /// This is used to trigger events.
        /// </summary>
        /// <param name="aObject"></param>
        /// <returns></returns>
        public virtual bool Matches(object aObject)
        {
            //Default implementation assumes event are the same if types are the same
            bool res= aObject.GetType() == GetType();
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void DoConstruct()
        {
            base.DoConstruct();
            if (null == Context)
            {
                Context = new Context();
            }            
        }

    };

}