//


using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace SharpLib.Ear
{
    [DataContract]
    public class EventActionManager
    {
        public static EventActionManager Current = null;
        public IDictionary<string, Type> ActionTypes;
        private IDictionary<string, Event> Events;

        public EventActionManager()
        {
            ActionTypes = Utils.Reflection.GetConcreteClassesDerivedFromByName<Action>();
            Events = Utils.Reflection.GetConcreteClassesInstanceDerivedFromByName<Event>();
        }

        public Event GetEvent<T>() where T : class
        {
            return Events[typeof(T).Name];
        }

    }
}