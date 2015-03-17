using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
//
using Hid = SharpLib.Hid;
using SharpLib.Win32;

namespace SharpDisplayManager
{
	[System.ComponentModel.DesignerCategory("Code")]
	public partial class MainForm: Form
	{
		public delegate void OnHidEventDelegate(object aSender, Hid.Event aHidEvent);

		/// <summary>
		/// Use notably to handle green start key from IR remote control
		/// </summary>
		private Hid.Handler iHidHandler;

		void RegisterHidDevices()
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
				//We are in the proper thread
				//listViewEvents.Items.Insert(0, aHidEvent.ToListViewItem());
				//toolStripStatusLabelDevice.Text = aHidEvent.Device.FriendlyName;

				if (aHidEvent.Usages.Count > 0
					&& aHidEvent.UsagePage == (ushort)Hid.UsagePage.WindowsMediaCenterRemoteControl
					&& aHidEvent.Usages[0] == (ushort)Hid.Usage.WindowsMediaCenterRemoteControl.GreenStart)
				{
					//Hard coding it all for now
					// Prepare the process to run
					ProcessStartInfo start = new ProcessStartInfo();
					// Enter in the command line arguments, everything you would enter after the executable name itself
					//start.Arguments = arguments; 
					// Enter the executable to run, including the complete path
					start.FileName = "C:\\Program Files (x86)\\Team MediaPortal\\MediaPortal\\MediaPortal.exe";
					// Do you want to show a console window?
					start.WindowStyle = ProcessWindowStyle.Hidden;
					start.CreateNoWindow = true;
					// Run the external process & wait for it to finish
					Process proc = Process.Start(start);
				}
			}
		}

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
