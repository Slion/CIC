using SharpLib.Ear;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using SharpLib.MonitorConfig;
using System.Reflection;
using System.Diagnostics;

namespace SharpDisplayManager
{
    /// <summary>
    /// Abstract Monitor Config action .
    /// </summary>
    [DataContract]
    [AttributeObject(Id = "MicroInput.Keyboard", Name = "Micro Input Keyboard", Description = "Send a keyboard event through Teensy hardware.")]
    public class ActionMicroInputKeyboard : SharpLib.Ear.Action
    {
        [DataMember]
        [AttributeObjectProperty
            (
            Id = "Action.MicroInputKeyboardModifiers",
            Name = "Keyboard modifiers",
            Description = "Select keyboard modifiers."
            )
        ]
        public PropertyCheckedListBox Modifiers { get; set; } = new PropertyCheckedListBox();


        [DataMember]
        [AttributeObjectProperty
            (
            Id = "Action.MicroInputKeyboardKey",
            Name = "Keyboard key",
            Description = "Select the keyboard key you want to send."
            )
        ]
        public PropertyComboBox Key { get; set; } = new PropertyComboBox();


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
            string brief = AttributeName + " ";
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
            Key.Items = new List<string>();

            foreach (FieldInfo field in typeof(SharpLib.MicroInput.Keyboard.Key).GetFields())
            {
                if (field.IsPublic)
                {
                    Key.Items.Add(field.Name);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopulateKeyboardModifers()
        {
            Modifiers.Items = new List<string>();

            foreach (FieldInfo field in typeof(SharpLib.MicroInput.Keyboard.Modifier).GetFields())
            {
                if (field.IsPublic)
                {
                    Modifiers.Items.Add(field.Name);
                }
            }
        }

        protected override async Task DoExecute(Context aContext)
        {
            if (Program.iMicroInput == null || !Program.iMicroInput.IsOpen)
            {
                Trace.WriteLine("WARNING: No Micro Input installed.");
                return;
            }
           
            // Pack selected modifiers
            ushort modifiers = 0;
            foreach (string item in Modifiers.CheckedItems)
            {
                modifiers |= (ushort)typeof(SharpLib.MicroInput.Keyboard.Modifier).GetField(item).GetValue(null);
            }

            // Execute our keyboard action
            Program.iMicroInput.KeyboardAction((ushort)typeof(SharpLib.MicroInput.Keyboard.Key).GetField(Key.CurrentItem).GetValue(null), modifiers);
        }

    }
}
