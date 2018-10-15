using SharpLib.Ear;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using SharpLib.MonitorConfig;
using System.Reflection;
using System.Diagnostics;

namespace SharpDisplayManager
{
    /// <summary>
    /// Define a monitor power action.
    /// Executed using Windows system command.
    /// Tested on Windows 10 against Dell U3818DW.
    /// Off: Puts the monitor in standby mode.
    /// On: Monitor scans for input signal, does not find one and goes back to sleep.
    /// Standby: No effect.
    /// Other monitors may behave differently 
    /// </summary>
    [DataContract]
    [AttributeObject(Id = "Monitor.Power", Name = "Monitor Power", Description = "Turn monitor on and off.")]
    public class ActionMonitorPower : SharpLib.Ear.Action
    {
        [DataMember]
        [AttributeObjectProperty
            (
            Id = "Action.MonitorPowerAction",
            Name = "Monitor action",
            Description = "Select the monitor power action you want to execute."
            )
        ]
        public SharpLib.Win32.MonitorState Action { get; set; }


        protected override void DoConstruct()
        {
            base.DoConstruct();

            if (Action==0) // 0 is not a valid action
            {
                // Use our default action instead
                Action = SharpLib.Win32.MonitorState.Off;
            }
        }

        public override string BriefBase()
        {
            string brief = AttributeName + " " + Action.ToString();
            return brief;
        }

        protected override async Task DoExecute(Context aContext)
        {
            SharpLib.Win32.Function.SendMessage(FormMain.HWND, SharpLib.Win32.Const.WM_SYSCOMMAND, SharpLib.Win32.Const.SC_MONITORPOWER, (int)Action);
        }

    }
}
