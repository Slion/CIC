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
    ///
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class Callback : ICallback, IDisposable
    {
        private MainForm MainForm { get; set; }

        public Callback(MainForm aMainForm)
        {
            MainForm = aMainForm;
        }

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
    ///
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
                    SetFields(Fields);
                }
                finally
                {
                    //Make sure our this state does not get out of sync
                    resetingConnection = true;
                }
            }
        }

        public void SetName(string aClientName)
        {
            Name = aClientName;
            CheckConnection();
            iClient.SetName(aClientName);
        }


        public void SetLayout(TableLayout aLayout)
        {
            Layout = aLayout;
            CheckConnection();
            iClient.SetLayout(aLayout);
        }


        public void SetField(DataField aField)
        {
            //TODO: Create fields if not present
            int i = 0;
            foreach (DataField field in Fields)
            {
                if (field.Index == aField.Index)
                {
                    //Update our field then
                    Fields[i] = aField;
                    break;
                }
                i++;
            }

            CheckConnection();
            iClient.SetField(aField);
        }

        public void SetFields(System.Collections.Generic.IList<DataField> aFields)
        {
            Fields = aFields;
            CheckConnection();
            iClient.SetFields(aFields);
        }


        public int ClientCount()
        {
            CheckConnection();
            return iClient.ClientCount();
        }



    }


}
