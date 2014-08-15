using System;
using System.Windows.Forms;
using System.Collections;
using System.ServiceModel;
using System.Collections.Generic;
using System.Linq;
using SharpDisplayInterface;
using System.Diagnostics;

namespace SharpDisplayManager
{
    /// <summary>
    /// Implement our display service.
    /// This class is instantiated anew whenever a client send a request.
    /// </summary>
    [ServiceBehavior(   
                        ConcurrencyMode = ConcurrencyMode.Multiple,
                        InstanceContextMode = InstanceContextMode.PerSession                       
                    )]
    class DisplayServer : IDisplayService, IDisposable
    {
        public string SessionId { get; set; }

        DisplayServer()
        {
            Trace.TraceInformation("Server session opening.");
            //First save our session ID. It will be needed in Dispose cause our OperationContxt won't be available then.
            SessionId = OperationContext.Current.SessionId;
            IDisplayServiceCallback callback = OperationContext.Current.GetCallbackChannel<IDisplayServiceCallback>();
            //
            Program.iMainForm.AddClientThreadSafe(SessionId,callback);

        }

        public void Dispose()
        {
            Trace.TraceInformation("Server session closing.");
            Program.iMainForm.RemoveClientThreadSafe(SessionId);
        }
        
        //From IDisplayService
        public void SetTexts(System.Collections.Generic.IList<string> aTexts)
        {
            Program.iMainForm.SetTextsThreadSafe(aTexts);
        }

        //
        public void SetText(int aLineIndex, string aText)
        {
            Program.iMainForm.SetTextThreadSafe(aLineIndex, aText);
        }

        //
        public void Connect(string aClientName)
        {
            //Disconnect(aClientName);

            //Register our client and its callback interface
            //IDisplayServiceCallback callback = OperationContext.Current.GetCallbackChannel<IDisplayServiceCallback>();
            //Program.iMainForm.iClients.Add(aClientName, callback);
            //Program.iMainForm.treeViewClients.Nodes.Add(aClientName, aClientName);
            //For some reason MP still hangs on that one
            //callback.OnConnected();
        }

        ///
        public void Disconnect(string aClientName)
        {
            //remove the old client if any
            /*
            if (Program.iMainForm.iClients.Keys.Contains(aClientName))
            {
                Program.iMainForm.iClients.Remove(aClientName);
                Program.iMainForm.treeViewClients.Nodes.Remove(Program.iMainForm.treeViewClients.Nodes.Find(aClientName,false)[0]);
            }
             */

        }

        

    }

}
