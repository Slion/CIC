//


using System.Runtime.Serialization;
using System.Threading;

namespace Slions.Ear
{
    

    [DataContract]
    class ActionSleep : Action
    {
        static readonly string Name = "Sleep";
        static readonly string Description = "Have the current thread sleep for the specified amount of milliseconds.";

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