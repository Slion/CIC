//


using System.Runtime.Serialization;
using System.Threading;

namespace Slions.Ear
{
    [DataContract]
    abstract class Action
    {
        public abstract void Execute();
    }


}