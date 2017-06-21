
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Globalization;
using <#= SpeechType #>.Speech.Recognition;

namespace SharpDisplayManager
{
    class SpeechManager<#= SpeechType #> : SpeechManager
    {
        /// <summary>
        /// Speech recognition engine using audio data from Kinect.
        /// </summary>
        private SpeechRecognitionEngine iSpeechEngine = null;

        /// <summary>
        /// 
        /// </summary>
        public override void StartSpeechRecognition()
        {
            RecognizerInfo ri = GetKinectRecognizer();

            if (null == ri)
            {
                return;
            }

            iSpeechEngine = new SpeechRecognitionEngine(ri.Id);

            // Create a speech recognition grammar based on our speech events
            var ear = Properties.Settings.Default.EarManager;
            var choices = new Choices();
            bool noChoices = true;
            foreach (EventSpeech e in ear.Events.Where(e => e.GetType() == typeof(EventSpeech)))
            {
                // For each events associates its phrases with its semantic
                string[] phrases = e.Phrases.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                foreach (string phrase in phrases)
                {
                    if (string.IsNullOrWhiteSpace(phrase))
                    {
                        // defensive
                        continue;
                    }
                    choices.Add(new SemanticResultValue(phrase, e.Semantic));
                    noChoices = false;
                }
            }

            if (noChoices)
            {
                // Grammar build throws exception if no choice registered
                // TODO: review error handling in that function.
                // I guess we should have a Try variant.
                return;
            }

            // Set our culture
            Culture = ri.Culture;
            var gb = new GrammarBuilder { Culture = ri.Culture };
            gb.Append(choices);

            var g = new Grammar(gb);

            iSpeechEngine.LoadGrammar(g);

            iSpeechEngine.SpeechRecognized += this.SpeechRecognized;
            iSpeechEngine.SpeechRecognitionRejected += this.SpeechRejected;

            // For long recognition sessions (a few hours or more), it may be beneficial to turn off adaptation of the acoustic model. 
            // This will prevent recognition accuracy from degrading over time.
            iSpeechEngine.UpdateRecognizerSetting("AdaptationOn", 0);

            iSpeechEngine.SetInputToDefaultAudioDevice();
            iSpeechEngine.RecognizeAsync(RecognizeMode.Multiple);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void StopSpeechRecognition()
        {

            if (null != iSpeechEngine)
            {
                iSpeechEngine.SpeechRecognized -= this.SpeechRecognized;
                iSpeechEngine.SpeechRecognitionRejected -= this.SpeechRejected;
                iSpeechEngine.RecognizeAsyncStop();
            }

        }


        /// <summary>
        /// Gets the metadata for the speech recognizer (acoustic model) most suitable to
        /// process audio from Kinect device.
        /// </summary>
        /// <returns>
        /// RecognizerInfo if found, <code>null</code> otherwise.
        /// </returns>
        private static RecognizerInfo GetKinectRecognizer()
        {
            IEnumerable<RecognizerInfo> recognizers = SpeechRecognitionEngine.InstalledRecognizers();

            CultureInfo ci = CultureInfo.CurrentCulture;
            RecognizerInfo ri = null;

            foreach (RecognizerInfo recognizer in recognizers)
            {
                string value;
                //recognizer.AdditionalInfo.TryGetValue("Kinect", out value);
                //if ("True".Equals(value, StringComparison.OrdinalIgnoreCase) && "en-US".Equals(recognizer.Culture.Name, StringComparison.OrdinalIgnoreCase))
                
                if (ri==null)
                {
                    ri = recognizer;
                }
                else if (recognizer.Name.Equals(ci.Name))
                {
                    // We have a recognizer matching our current culture
                    return recognizer;
                }
            }

            // TODO: throw an exception if null
            return ri;
        }


        /// <summary>
        /// Handler for recognized speech events.
        /// </summary>
        /// <param name="sender">object sending the event.</param>
        /// <param name="e">event arguments.</param>
        private void SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            // Set our search clue semantic
            iEventSpeech.Semantic = e.Result.Semantics.Value.ToString();
            iEventSpeech.Confidence = e.Result.Confidence;
            iEventSpeech.Context.Variables.Clear();
            iEventSpeech.Context.Variables["$confidence"] = iEventSpeech.Confidence.ToString("0.00");
            iEventSpeech.Context.Variables["$semantic"] = iEventSpeech.Semantic;
            // Trigger any matching events
            Properties.Settings.Default.EarManager.TriggerEvents(iEventSpeech);
        }

        /// <summary>
        /// Handler for rejected speech events.
        /// </summary>
        /// <param name="sender">object sending the event.</param>
        /// <param name="e">event arguments.</param>
        private void SpeechRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            iEventSpeechDiscarded.Context.Variables.Clear();
            iEventSpeechDiscarded.Context.Variables["$confidence"] = iEventSpeech.Confidence.ToString("0.00");
            iEventSpeechDiscarded.Context.Variables["$semantic"] = iEventSpeech.Semantic;
            // TODO: Look up the event with that semantic and add $targetConfidence variable
            Properties.Settings.Default.EarManager.TriggerEvents(iEventSpeechDiscarded);
        }

    }
}
