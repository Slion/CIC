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

namespace SharpDisplayManager
{
    [DataContract]
    [Ear.AttributeObject(Id = "Event.Hid", Name = "HID", Description = "Corresponding HID message received.")]
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
        public bool IsMouse { get; set; }

        [DataMember]
        public bool IsKeyboard { get; set; }

        [DataMember]
        public bool IsGeneric { get; set; }

        [DataMember]
        public bool HasModifierShift { get; set; } = false;

        [DataMember]
        public bool HasModifierControl { get; set; } = false;

        [DataMember]
        public bool HasModifierAlt { get; set; } = false;

        [DataMember]
        public bool HasModifierWindows { get; set; } = false;


        protected override void DoConstruct()
        {
            base.DoConstruct();
            UpdateDynamicProperties();
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
            string brief = Name + ": ";

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
        public override bool Equals(object obj)
        {
            if (obj is EventHidKeyboard)
            {
                EventHidKeyboard e = (EventHidKeyboard)obj;
                return e.Key == Key
                        && e.IsKeyUp == IsKeyUp
                        && e.HasModifierAlt == HasModifierAlt
                        && e.HasModifierControl == HasModifierControl
                        && e.HasModifierShift == HasModifierShift
                        && e.HasModifierWindows == HasModifierWindows;
            }

            return false;
        }
    }
}
