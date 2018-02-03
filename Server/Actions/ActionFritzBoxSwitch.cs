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
using SmartHome = SharpLib.FritzBox.SmartHome;

namespace SharpDisplayManager
{
    /// <summary>
    /// Filter FRITZ! Box device with Switch Socket function
    /// </summary>
    [DataContract]
    public abstract class ActionFritzBoxSwitch : ActionFritzBoxDevice
    {
        // Function filter
        public override SmartHome.Function Function()
        {
            return SmartHome.Function.SwitchSocket;
        }
    }
}
