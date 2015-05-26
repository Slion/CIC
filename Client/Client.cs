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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpDisplay;
using System.ServiceModel;
using System.ServiceModel.Channels;


namespace SharpDisplayClient
{
    /// <summary>
    /// Client side Sharp Display callback implementation.
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class Callback : ICallback, IDisposable
    {
        private MainForm MainForm { get; set; }

        public Callback(MainForm aMainForm)
        {
            MainForm = aMainForm;
        }

        /// <summary>
        /// Not used I believe.
        /// </summary>
        public void OnConnected()
        {
            //Debug.Assert(Thread.CurrentThread.IsThreadPoolThread);
            //Trace.WriteLine("Callback thread = " + Thread.CurrentThread.ManagedThreadId);

            MessageBox.Show("OnConnected()", "Client");
        }


        public void OnCloseOrder()
        {
            //Debug.Assert(Thread.CurrentThread.IsThreadPoolThread);
            //Trace.WriteLine("Callback thread = " + Thread.CurrentThread.ManagedThreadId);

            //MessageBox.Show("OnServerClosing()", "Client");
            MainForm.CloseConnectionThreadSafe();
            MainForm.CloseThreadSafe();
        }

        //From IDisposable
        public void Dispose()
        {

        }
    }


    /// <summary>
    /// Client side implementation of our Sharp Display Service.
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class Client : DuplexClientBase<IService>
    {
        public string SessionId { get { return InnerChannel.SessionId; } }

        public Client(ICallback aCallback)
            : base(new InstanceContext(aCallback), new NetTcpBinding(SecurityMode.None, true), new EndpointAddress("net.tcp://localhost:8001/DisplayService"))
        { }

        public void SetName(string aClientName)
        {
            Channel.SetName(aClientName);
        }

        public void SetLayout(TableLayout aLayout)
        {
            Channel.SetLayout(aLayout);
        }

        public void SetField(DataField aField)
        {
            Channel.SetField(aField);
        }

        public void SetFields(System.Collections.Generic.IList<DataField> aFields)
        {
            Channel.SetFields(aFields);
        }

        public int ClientCount()
        {
            return Channel.ClientCount();
        }

        public bool IsReady()
        {
            return State == CommunicationState.Opened || State == CommunicationState.Created;
        }
    }


    /// <summary>
    /// Handle connection with our Sharp Display Server.
    /// If the connection is faulted it will attempt to restart it.
    /// </summary>
    public class DisplayClient
    {
        private Client iClient;
        private Callback iCallback;
        private bool resetingConnection = false;

        private MainForm MainForm { get; set; }
        public string SessionId { get { return iClient.SessionId; } }
        public string Name { get; private set; }
        private TableLayout Layout { get; set; }
        private System.Collections.Generic.IList<DataField> Fields { get; set; }


        public DisplayClient(MainForm aMainForm)
        {
            MainForm = aMainForm;
            Name = "";
            Fields = new DataField[]{};
        }

        /// <summary>
        /// Initialize our server connection.
        /// </summary>
        public void Open()
        {
            iCallback = new Callback(MainForm);
            iClient = new Client(iCallback);
        }

        /// <summary>
        /// Terminate our server connection.
        /// </summary>
        public void Close()
        {
            iClient.Close();
            iClient = null;
            iCallback.Dispose();
            iCallback = null;
        }

        /// <summary>
        /// Tells whether a server connection is available.
        /// </summary>
        /// <returns>True if a server connection is available. False otherwise.</returns>
        public bool IsReady()
        {
            return (iClient != null && iCallback != null && iClient.IsReady());
        }

        /// <summary>
        /// Check if our server connection is available and attempt reset it if it isn't.
        /// This is notably dealing with timed out connections.
        /// </summary>
        public void CheckConnection()
        {
            if (!IsReady() && !resetingConnection)
            {
                //Try to reconnect
                Open();

                //Avoid stack overflow in case of persisting failure
                resetingConnection = true;

                try
                {
                    //On reconnect there is a bunch of properties we need to reset
                    if (Name != "")
                    {
                        iClient.SetName(Name);
                    }

                    SetLayout(Layout);
                    iClient.SetFields(Fields);
                }
                finally
                {
                    //Make sure our this state does not get out of sync
                    resetingConnection = true;
                }
            }
        }

        /// <summary>
        /// Set our client's name.
        /// Client's name is typically user friendly.
        /// It does not have to be unique.
        /// </summary>
        /// <param name="aClientName">Our client name.</param>
        public void SetName(string aClientName)
        {
            Name = aClientName;
            CheckConnection();
            iClient.SetName(aClientName);
        }

        /// <summary>
        /// Set your client fields' layout.
        /// </summary>
        /// <param name="aLayout">The layout to apply for this client.</param>
        public void SetLayout(TableLayout aLayout)
        {
            Layout = aLayout;
            CheckConnection();
            iClient.SetLayout(aLayout);
        }

        /// <summary>
        /// Set the specified field.
        /// </summary>
        /// <param name="aField"></param>
        /// <returns>True if the specified field was set client side. False means you need to redefine all your fields using CreateFields.</returns>
        public bool SetField(DataField aField)
        {
            int i = 0;
            bool fieldFound = false;
            foreach (DataField field in Fields)
            {
                if (field.Index == aField.Index)
                {
                    //Update our field then
                    Fields[i] = aField;
                    fieldFound = true;
                    break;
                }
                i++;
            }

            if (!fieldFound)
            {
                //Field not found, make sure to use CreateFields first after setting your layout.
                return false;
            }

            CheckConnection();
            iClient.SetField(aField);
            return true;
        }

        /// <summary>
        /// Use this function when updating existing fields.
        /// </summary>
        /// <param name="aFields"></param>
        public bool SetFields(System.Collections.Generic.IList<DataField> aFields)
        {
            int fieldFoundCount = 0;
            foreach (DataField fieldUpdate in aFields)
            {
                int i = 0;
                foreach (DataField existingField in Fields)
                {
                    if (existingField.Index == fieldUpdate.Index)
                    {
                        //Update our field then
                        Fields[i] = fieldUpdate;
                        fieldFoundCount++;
                        //Move on to the next field
                        break;
                    }
                    i++;
                }
            }

            //
            if (fieldFoundCount!=aFields.Count)
            {
                //Field not found, make sure to use CreateFields first after setting your layout.
                return false;
            }

            CheckConnection();
            iClient.SetFields(aFields);
            return true;
        }

        /// <summary>
        /// Use this function when creating your fields.
        /// This must be done at least once after setting your layout.
        /// </summary>
        /// <param name="aFields"></param>
        public void CreateFields(System.Collections.Generic.IList<DataField> aFields)
        {
            Fields = aFields;
            CheckConnection();
            iClient.SetFields(aFields);
        }

        /// <summary>
        /// Provide the number of clients currently connected to our server.
        /// </summary>
        /// <returns>Number of clients currently connected to our server.</returns>
        public int ClientCount()
        {
            CheckConnection();
            return iClient.ClientCount();
        }



    }


}
