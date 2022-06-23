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

namespace SharpDisplayManager.Events
{

    [DataContract]
    [Ear.AttributeObject(Id = "Event.Hid.Axis", Name = "HID Axis", Description = "Handle input from a controller axis.")]
    class EventHidAxis : EventHid
    {

        protected override void DoConstruct()
        {
            base.DoConstruct();

            if (Axis == null) // Can be the case when loading from a save that did not have support for it
            {
                Axis = new Ear.PropertyComboBox();
            }

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

            var device = (Hid.Device.Input)Device.Items.Find((d) => { return ((Hid.Device.Input)d).InstancePath.Equals(Device.CurrentItem, StringComparison.OrdinalIgnoreCase); });

            if (device == null)
            {
                // Device not connected
                return;
            }

            Axis.Items = new List<object>();

            // For each potential axis
            foreach (var axis in device.InputValueCapabilities)
            {
                // Check if it is actually a known axis
                if (!axis.IsRange && Enum.IsDefined(typeof(UsagePage), axis.UsagePage))
                {
                    Type usageType = Utils.UsageType((UsagePage)axis.UsagePage);
                    if (usageType == null)
                    {
                        continue;
                    }

                    // Get its name
                    string name = Enum.GetName(usageType, axis.NotRange.Usage);

                    // Add it to our collection
                    // TODO: Should we use an object instead?
                    // SharpLibHid could implement an axis class I guess?
                    Axis.Items.Add(name);
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
            return AttributeName + ": " + Axis.CurrentItem;
        }


    }
}
