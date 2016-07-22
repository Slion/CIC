//


using System.Runtime.Serialization;

namespace Slions.Ear
{
    [DataContract]
    class EventMonitorPowerOn : Event
    {
        public EventMonitorPowerOn()
        {
            Name = "Monitor Power On";
            Description = "";
        }

    }

}