//


using System.Runtime.Serialization;
using System.Threading;

namespace SharpLib.Ear
{
    [DataContract]
    public abstract class ActionType
    {
        [DataMember]
        public string Name { get; protected set;  }
        [DataMember]
        public string Description { get; protected set; }
    }


}