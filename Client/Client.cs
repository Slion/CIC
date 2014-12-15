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
        public string Name { get; set; }
        public string SessionId { get { return InnerChannel.SessionId; } }

        public Client(ICallback aCallback)
            : base(new InstanceContext(aCallback), new NetTcpBinding(SecurityMode.None, true), new EndpointAddress("net.tcp://localhost:8001/DisplayService"))
        { }

        public void SetName(string aClientName)
        {
            Name = aClientName;
            Channel.SetName(aClientName);
        }


        public void SetLayout(TableLayout aLayout)
        {
            Channel.SetLayout(aLayout);
        }

        public void SetText(DataField aField)
        {
            Channel.SetText(aField);
        }

        public void SetTexts(System.Collections.Generic.IList<DataField> aFields)
        {
            Channel.SetTexts(aFields);
        }

        public void SetBitmap(DataField aField)
        {
            Channel.SetBitmap(aField);
        }

        public int ClientCount()
        {
            return Channel.ClientCount();
        }

        public bool IsReady()
        {
            return State == CommunicationState.Opened;
        }
    }


    /// <summary>
    ///
    /// </summary>
    public class DisplayClient
    {
        Client iClient;
        Callback iCallback;
        private MainForm MainForm { get; set; }

        public string Name { get; set; }
        public string SessionId { get { return iClient.SessionId; } }

        public DisplayClient(MainForm aMainForm)
        {
            MainForm = aMainForm;
            Name = "";
        }

        public void Open()
        {
            iCallback = new Callback(MainForm);
            iClient = new Client(iCallback);
            if (Name != "")
            {
                iClient.SetName(Name);
            }
        }

        public void Close()
        {
            iClient.Close();
            iClient = null;
            iCallback.Dispose();
            iCallback = null;
        }

        public bool IsReady()
        {
            return (iClient != null && iCallback != null && iClient.IsReady());
        }

        public void CheckConnection()
        {
            if (!IsReady())
            {
                Open();
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
            CheckConnection();
            iClient.SetLayout(aLayout);
        }

        public void SetText(DataField aField)
        {
            CheckConnection();
            iClient.SetText(aField);
        }

        public void SetTexts(System.Collections.Generic.IList<DataField> aFields)
        {
            CheckConnection();
            iClient.SetTexts(aFields);
        }

        public void SetBitmap(DataField aField)
        {
            CheckConnection();
            iClient.SetBitmap(aField);
        }

        public int ClientCount()
        {
            CheckConnection();
            return iClient.ClientCount();
        }



    }


}
