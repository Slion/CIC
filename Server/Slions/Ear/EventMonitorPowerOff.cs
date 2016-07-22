//


using System.Runtime.Serialization;

namespace Slions.Ear
{
    [DataContract]
    class EventMonitorPowerOff : Event
    {
        public EventMonitorPowerOff()
        {
            Name = "Monitor Power Off";
            Description = "";
        }
    }

}