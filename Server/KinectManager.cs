using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using Microsoft.Speech.AudioFormat;
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
        /// Active Kinect sensor.
        /// </summary>
        private KinectSensor iKinectSensor = null;

        /// <summary>
        /// Stream for 32b-16b conversion.
        /// </summary>
        private KinectAudioStream iAudioStream = null;

        /// <summary>
        /// Speech recognition engine using audio data from Kinect.
        /// </summary>
        private SpeechRecognitionEngine iSpeechEngine = null;

        /// <summary>
        /// 
        /// </summary>
        private EventSpeech iEventSpeechMatching = new EventSpeech();

        /// <summary>
        /// 
        /// </summary>
        public void StartSpeechRecognition()
        {
            iKinectSensor = KinectSensor.GetDefault();

            if (iKinectSensor == null)
            {
                return;
            }

            // open the sensor
            iKinectSensor.Open();

            // grab the audio stream
            IReadOnlyList<AudioBeam> audioBeamList = iKinectSensor.AudioSource.AudioBeams;
            System.IO.Stream audioStream = audioBeamList[0].OpenInputStream();

            // create the convert stream
            iAudioStream = new KinectAudioStream(audioStream);


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
                string[] phrases = e.Phrases.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
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

            // let the convertStream know speech is going active
            iAudioStream.SpeechActive = true;

            // For long recognition sessions (a few hours or more), it may be beneficial to turn off adaptation of the acoustic model. 
            // This will prevent recognition accuracy from degrading over time.
            ////speechEngine.UpdateRecognizerSetting("AdaptationOn", 0);

            iSpeechEngine.SetInputToAudioStream(iAudioStream, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
            iSpeechEngine.RecognizeAsync(RecognizeMode.Multiple);
        }

        /// <summary>
        /// 
        /// </summary>
        public void StopSpeechRecognition()
        {
            if (null != iAudioStream)
            {
                iAudioStream.SpeechActive = false;
            }

            if (null != iSpeechEngine)
            {
                iSpeechEngine.SpeechRecognized -= this.SpeechRecognized;
                iSpeechEngine.SpeechRecognitionRejected -= this.SpeechRejected;
                iSpeechEngine.RecognizeAsyncStop();
            }

            if (null != iKinectSensor)
            {
                iKinectSensor.Close();
                iKinectSensor = null;
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
            iEventSpeechMatching.Semantic = e.Result.Semantics.Value.ToString();
            iEventSpeechMatching.Confidence = e.Result.Confidence;
            // Trigger any matching events
            Properties.Settings.Default.EarManager.TriggerEvents(iEventSpeechMatching);
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
