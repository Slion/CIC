using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;

namespace SharpDisplayManager
{
    /**
     * Contains some code taken from Microsoft Samples Kinect SpeechBasics.
     */
    public abstract class SpeechManager
    {
        /// <summary>
        /// 
        /// </summary>
        protected EventSpeech iEventSpeech = new EventSpeech();

        /// <summary>
        /// 
        /// </summary>
        protected EventSpeechDiscarded iEventSpeechDiscarded = new EventSpeechDiscarded();

        /// <summary>
        /// Tells if the selected recognizer is a intended for Kinect.
        /// </summary>
        public bool IsKinectRecognizer = false;

        /// <summary>
        /// Our recognizer culture.
        /// </summary>
        public CultureInfo Culture = CultureInfo.CurrentCulture;

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
        public abstract void StartSpeechRecognition();

        /// <summary>
        /// 
        /// </summary>
        public abstract void StopSpeechRecognition();


    }
}
