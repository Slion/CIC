using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Reflection;


namespace SharpLib.Utils
{
    public class Reflection
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<Type> GetConcreteClassesDerivedFrom<T>() where T : class
        {
            List<Type> objects = new List<Type>();
            foreach (Type type in
                Assembly.GetAssembly(typeof(T)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T))))
            {
                objects.Add(type);
            }

            return objects;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IDictionary<string, Type> GetConcreteClassesDerivedFromByName<T>() where T : class
        {
            Dictionary<string, Type> objects = new Dictionary<string, Type>();
            foreach (Type type in
                Assembly.GetAssembly(typeof(T)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T))))
            {
                objects.Add(type.Name,type);
            }

            return objects;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> GetConcreteClassesInstanceDerivedFrom<T>() where T : class
        {
            List<T> objects = new List<T>();
            foreach (Type type in
                Assembly.GetAssembly(typeof(T)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T))))
            {
                objects.Add((T)Activator.CreateInstance(type));
            }

            if (objects.Count>0
                && objects[0] is IComparable)
            {
                objects.Sort();
            }

            return objects;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IDictionary<string, T> GetConcreteClassesInstanceDerivedFromByName<T>() where T : class
        {
            Dictionary<string, T> objects = new Dictionary<string, T>();
            foreach (Type type in
                Assembly.GetAssembly(typeof(T)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T))))
            {
                objects.Add(type.Name,(T)Activator.CreateInstance(type));
            }


            return objects;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aType"></param>
        /// <returns></returns>
        public static T GetAttribute<T>(Type aType) where T : class
        {
            object[] attrs = aType.GetCustomAttributes(true);
            foreach (object attr in attrs)
            {
                T attribute = attr as T;
                if (attribute != null)
                {
                    return attribute;
                }
            }

            return null;
        }

    }

}
