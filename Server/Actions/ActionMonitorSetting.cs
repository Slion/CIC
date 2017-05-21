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
    public abstract class ActionMonitorSetting : ActionMonitorConfig
    {
        [DataMember]
        [AttributeObjectProperty
            (
            Id = "MonitorSetting.Modifier",
            Name = "Setting modifier",
            Description = "Monitor setting modifier, can be negative.",
            Minimum = "-1000",
            Maximum = "1000",
            Increment = "1"
            )
        ]
        public int Modifier { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string BriefBase()
        {
            string brief = AttributeName + ": " + (Modifier > 0 ? "+" : "") + Modifier;
            return brief;
        }

        /// <summary>
        /// Tell if a monitor support that feature
        /// </summary>
        /// <param name="aPhysicalMonitor"></param>
        /// <returns></returns>
        public override bool Supported(PhysicalMonitor aPhysicalMonitor)
        {
            return aPhysicalMonitor.SupportsBrightness;
        }

        protected abstract Setting GetSetting();
        protected abstract void SetSetting(Setting aSetting);


        /// <summary>
        /// 
        /// </summary>
        protected override async Task DoExecute()
        {
            CheckCurrentItem(); // TODO: Should not need to do that everytime?
            if (PhysicalMonitor != null)
            {
                // Send our command and wait for it async
                Setting setting = GetSetting();
                // Cast
                int current = (int)setting.Current;
                // Apply increment
                current += Modifier;
                // Bound our value
                if (current < setting.Min)
                {
                    current = (int)setting.Min;
                }

                if (current > setting.Max)
                {
                    current = (int)setting.Max;
                }

                setting.Current = (uint)current;
                SetSetting(setting);
            }
            else
            {
                Trace.WriteLine("WARNING: No valid monitor specified.");
            }
        }

    }
}
