using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Windows.Forms;
using Ear = SharpLib.Ear;
using Hid = SharpLib.Hid;
using SharpLib.Hid;
using SharpLib.Win32;

namespace SharpDisplayManager.Events
{

    public class Axis
    {
        /// <summary>
        /// Higher 16 bits are the usage page.
        /// Lower 16 bits are the usage on that page.
        /// </summary>
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string FullName { get; private set; }


        public static string NameFromId(int aId)
        {
            var usagePage = aId >> 16;
            var usage = aId & 0x00001111;

            Type usageType = Utils.UsageType((UsagePage)usagePage);
            if (usageType==null)
            {
                return "Unknown usage type";
            }

            return Enum.GetName(usageType, usage);
        }


        /// <summary>
        /// Utility method to check if the given input value is an axis
        /// </summary>
        /// <param name="aCaps"></param>
        /// <returns></returns>
        public static bool IsAxis(HIDP_VALUE_CAPS aCaps)
        {
            if (!aCaps.IsRange && Enum.IsDefined(typeof(UsagePage), aCaps.UsagePage))
            {
                Type usageType = Utils.UsageType((UsagePage)aCaps.UsagePage);
                if (usageType == null)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        public Axis(HIDP_VALUE_CAPS aCaps)
        {
            if (aCaps.IsRange)
            {
                throw new ArgumentException("Range input values are not axis");
            }

            if (!Enum.IsDefined(typeof(UsagePage), aCaps.UsagePage))
            {
                throw new ArgumentException("Unknown axis usage page");
            }

            Type usageType = Utils.UsageType((UsagePage)aCaps.UsagePage);
            if (usageType == null)
            {
                throw new ArgumentException("Unknown axis usage type");
            }

            // Build our axis id which is combined of usage page and usage
            Id = (aCaps.UsagePage << 16) | aCaps.NotRange.Usage;
            //
            Name = Enum.GetName(usageType, aCaps.NotRange.Usage);
            FullName = Enum.GetName(typeof(UsagePage), aCaps.UsagePage) + "." + Name;
        }

    }



    [DataContract]
    [Ear.AttributeObject(Id = "Event.Hid.Axis", Name = "HID Axis", Description = "Handle input from a controller axis.")]
    class EventHidAxis : EventHid
    {

        string iDeviceInstancePathForAxis = "";

        protected override void DoConstruct()
        {
            base.DoConstruct();

            if (Axis == null) // Can be the case when loading from a save that did not have support for it
            {
                Axis = new Ear.PropertyComboBox();
            }

            iDeviceInstancePathForAxis = "";
            Axis.DisplayMember = "Name";
            // Need to be a string property as we save it as a string in CurrentItem. Failing that you won't be able to set current item in ComboBox control.
            Axis.ValueMember = "FullName";

            PropertyChanged += EventHidAxis_PropertyChanged;
        }

        private void EventHidAxis_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName=="Device" && CurrentState != State.Rest)
            {
                PopulateAxisList();
            }
        }

        protected override bool IsSupportedDevice(Hid.Device.Input aDevice)
        {
            // We only want gamepads and joysticks here
            return aDevice.IsGamePad || aDevice.IsJoystick;
        }

        [DataMember]
        [Ear.AttributeObjectProperty
        (Id = "Axis",
         Name = "Axis",
         Description = "Select an axis.")]
        public Ear.PropertyComboBox Axis { get; set; } = new Ear.PropertyComboBox();


        void PopulateAxisList()
        {
            if (Device == null || string.IsNullOrEmpty(Device.CurrentItem))
            {
                return;
            }

            // Check if device has changed since we were last here
            if (Axis.Items!=null && iDeviceInstancePathForAxis.Equals(Device.CurrentItem, StringComparison.OrdinalIgnoreCase))
            {
                // We already have axis for that device
                return;
            }
          
            var device = (Hid.Device.Input)Device.Items.Find((d) => { return ((Hid.Device.Input)d).InstancePath.Equals(Device.CurrentItem, StringComparison.OrdinalIgnoreCase); });

            if (device == null)
            {
                // Device not connected
                return;
            }

            // Remember the device those axis belong to to avoid unneeded updates
            iDeviceInstancePathForAxis = Device.CurrentItem;

            Axis.Items = new List<object>();

            // For each potential axis
            foreach (var axis in device.InputValueCapabilities)
            {
                // Check if it is actually a known axis
                if (SharpDisplayManager.Events.Axis.IsAxis(axis))
                {

                    var a = new Axis(axis);

                    // Add it to our collection
                    // TODO: Should we use an object instead?
                    // SharpLibHid could implement an axis class I guess?
                    Axis.Items.Add(a);
                }
            }

            //TODO: Adjust CurrentItem
            OnPropertyChanged("Axis");
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnStateLeave()
        {
            base.OnStateLeave();

            if (CurrentState == State.Edit)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnStateEnter()
        {
            base.OnStateEnter();

            if (CurrentState == State.Rest)
            {
                // Reset our device list when we stop editing
                Axis.Items = null;
                iDeviceInstancePathForAxis = "";
            }
            else if (CurrentState == State.PrepareEdit)
            {
                // Create our device list as we start editing 
                PopulateAxisList();
            }
            else if (CurrentState == State.Edit)
            {
            }
        }

        public override string Brief()
        {
            // TODO: How to get the device name for its instance path in Device.CurrentItem?
            //return AttributeName + ": " + Events.Axis.NameFromId(int.Parse(Axis.CurrentItem));
            return AttributeName + ": " + DeviceFriendlyName + " - " + Axis.CurrentItem.Split('.')[1];
        }


    }
}
