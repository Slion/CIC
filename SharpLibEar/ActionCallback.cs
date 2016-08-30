//

using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SharpLib.Ear
{
    [DataContract]
    public abstract class ActionCallback : Action
    {
        public delegate void Delegate();

        private readonly Delegate iCallback;

        public ActionCallback(Delegate aCallback = null)
        {
            iCallback = aCallback;
        }

        protected override async Task DoExecute()
        {
            if (iCallback != null)
            {
                iCallback.Invoke();
            }            
        }
    }

}