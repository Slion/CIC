using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Ear = SharpLib.Ear;

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
            Description = "The phrases activating your voice command.",
            Multiline = true,
            HeightInLines = 3
            )]
        public string Phrases { get; set; } = "";

        [DataMember]
        [Ear.AttributeObjectProperty
            (
            Id = "Speech.Semantic",
            Name = "Semantic",
            Description = "The meaning of your voice command."
            )]
        public string Semantic { get; set; }

        [DataMember]
        [Ear.AttributeObjectProperty
            (
            Id = "Speech.Confidence",
            Name = "Confidence",
            Description = "Confidence threshold, the lower the more false positive you could get.\n Set it too high and you will have to repeat yourself.",
            Maximum = 1.0,
            Minimum = 0.0,
            Increment = 0.01,
            DecimalPlaces = 2
            )]
        public float Confidence { get; set; } = 0.3f;


        protected override void DoConstruct()
        {
            base.DoConstruct();
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
            string brief = AttributeName + ": " + Semantic + " - " + Confidence.ToString();

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
                e.Context.Variables["$targetConfidence"]= Confidence.ToString("0.00");
                // Speech events are matching if they have the same semantic and we are confident enough
                if (e.Semantic.Equals(Semantic))
                {
                    if (e.Confidence >= Confidence)
                    {
                        EventSpeechRecognized recognizedEvent = new EventSpeechRecognized();
                        recognizedEvent.Context = e.Context;
                        Properties.Settings.Default.EarManager.TriggerEvents(recognizedEvent);
                        return true;
                    }
                    else
                    {
                        // Not the best place to do that but here we go
                        // Trigger discarded event when confidence is not high enough
                        EventSpeechDiscarded discardedEvent = new EventSpeechDiscarded();
                        discardedEvent.Context = e.Context;                        
                        Properties.Settings.Default.EarManager.TriggerEvents(discardedEvent);
                    }
                }
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
                // Create our kinect manager if needed
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
                // Destroy our kinect manager while editing events
                Program.iFormMain.DestroyKinectManager();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Phrases) && !string.IsNullOrWhiteSpace(Semantic);
        }
    }
}
