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
    public class ActionMonitorBrightness : ActionMonitorConfig
    {
        [DataMember]
        [AttributeObjectProperty
            (
            Id = "MonitorBrightness.Increment",
            Name = "Brightness modifier",
            Description = "Monitor brightness modifier, can be negative.",
            Minimum = "-1000",
            Maximum = "1000",
            Increment = "1"
            )
        ]
        public int Increment { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string BriefBase()
        {
            string brief = "Monitor Brightness: " + (Increment>0?"+":"") + Increment;
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

        /// <summary>
        /// 
        /// </summary>
        protected override async Task DoExecute()
        {
            CheckCurrentItem(); // TODO: Should not need to do that everytime?
            if (PhysicalMonitor != null)
            {
                // Send our command and wait for it async
                Setting brightness = PhysicalMonitor.Brightness;
                // Cast
                int current = (int)brightness.Current;
                // Apply increment
                current += Increment;
                // Bound our value
                if (current < brightness.Min)
                {
                    current = (int)brightness.Min;
                }

                if (current > brightness.Max)
                {
                    current = (int)brightness.Max;
                }
               
                brightness.Current = (uint)current;
                PhysicalMonitor.Brightness = brightness;
            }
            else
            {
                Trace.WriteLine("WARNING: No valid monitor specified.");
            }
        }

    }
}
