//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using SharpLib.Utils;


namespace SharpLib.Ear
{
    /// <summary>
    /// Event Action Router (Ear) is a generic and extensible framework allowing users to execute actions in response to events. 
    /// Users can implement their own events and actions.
    /// </summary>
    [DataContract]
    [KnownType("DerivedTypes")]
    public class Manager
    {
        /// <summary>
        /// Our events instances.
        /// </summary>
        [DataMember]
        public List<Event> Events;

        /// <summary>
        /// Constructor
        /// </summary>
        public Manager()
        {
            Init();
        }

        /// <summary>
        /// Executes after internalization took place.
        /// </summary>
        public void Init()
        {
            if (Events == null)
            {
                Events = new List<Event>();
            }
            
        }

        /// <summary>
        /// Trigger the given event.
        /// </summary>
        /// <param name="aEventType"></param>
        public void TriggerEvent<T>() where T: class
        {
            //Only trigger enabled events matching the desired type
            foreach (Event e in Events.Where(e => e.GetType() == typeof(T) && e.Enabled))
            {
                e.Trigger();
            }
        }

        /// <summary>
        /// Trigger the given event.
        /// </summary>
        /// <param name="aEventType"></param>
        public void TriggerEvent<T>(T aEvent) where T : class
        {
            //Only trigger events matching the desired type
            foreach (Event e in Events.Where(e => e.Equals(aEvent) && e.Enabled))
            {
                e.Trigger();
            }
        }


        /// <summary>
        /// Remove the specified action from the event it belongs too.
        /// </summary>
        /// <param name="aAction"></param>
        public void RemoveAction(Action aAction)
        {
            foreach (Event e in Events)
            {
                if (e.Actions.Remove(aAction))
                {
                    //We removed our action, we are done here.
                    return;
                }
            }
        }

        /// <summary>
        /// Allow extending our data contract.
        /// See KnownType above.
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<Type> DerivedTypes()
        {
            return SharpLib.Utils.Reflection.GetDerivedTypes<Manager>();
        }
    }
}