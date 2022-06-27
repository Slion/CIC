using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Ear = SharpLib.Ear;
using Hid = SharpLib.Hid;
using SharpLib.Hid;
using SharpLib.Win32;

namespace SharpDisplayManager.Events
{
    [DataContract]
    [Ear.AttributeObject(Id = "Event.Hid.Axis.Zones", Name = "HID Axis zones", Description = "When your axis enter a certain zone it will trigger an action at a specific index.")]
    class EventHidAxisZones : EventHidAxis
    {
        [DataMember]
        [Ear.AttributeObjectProperty
            (
            Id = "EventHidAxisZones.Size",
            Name = "Size",
            Description = "Specifies the size of your zones in percentage of your axis range.",
            Minimum = 1,
            Maximum = 45,
            Increment = 1
            )
        ]
        public int Size { get; set; } = 25;


        [DataMember]
        [Ear.AttributeObjectProperty
            (
            Id = "EventHidAxisZones.Invert",
            Name = "Invert",
            Description = "Invert axis value before processing."
            )
        ]
        public bool Invert { get; set; } = false;


        /// <summary>
        /// 
        /// </summary>
        int iLastValue = 0;
        int iActionIndex = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Matches(object obj)
        {
            if (!(obj is EventHid))
            {
                return false;
            }

            EventHid e = (EventHid)obj;

            bool joystick = e.IsJoystick == IsJoystick;
            bool isJoystick = joystick && IsJoystick;
            bool gamepad = e.IsGamePad == IsGamePad;
            bool isGamepad = gamepad && IsGamePad;
            if (!isJoystick && !isGamepad)
            {
                return false;
            }

            bool sameDevice = e.Device.CurrentItem.Equals(Device.CurrentItem, StringComparison.OrdinalIgnoreCase);
            if (!sameDevice)
            {
                return false;
            }

            //For each axis on that device
            foreach (KeyValuePair<HIDP_VALUE_CAPS, uint> entry in e.HidEvent.UsageValues)
            {
                if (!Events.Axis.IsAxis(entry.Key))
                {
                    continue;
                }

                // TODO: Avoid allocations here
                var axis = new Axis(entry.Key);
                axis.Value = (int)entry.Value;
                if (Invert)
                {
                    axis.Value = axis.Range - axis.Value;
                }
                

                //TODO: Could we do it without string comparison, to use the axis ID we would need to save it too
                if (axis.FullName == Axis.CurrentItem)
                {
                    int lowBound = (axis.Range * Size / 100);
                    int highBound = axis.Range - lowBound;


                    bool match = false;
                    if (axis.Value > highBound && !(iLastValue > highBound))
                    {
                        // We passed our threshold trigger that event
                        iActionIndex = 0;
                        match = true;
                    } 
                    else if (axis.Value < lowBound && !(iLastValue < lowBound)) 
                    {
                        // We passed our threshold trigger that event
                        iActionIndex = 3;
                        match = true;
                    } 
                    // We are between our two boundaries somewhere in the middle of our axis
                    else if (axis.Value >= lowBound && axis.Value <= highBound)
                    {
                        if (iLastValue > highBound)
                        {
                            // We were just going down
                            iActionIndex = 1;
                            match = true;
                        }
                        else if (iLastValue < lowBound)
                        {
                            // We were just going up
                            iActionIndex = 2;
                            match = true;
                        }
                    }

                    iLastValue = axis.Value;
                    return match;
                }
            }

            return false;
        }

        public override async Task Trigger()
        {
            //Trace.WriteLine("Event triggered: " + AttributeName);
            //foreach (Ear.Action action in Objects.OfType<Ear.Action>())
            //{
            //    await action.Execute(Context);
            //}

            // Only execute the action corresponding to our zone
            if (Objects.Count() > iActionIndex)
            {
                await ((Ear.Action)Objects[iActionIndex]).Execute(Context);
            }
        }

    }
}
