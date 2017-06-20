using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Speech.Recognition;
using System.Runtime.InteropServices;

namespace SharpDisplayManager
{
    /**
     * TODO: Generalize speech recognition for it to work without Kinect?
     * Contains some code taken from Microsoft Samples Kinect SpeechBasics.
     */
    public class KinectManager
    {
        /// <summary>
        /// Speech recognition engine using audio data from Kinect.
        /// </summary>
        private SpeechRecognitionEngine iSpeechEngine = null;

        /// <summary>
        /// 
        /// </summary>
        private EventSpeech iEventSpeech = new EventSpeech();

        /// <summary>
        /// 
        /// </summary>
        private EventSpeechDiscarded iEventSpeechDiscarded = new EventSpeechDiscarded();

        /// <summary>
        /// 
        /// </summary>
        public void TryStartSpeechRecognition()
        {
            try
            {
                StartSpeechRecognition();
            }
            catch (Exception ex)
            {
                Debug.Print(ex.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void TryStopSpeechRecognition()
        {
            try
            {
                StopSpeechRecognition();
            }
            catch (Exception ex)
            {
                Debug.Print(ex.ToString());
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void StartSpeechRecognition()
        {
            RecognizerInfo ri = TryGetKinectRecognizer();

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

            var gb = new GrammarBuilder { Culture = ri.Culture };
            gb.Append(choices);

            var g = new Grammar(gb);

            iSpeechEngine.LoadGrammar(g);

            iSpeechEngine.SpeechRecognized += this.SpeechRecognized;
            iSpeechEngine.SpeechRecognitionRejected += this.SpeechRejected;

            iSpeechEngine.SetInputToDefaultAudioDevice();

            iSpeechEngine.RecognizeAsync(RecognizeMode.Multiple);
        }

        /// <summary>
        /// 
        /// </summary>
        private void StopSpeechRecognition()
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
        private static RecognizerInfo TryGetKinectRecognizer()
        {
            IEnumerable<RecognizerInfo> recognizers;

            // This is required to catch the case when an expected recognizer is not installed.
            // By default - the x86 Speech Runtime is always expected. 
            try
            {
                recognizers = SpeechRecognitionEngine.InstalledRecognizers();
            }
            catch (COMException)
            {
                Debug.Print("Warning: Can't find any speech recognizers.");
                return null;
            }

            foreach (RecognizerInfo recognizer in recognizers)
            {
                string value;
                recognizer.AdditionalInfo.TryGetValue("Kinect", out value);
                if ("True".Equals(value, StringComparison.OrdinalIgnoreCase) && "en-US".Equals(recognizer.Culture.Name, StringComparison.OrdinalIgnoreCase))
                {
                    return recognizer;
                }
            }

            return null;
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
            
        }

    }
}
