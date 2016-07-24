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
        public IDictionary<string, Type> ActionTypes;
        public IDictionary<string, Event> Events;
        [DataMember]
        public Dictionary<string, List<Action>> ActionsByEvents = new Dictionary<string, List<Action>>();


        public ManagerEventAction()
        {
            Init();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Init()
        {
            //Create our list of supported actions
            ActionTypes = Utils.Reflection.GetConcreteClassesDerivedFromByName<Action>();
            //Create our list or support events
            Events = Utils.Reflection.GetConcreteClassesInstanceDerivedFromByName<Event>();

            if (ActionsByEvents == null)
            {                
                ActionsByEvents = new Dictionary<string, List<Action>>();
            }

            //Hook in loaded actions with corresponding events
            foreach (string key in Events.Keys)
            {
                Event e = Events[key];
                if (ActionsByEvents.ContainsKey(key))
                {
                    //We have actions for that event, hook them in then
                    e.Actions = ActionsByEvents[key];
                }
                else
                {
                    //We do not have actions for that event yet, create empty action list
                    e.Actions = new List<Action>();
                    ActionsByEvents[key] = e.Actions;
                }
            }
        }

        /// <summary>
        /// Get and event instance from its type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Event GetEvent<T>() where T : class
        {
            return Events[typeof(T).Name];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aAction"></param>
        public void RemoveAction(Action aAction)
        {
            foreach (string key in Events.Keys)
            {
                Event e = Events[key];
                if (e.Actions.Remove(aAction))
                {
                    //We removed our action, we are done here.
                    return;
                }

            }
        }

    }
}