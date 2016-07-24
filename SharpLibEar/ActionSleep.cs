//


using System.Runtime.Serialization;
using System.Threading;


namespace SharpLib.Ear
{
    

    [DataContract]
    [AttributeAction(Id = "Thread.Sleep", Name = "Sleep", Description = "Have the current thread sleep for the specified amount of milliseconds.")]
    public class ActionSleep : Action
    {
        [DataMember]
        private readonly int iMillisecondsTimeout;

        public ActionSleep()
        {
            iMillisecondsTimeout = 1000;
        }


        public ActionSleep(int aMillisecondsTimeout)
        {
            iMillisecondsTimeout = aMillisecondsTimeout;
        }

        public override void Execute()
        {
            Thread.Sleep(iMillisecondsTimeout);
        }

    }



}