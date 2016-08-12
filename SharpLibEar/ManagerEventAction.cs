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
    [TypeConverter(typeof(TypeConverterJson<ManagerEventAction>))]
    [DataContract]
    public class ManagerEventAction
    {
        public static ManagerEventAction Current = null;
        //public IDictionary<string, Type> ActionTypes;
        //public IDictionary<string, Type> EventTypes;

        /// <summary>
        /// Our events instances.
        /// </summary>
        [DataMember]
        public List<Event> Events;




        public ManagerEventAction()
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
        /// 
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
        /// 
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
    }
}