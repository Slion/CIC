//

using System.Runtime.Serialization;


namespace Slions.Ear
{
    [DataContract]
    abstract class ActionCallback : Action
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