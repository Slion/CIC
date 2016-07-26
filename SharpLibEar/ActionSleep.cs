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
        [AttributeActionProperty(Id = "Thread.Sleep.Timeout", Name = "Timeout",
            Description = "Specifies the number of milliseconds this action will sleep for.")]
        public int TimeoutInMilliseconds { get; set; }

        public ActionSleep()
        {
            TimeoutInMilliseconds = 1000;
        }


        public ActionSleep(int aMillisecondsTimeout)
        {
            TimeoutInMilliseconds = aMillisecondsTimeout;
        }

        public override void Execute()
        {
            Thread.Sleep(TimeoutInMilliseconds);
        }

    }



}