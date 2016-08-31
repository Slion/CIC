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
    public class Manager: Object
    {
        /// <summary>
        /// Our events instances.
        /// </summary>
        [DataMember]
        public List<Event> Events;


        /// <summary>
        /// Executes after internalization took place.
        /// </summary>
        protected override void DoConstruct()
        {
            base.DoConstruct();

            if (Events == null)
            {
                Events = new List<Event>();
            }

            // TODO: Object properties should be constructed too
            foreach (Event e in Events)
            {
                e.Construct();
            }
            
        }

        /// <summary>
        /// Trigger the given event.
        /// </summary>
        /// <param name="aEventType"></param>
        public async void TriggerEvents<T>() where T: class
        {
            //Only trigger enabled events matching the desired type
            foreach (Event e in Events.Where(e => e.GetType() == typeof(T) && e.Enabled))
            {
                await e.Trigger();
            }
        }

        /// <summary>
        /// Trigger the given event.
        /// </summary>
        /// <param name="aEventType"></param>
        public async void TriggerEvents<T>(T aEvent) where T : class
        {
            //Only trigger events matching the desired type
            foreach (Event e in Events.Where(e => e.Equals(aEvent) && e.Enabled))
            {
                await e.Trigger();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aEvent"></param>
        public async void TriggerEventsByName(string aName)
        {
            if (string.IsNullOrEmpty(aName))
            {
                //Just don't do that, that would be silly
                return;
            }
            // Only trigger events matching the desired type
            // Doing some safety checks as well to prevent crashing if name was left null for some reason
            // This was the case when loading existing settings after event Name was introduced
            foreach (Event e in Events.Where(e => !string.IsNullOrEmpty(e.Name) && aName.Equals(e.Name)))
            {
                await e.Trigger();
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
                if (RemoveObject(e,aAction))
                {
                    //We removed our action, we are done here.
                    return;
                }
            }
        }

        /// <summary>
        /// Remove the specified action from the event it belongs too.
        /// </summary>
        /// <param name="aAction"></param>
        private static bool RemoveObject(Object aCurrent, Object aToRemove)
        {
            //Exit condition
            if (aCurrent.Objects.Remove(aToRemove))
            {
                //We removed our action, we are done here.
                return true;
            }

            foreach (Object o in aCurrent.Objects)
            {
                bool done = RemoveObject(o, aToRemove);
                if (done)
                {
                    return true;
                } 
            }

            return false;
        }

    }
}