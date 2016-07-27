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
        public abstract void DoExecute();

        public void Execute()
        {
            Console.WriteLine("Executing action: " + Brief());
            DoExecute();
        }

        public string Name {
            //Get the name of this object action attribute
            get { return Utils.Reflection.GetAttribute<AttributeAction>(GetType()).Name; }
            private set { }
        }

        public virtual string Brief()
        {
            return Name;
        }

        public int CompareTo(object obj)
        {
            //Sort by action name
            return Utils.Reflection.GetAttribute<AttributeAction>(GetType()).Name.CompareTo(obj.GetType());            
        }

        private static IEnumerable<Type> DerivedTypes()
        {
            return SharpLib.Utils.Reflection.GetDerivedTypes<Action>();
        }

    }


}