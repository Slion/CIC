using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace SharpLib.Ear
{

    /// <summary>
    /// EAR object provides serialization support.
    /// It assumes most derived class is decorated with AttributeObject.
    /// </summary>
    [DataContract]
    [KnownType("DerivedTypes")]
    public abstract class Object: IComparable
    {

        public string Name
        {
            //Get the name of this object attribute
            get { return Utils.Reflection.GetAttribute<AttributeObject>(GetType()).Name; }
            private set { }
        }

        public string Description
        {
            //Get the description of this object attribute
            get { return Utils.Reflection.GetAttribute<AttributeObject>(GetType()).Description; }
            private set { }
        }

        public virtual string Brief()
        {
            return Name;
        }

        public int CompareTo(object obj)
        {
            //Sort by object name
            return Utils.Reflection.GetAttribute<AttributeObject>(GetType()).Name.CompareTo(obj.GetType());
        }

        /// <summary>
        /// So that data contract knows all our types.
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<Type> DerivedTypes()
        {
            return SharpLib.Utils.Reflection.GetDerivedTypes<Object>();
        }

    }
}
