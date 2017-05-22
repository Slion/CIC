using SharpLib.Ear;
using SharpLib.MonitorConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SharpDisplayManager.Actions
{
    [DataContract]
    [AttributeObject(Id = "MonitorContrast", Name = "Monitor Contrast", Description = "Control monitor contrast.")]
    class ActionMonitorContrast : ActionMonitorSetting
    {
        protected override bool Supported(PhysicalMonitor aPhysicalMonitor)
        {
            return aPhysicalMonitor.SupportsContrast;
        }

        protected override Setting GetSetting()
        {
            return PhysicalMonitor.Contrast;
        }

        protected override void SetSetting(Setting aSetting)
        {
            PhysicalMonitor.Contrast = aSetting;
        }

    }
}
