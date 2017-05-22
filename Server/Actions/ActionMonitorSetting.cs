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
            string brief = AttributeName + ": " + (Modifier > 0 ? "+" : "") + Modifier + " on " + Monitor.CurrentItem;
            return brief;
        }


        protected abstract Setting GetSetting();
        protected abstract void SetSetting(Setting aSetting);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
            // Check if modifier is whithin range.
            // Though it is a rather dumb approach.
            Setting setting = GetSetting();

            if ((setting.Max - setting.Min) < Math.Abs(Modifier))
            {
                return false;
            }

            return true;
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
