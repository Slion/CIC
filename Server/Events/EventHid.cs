using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Windows.Forms;
using Ear = SharpLib.Ear;
using Hid = SharpLib.Hid;
using SharpLib.Win32;
using System.Runtime.InteropServices;

namespace SharpDisplayManager
{
    /// <summary>
    /// TODO: Consider an option to make it device specific.
    /// </summary>
    [DataContract]
    [Ear.AttributeObject(Id = "Event.Hid", Name = "HID", Description = "Handle input from Keyboards and Remotes.")]
    public class EventHid: Ear.Event
    {
        public EventHid()
        {
        }

        [DataMember]
        public ushort UsagePage { get; set; }

        [DataMember]
        public ushort UsageCollection { get; set; }

        [DataMember]
        public ushort Usage { get; set; }

        [DataMember]
        public Keys Key { get; set; }

        [DataMember]
        [Ear.AttributeObjectProperty
            (
            Id = "HID.Keyboard.IsKeyUp",
            Name = "Key Up",
            Description = "Key up if set, key down otherwise."
            )]
        public bool IsKeyUp { get; set; } = false;

        [DataMember]
        public bool IsMouse { get; set; } = false;

        [DataMember]
        public bool IsKeyboard { get; set; } = false;

        [DataMember]
        public bool IsGeneric { get; set; } = false;

        [DataMember]
        public bool IsGamePad { get; set; } = false;

        [DataMember]
        public bool IsJoystick { get; set; } = false;

        [DataMember]
        public bool HasModifierShift { get; set; } = false;

        [DataMember]
        public bool HasModifierControl { get; set; } = false;

        [DataMember]
        public bool HasModifierAlt { get; set; } = false;

        [DataMember]
        public bool HasModifierWindows { get; set; } = false;

        [DataMember]
        public string UsageName { get; set; } = "Press a key";

        [DataMember]
        [Ear.AttributeObjectProperty
            (
            Id = "HID.Device",
            Name = "HID device",
            Description = "Select an HID device."
            )
        ]
        public Ear.PropertyComboBox Device { get; set; } = new Ear.PropertyComboBox();

