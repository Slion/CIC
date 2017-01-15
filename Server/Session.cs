//
// Copyright (C) 2014-2015 Stéphane Lenclud.
//
// This file is part of SharpDisplayManager.
//
// SharpDisplayManager is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// SharpDisplayManager is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with SharpDisplayManager.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.Windows.Forms;
using System.Collections;
using System.ServiceModel;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using SharpLib.Display;

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
        public uint Priority { get; set; }
        public Target Target { get; set; }

        Session()
        {
            Trace.TraceInformation("Server session opening.");
            //First save our session ID. It will be needed in Dispose cause our OperationContxt won't be available then.
            SessionId = OperationContext.Current.SessionId;
            ICallback callback = OperationContext.Current.GetCallbackChannel<ICallback>();
            //
            SharpDisplayManager.Program.iFormMain.AddClientThreadSafe(SessionId,callback);

        }

        public void Dispose()
        {
            Trace.TraceInformation("Server session closing.");
            SharpDisplayManager.Program.iFormMain.RemoveClientThreadSafe(SessionId);
        }

        //
        public void SetName(string aClientName)
        {
            Name = aClientName;
            SharpDisplayManager.Program.iFormMain.SetClientNameThreadSafe(SessionId, Name);
            //Disconnect(aClientName);

            //Register our client and its callback interface
            //IDisplayServiceCallback callback = OperationContext.Current.GetCallbackChannel<IDisplayServiceCallback>();
            //Program.iFormMain.iClients.Add(aClientName, callback);
            //Program.iFormMain.treeViewClients.Nodes.Add(aClientName, aClientName);
            //For some reason MP still hangs on that one
            //callback.OnConnected();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aPriority"></param>
        public void SetPriority(uint aPriority)
        {
            Priority = aPriority;
            SharpDisplayManager.Program.iFormMain.SetClientPriorityThreadSafe(SessionId, Priority);
        }

        public void SetTarget(Target aTarget)
        {
            Target = aTarget;
            SharpDisplayManager.Program.iFormMain.SetClientTargetThreadSafe(SessionId, Target);
        }


        public void SetLayout(TableLayout aLayout)
        {
            SharpDisplayManager.Program.iFormMain.SetClientLayoutThreadSafe(SessionId, aLayout);
        }

        //
        public void SetField(DataField aField)
        {
            SharpDisplayManager.Program.iFormMain.SetClientFieldThreadSafe(SessionId, aField);
        }

        //From IDisplayService
        public void SetFields(System.Collections.Generic.IList<DataField> aFields)
        {
            SharpDisplayManager.Program.iFormMain.SetClientFieldsThreadSafe(SessionId, aFields);
        }

        ///
        public int ClientCount()
        {
            return SharpDisplayManager.Program.iFormMain.iClients.Count;
        }

        public void  TriggerEventsByName(string aName)
        {
            SharpDisplayManager.Properties.Settings.Default.EarManager.TriggerEventsByName(aName);
        }


    }

}
