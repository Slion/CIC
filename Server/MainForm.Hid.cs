using System;
using System.IO;
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
	public class MainFormHid : Form
	{

		[DllImport("USER32.DLL")]
		public static extern bool SetForegroundWindow(IntPtr hWnd);

		[DllImport("USER32.DLL")]
		public static extern IntPtr GetForegroundWindow();

		[System.Runtime.InteropServices.DllImportAttribute("user32.dll", EntryPoint = "GetWindowThreadProcessId")]
		public static extern uint GetWindowThreadProcessId([System.Runtime.InteropServices.InAttribute()] System.IntPtr hWnd, System.IntPtr lpdwProcessId);

		[System.Runtime.InteropServices.DllImportAttribute("kernel32.dll", EntryPoint = "GetCurrentThreadId")]
		public static extern uint GetCurrentThreadId();

		[System.Runtime.InteropServices.DllImportAttribute("user32.dll", EntryPoint = "AttachThreadInput")]
		[return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
		public static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)] bool fAttach);

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
				//We are in the proper thread
				if (aHidEvent.Usages.Count > 0
					&& aHidEvent.UsagePage == (ushort)Hid.UsagePage.WindowsMediaCenterRemoteControl
					&& aHidEvent.Usages[0] == (ushort)Hid.Usage.WindowsMediaCenterRemoteControl.GreenStart)
				//&& aHidEvent.UsagePage == (ushort)Hid.UsagePage.Consumer
				//&& aHidEvent.Usages[0] == (ushort)Hid.Usage.ConsumerControl.ThinkPadFullscreenMagnifier)
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
						ForceForegroundWindow(existingProcesses[0].MainWindowHandle);
					}
				}
			}
		}


		/// <summary>
		/// For the Window with the given handle to the foreground no matter what.
		/// That works around flashing Window issues.
		/// As seen on http://www.asyncop.com/MTnPDirEnum.aspx?treeviewPath=[o]+Open-Source\WinModules\Infrastructure\SystemAPI.cpp
		/// </summary>
		/// <param name="hTo"></param>
		/// <returns></returns>
		IntPtr ForceForegroundWindow(IntPtr hTo)
		{
			if (hTo == IntPtr.Zero)
			{
				return IntPtr.Zero;
			}
			IntPtr hFrom = GetForegroundWindow();

			if (hFrom != IntPtr.Zero)
			{
				SetForegroundWindow(hTo); 
				return IntPtr.Zero;
			}
			if (hTo == hFrom)
			{
				return IntPtr.Zero;
			}

			uint pid = GetWindowThreadProcessId(hFrom, IntPtr.Zero);
			uint tid = GetCurrentThreadId();
			if (tid == pid)
			{
				SetForegroundWindow(hTo);
				return (hFrom);
			}
			if (pid != 0)
			{
				if (!AttachThreadInput(tid, pid, true))
				{
					return IntPtr.Zero;
				}
				SetForegroundWindow(hTo);
				AttachThreadInput(tid, pid, false);
			}
			return (hFrom);
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
