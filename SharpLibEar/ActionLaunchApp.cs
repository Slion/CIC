using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SharpLib.Ear
{
    [DataContract]
    [AttributeObject(Id = "Action.Launch.App", Name = "Launch application", Description = "Launch an application.")]
    public class ActionLaunchApp : Action
    {
        [DataMember]
        [AttributeObjectProperty
            (
            Id = "Action.Launch.App.File",
            Name = "File to launch",
            Description = "Specifies the application file to launch.",
            Filter = "EXE files (*.exe)|*.exe"
            )
        ]
        public PropertyFile File { get; set; } = new PropertyFile();

        [DataMember]
        [AttributeObjectProperty
            (
            Id = "Action.Launch.App.SwitchTo",
            Name = "Switch to",
            Description = "Specifies if we should switch the application if it's already launched."
            )
        ]
        public bool SwitchTo { get; set; } = true;

        [DataMember]
        [AttributeObjectProperty
            (
            Id = "Action.Launch.App.MultipleInstance",
            Name = "Multiple Instance",
            Description = "Specifies if We should launch multiple instance."
            )
        ]
        public bool MultipleInstance { get; set; } = false;

        public override string Brief()
        {
            return Name + ": " + Path.GetFileName(File.FullPath);
        }

        public override bool IsValid()
        {
            // This is a valid configuration only if our file exists
            return System.IO.File.Exists(File.FullPath);
        }

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll", EntryPoint = "SwitchToThisWindow")]
        public static extern void SwitchToThisWindow([System.Runtime.InteropServices.InAttribute()] System.IntPtr hwnd, [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)] bool fUnknown);


        protected override void DoExecute()
        {
            //First check if the process we want to launch already exists
            string procName = Path.GetFileNameWithoutExtension(File.FullPath);
            Process[] existingProcesses = Process.GetProcessesByName(procName);
            if (existingProcesses == null || existingProcesses.Length == 0 || MultipleInstance)
            {
                // Process do not exists just try to launch it
                ProcessStartInfo start = new ProcessStartInfo();
                // Enter in the command line arguments, everything you would enter after the executable name itself
                //start.Arguments = arguments; 
                // Enter the executable to run, including the complete path
                start.FileName = File.FullPath;
                start.WindowStyle = ProcessWindowStyle.Normal;
                start.CreateNoWindow = true;
                start.UseShellExecute = true;
                // Run the external process & wait for it to finish
                Process proc = Process.Start(start);

                //SL: We could have used that too
                //Shell32.Shell shell = new Shell32.Shell();
                //shell.ShellExecute(Properties.Settings.Default.StartFileName);
            }
            else if (SwitchTo)
            {
                //This won't work properly until we have a manifest that enables uiAccess.
                //However uiAccess just won't work with ClickOnce so we will have to use a different deployment system.
                SwitchToThisWindow(existingProcesses[0].MainWindowHandle, true);
            }

        }
    }
}
