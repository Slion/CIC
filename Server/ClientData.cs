using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//
using SharpLib.Display;

namespace SharpDisplayManager
{
    /// <summary>
    /// A UI thread copy of a client relevant data.
    /// Keeping this copy in the UI thread helps us deal with threading issues.
    /// </summary>
    public class ClientData
    {
        public ClientData(string aSessionId, ICallback aCallback)
        {
            SessionId = aSessionId;
            Name = "";
            Priority = Priorities.Default;
            Callback = aCallback;
            HasNewLayout = true;
            View = new View();       
        }

        public string SessionId { get; set; }
        public string Name { get; set; }
        public uint Priority { get; set; }
        public Target Target { get; set; }
        public View View { get; set; }
        public ICallback Callback { get; set; }
        

        public bool HasNewLayout { get; set; }

        //Client management
        public DateTime LastSwitchTime { get; set; }



    }
}
