//


using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace SharpLib.Ear
{
    

    [DataContract]
    [AttributeObject(Id = "Thread.Sleep", Name = "Sleep", Description = "Have the current thread sleep for the specified amount of milliseconds.")]
    public class ActionSleep : Action
    {
        [DataMember]
        [AttributeObjectProperty
            (
                Id = "Thread.Sleep.Timeout",
                Name = "Timeout (ms)",
                Description = "Specifies the number of milliseconds this action will sleep for.",
                Minimum = "0",
                Maximum = "10000",
                Increment =  "1"
            )
        ]
        public int TimeoutInMilliseconds { get; set; }

        public ActionSleep()
        {
            TimeoutInMilliseconds = 1000;
        }


        public ActionSleep(int aMillisecondsTimeout)
        {
            TimeoutInMilliseconds = aMillisecondsTimeout;
        }

        public override string Brief()
        {
            return Name + " for " + TimeoutInMilliseconds + " ms";
        }


        protected override async Task DoExecute()
        {
            await Task.Delay(TimeoutInMilliseconds);
        }

    }



}