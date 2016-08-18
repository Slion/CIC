using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Ear = SharpLib.Ear;
using Hid = SharpLib.Hid;

namespace SharpDisplayManager
{
    [DataContract]
    [Ear.AttributeObject(Id = "Event.Hid.WindowsMediaCenter", Name = "HID Windows Media Center", Description = "Corresponding HID message received.")]
    public class EventHidWindowsMediaCenter : Ear.Event
    {
        public EventHidWindowsMediaCenter()
        {
        }

        [DataMember]
        [Ear.AttributeObjectProperty
            (
            Id = "HID.WMC.Usage",
            Name = "Usage",
            Description = "The usage corresponding to your remote button."
            )]
        public Hid.Usage.WindowsMediaCenterRemoteControl Usage { get; set; }

        /// <summary>
        /// Make sure we distinguish between various configuration of this event 
        /// </summary>
        /// <returns></returns>
        public override string Brief()
        {
            return Name + ": " + Usage.ToString();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is EventHidWindowsMediaCenter)
            {
                EventHidWindowsMediaCenter e = (EventHidWindowsMediaCenter)obj;
                bool res = (e.Usage == Usage);
                return res;
            }

            return false;
        }

    }
}
