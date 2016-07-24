using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using CecSharp;
using SharpLib.Ear;

namespace SharpDisplayManager
{
    class ConsumerElectronicControl
    {
        ///
        private PowerManager.SettingNotifier iPowerSettingNotifier;
        ///
        public Cec.Client Client;
        ///This flag will only work properly if both on and off events are monitored.
        ///TODO: have a more solid implementation
        public bool MonitorPowerOn;

        private bool iReconnectToPowerTv = false;

        public void TestSendKeys()
        {
            Client.TestSendKey();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aWndHandle"></param>
        /// <param name="aDeviceName"></param>
        /// <param name="aHdmiPort"></param>
        public void Start(IntPtr aWndHandle, string aDeviceName, byte aHdmiPort, bool aMonitorOn, bool aMonitorOff, bool aReconnectToPowerTv)
        {
            //Assuming monitor is on when we start up
            MonitorPowerOn = true;

            iReconnectToPowerTv = aReconnectToPowerTv;

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
            Client = new Cec.Client(aDeviceName,aHdmiPort, CecDeviceType.PlaybackDevice);
            ConnectCecClient();
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
            if (Client != null)
            {
                Client.Close();
                Client.Dispose();
                Client = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ConnectCecClient()
        {
            //Our client takes care of closing before trying to connect
            if (!Client.Open(1000))
            {
                Debug.WriteLine("WARNING: No CEC connection!");
            }
        }

        private void OnMonitorPowerOn()
        {
            ManagerEventAction.Current.GetEvent<EventMonitorPowerOn>().Trigger();

            Console.WriteLine("OnMonitorPowerOn");

            if (iReconnectToPowerTv)
            {
                ConnectCecClient();
            }            

            //Turn on the TV
            //iCecClient.Lib.PowerOnDevices(CecLogicalAddress.Tv);
            //iCecClient.Lib.SendKeypress(CecLogicalAddress.Tv,CecUserControlCode.PowerOnFunction,true);
            //Set ourselves as the active source
            Client.Lib.SetActiveSource(CecDeviceType.PlaybackDevice);
            MonitorPowerOn = true;
        }

        private void OnMonitorPowerOff()
        {
            ManagerEventAction.Current.GetEvent<EventMonitorPowerOff>().Trigger();

            Console.WriteLine("OnMonitorPowerOff");

            if (iReconnectToPowerTv)
            {
                ConnectCecClient();
            }

            //Try turning off the TV
            Client.Lib.StandbyDevices(CecLogicalAddress.Tv);
            //iCecClient.Lib.SendKeypress(CecLogicalAddress.Tv, CecUserControlCode.PowerOffFunction, true);
            //Tell everyone that we are no longer active
            //iCecClient.Lib.SetInactiveView();

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