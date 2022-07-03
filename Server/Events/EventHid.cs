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
using System.Diagnostics;

namespace SharpDisplayManager
{

    /// <summary>
    /// Base HID class can be used to handle any kind of HID events.
    /// TODO: Move key specific stuff to EventHidKey.
    /// </summary>
    [DataContract]
    public class EventHid: Ear.Event
    {
        public EventHid()
        {
            //Device.Items = null;
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

        [DataMember]
        public string DeviceFriendlyName { get; set; } = "";


        /// <summary>
        /// Manage button state for joysticks and gampads
        /// </summary>
        public bool ButtonDown { get; set; }

        /// <summary>
        /// Needed to manage buttons states for Joysticks and GamePads.
        /// TODO: Remove our legacy single Usage and use only this one.
        /// Make it persistent too.
        /// </summary>
        public List<ushort> Usages { get; set; }

        /// <summary>
        /// Incoming HID event.
        /// Could be useful for our axis implementation.
        /// </summary>
        public Hid.Event HidEvent { get; set; } = null;

        protected override void DoConstruct()
        {
            base.DoConstruct();

            if (Device==null) // Can be the case when loading from a save that did not have support for it
            {
                Device = new Ear.PropertyComboBox();
            }

            // Our ComboBox will display our FriendlyName property
            Device.DisplayMember = "FriendlyName";
            // But we will store our InstancePath which uniquely identifies an HID device
            Device.ValueMember = "InstancePath";
            // Our ComboBox sorts our HID devices by name
            Device.Sorted = true;
            //
            UpdateDynamicProperties();

            PropertyChanged += EventHid_PropertyChanged;
        }

        private void EventHid_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName== "Device")
            {
                // Save our device name as we like to use it to describe this event to our user
                DeviceFriendlyName = CurrentDevice()?.FriendlyName ?? "Device not found";
            }
            
        }

        /// <summary>
        /// TODO: Find a way to cache that
        /// </summary>g
        private void PopulateDeviceList()
        {
            if (Device.Items!=null && Device.Items.Count!=0)
            {
                // Keep using that list
                return;
            }

            Device.Items = new List<object>();
            
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
                Hid.Device.Input hidDevice;

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

                // Allow derived class to filter devices
                if (IsSupportedDevice(hidDevice))
                {
                    // Use the device object itself
                    Device.Items.Add(hidDevice);
                }                
            }

            // Sort them by friendly name
            Device.Items.Sort(delegate (object x, object y)
            {
                return ((Hid.Device.Input)x).FriendlyName.CompareTo(((Hid.Device.Input)y).FriendlyName);
            });

