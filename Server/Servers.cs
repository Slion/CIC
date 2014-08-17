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
        public string Name { get; set; }

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
            Program.iMainForm.SetTextsThreadSafe(SessionId, aTexts);
        }

        //
        public void SetText(int aLineIndex, string aText)
        {
            Program.iMainForm.SetTextThreadSafe(SessionId, aLineIndex, aText);
        }

        //
        public void SetName(string aClientName)
        {
            Name = aClientName;
            Program.iMainForm.SetClientNameThreadSafe(SessionId, Name);
            //Disconnect(aClientName);

            //Register our client and its callback interface
            //IDisplayServiceCallback callback = OperationContext.Current.GetCallbackChannel<IDisplayServiceCallback>();
            //Program.iMainForm.iClients.Add(aClientName, callback);
            //Program.iMainForm.treeViewClients.Nodes.Add(aClientName, aClientName);
            //For some reason MP still hangs on that one
            //callback.OnConnected();
        }

        ///
        public int ClientCount()
        {
            return Program.iMainForm.iClients.Count;
        }

        

    }

}
