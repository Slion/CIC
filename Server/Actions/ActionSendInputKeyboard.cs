using SharpLib.Ear;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Diagnostics;
using GregsStack.InputSimulatorStandard.Native;

namespace SharpDisplayManager
{
    /// <summary>
    /// Use Win32 SendInput through GregsStack/InputSimulatorStandard.
    /// See: https://github.com/GregsStack/InputSimulatorStandard
    /// </summary>
    [DataContract]
    [AttributeObject(Id = "SendInput.Keyboard", Name = "SendInput Keyboard", Description = "Send a keyboard event through Win32 SendInput.")]
    public class ActionSendInputKeyboard: SharpLib.Ear.Action
    {

        [DataMember]
        [AttributeObjectProperty
            (
            Id = "Action.SendInputKeyboardFunction",
            Name = "Keyboard function",
            Description = "Select the keyboard function you want to achieve."
            )
        ]
        public KeyboardFunction Function { get; set; }

        [DataMember]
        [AttributeObjectProperty
            (
            Id = "Action.SendInputKeyboardModifiers",
            Name = "Keyboard modifiers",
            Description = "Select keyboard modifiers."
            )
        ]
        public PropertyCheckedListBox Modifiers { get; set; } = new PropertyCheckedListBox();


        [DataMember]
        [AttributeObjectProperty
            (
            Id = "Action.SendInputKeyboardKey",
            Name = "Keyboard key",
            Description = "Select the keyboard key you want to send."
            )
        ]
        public PropertyComboBox Key { get; set; } = new PropertyComboBox();

        public enum KeyboardFunction
        {
            Action = 0,
            Press = 1,
            Release = 2
        }

        protected override void DoConstruct()
        {
            base.DoConstruct();
            PopulateKeyboardModifers();
            PopulateKeyboardKeys();
            Modifiers.Sorted = false;
            Key.Sorted = false;                        
        }

        public override string BriefBase()
        {
            string brief = AttributeName + " " + Function.ToString() + " ";
            foreach (string modifier in Modifiers.CheckedItems)
            {
                brief += modifier + " + ";
            }

            brief += Key.CurrentItem;
            return brief;
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopulateKeyboardKeys()
        {
            Key.Items = new List<object>();

            var names = Enum.GetNames(typeof(VirtualKeyCode));

            foreach (string name in names)
            {
                Key.Items.Add(name);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopulateKeyboardModifers()
        {
            Modifiers.Items = new List<object>();

            // SHIFT
            Modifiers.Items.Add(Enum.GetName(typeof(VirtualKeyCode), VirtualKeyCode.SHIFT));
            Modifiers.Items.Add(Enum.GetName(typeof(VirtualKeyCode), VirtualKeyCode.LSHIFT));
            Modifiers.Items.Add(Enum.GetName(typeof(VirtualKeyCode), VirtualKeyCode.RSHIFT));
            // CTRL
            Modifiers.Items.Add(Enum.GetName(typeof(VirtualKeyCode), VirtualKeyCode.CONTROL));
            Modifiers.Items.Add(Enum.GetName(typeof(VirtualKeyCode), VirtualKeyCode.LCONTROL));
            Modifiers.Items.Add(Enum.GetName(typeof(VirtualKeyCode), VirtualKeyCode.RCONTROL));
            // ALT
            Modifiers.Items.Add(Enum.GetName(typeof(VirtualKeyCode), VirtualKeyCode.MENU));
            Modifiers.Items.Add(Enum.GetName(typeof(VirtualKeyCode), VirtualKeyCode.LMENU));
            Modifiers.Items.Add(Enum.GetName(typeof(VirtualKeyCode), VirtualKeyCode.RMENU));
            // WIN
            Modifiers.Items.Add(Enum.GetName(typeof(VirtualKeyCode), VirtualKeyCode.LWIN));
            Modifiers.Items.Add(Enum.GetName(typeof(VirtualKeyCode), VirtualKeyCode.RWIN));
        }


        private void KeysDown(IEnumerable<VirtualKeyCode> aMods)
        {
            foreach (var mod in aMods)
            {
                Program.iInputSimulator.Keyboard.KeyDown(mod);
            }
        }

        private void KeysUp(IEnumerable<VirtualKeyCode> aMods)
        {
            foreach (var mod in aMods)
            {
                Program.iInputSimulator.Keyboard.KeyUp(mod);
            }
        }


        protected override async Task DoExecute(Context aContext)
        {

            // Build our list of modifiers
            List<VirtualKeyCode> modifiers = new List<VirtualKeyCode>();

            foreach (string item in Modifiers.CheckedItems)
            {
                VirtualKeyCode kc;
                if (Enum.TryParse(item,out kc))
                {
                    modifiers.Add(kc);
                }
            }


            VirtualKeyCode key;
            if (!Enum.TryParse(Key.CurrentItem, out key))
            {
                // Don't know that key
                return; // Defensive
            }


            // Execute our keyboard action
            switch (Function)
            {
                case KeyboardFunction.Action:
                    KeysDown(modifiers);
                    Program.iInputSimulator.Keyboard.KeyDown(key);
                    Program.iInputSimulator.Keyboard.KeyUp(key);
                    KeysUp(modifiers);
                    break;

                case KeyboardFunction.Press:
                    KeysDown(modifiers);
                    Program.iInputSimulator.Keyboard.KeyDown(key);
                    break;

                case KeyboardFunction.Release:
                    Program.iInputSimulator.Keyboard.KeyUp(key);
                    KeysUp(modifiers);
                    break;
            }

        }

    }
}
