using SharpLib.Ear;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using SharpLib.MonitorConfig;

namespace SharpDisplayManager
{
    /// <summary>
    /// Abstract Monitor Config action .
    /// </summary>
    [DataContract]
    public abstract class ActionMonitorConfig : SharpLib.Ear.Action
    {
        //[DataMember]
        //public string DeviceName { get; set; }

        [DataMember]
        [AttributeObjectProperty
        (
            Id = "Action.MonitorConfig",
            Name = "Monitor to control",
            Description = "Select the monitor you want to control."
            )
        ]
        public PropertyComboBox Monitor { get; set; } = new PropertyComboBox();


        protected override void DoConstruct()
        {
            base.DoConstruct();
            PopulateMonitors();
            CheckCurrentItem();
        }

        /// <summary>
        /// Tell if a monitor support that feature
        /// </summary>
        /// <param name="aPhysicalMonitor"></param>
        /// <returns></returns>
        protected abstract bool Supported(PhysicalMonitor aPhysicalMonitor);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aPhysicalMonitor"></param>
        /// <returns></returns>
        static public string MonitorId(int aIndex, VirtualMonitor aVirtualMonitor)
        {
            return aIndex + ": " + aVirtualMonitor.FriendlyName;
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopulateMonitors()
        {
            if (Monitors == null)
            {
                Monitors = new Monitors();
                Monitors.Scan();
            }            

            //Reset our list of drives
            Monitor.Items = new List<object>();
            //Go through each drives on our system and collected the optical ones in our list            
            int i = 0;
            foreach (VirtualMonitor vm in Monitors.VirtualMonitors)
            {
                i++;
                if (Supported(vm.PhysicalMonitors[0]))
                {
                    //This monitor is supported, add it now
                    Monitor.Items.Add(MonitorId(i,vm));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void CheckCurrentItem()
        {
            PhysicalMonitor = null;
            if (!Monitor.Items.Contains(Monitor.CurrentItem) && Monitor.Items.Count > 0)
            {
                //Current item unknown, reset it then
                Monitor.CurrentItem = Monitor.Items[0].ToString();
            }

            int i = 0;
            // Current monitor exist locate it then
            foreach (VirtualMonitor vm in Monitors.VirtualMonitors)
            {
                i++;
                if (Supported(vm.PhysicalMonitors[0]) && MonitorId(i,vm).Equals(Monitor.CurrentItem))
                {
                    PhysicalMonitor = vm.PhysicalMonitors[0];
                }
            }

        }


        protected PhysicalMonitor PhysicalMonitor;


        static public Monitors Monitors { get; set; }
    }
}
