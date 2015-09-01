using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32.SafeHandles;
//
using Hid = SharpLib.Hid;
using SharpLib.Win32;

namespace SharpDisplayManager
{
    /// <summary>
    /// Implement handling of HID input reports notably to be able to launch an application using the Green Start button from IR remotes.
    /// </summary>
    [System.ComponentModel.DesignerCategory("Code")]
    public class MainFormHid : Form
    {
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll", EntryPoint = "SwitchToThisWindow")]
        public static extern void SwitchToThisWindow([System.Runtime.InteropServices.InAttribute()] System.IntPtr hwnd, [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)] bool fUnknown);
        //
        public delegate void OnHidEventDelegate(object aSender, Hid.Event aHidEvent);

        /// <summary>
        /// Use notably to handle green start key from IR remote control
        /// </summary>
        private Hid.Handler iHidHandler;

        /// <summary>
        /// Register HID devices so that we receive corresponding WM_INPUT messages.
        /// </summary>
        protected void RegisterHidDevices()
        {
            // Register the input device to receive the commands from the
            // remote device. See http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnwmt/html/remote_control.asp
            // for the vendor defined usage page.

            RAWINPUTDEVICE[] rid = new RAWINPUTDEVICE[5];

            int i = 0;
            rid[i].usUsagePage = (ushort)SharpLib.Hid.UsagePage.WindowsMediaCenterRemoteControl;
            rid[i].usUsage = (ushort)SharpLib.Hid.UsageCollection.WindowsMediaCenter.WindowsMediaCenterRemoteControl;
            rid[i].dwFlags = Const.RIDEV_INPUTSINK;
            rid[i].hwndTarget = Handle;

            i++;
            rid[i].usUsagePage = (ushort)SharpLib.Hid.UsagePage.Consumer;
            rid[i].usUsage = (ushort)SharpLib.Hid.UsageCollection.Consumer.ConsumerControl;
            rid[i].dwFlags = Const.RIDEV_INPUTSINK;
            rid[i].hwndTarget = Handle;

            i++;
            rid[i].usUsagePage = (ushort)SharpLib.Hid.UsagePage.Consumer;
            rid[i].usUsage = (ushort)SharpLib.Hid.UsageCollection.Consumer.Selection;
            rid[i].dwFlags = Const.RIDEV_INPUTSINK;
            rid[i].hwndTarget = Handle;

            i++;
            rid[i].usUsagePage = (ushort)SharpLib.Hid.UsagePage.GenericDesktopControls;
            rid[i].usUsage = (ushort)SharpLib.Hid.UsageCollection.GenericDesktop.SystemControl;
            rid[i].dwFlags = Const.RIDEV_INPUTSINK;
            rid[i].hwndTarget = Handle;

            i++;
            rid[i].usUsagePage = (ushort)SharpLib.Hid.UsagePage.GenericDesktopControls;
            rid[i].usUsage = (ushort)SharpLib.Hid.UsageCollection.GenericDesktop.GamePad;
            rid[i].dwFlags = Const.RIDEV_INPUTSINK;
            rid[i].hwndTarget = Handle;

            //i++;
            //rid[i].usUsagePage = (ushort)SharpLib.Hid.UsagePage.GenericDesktopControls;
            //rid[i].usUsage = (ushort)SharpLib.Hid.UsageCollection.GenericDesktop.Keyboard;
            //rid[i].dwFlags = Const.RIDEV_EXINPUTSINK;
            //rid[i].hwndTarget = Handle;

            //i++;
            //rid[i].usUsagePage = (ushort)Hid.UsagePage.GenericDesktopControls;
            //rid[i].usUsage = (ushort)Hid.UsageCollection.GenericDesktop.Mouse;
            //rid[i].dwFlags = Const.RIDEV_EXINPUTSINK;
            //rid[i].hwndTarget = aHWND;


            iHidHandler = new SharpLib.Hid.Handler(rid);
            if (!iHidHandler.IsRegistered)
            {
                Debug.WriteLine("Failed to register raw input devices: " + Marshal.GetLastWin32Error().ToString());
            }
            iHidHandler.OnHidEvent += HandleHidEventThreadSafe;
        }

