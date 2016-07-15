using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using CecSharp;

namespace SharpDisplayManager
{
    class ConsumerElectronicControl
    {
        ///
        private PowerManager.SettingNotifier iPowerSettingNotifier;
        ///
        private Cec.Client iCecClient;
        ///This flag will only work properly if both on and off events are monitored.
        ///TODO: have a more solid implementation
        public bool MonitorPowerOn;

        public void TestSendKeys()
        {
            iCecClient.TestSendKey();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aWndHandle"></param>
        /// <param name="aDeviceName"></param>
        /// <param name="aHdmiPort"></param>
        public void Start(IntPtr aWndHandle, string aDeviceName, byte aHdmiPort, bool aMonitorOn, bool aMonitorOff)
        {
            //Assuming monitor is on when we start up
            MonitorPowerOn = true;

            //Create our power setting notifier and register the event we are interested in
            iPowerSettingNotifier = new PowerManager.SettingNotifier(aWndHandle);

            //
            if (aMonitorOn)
            {
                iPowerSettingNotifier.OnMonitorPowerOn += OnMonitorPowerOn;
            }

            //
            if (aMonitorOff)
            {
                iPowerSettingNotifier.OnMonitorPowerOff += OnMonitorPowerOff;
            }

            //CEC
            iCecClient = new Cec.Client(aDeviceName,aHdmiPort, CecLogLevel.Notice);
            if (!iCecClient.Connect(1000))
            {
                Debug.WriteLine("WARNING: No CEC connection!");
            }
        }

        //
        public void Stop()
        {
            //
            if (iPowerSettingNotifier != null)
            {
                iPowerSettingNotifier.OnMonitorPowerOn -= OnMonitorPowerOn;
                iPowerSettingNotifier.OnMonitorPowerOff -= OnMonitorPowerOff;
                iPowerSettingNotifier = null;
            }
            //
            if (iCecClient != null)
            {
                iCecClient.Close();
                iCecClient.Dispose();
                iCecClient = null;
            }
        }


        private void OnMonitorPowerOn()
        {
            Debug.WriteLine("ON");
            iCecClient.Lib.PowerOnDevices(CecLogicalAddress.Tv);
            iCecClient.Lib.SetActiveSource(CecDeviceType.Tv);
            MonitorPowerOn = true;
        }

        private void OnMonitorPowerOff()
        {
            Debug.WriteLine("OFF");
            iCecClient.Lib.StandbyDevices(CecLogicalAddress.Tv);
            MonitorPowerOn = false;
        }

        /// <summary>
        /// We need to handle WM_POWERBROADCAST.
        /// </summary>
        /// <param name="message"></param>
        public void OnWndProc(ref Message message)
        {
            //Hook in our power manager
            if (iPowerSettingNotifier != null)
            {
                iPowerSettingNotifier.WndProc(ref message);
            }
        }

    }
}