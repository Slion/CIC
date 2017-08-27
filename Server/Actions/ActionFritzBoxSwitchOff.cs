using Ear = SharpLib.Ear;
using SharpLib.Ear;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using SmartHome = SharpLib.FritzBox.SmartHome;

namespace SharpDisplayManager
{
    [DataContract]
    [AttributeObject(Id = "FritzBox.Switch.Off", Name = "FRITZ!Box Switch Off", Description = "Turn off a FRITZ!Box Switch.")]
    public class ActionFritzBoxSwitchOff : ActionFritzBoxSwitch
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
            return "Turn off " + Device.CurrentItem;
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
            
            // Try and toggle our switch
            bool done = await Program.FritzBoxClient.SwitchOff(DeviceId);
            if (!done)
            {
                // In case of failure authenticate anew before trying one last time 
                await Program.FritzBoxClient.Authenticate(Properties.Settings.Default.FritzBoxLogin, Properties.Settings.Default.DecryptFritzBoxPassword());
                await Program.FritzBoxClient.SwitchOff(DeviceId);
            }
        }
    }
}
