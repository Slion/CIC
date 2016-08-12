//


using System.Runtime.Serialization;

namespace SharpLib.Ear
{
    [DataContract]
    [AttributeObject(Id = "Event.Monitor.PowerOn", Name = "Monitor Power On", Description = "Windows is powering on your monitor.")]
    public class EventMonitorPowerOn : Event
    {
        public EventMonitorPowerOn()
        {
        }

    }

}