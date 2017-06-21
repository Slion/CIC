using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        protected abstract void StartSpeechRecognition();

        /// <summary>
        /// 
        /// </summary>
        protected abstract void StopSpeechRecognition();

    }
}
