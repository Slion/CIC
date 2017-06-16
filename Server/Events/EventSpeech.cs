using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Windows.Forms;
using Ear = SharpLib.Ear;
using Hid = SharpLib.Hid;
//using Ear = SharpLib.Ear;
using SharpLib.Ear;

namespace SharpDisplayManager
{
    [DataContract]
    [Ear.AttributeObject(Id = "Event.Speech", Name = "Speech", Description = "Handle voice command.")]
    public class EventSpeech: Ear.Event
    {
        public EventSpeech()
        {
        }

        [DataMember]
        [Ear.AttributeObjectProperty
            (
            Id = "Speech.Phrases",
            Name = "Phrases",
            Description = "The phrases activating your voice command."
            )]
        public PropertyText Phrases { get; set; } = new PropertyText { Text = "", Multiline = true, HeightInLines = 3 };

        [DataMember]
        [Ear.AttributeObjectProperty
            (
            Id = "Speech.Semantic",
            Name = "Semantic",
            Description = "The meaning of your voice command."
            )]
        public string Semantic { get; set; }

 

        protected override void DoConstruct()
        {
            base.DoConstruct();
            if (Phrases==null)
            {
                // Defense, we hit this durring development but should not happen in production
                // When internalizing empty phrases               
                Phrases = new PropertyText { Text = "", Multiline=true, HeightInLines = 3 };
            }
            else
            {
                // Define our default here too
                Phrases.Multiline = true;
                Phrases.HeightInLines = 3;
            }
            UpdateDynamicProperties();
        }

        private void UpdateDynamicProperties()
        {
            
        }

        /// <summary>
        /// Make sure we distinguish between various configuration of this event 
        /// </summary>
        /// <returns></returns>
        public override string Brief()
        {
            string brief = AttributeName + ": " + Semantic;

            return brief;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Matches(object obj)
        {
            if (obj is EventSpeech)
            {
                EventSpeech e = (EventSpeech)obj;
                // Speech events are matching if they have the same semantic
                return e.Semantic.Equals(Semantic);                 
            }

            return false;
        }


        /// <summary>
        /// 
        /// </summary>
        protected override void OnStateLeave()
        {
            if (CurrentState == State.Edit)
            {
                // Leaving edit mode
                // Unhook HID events
                Program.iFormMain.CreateKinectManagerIfNeeded();

            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnStateEnter()
        {
            if (CurrentState == State.Edit)
            {
                // Enter edit mode
                // Hook-in HID events
                Program.iFormMain.DestroyKinectManager();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Phrases.Text) && !string.IsNullOrWhiteSpace(Semantic);
        }
    }
}
