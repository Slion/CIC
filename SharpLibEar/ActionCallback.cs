//

using System.Runtime.Serialization;


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

        public override void Execute()
        {
            if (iCallback != null)
            {
                iCallback.Invoke();
            }            
        }
    }

}