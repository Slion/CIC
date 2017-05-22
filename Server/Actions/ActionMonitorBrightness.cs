using CecSharp;
using SharpLib.Ear;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using SharpLib.MonitorConfig;
using System.Diagnostics;

namespace SharpDisplayManager
{
    /// <summary>
    /// Abstract CEC action using a device type.
    /// </summary>
    [DataContract]
    [AttributeObject(Id = "MonitorConfig.Brightness", Name = "Monitor Brightness", Description = "Control monitor brightness.")]
    public class ActionMonitorBrightness : ActionMonitorSetting
    {
        protected override bool Supported(PhysicalMonitor aPhysicalMonitor)
        {
            return aPhysicalMonitor.SupportsBrightness;
        }

        protected override Setting GetSetting()
        {
            return PhysicalMonitor.Brightness;
        }

        protected override void SetSetting(Setting aSetting)
        {
            PhysicalMonitor.Brightness = aSetting;
        }
    }
}
