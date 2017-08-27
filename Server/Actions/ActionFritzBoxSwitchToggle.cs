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
    [DataContract]
    [AttributeObject(Id = "FritzBox.Switch.Toggle", Name = "FRITZ!Box Switch Toggle", Description = "Toggle a FRITZ!Box Switch.")]
    public class ActionFritzBoxSwitchToggle : ActionFritzBoxSwitch
    {
        // Function filter
        public override SmartHome.Function Function()
        {
            return SmartHome.Function.SwitchSocket;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string BriefBase()
        {
            return "Toggle Switch " + Device.CurrentItem;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override async Task DoExecute(Context aContext)
        {
            if (Program.FritzBoxClient == null)
            {
                Trace.WriteLine("WARNING: No FRITZ!Box client.");
                return;
            }

            await Program.FritzBoxClient.Authenticate(Properties.Settings.Default.FritzBoxLogin, Properties.Settings.Default.FritzBoxPassword);
            //Go through each device and find the one we want
            //TODO: Use device ID instead?
            await Program.FritzBoxClient.SwitchToggle(DeviceId);
            /*
            SmartHome.DeviceList deviceList = await Program.FritzBoxClient.GetDeviceList();
            foreach (SmartHome.Device d in deviceList.Devices)
            {               
                //Add that device if the function matches
                if (d.Name.Equals(Device.CurrentItem))
                {
                    await d.SwitchToggle();
                    return;
                }
            }
            */
        }
    }
}
