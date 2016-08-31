using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        [DataMember]
        public List<Object> Objects = new List<Object>();

        /// <summary>
        /// </summary>
        [DataMember]
        [AttributeObjectProperty
            (
                Id = "Object.Name",
                Name = "Name",
                Description = "Given object name."
            )
        ]
        public string Name { get; set; } = "";


        protected Object()
        {
            Construct();
        }

        /// <summary>
        /// Needed as our constructor is not called following internalization.
        /// </summary>
        public void Construct()
        {
            //Construct ourselves first
            if (!iConstructed)
            {
                DoConstruct();
                iConstructed = true;
            }

            //Then construct our children
            foreach (Object o in Objects)
            {
                o.Construct();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void DoConstruct()
        {
            //Make sure our name is not null
            if (Name == null)
            {
                Name = "";
            }

            // Makes sure our objects are not null
            if (Objects == null)
            {
                Objects = new List<Object>();
            }
        }

        public enum State
        {
            Rest=0,
            Edit
        }

        State iCurrentState = State.Rest;
        public State CurrentState { get { return iCurrentState; } set { OnStateLeave(); iCurrentState = value; OnStateEnter(); } }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnStateLeave()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnStateEnter()
        {

        }

        /// <summary>
        /// Static object name.
        /// </summary>
        public string AttributeName
        {
            //Get the name of this object attribute
            get { return Utils.Reflection.GetAttribute<AttributeObject>(GetType()).Name; }
            private set { }
        }

        /// <summary>
        /// Static object description.
        /// </summary>
        public string AttributeDescription
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
            return AttributeName;
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
