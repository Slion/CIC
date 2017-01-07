using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//CSCore
using CSCore;
using CSCore.Win32;
using CSCore.DSP;
using CSCore.Streams;
using CSCore.CoreAudioAPI;
using CSCore.SoundIn;
// Visualization
using Visualization;


namespace SharpDisplayManager
{
    class AudioManager
    {
        // Volume management
        private MMDeviceEnumerator iMultiMediaDeviceEnumerator;
        private MMNotificationClient iMultiMediaNotificationClient;
        private MMDevice iMultiMediaDevice;
        private AudioEndpointVolume iAudioEndpointVolume;
        private AudioEndpointVolumeCallback iAudioEndpointVolumeCallback;
        EventHandler<DefaultDeviceChangedEventArgs> iDefaultDeviceChangedHandler;
        EventHandler<AudioEndpointVolumeCallbackEventArgs> iVolumeChangedHandler;

        // Audio visualization
        private WasapiCapture iSoundIn;
        private IWaveSource iWaveSource;
        private LineSpectrum iLineSpectrum;


        public LineSpectrum Spectrum { get { return iLineSpectrum; } }
        public AudioEndpointVolume Volume { get { return iAudioEndpointVolume; } }
        public MMDevice DefaultDevice { get { return iMultiMediaDevice; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aDefaultDeviceChangedHandler"></param>
        /// <param name="aVolumeChangedHandler"></param>
        public void Open(   EventHandler<DefaultDeviceChangedEventArgs> aDefaultDeviceChangedHandler,
                            EventHandler<AudioEndpointVolumeCallbackEventArgs> aVolumeChangedHandler)
        {
            //Create device and register default device change notification
            iMultiMediaDeviceEnumerator = new MMDeviceEnumerator();
            iMultiMediaNotificationClient = new MMNotificationClient(iMultiMediaDeviceEnumerator);
            iMultiMediaNotificationClient.DefaultDeviceChanged += iDefaultDeviceChangedHandler = aDefaultDeviceChangedHandler;
            iMultiMediaDevice = iMultiMediaDeviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render,Role.Multimedia);
            //Register to get volume modifications
            iAudioEndpointVolume = AudioEndpointVolume.FromDevice(iMultiMediaDevice);
            iAudioEndpointVolumeCallback = new AudioEndpointVolumeCallback();
            iAudioEndpointVolumeCallback.NotifyRecived += iVolumeChangedHandler = aVolumeChangedHandler;
            iAudioEndpointVolume.RegisterControlChangeNotify(iAudioEndpointVolumeCallback);

            StartAudioVisualization();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Close()
        {
            StopAudioVisualization();

            // Client up our MM objects in reverse order
            if (iAudioEndpointVolumeCallback != null && iAudioEndpointVolume != null)
            {
                iAudioEndpointVolume.UnregisterControlChangeNotify(iAudioEndpointVolumeCallback);
            }

            if (iAudioEndpointVolumeCallback != null)
            {
                iAudioEndpointVolumeCallback.NotifyRecived -= iVolumeChangedHandler;
                iAudioEndpointVolumeCallback = null;
            }

            if (iAudioEndpointVolume != null)
            {
                iAudioEndpointVolume.Dispose();
                iAudioEndpointVolume = null;
            }

            if (iMultiMediaDevice != null)
            {
                iMultiMediaDevice.Dispose();
                iMultiMediaDevice = null;
            }

            if (iMultiMediaNotificationClient != null)
            {
                iMultiMediaNotificationClient.DefaultDeviceChanged -= iDefaultDeviceChangedHandler;
                iMultiMediaNotificationClient.Dispose();
                iMultiMediaNotificationClient = null;
            }

            if (iMultiMediaDeviceEnumerator != null)
            {
                iMultiMediaDeviceEnumerator.Dispose();
                iMultiMediaDeviceEnumerator = null;
            }

        }


        /// <summary>
        /// 
        /// </summary>
        private void StartAudioVisualization()
        {
            //Open the default device 
            iSoundIn = new WasapiLoopbackCapture();
            //Our loopback capture opens the default render device by default so the following is not needed
            //iSoundIn.Device = MMDeviceEnumerator.DefaultAudioEndpoint(DataFlow.Render, Role.Console);
            iSoundIn.Initialize();

            SoundInSource soundInSource = new SoundInSource(iSoundIn);
            ISampleSource source = soundInSource.ToSampleSource();

            const FftSize fftSize = FftSize.Fft2048;
            //create a spectrum provider which provides fft data based on some input
            BasicSpectrumProvider spectrumProvider = new BasicSpectrumProvider(source.WaveFormat.Channels, source.WaveFormat.SampleRate, fftSize);

            //linespectrum and voiceprint3dspectrum used for rendering some fft data
            //in oder to get some fft data, set the previously created spectrumprovider 
            iLineSpectrum = new LineSpectrum(fftSize)
            {
                SpectrumProvider = spectrumProvider,
                UseAverage = false,
                BarCount = 16,
                BarSpacing = 1,
                IsXLogScale = true,
                ScalingStrategy = ScalingStrategy.Decibel
            };


            //the SingleBlockNotificationStream is used to intercept the played samples
            var notificationSource = new SingleBlockNotificationStream(source);
            //pass the intercepted samples as input data to the spectrumprovider (which will calculate a fft based on them)
            notificationSource.SingleBlockRead += (s, a) => spectrumProvider.Add(a.Left, a.Right);

            iWaveSource = notificationSource.ToWaveSource(16);


            // We need to read from our source otherwise SingleBlockRead is never called and our spectrum provider is not populated
            byte[] buffer = new byte[iWaveSource.WaveFormat.BytesPerSecond / 2];
            soundInSource.DataAvailable += (s, aEvent) =>
            {
                int read;
                while ((read = iWaveSource.Read(buffer, 0, buffer.Length)) > 0) ;
            };


            //Start recording
            iSoundIn.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        private void StopAudioVisualization()
        {
            if (iWaveSource != null)
            {
                iWaveSource.Dispose();
                iWaveSource = null;
            }

            if (iSoundIn != null)
            {
                iSoundIn.Stop();
                iSoundIn.Dispose();
                iSoundIn = null;
            }

        }


    }
}
