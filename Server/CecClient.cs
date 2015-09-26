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

namespace Cec
{
    class Client : CecCallbackMethods
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aDeviceName"></param>
        /// <param name="aHdmiPort"></param>
        public Client(string aDeviceName, byte aHdmiPort)
        {
            Config = new LibCECConfiguration();
            Config.DeviceTypes.Types[0] = CecDeviceType.Tv;
            Config.DeviceName = aDeviceName;
            Config.HDMIPort = aHdmiPort;
            //Config.ClientVersion = LibCECConfiguration.CurrentVersion;
            Config.SetCallbacks(this);
            LogLevel = (int)CecLogLevel.All;

            iLib = new LibCecSharp(Config);
            iLib.InitVideoStandalone();

            //Console.WriteLine("CEC Parser created - libCEC version " + Lib.VersionToString(Config.ServerVersion));
            Console.WriteLine("CEC Parser created - libCEC version " + Config.ServerVersion);
        }

        public override int ReceiveCommand(CecCommand command)
        {
            return 1;
        }

        public override int ReceiveKeypress(CecKeypress key)
        {
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
                        strLevel = "ERROR:     ";
                        break;
                    case CecLogLevel.Warning:
                        strLevel = "WARNING: ";
                        break;
                    case CecLogLevel.Notice:
                        strLevel = "NOTICE:    ";
                        break;
                    case CecLogLevel.Traffic:
                        strLevel = "TRAFFIC: ";
                        break;
                    case CecLogLevel.Debug:
                        strLevel = "DEBUG:     ";
                        break;
                    default:
                        break;
                }
                string strLog = string.Format("{0} {1,16} {2}", strLevel, message.Time, message.Message);
                Console.WriteLine(strLog);
            }
            return 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public bool Connect(int timeout)
        {
            CecAdapter[] adapters = iLib.FindAdapters(string.Empty);
            if (adapters.Length > 0)
                return Connect(adapters[0].ComPort, timeout);
            else
            {
                Console.WriteLine("Did not find any CEC adapters");
                return false;
            }
        }

        public bool Connect(string port, int timeout)
        {
            return iLib.Open(port, timeout);
        }

        public void Close()
        {
            iLib.Close();
        }

        public void ListDevices()
        {
            int iAdapter = 0;
            foreach (CecAdapter adapter in iLib.FindAdapters(string.Empty))
            {
                Console.WriteLine("Adapter:    " + iAdapter++);
                Console.WriteLine("Path:         " + adapter.Path);
                Console.WriteLine("Com port: " + adapter.ComPort);
            }
        }

        void ShowConsoleHelp()
        {
            Console.WriteLine(
                "================================================================================" + Environment.NewLine +
                "Available commands:" + Environment.NewLine +
                Environment.NewLine +
                "[tx] {bytes}                            transfer bytes over the CEC line." + Environment.NewLine +
                "[txn] {bytes}                         transfer bytes but don't wait for transmission ACK." + Environment.NewLine +
                "[on] {address}                        power on the device with the given logical address." + Environment.NewLine +
                "[standby] {address}             put the device with the given address in standby mode." + Environment.NewLine +
                "[la] {logical_address}        change the logical address of the CEC adapter." + Environment.NewLine +
                "[pa] {physical_address}     change the physical address of the CEC adapter." + Environment.NewLine +
                "[osd] {addr} {string}         set OSD message on the specified device." + Environment.NewLine +
                "[ver] {addr}                            get the CEC version of the specified device." + Environment.NewLine +
                "[ven] {addr}                            get the vendor ID of the specified device." + Environment.NewLine +
                "[lang] {addr}                         get the menu language of the specified device." + Environment.NewLine +
                "[pow] {addr}                            get the power status of the specified device." + Environment.NewLine +
                "[poll] {addr}                         poll the specified device." + Environment.NewLine +
                "[scan]                                        scan the CEC bus and display device info" + Environment.NewLine +
                "[mon] {1|0}                             enable or disable CEC bus monitoring." + Environment.NewLine +
                "[log] {1 - 31}                        change the log level. see cectypes.h for values." + Environment.NewLine +
                "[ping]                                        send a ping command to the CEC adapter." + Environment.NewLine +
                "[bl]                                            to let the adapter enter the bootloader, to upgrade" + Environment.NewLine +
                "                                                    the flash rom." + Environment.NewLine +
                "[r]                                             reconnect to the CEC adapter." + Environment.NewLine +
                "[h] or [help]                         show this help." + Environment.NewLine +
                "[q] or [quit]                         to quit the CEC test client and switch off all" + Environment.NewLine +
                "                                                    connected CEC devices." + Environment.NewLine +
                "================================================================================");
        }

        public void MainLoop()
        {
            bool bContinue = true;
            string command;
            while (bContinue)
            {
                Console.WriteLine("waiting for input");

                command = Console.ReadLine();
                if (command != null && command.Length == 0)
                    continue;
                string[] splitCommand = command.Split(' ');
                if (splitCommand[0] == "tx" || splitCommand[0] == "txn")
                {
                    CecCommand bytes = new CecCommand();
                    for (int iPtr = 1; iPtr < splitCommand.Length; iPtr++)
                    {
                        bytes.PushBack(byte.Parse(splitCommand[iPtr], System.Globalization.NumberStyles.HexNumber));
                    }

                    if (command == "txn")
                        bytes.TransmitTimeout = 0;

                    iLib.Transmit(bytes);
                }
                else if (splitCommand[0] == "on")
                {
                    if (splitCommand.Length > 1)
                        iLib.PowerOnDevices((CecLogicalAddress)byte.Parse(splitCommand[1], System.Globalization.NumberStyles.HexNumber));
                    else
                        iLib.PowerOnDevices(CecLogicalAddress.Broadcast);
                }
                else if (splitCommand[0] == "standby")
                {
                    if (splitCommand.Length > 1)
                        iLib.StandbyDevices((CecLogicalAddress)byte.Parse(splitCommand[1], System.Globalization.NumberStyles.HexNumber));
                    else
                        iLib.StandbyDevices(CecLogicalAddress.Broadcast);
                }
                else if (splitCommand[0] == "poll")
                {
                    bool bSent = false;
                    if (splitCommand.Length > 1)
                        bSent = iLib.PollDevice((CecLogicalAddress)byte.Parse(splitCommand[1], System.Globalization.NumberStyles.HexNumber));
                    else
                        bSent = iLib.PollDevice(CecLogicalAddress.Broadcast);
                    if (bSent)
                        Console.WriteLine("POLL message sent");
                    else
                        Console.WriteLine("POLL message not sent");
                }
                else if (splitCommand[0] == "la")
                {
                    if (splitCommand.Length > 1)
                        iLib.SetLogicalAddress((CecLogicalAddress)byte.Parse(splitCommand[1], System.Globalization.NumberStyles.HexNumber));
                }
                else if (splitCommand[0] == "pa")
                {
                    if (splitCommand.Length > 1)
                        iLib.SetPhysicalAddress(ushort.Parse(splitCommand[1], System.Globalization.NumberStyles.HexNumber));
                }
                else if (splitCommand[0] == "osd")
                {
                    if (splitCommand.Length > 2)
                    {
                        StringBuilder osdString = new StringBuilder();
                        for (int iPtr = 1; iPtr < splitCommand.Length; iPtr++)
                        {
                            osdString.Append(splitCommand[iPtr]);
                            if (iPtr != splitCommand.Length - 1)
                                osdString.Append(" ");
                        }
                        iLib.SetOSDString((CecLogicalAddress)byte.Parse(splitCommand[1], System.Globalization.NumberStyles.HexNumber), CecDisplayControl.DisplayForDefaultTime, osdString.ToString());
                    }
                }
                else if (splitCommand[0] == "ping")
                {
                    iLib.PingAdapter();
                }
                else if (splitCommand[0] == "mon")
                {
                    bool enable = splitCommand.Length > 1 ? splitCommand[1] == "1" : false;
                    iLib.SwitchMonitoring(enable);
                }
                else if (splitCommand[0] == "bl")
                {
                    iLib.StartBootloader();
                }
                else if (splitCommand[0] == "lang")
                {
                    if (splitCommand.Length > 1)
                    {
                        string language = iLib.GetDeviceMenuLanguage((CecLogicalAddress)byte.Parse(splitCommand[1], System.Globalization.NumberStyles.HexNumber));
                        Console.WriteLine("Menu language: " + language);
                    }
                }
                else if (splitCommand[0] == "ven")
                {
                    if (splitCommand.Length > 1)
                    {
                        CecVendorId vendor = iLib.GetDeviceVendorId((CecLogicalAddress)byte.Parse(splitCommand[1], System.Globalization.NumberStyles.HexNumber));
                        Console.WriteLine("Vendor ID: " + iLib.ToString(vendor));
                    }
                }
                else if (splitCommand[0] == "ver")
                {
                    if (splitCommand.Length > 1)
                    {
                        CecVersion version = iLib.GetDeviceCecVersion((CecLogicalAddress)byte.Parse(splitCommand[1], System.Globalization.NumberStyles.HexNumber));
                        Console.WriteLine("CEC version: " + iLib.ToString(version));
                    }
                }
                else if (splitCommand[0] == "pow")
                {
                    if (splitCommand.Length > 1)
                    {
                        CecPowerStatus power = iLib.GetDevicePowerStatus((CecLogicalAddress)byte.Parse(splitCommand[1], System.Globalization.NumberStyles.HexNumber));
                        Console.WriteLine("power status: " + iLib.ToString(power));
                    }
                }
                else if (splitCommand[0] == "r")
                {
                    Console.WriteLine("closing the connection");
                    iLib.Close();

                    Console.WriteLine("opening a new connection");
                    Connect(10000);

                    Console.WriteLine("setting active source");
                    iLib.SetActiveSource(CecDeviceType.PlaybackDevice);
                }
                else if (splitCommand[0] == "scan")
                {
                    StringBuilder output = new StringBuilder();
                    output.AppendLine("CEC bus information");
                    output.AppendLine("===================");
                    CecLogicalAddresses addresses = iLib.GetActiveDevices();
                    for (int iPtr = 0; iPtr < addresses.Addresses.Length; iPtr++)
                    {
                        CecLogicalAddress address = (CecLogicalAddress)iPtr;
                        if (!addresses.IsSet(address))
                            continue;

                        CecVendorId iVendorId = iLib.GetDeviceVendorId(address);
                        bool bActive = iLib.IsActiveDevice(address);
                        ushort iPhysicalAddress = iLib.GetDevicePhysicalAddress(address);
                        string strAddr = "todo: fixme"; //Lib.PhysicalAddressToString(iPhysicalAddress);
                        CecVersion iCecVersion = iLib.GetDeviceCecVersion(address);
                        CecPowerStatus power = iLib.GetDevicePowerStatus(address);
                        string osdName = iLib.GetDeviceOSDName(address);
                        string lang = iLib.GetDeviceMenuLanguage(address);

                        output.AppendLine("device #" + iPtr + ": " + iLib.ToString(address));
                        output.AppendLine("address:             " + strAddr);
                        output.AppendLine("active source: " + (bActive ? "yes" : "no"));
                        output.AppendLine("vendor:                " + iLib.ToString(iVendorId));
                        output.AppendLine("osd string:        " + osdName);
                        output.AppendLine("CEC version:     " + iLib.ToString(iCecVersion));
                        output.AppendLine("power status:    " + iLib.ToString(power));
                        if (!string.IsNullOrEmpty(lang))
                            output.AppendLine("language:            " + lang);
                        output.AppendLine("");
                    }
                    Console.WriteLine(output.ToString());
                }
                else if (splitCommand[0] == "h" || splitCommand[0] == "help")
                    ShowConsoleHelp();
                else if (splitCommand[0] == "q" || splitCommand[0] == "quit")
                    bContinue = false;
                else if (splitCommand[0] == "log" && splitCommand.Length > 1)
                    LogLevel = int.Parse(splitCommand[1]);                
            }
        }

        /// TODO: remove that
        static void Main(string[] args)
        {
            Client p = new Client("CEC",2);
            if (p.Connect(10000))
            {
                p.MainLoop();
            }
            else
            {
                Console.WriteLine("Could not open a connection to the CEC adapter");
            }
        }

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
        private int LogLevel;
        /// <summary>
        /// 
        /// </summary>
        private LibCecSharp iLib;
        /// <summary>
        /// 
        /// </summary>
        private LibCECConfiguration Config;
    }
}
