//


using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace SharpLib.Ear
{
    [DataContract]
    [AttributeObject(Id = "Task.Delay", Name = "Delay", Description = "Delay the execution of the next task by the specified amount of milliseconds.")]
    public class ActionDelay : Action
    {
        [DataMember]
        [AttributeObjectProperty
            (
                Id = "Task.Delay.Milliseconds",
                Name = "Delay (ms)",
                Description = "Specifies our delay in milliseconds.",
                Minimum = "0",
                Maximum = "60000",
                Increment = "1000"
            )
        ]
        public int Milliseconds { get; set; }

        public ActionDelay()
        {
            Milliseconds = 1000;
        }


        public ActionDelay(int aMillisecondsTimeout)
        {
            Milliseconds = aMillisecondsTimeout;
        }

        public override string Brief()
        {
            return AttributeName + " for " + Milliseconds/1000.0 + " seconds";
        }


        protected override async Task DoExecute()
        {
            await Task.Delay(Milliseconds);
        }

    }



}