using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SharpLib.Ear
{

    /// <summary>
    /// EAR object provides serialization support.
    /// It assumes most derived class is decorated with AttributeObject.
    /// </summary>
    [DataContract]
    [KnownType("DerivedTypes")]
    public abstract class Object: IComparable, INotifyPropertyChanged
    {
        private bool iConstructed = false;

        protected Object()
        {
            Construct();
        }

        /// <summary>
        /// Needed as our constructor is not called following internalization.
        /// </summary>
        public void Construct()
        {
            if (!iConstructed)
            {
                DoConstruct();
                iConstructed = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void DoConstruct()
        {

        }

        /// <summary>
        /// Static object name.
        /// </summary>
        public string Name
        {
            //Get the name of this object attribute
            get { return Utils.Reflection.GetAttribute<AttributeObject>(GetType()).Name; }
            private set { }
        }

        /// <summary>
        /// Static object description.
        /// </summary>
        public string Description
        {
            //Get the description of this object attribute
            get { return Utils.Reflection.GetAttribute<AttributeObject>(GetType()).Description; }
            private set { }
        }

        /// <summary>
        /// Dynamic object description.
        /// </summary>
        /// <returns></returns>
        public virtual string Brief()
        {
            return Name;
        }

        /// <summary>
        /// Needed to make sure our sorting makes sense
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            //Sort by object name
            return Utils.Reflection.GetAttribute<AttributeObject>(GetType()).Name.CompareTo(obj.GetType());
        }

        /// <summary>
        /// Tells whether the current object configuration is valid.
        /// </summary>
        /// <returns></returns>
        public virtual bool IsValid()
        {
            return true;
        }

        /// <summary>
        /// So that data contract knows all our types.
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<Type> DerivedTypes()
        {
            return SharpLib.Utils.Reflection.GetDerivedTypes<Object>();
        }

        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Invoke our event.
        /// </summary>
        /// <param name="name"></param>
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


    }
}
