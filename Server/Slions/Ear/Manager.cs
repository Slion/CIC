//


using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Slions.Ear
{
    public static class ReflectiveEnumerator
    {
        static ReflectiveEnumerator() { }

        public static IEnumerable<T> GetEnumerableOfType<T>() where T : class
        {
            List<T> objects = new List<T>();
            foreach (Type type in
                Assembly.GetAssembly(typeof(T)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T))))
            {
                objects.Add((T)Activator.CreateInstance(type));
            }
            //objects.Sort();
            return objects;
        }
    }


    [DataContract]
    class Manager
    {
        private IEnumerable<Action> Actions;
        private IEnumerable<Event> Events;

        public Manager()
        {
            Actions = ReflectiveEnumerator.GetEnumerableOfType<Action>();
            Events = ReflectiveEnumerator.GetEnumerableOfType<Event>();

            //CollectActions();
            //CollectEvents();
        }



    }
}