            CheckDeviceExistance();                        
        }


        /// <summary>
        /// Allow derived class to decide which HID device should be displayed.
        /// </summary>
        /// <param name="aDevice"></param>
        /// <returns></returns>
        protected virtual bool IsSupportedDevice(Hid.Device.Input aDevice)
        {
            return true;
        }

        /// <summary>
        /// Check if our current device exists in our list and mark the result.
        /// </summary>
        private void CheckDeviceExistance()
        {
            Device.CurrentItemNotFound = !Device.Items.Exists(delegate (object x)
            {
                return ((Hid.Device.Input)x).InstancePath.Equals(Device.CurrentItem, StringComparison.OrdinalIgnoreCase);
            });
        }

        /// <summary>
        /// Provide the current device object
        /// </summary>
        /// <returns></returns>
        public Hid.Device.Input CurrentDevice()
        {
            // Search our device in our list
            return  GetDevice(Device.CurrentItem);
        }

        /// <summary>
        /// Provide the device match the given instance path
        /// </summary>
        /// <param name="aInstancePath"></param>
        /// <returns></returns>
        public Hid.Device.Input GetDevice(string aInstancePath)
        {
            // Search our device in our list
            return (Hid.Device.Input)Device.Items.Find((d) => { return ((Hid.Device.Input)d).InstancePath.Equals(aInstancePath, StringComparison.OrdinalIgnoreCase); });
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

            if (CurrentState == State.Edit || CurrentState == State.PrepareEdit)
            {
                if (Device.CurrentItemNotFound)
                {
                    // Warn the user the device was not found
                    // I guess it needs to be connected
                    brief = "Device not found - " + brief;
                }
            }

            return brief;
        }

        /// <summary>
        /// In our EAR framework this just checks if an object matches another one.
        /// However an HID event also uses this to manage various states when receiving incoming events.
        /// TODO: Recode that logic to support mouse, remote control and other generic device key up and down event
        /// TODO: Maybe implement a sub class to support multiple buttons press for gamePads and joysticks
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Matches(object obj)
        {
            // TODO: have an option to make it device specific and test the Device InstancePath
            if (obj is EventHid)
            {
                EventHid e = (EventHid)obj;
                bool joystick = e.IsJoystick == IsJoystick;
                bool isJoystick = joystick && IsJoystick;
                bool gamepad = e.IsGamePad == IsGamePad;
                bool isGamepad = gamepad && IsGamePad;
                bool sameUsage = e.Usage == Usage;
                bool sameDevice = e.Device.CurrentItem.Equals(Device.CurrentItem, StringComparison.OrdinalIgnoreCase);
                bool match = e.Key == Key
                    && e.UsagePage == UsagePage
                    && e.UsageCollection == UsageCollection
                    && e.IsGeneric == IsGeneric
                    && e.IsKeyboard == IsKeyboard
                    && e.IsMouse == IsMouse
                    && joystick
                    && gamepad
                    && e.HasModifierAlt == HasModifierAlt
                    && e.HasModifierControl == HasModifierControl
                    && e.HasModifierShift == HasModifierShift
                    && e.HasModifierWindows == HasModifierWindows;

                if (match)
                {
                    if (isJoystick || isGamepad)
                    {
                        if (sameDevice)
                        {
                            if (e.Usages.Contains(Usage))
                            {
                                if (!ButtonDown)
                                {
                                    //Trace.WriteLine("ButtonDown: " + Brief());
                                    ButtonDown = true;
                                    if (!IsKeyUp)
                                    {
                                        return true;
                                    }
                                }
                            }
                            else
                            {
                                if (ButtonDown)
                                {
                                    //Trace.WriteLine("ButtonUp: " + Brief());
                                    ButtonDown = false;
                                    if (IsKeyUp)
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        // Not a joystick or a gamepad
                        // Just check if our key up is a match and we are done here
                        // TODO: We should only do that for keyboard I guess as remote control and other generic HID may need same treatment as joysticks to support key up and down
                        return e.IsKeyUp == IsKeyUp && sameUsage;
                    }
                }
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
            if (CurrentState == State.Rest)
            {
                // Reset our device list when we stop editing
                Device.Items = null;            
            }
            else if (CurrentState == State.PrepareEdit)
            {
                // Create our device list as we start editing 
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
        /// Used when recording during edition mode.
        /// </summary>
        /// <param name="aSender"></param>
        /// <param name="aHidEvent"></param>
        public virtual void HandleHidEvent(object aSender, SharpLib.Hid.Event aHidEvent)
        {
            var instancePath = Device.CurrentItem;

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

            // Check if our device changed
            if (!instancePath.Equals(Device.CurrentItem))
            {
                // Only trigger that one if our device actually changed, we could find ourself in endless loops with HID axis otherwise somehow
                OnPropertyChanged("Device");
            }            
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
        protected void PrivateCopy(Hid.Event aHidEvent)
        {
            HidEvent = aHidEvent;

            //Copy for scan
            Usages = aHidEvent.Usages;
            UsagePage = aHidEvent.UsagePage;
            UsageCollection = aHidEvent.UsageCollection;
            Device.CurrentItem = aHidEvent.Device?.InstancePath ?? "";
            CheckDeviceExistance();
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
            if (CurrentState == State.Edit && Device.CurrentItemNotFound)
            {
                return false;
            }
            return (IsGeneric || IsKeyboard);
        }
    }
}
