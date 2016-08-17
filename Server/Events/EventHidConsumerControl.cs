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
        [Ear.AttributeObject(Id = "Event.Hid.ConsumerControl", Name = "HID Consumer Control", Description = "Corresponding HID message received.")]
        public class EventHidConsumerControl : Ear.Event
        {
            public EventHidConsumerControl()
            {
            }

        [DataMember]
        [Ear.AttributeObjectProperty
            (
            Id = "HID.ConsumerControl.Usage",
            Name = "Usage",
            Description = "The usage corresponding to your remote button."
            )]
        public Hid.Usage.ConsumerControl Usage { get; set; }

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
            EventHidConsumerControl e = (EventHidConsumerControl) obj;
            bool res = (e != null && e.Usage == Usage);
            return res;
        }

    }
}
