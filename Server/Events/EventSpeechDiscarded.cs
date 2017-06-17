using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Ear = SharpLib.Ear;

namespace SharpDisplayManager
{
    [DataContract]
    [Ear.AttributeObject(Id = "Event.Speech.Discarded", Name = "Speech dicarded", Description = "Triggered when a voidce command is discarded.")]
    public class EventSpeechDiscarded : Ear.Event
    {
        public EventSpeechDiscarded()
        {
        }
    }
}
