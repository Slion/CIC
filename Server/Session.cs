﻿using System;
using System.Windows.Forms;
using System.Collections;
using System.ServiceModel;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using SharpDisplay;

namespace SharpDisplay
{
    /// <summary>
    /// Implement our display services.
    /// Each client connection has such a session object server side.
    /// </summary>
    [ServiceBehavior(
                        ConcurrencyMode = ConcurrencyMode.Multiple,
                        InstanceContextMode = InstanceContextMode.PerSession
                    )]
    class Session : IService, IDisposable
    {
        public string SessionId { get; set; }
        public string Name { get; set; }

        Session()
        {
            Trace.TraceInformation("Server session opening.");
            //First save our session ID. It will be needed in Dispose cause our OperationContxt won't be available then.
            SessionId = OperationContext.Current.SessionId;
            ICallback callback = OperationContext.Current.GetCallbackChannel<ICallback>();
            //
            SharpDisplayManager.Program.iMainForm.AddClientThreadSafe(SessionId,callback);

        }

        public void Dispose()
        {
            Trace.TraceInformation("Server session closing.");
            SharpDisplayManager.Program.iMainForm.RemoveClientThreadSafe(SessionId);
        }

        //
        public void SetName(string aClientName)
        {
            Name = aClientName;
            SharpDisplayManager.Program.iMainForm.SetClientNameThreadSafe(SessionId, Name);
            //Disconnect(aClientName);

            //Register our client and its callback interface
            //IDisplayServiceCallback callback = OperationContext.Current.GetCallbackChannel<IDisplayServiceCallback>();
            //Program.iMainForm.iClients.Add(aClientName, callback);
            //Program.iMainForm.treeViewClients.Nodes.Add(aClientName, aClientName);
            //For some reason MP still hangs on that one
            //callback.OnConnected();
        }

        public void SetLayout(TableLayout aLayout)
        {
            SharpDisplayManager.Program.iMainForm.SetClientLayoutThreadSafe(SessionId, aLayout);
        }

        //From IDisplayService
        public void SetFields(System.Collections.Generic.IList<DataField> aFields)
        {
            SharpDisplayManager.Program.iMainForm.SetClientTextsThreadSafe(SessionId, aFields);
        }

        //
        public void SetField(DataField aField)
        {
            if (aField.HasBitmap)
            {
                SharpDisplayManager.Program.iMainForm.SetClientBitmapThreadSafe(SessionId, aField);
            }
            else
            {
                SharpDisplayManager.Program.iMainForm.SetClientTextThreadSafe(SessionId, aField);
            }

        }


        ///
        public int ClientCount()
        {
            return SharpDisplayManager.Program.iMainForm.iClients.Count;
        }



    }

}
