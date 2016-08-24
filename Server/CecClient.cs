/*
 * This file is part of the libCEC(R) library.
 *
 * libCEC(R) is Copyright (C) 2011-2013 Pulse-Eight Limited.    All rights reserved.
 * libCEC(R) is an original work, containing original code.
 *
 * libCEC(R) is a trademark of Pulse-Eight Limited.
 *
 * This program is dual-licensed; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.    See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA 02111-1307, USA.
 *
 *
 * Alternatively, you can license this library under a commercial license,
 * please contact Pulse-Eight Licensing for more information.
 *
 * For more information contact:
 * Pulse-Eight Licensing             <license@pulse-eight.com>
 *         http://www.pulse-eight.com/
 *         http://www.pulse-eight.net/
 */

using System;
using System.Text;
using CecSharp;
using System.Threading;
using System.Diagnostics;

namespace Cec
{
    class Client : CecCallbackMethods
    {
        /// <summary>
        /// Enable public static access
        /// </summary>
        public static Client Static;

        /// <summary>
        /// Provide direct access to CEC library
        /// </summary>
        public LibCecSharp Lib
        {
            get
            {
                return iLib;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int LogLevel = (int)CecLogLevel.Notice;

        /// <summary>
        /// 
        /// </summary>
        public bool FilterOutPollLogs = true;

        /// <summary>
        /// 
        /// </summary>
        private LibCecSharp iLib;
        /// <summary>
        /// 
        /// </summary>
        private LibCECConfiguration Config;

        /// <summary>
        /// 
        /// </summary>
        private bool iConnected;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aDeviceName"></param>
        /// <param name="aHdmiPort"></param>
        public Client(string aDeviceName, byte aHdmiPort, CecDeviceType aDeviceType = CecDeviceType.PlaybackDevice)
        {
            Config = new LibCECConfiguration();
            Config.DeviceTypes.Types[0] = aDeviceType;
            Config.DeviceName = aDeviceName;
            Config.HDMIPort = aHdmiPort;
            //Config.ClientVersion = LibCECConfiguration.CurrentVersion;
            Config.SetCallbacks(this);

            iLib = new LibCecSharp(Config);
            iLib.InitVideoStandalone();

            if (Static != null)
            {
                Trace.WriteLine("WARNING: CEC client static already exists");
            }
            else
            {
                Static = this;
            }
            

            //Trace.WriteLine("CEC Parser created - libCEC version " + Lib.VersionToString(Config.ServerVersion));
            Trace.WriteLine("INFO: CEC Parser created - libCEC version " + Config.ServerVersion);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="alert"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public override int ReceiveAlert(CecAlert alert, CecParameter data)
        {
            string log = "CEC alert: " + alert.ToString();
            if (data != null && data.Type == CecParameterType.ParameterTypeString)
            {
                log += " " + data.Data;
            }

            Trace.WriteLine(log);

            Close();
            //Try reconnect
            Open(1000);
            return 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newVal"></param>
        /// <returns></returns>
        public override int ReceiveMenuStateChange(CecMenuState newVal)
        {
            Trace.WriteLine("CEC menu state changed to: " + iLib.ToString(newVal));
            return 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logicalAddress"></param>
        /// <param name="activated"></param>
        public override void SourceActivated(CecLogicalAddress logicalAddress, bool activated)
        {
            Trace.WriteLine("CEC source activated: " + iLib.ToString(logicalAddress) + "/" + activated.ToString() );
            return;
        }

        public override int ReceiveCommand(CecCommand command)
        {
            Trace.WriteLine(string.Format("CEC command '{5}' from {0} to {1} - Ack: {2} Eom: {3} OpcodeSet: {4} Timeout: {6}",
                iLib.ToString(command.Initiator),
                iLib.ToString(command.Destination),
                command.Ack.ToString(),
                command.Eom.ToString(),
                command.OpcodeSet.ToString(),
                iLib.ToString(command.Opcode),
                command.TransmitTimeout.ToString()
                ));
            return 1;
        }

        public override int ReceiveKeypress(CecKeypress key)
        {
            Trace.WriteLine(string.Format("CEC keypress: {0} Duration:{1} Empty: {2}",
                key.Keycode.ToString(), key.Duration.ToString(), key.Empty.ToString()));
            return 1;
        }

        public override int ReceiveLogMessage(CecLogMessage message)
        {
            if (((int)message.Level & LogLevel) == (int)message.Level)
            {
                string strLevel = "";
                switch (message.Level)
                {
                    case CecLogLevel.Error:
                        strLevel = "ERROR: ";
                        break;
                    case CecLogLevel.Warning:
                        strLevel = "WARNING: ";
                        break;
                    case CecLogLevel.Notice:
                        strLevel = "NOTICE: ";
                        break;
                    case CecLogLevel.Traffic:
                        strLevel = "TRAFFIC: ";
                        break;
                    case CecLogLevel.Debug:
                        strLevel = "DEBUG: ";
                        if (message.Message.IndexOf("POLL") != -1 && FilterOutPollLogs)
                        {
                            //We have an option to prevent spamming with poll debug logs
                            return 1;
                        }
                        break;
                    default:
                        break;
                }
                string strLog = string.Format("{0} {1} {2}", strLevel, message.Time, message.Message);
                Trace.WriteLine(strLog);
                
            }
            return 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public bool Open(int timeout)
        {
            Close();         
            CecAdapter[] adapters = iLib.FindAdapters(string.Empty);
            if (adapters.Length > 0)
            {
                Open(adapters[0].ComPort, timeout);                
            }                
            else
            {
                Trace.WriteLine("CEC did not find any adapters");
            }

            return iConnected;
        }

        public bool Open(string port, int timeout)
        {
            Close();
            iConnected = iLib.Open(port, timeout);
            return iConnected;
        }

        public void Close()
        {            
            iLib.Close();
            iConnected = false;
        }


        public void TestSendKey()
        {
            //SetupMenu: opens option menu
            //RootMenu: opens Android home menu
            //ContentsMenu: nop
            //FavoriteMenu: nop

            //Philips TopMenu = 16
            //Philips PopupMenu = 17

            //bool res = iLib.SendKeypress(CecLogicalAddress.Tv, CecUserControlCode.DisplayInformation, true);
            //Thread.Sleep(3000); //Wait few seconds for menu to open
                                //res = iLib.SendKeypress(CecLogicalAddress.Tv, CecUserControlCode.SetupMenu, true);

            for (int i = 0; i < 256; i++)
            {
                //Thread.Sleep(100);
                //res = iLib.SendKeyRelease(CecLogicalAddress.Tv, true);
                //Thread.Sleep(100);
                switch ((CecUserControlCode)i)
                {
                    case CecUserControlCode.Power:
                    case CecUserControlCode.PowerOffFunction:
                    case CecUserControlCode.PowerOnFunction:
                    case CecUserControlCode.PowerToggleFunction:
                    case CecUserControlCode.ElectronicProgramGuide:
                    case CecUserControlCode.InputSelect:
                    case CecUserControlCode.RootMenu:

                        break;

                    default:
                        Trace.WriteLine(i.ToString());
                        Thread.Sleep(1000);
                        iLib.SendKeypress(CecLogicalAddress.Tv, (CecUserControlCode)i, true);
                        
                        break;

                }
                
                //
            }


            for (int i=0;i<7;i++)
            {
                //Thread.Sleep(100);
                //res = iLib.SendKeyRelease(CecLogicalAddress.Tv, true);
                //Thread.Sleep(100);
                //res = iLib.SendKeypress(CecLogicalAddress.Tv, CecUserControlCode.Down, true);
                //
            }

            //res = iLib.SendKeypress(CecLogicalAddress.Tv, CecUserControlCode.Select, true);

            //res = iLib.SendKeypress(CecLogicalAddress.Tv, CecUserControlCode.Select, true);
            //res = iLib.SendKeyRelease(CecLogicalAddress.Tv, true);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Scan()
        {
            string scanRes = "";
            scanRes += "CEC bus information\n";
            scanRes += "===================\n";
            CecLogicalAddresses addresses = Lib.GetActiveDevices();
            for (int iPtr = 0; iPtr < addresses.Addresses.Length; iPtr++)
            {
                CecLogicalAddress address = (CecLogicalAddress) iPtr;
                if (!addresses.IsSet(address))
                    continue;

                CecVendorId iVendorId = Lib.GetDeviceVendorId(address);
                bool bActive = Lib.IsActiveDevice(address);
                ushort iPhysicalAddress = Lib.GetDevicePhysicalAddress(address);
                string strAddr = Lib.PhysicalAddressToString(iPhysicalAddress);
                CecVersion iCecVersion = Lib.GetDeviceCecVersion(address);
                CecPowerStatus power = Lib.GetDevicePowerStatus(address);
                string osdName = Lib.GetDeviceOSDName(address);
                string lang = Lib.GetDeviceMenuLanguage(address);

                scanRes += "device #" + iPtr + ": " + Lib.ToString(address) + "\n";
                scanRes += "address:       " + strAddr + "\n";
                scanRes += "active source: " + (bActive ? "yes" : "no") + "\n";
                scanRes += "vendor:        " + Lib.ToString(iVendorId) + "\n";
                scanRes += "osd string:    " + osdName + "\n";
                scanRes += "CEC version:   " + Lib.ToString(iCecVersion) + "\n";
                scanRes += "power status:  " + Lib.ToString(power) + "\n";
                if (!string.IsNullOrEmpty(lang))
                    scanRes += "language:      " + lang + "\n";
                scanRes += "===================" + "\n";
            }

            Trace.Write(scanRes);
        }

        public void ListAdapters()
        {
            int iAdapter = 0;
            foreach (CecAdapter adapter in iLib.FindAdapters(string.Empty))
            {
                Trace.WriteLine("Adapter:    " + iAdapter++);
                Trace.WriteLine("Path:         " + adapter.Path);
                Trace.WriteLine("Com port: " + adapter.ComPort);
            }
        }
       

    }
}
