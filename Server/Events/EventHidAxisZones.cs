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
    /// <summary>
    /// Enable mapping an axis to keys for instance, with a couple of those you can thus enable WASD.
    /// That event is special cause it does not trigger all events upon match.
    /// Instead it triggers only the event at a specific index corresponding to a sub events:
    /// - Action index 0: When the axis goes in the zone above the higher boundary.
    /// - Action index 1: When the axis comes back from the zone above the higher boundary.
    /// - Action index 3: When the axis comes back from the zone below the lower boundary.
    /// - Action index 4: When the axis goes in the zone below the lower boundary.
    /// 
    /// In theory we could implement four different events for that but unless they share some states it means we would do 4 times the same processing.
    /// I guess the action selection is not that bad, also saves user from having to setup four events. It just needs to be well documented.
    /// </summary>
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

        Events.Axis TargetAxis { get; set; }

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

            // TODO: Avoid string comparison here? Compute a hash maybe?
            bool sameDevice = e.Device.CurrentItem.Equals(Device.CurrentItem, StringComparison.OrdinalIgnoreCase);
            if (!sameDevice)
            {
                // That's not the device we are interested in
                return false;
            }

            //For each axis on that device
            foreach (KeyValuePair<HIDP_VALUE_CAPS, uint> entry in e.HidEvent.UsageValues)
            {
                if (!Events.Axis.IsAxis(entry.Key))
                {
                    continue;
                }

                if (Events.Axis.IdFromValueCaps(entry.Key)!=AxisId)
                {
                    // That's not the axis we are interested in
                    continue;
                }

                // First time here
                if (TargetAxis==null)
                {
                    TargetAxis = new Axis(entry.Key);
                }

                var axis = TargetAxis;
                axis.Value = (int)entry.Value;
                if (Invert)
                {
                    axis.Value = axis.Range - axis.Value;
                }
                
                // Compute trigger boundaries from our range
                int lowBound = (axis.Range * Size / 100);
                int highBound = axis.Range - lowBound;

                //
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


        protected override void OnStateEnter()
        {
            base.OnStateEnter();

            if (CurrentState == State.Rest)
            {
                // May have changed after edition
                TargetAxis = null;
            }
        }


    }
}
