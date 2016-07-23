//


using System;
using System.Runtime.Serialization;
using System.Threading;

namespace SharpLib.Ear
{
    [DataContract]
    public abstract class Action: IComparable
    {
        public abstract void Execute();

        public int CompareTo(object obj)
        {
            //Sort by action name
            return Utils.Reflection.GetAttribute<AttributeAction>(GetType()).Name.CompareTo(obj.GetType());            
        }
    }


}