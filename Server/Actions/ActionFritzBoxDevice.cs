using Ear = SharpLib.Ear;
using SharpLib.Ear;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Windows.Forms;
using CodeProject.Dialog;
using SmartHome = SharpLib.FritzBox.SmartHome;

namespace SharpDisplayManager
{
    /// <summary>
    /// Abstract FRITZ! Box base class .
    /// </summary>
    [DataContract]
    public abstract class ActionFritzBoxDevice : Ear.Action
    {
        //TODO: Have a single global device list
        SmartHome.DeviceList iDeviceList;

        // Function filter
        public abstract SmartHome.Function Function();

        [DataMember]
        public string DeviceId { get; set; } = string.Empty;

        [DataMember]
        [AttributeObjectProperty
            (
            Id = "Action.FritzBox.Device",
            Name = "Smart Home Device",
            Description = "Select the device you want to use."
            )
        ]
        public PropertyComboBox Device { get; set; } = new PropertyComboBox();


        protected override void DoConstruct()
        {
            base.DoConstruct();

            // Fetch our list of the devices from the FRITZ!Box
            //UpdateDeviceList();
            // Populate our combo box with devices
            //PopulateDevices();

            //Device.SelectedIndexChangedEventHandler = SelectedIndexChangedEventHandler;
            PropertyChanged += PropertyChangedEventHandler;
        }

        void PropertyChangedEventHandler(object sender, EventArgs e)
        {
            // At this stage we must have a device list
            // Just update our ID from our new device name
            DeviceId = DeviceIdFromDeviceName(Device.CurrentItem);
        }

        /// <summary>
        /// Synchronously update our device list
        /// </summary>
        void UpdateDeviceList()
        {
            // Run this synchronously for our form to be complete
            // See: https://stackoverflow.com/questions/14485115/synchronously-waiting-for-an-async-operation-and-why-does-wait-freeze-the-pro
            Task task = Task.Run(async () => { await UpdateDeviceListAsync(); });
            task.Wait();
        }

        /// <summary>
        /// Asynchronously update our device list
        /// </summary>
        async Task UpdateDeviceListAsync()
        {
            if (Program.FritzBoxClient == null)
            {
                return;
            }

            iDeviceList = await Program.FritzBoxClient.GetDeviceList();
            if (iDeviceList==null)
            {
                // Authenticate
                await Program.FritzBoxClient.Authenticate(Properties.Settings.Default.FritzBoxLogin, Properties.Settings.Default.FritzBoxPassword);
                // Try again
                iDeviceList = await Program.FritzBoxClient.GetDeviceList();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aDeviceName"></param>
        /// <returns></returns>
        public string DeviceIdFromDeviceName(string aDeviceName)
        {
            if (iDeviceList==null)
            {
                return string.Empty;
            }

            foreach (SmartHome.Device d in iDeviceList.Devices)
            {
                // Return ID if name matches
                if (d.Name.Equals(aDeviceName))
                {
                    return d.Identifier;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aDeviceName"></param>
        /// <returns></returns>
        public string DeviceNameFromDeviceId(string aDeviceId)
        {
            if (iDeviceList == null)
            {
                return string.Empty;
            }

            foreach (SmartHome.Device d in iDeviceList.Devices)
            {
                // Return ID if name matches
                if (d.Identifier.Equals(aDeviceId))
                {
                    return d.Name;
                }
            }

            return string.Empty;
        }


        /// <summary>
        /// 
        /// </summary>
        private void PopulateDevices()
        {
            //Reset our list of devices
            Device.Items = new List<string>();

            if (iDeviceList == null)
            {
                return;
            }

            //Go through each device and filter out the ones we don't want
            foreach (SmartHome.Device d in iDeviceList.Devices)
            {
                Debug.WriteLine("Smart Home Device " + d.Name);

                //Add that device if the function matches
                if (d.Has(Function()))
                {
                    Device.Items.Add(d.Name);
                }
            }

            // Take this opportunity to update our device name if it was changed
            Device.CurrentItem = DeviceNameFromDeviceId(DeviceId);

            // Set current item if not yet valid
            if (string.IsNullOrEmpty(Device.CurrentItem) && Device.Items.Count>0)
            {
                Device.CurrentItem = Device.Items[0];
                DeviceId = DeviceIdFromDeviceName(Device.CurrentItem);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
            if (Program.FritzBoxClient == null)
            {
                return false;
            }

            //TODO: check if the device exists
            return !string.IsNullOrEmpty(DeviceId);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnStateEnter()
        {
            //Ear.Action.OnStateEnter();

            if (CurrentState == State.PrepareEdit)
            {
                // Prepare edit mode
                // Fetch our list of the devices from the FRITZ!Box
                UpdateDeviceList();
                // Populate our combo box with devices
                PopulateDevices();
            }
        }


    }
}
