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
    [Ear.AttributeObject(Id = "Event.Speech.Recognized", Name = "Speech recognized", Description = "Triggered when a voice command is recognized.")]
    public class EventSpeechRecognized : Ear.Event
    {
        public EventSpeechRecognized()
        {
        }
    }
}
