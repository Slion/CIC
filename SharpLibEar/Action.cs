//


using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading;

namespace SharpLib.Ear
{
    [DataContract]
    [KnownType("DerivedTypes")]
    public abstract class Action: IComparable
    {
        protected abstract void DoExecute();

        /// <summary>
        /// Allows testing from generic edit dialog.
        /// </summary>
        public void Test()
        {
            Console.WriteLine("Action test");
            Execute();
        }

        public void Execute()
        {
            Console.WriteLine("Action executing: " + Brief());
            DoExecute();
        }

        public string Name {
            //Get the name of this object action attribute
            get { return Utils.Reflection.GetAttribute<AttributeObject>(GetType()).Name; }
            private set { }
        }

        public virtual string Brief()
        {
            return Name;
        }

        public int CompareTo(object obj)
        {
            //Sort by action name
            return Utils.Reflection.GetAttribute<AttributeObject>(GetType()).Name.CompareTo(obj.GetType());            
        }

        private static IEnumerable<Type> DerivedTypes()
        {
            return SharpLib.Utils.Reflection.GetDerivedTypes<Action>();
        }

    }


}