        /// <summary>
        /// Here we receive HID events from our HID library.
        /// </summary>
        /// <param name="aSender"></param>
        /// <param name="aHidEvent"></param>
        public void HandleHidEventThreadSafe(object aSender, SharpLib.Hid.Event aHidEvent)
        {
            if (aHidEvent.IsStray)
            {
                //Stray event just ignore it
                return;
            }

            if (this.InvokeRequired)
            {
                //Not in the proper thread, invoke ourselves
                OnHidEventDelegate d = new OnHidEventDelegate(HandleHidEventThreadSafe);
                this.Invoke(d, new object[] { aSender, aHidEvent });
            }
            else
            {
                if (aHidEvent.Usages.Count == 0)
                {
                    //No usage, nothing to do then
                    return;
                }

                //We are in the proper thread
                if (aHidEvent.UsagePage == (ushort) Hid.UsagePage.WindowsMediaCenterRemoteControl)
                {
                    switch (aHidEvent.Usages[0])
                    {
                        case (ushort)Hid.Usage.WindowsMediaCenterRemoteControl.GreenStart:
                            HandleGreenStart();
                            break;
                        case (ushort)Hid.Usage.WindowsMediaCenterRemoteControl.Eject:
                        case (ushort)Hid.Usage.WindowsMediaCenterRemoteControl.Ext2:
                            HandleEject();
                            break;
                    }
                }
            }
        }

        private SafeFileHandle OpenVolume()
        {
            return Function.CreateFile(  "E:",
                               SharpLib.Win32.FileAccess.GENERIC_READ,
                               SharpLib.Win32.FileShare.FILE_SHARE_READ | SharpLib.Win32.FileShare.FILE_SHARE_WRITE,
                               IntPtr.Zero,
                               CreationDisposition.OPEN_EXISTING,
                               0,
                               IntPtr.Zero);
        }

        /// <summary>
        /// 
        /// </summary>
        private void HandleEject()
        {
            SafeFileHandle handle = OpenVolume();
        }

        /// <summary>
        /// 
        /// </summary>
        private void HandleGreenStart()
        {
            //First check if the process we want to launch already exists
            string procName = Path.GetFileNameWithoutExtension(Properties.Settings.Default.StartFileName);
            Process[] existingProcesses = Process.GetProcessesByName(procName);
            if (existingProcesses == null || existingProcesses.Length == 0)
            {
                // Process do not exists just try to launch it
                ProcessStartInfo start = new ProcessStartInfo();
                // Enter in the command line arguments, everything you would enter after the executable name itself
                //start.Arguments = arguments; 
                // Enter the executable to run, including the complete path
                start.FileName = Properties.Settings.Default.StartFileName;
                start.WindowStyle = ProcessWindowStyle.Normal;
                start.CreateNoWindow = true;
                start.UseShellExecute = true;
                // Run the external process & wait for it to finish
                Process proc = Process.Start(start);

                //SL: We could have used that too
                //Shell32.Shell shell = new Shell32.Shell();
                //shell.ShellExecute(Properties.Settings.Default.StartFileName);
            }
            else
            {
                //This won't work properly until we have a manifest that enables uiAccess.
                //However uiAccess just won't work with ClickOnce so we will have to use a different deployment system.
                SwitchToThisWindow(existingProcesses[0].MainWindowHandle, true);
            }            
        }
        /// <summary>
        /// We need to handle WM_INPUT.
        /// </summary>
        /// <param name="message"></param>
        protected override void WndProc(ref Message message)
        {
            switch (message.Msg)
            {
                case Const.WM_INPUT:
                    //Returning zero means we processed that message.
                    message.Result = new IntPtr(0);
                    iHidHandler.ProcessInput(ref message);
                    break;
            }
            //Is that needed? Check the docs.
            base.WndProc(ref message);
        }
    }
}
