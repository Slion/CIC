//


using System.Runtime.Serialization;

namespace SharpLib.Ear
{
    [DataContract]
    public class EventMonitorPowerOn : Event
    {
        public EventMonitorPowerOn()
        {
            Name = "Monitor Power On";
            Description = "";
        }

    }

}