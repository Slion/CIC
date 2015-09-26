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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aWndHandle"></param>
        /// <param name="aDeviceName"></param>
        /// <param name="aHdmiPort"></param>
        public void Start(IntPtr aWndHandle, string aDeviceName, byte aHdmiPort, bool aMonitorOn, bool aMonitorOff)
        {
            //Create our power setting notifier and register the event we are insterrested in
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
            iCecClient = new Cec.Client(aDeviceName,aHdmiPort);
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
        }

        private void OnMonitorPowerOff()
        {
            Debug.WriteLine("OFF");
            iCecClient.Lib.StandbyDevices(CecLogicalAddress.Tv);
        }

        /// <summary>
        /// We need to handle WM_INPUT.
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