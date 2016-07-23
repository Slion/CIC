//


using System.Runtime.Serialization;

namespace SharpLib.Ear
{
    [DataContract]
    public class EventMonitorPowerOff : Event
    {
        public EventMonitorPowerOff()
        {
            Name = "Monitor Power Off";
            Description = "";
        }
    }

}