//


using System.Runtime.Serialization;

namespace SharpLib.Ear
{
    [DataContract]
    [AttributeObject(Id = "Event.Monitor.PowerOff", Name = "Monitor Power Off", Description = "Windows is powering off your monitor.")]
    public class EventMonitorPowerOff : Event
    {
        public EventMonitorPowerOff()
        {
        }
    }

}