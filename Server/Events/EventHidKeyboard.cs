using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Windows.Forms;
using Ear = SharpLib.Ear;
using Hid = SharpLib.Hid;

namespace SharpDisplayManager
{
    [DataContract]
    [Ear.AttributeObject(Id = "Event.Hid.Keyboard", Name = "HID Keyboard", Description = "Corresponding HID message received.")]
    public class EventHidKeyboard : Ear.Event
    {
        public EventHidKeyboard()
        {
        }

        [DataMember]
        [Ear.AttributeObjectProperty
            (
            Id = "HID.Keyboard.Key",
            Name = "Key",
            Description = "The virtual key code."
            )]
        public Keys Key { get; set; }

        [DataMember]
        [Ear.AttributeObjectProperty
            (
            Id = "HID.Keyboard.IsKeyUp",
            Name = "Key Up",
            Description = "Key up if set, key down otherwise."
            )]
        public bool IsKeyUp { get; set; } = true;

        [DataMember]
        [Ear.AttributeObjectProperty
            (
            Id = "HID.Keyboard.HasModifierShift",
            Name = "Shift",
            Description = "Shift modifier applied."
            )]
        public bool HasModifierShift { get; set; } = false;

        [DataMember]
        [Ear.AttributeObjectProperty
            (
            Id = "HID.Keyboard.HasModifierControl",
            Name = "Control",
            Description = "Control modifier applied."
            )]
        public bool HasModifierControl { get; set; } = false;

        [DataMember]
        [Ear.AttributeObjectProperty
            (
            Id = "HID.Keyboard.HasModifierAlt",
            Name = "Alt",
            Description = "Alt modifier applied."
            )]
        public bool HasModifierAlt { get; set; } = false;

        [DataMember]
        [Ear.AttributeObjectProperty
            (
            Id = "HID.Keyboard.HasModifierWindows",
            Name = "Windows",
            Description = "Windows modifier applied."
            )]
        public bool HasModifierWindows { get; set; } = false;



        /// <summary>
        /// Make sure we distinguish between various configuration of this event 
        /// </summary>
        /// <returns></returns>
        public override string Brief()
        {
            string brief = Name + ": " + Key.ToString();

            if (IsKeyUp)
            {
                brief += " (UP)";
            }
            else
            {
                brief += " (DOWN)";
            }

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
                return  e.Key == Key
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