        protected override void DoConstruct()
        {
            base.DoConstruct();

            if (Device==null) // Can be the case when loading from a save that did not have support for it
            {
                Device = new Ear.PropertyComboBox();
            }

            UpdateDynamicProperties();
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopulateDeviceList()
        {
            Device.Items.Clear();
            
            // TODO: Allow derived class to filter that list based on usage collection?
            // TODO: Add any option to disable device check

            //Get our list of devices
            RAWINPUTDEVICELIST[] ridList = null;
            uint deviceCount = 0;
            int res = Function.GetRawInputDeviceList(ridList, ref deviceCount, (uint)Marshal.SizeOf(typeof(RAWINPUTDEVICELIST)));
            if (res == -1)
            {
                //Just give up then
                return;
            }

            ridList = new RAWINPUTDEVICELIST[deviceCount];
            res = Function.GetRawInputDeviceList(ridList, ref deviceCount, (uint)Marshal.SizeOf(typeof(RAWINPUTDEVICELIST)));
            if (res != deviceCount)
            {
                //Just give up then
                return;
            }

            //For each our device add a node to our treeview
            foreach (RAWINPUTDEVICELIST device in ridList)
            {
                SharpLib.Hid.Device.Input hidDevice;

                //Try create our HID device.
                try
                {
                    hidDevice = new SharpLib.Hid.Device.Input(device.hDevice);
                }
                catch /*(System.Exception ex)*/
                {
                    //Just skip that device then
                    continue;
                }

                // TODO; Find a way to store the device object itself
                Device.Items.Add(hidDevice.FriendlyName);
    
            }

        }

        private void UpdateDynamicProperties()
        {
            
        }

        /// <summary>
        /// Make sure we distinguish between various configuration of this event 
        /// </summary>
        /// <returns></returns>
        public override string Brief()
        {
            string brief = AttributeName + ": ";

            if (!IsValid())
            {
                brief += "Press a key";
                return brief;
            }

            if (IsKeyboard)
            {
                brief += Key.ToString();

                if (HasModifierAlt)
                {
                    brief += " + ALT";
                }

                if (HasModifierControl)
                {
                    brief += " + CTRL";
                }

                if (HasModifierShift)
                {
                    brief += " + SHIFT";
                }

                if (HasModifierWindows)
                {
                    brief += " + WIN";
                }
            }
            else if (IsGeneric)
            {
                if (IsJoystick)
                {
                    brief += "Joystick ";
                }

                if (IsGamePad)
                {
                    brief += "GamePad ";
                }

                brief += UsageName;                
            }

            if (IsKeyUp)
            {
                brief += " (UP)";
            }
            else
            {
                brief += " (DOWN)";
            }

            return brief;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Matches(object obj)
        {
            if (obj is EventHid)
            {
                EventHid e = (EventHid)obj;
                return e.Key == Key
                    && e.Usage == Usage
                    && e.UsagePage == UsagePage
                    && e.UsageCollection == UsageCollection
                    && e.IsKeyUp == IsKeyUp
                    && e.IsGeneric == IsGeneric
                    && e.IsGamePad == IsGamePad
                    && e.IsJoystick == IsJoystick
                    && e.IsKeyboard == IsKeyboard
                    && e.IsMouse == IsMouse
                    && e.HasModifierAlt == HasModifierAlt
                    && e.HasModifierControl == HasModifierControl
                    && e.HasModifierShift == HasModifierShift
                    && e.HasModifierWindows == HasModifierWindows;
            }

            return false;
        }


        /// <summary>
        /// 
        /// </summary>
        protected override void OnStateLeave()
        {
            if (CurrentState == State.Edit)
            {
                // Leaving edit mode
                // Unhook HID events
                Program.HidHandler.OnHidEvent -= HandleHidEvent;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnStateEnter()
        {
            if (CurrentState == State.PrepareEdit)
            {
                PopulateDeviceList();
            }
            else if (CurrentState == State.Edit)
            {
                // Enter edit mode
                // Hook-in HID events
                Program.HidHandler.OnHidEvent += HandleHidEvent;                
            }
        }

        /// <summary>
        /// Here we receive HID events from our HID library.
        /// </summary>
        /// <param name="aSender"></param>
        /// <param name="aHidEvent"></param>
        public void HandleHidEvent(object aSender, SharpLib.Hid.Event aHidEvent)
        {
            if (CurrentState != State.Edit
                || aHidEvent.IsMouse
                || aHidEvent.IsButtonUp
                || !aHidEvent.IsValid
                || aHidEvent.IsBackground
                || aHidEvent.IsRepeat
                || aHidEvent.IsStray
                )
            {
                return;
            }

            PrivateCopy(aHidEvent);
            //

            //Tell observer the object itself changed
            OnPropertyChanged("Brief");
        }

        /// <summary>
        /// Used to capture incoming HID events
        /// </summary>
        /// <param name="aHidEvent"></param>
        public void Copy(Hid.Event aHidEvent)
        {
            PrivateCopy(aHidEvent);
            //We need the key up/down too here
            IsKeyUp = aHidEvent.IsButtonUp;
        }

        /// <summary>
        /// TODO: Optionally save the device name to make it specific to a device.
        /// </summary>
        /// <param name="aHidEvent"></param>
        private void PrivateCopy(Hid.Event aHidEvent)
        {
            //Copy for scan
            UsagePage = aHidEvent.UsagePage;
            UsageCollection = aHidEvent.UsageCollection;
            IsGeneric = aHidEvent.IsGeneric;
            IsKeyboard = aHidEvent.IsKeyboard;
            IsMouse = aHidEvent.IsMouse;
            IsGamePad = aHidEvent.Device?.IsGamePad ?? false;
            IsJoystick = aHidEvent.Device?.IsJoystick ?? false;

            if (IsGeneric)
            {
                if (IsGamePad || IsJoystick)
                {
                    if (aHidEvent.Usages.Count > 0)
                    {
                        Usage = aHidEvent.Usages[0];
                        UsageName = "Button " + Usage;
                    }
                }
                else
                {
                    // We were assuming this is a remote control
                    if (aHidEvent.Usages.Count > 0)
                    {
                        Usage = aHidEvent.Usages[0];
                        UsageName = aHidEvent.UsageName(0);
                    }
                }

                Key = Keys.None;
                HasModifierAlt = false;
                HasModifierControl = false;
                HasModifierShift = false;
                HasModifierWindows = false;
            }
            else if (IsKeyboard)
            {
                Usage = 0;
                Key = aHidEvent.VirtualKey;
                HasModifierAlt = aHidEvent.HasModifierAlt;
                HasModifierControl = aHidEvent.HasModifierControl;
                HasModifierShift = aHidEvent.HasModifierShift;
                HasModifierWindows = aHidEvent.HasModifierWindows;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
            return IsGeneric || IsKeyboard;
        }
    }
}
