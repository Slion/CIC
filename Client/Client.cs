using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpDisplayInterface;
using System.ServiceModel;
using System.ServiceModel.Channels;


namespace SharpDisplayClient
{
    /// <summary>
    ///
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class Callback : IDisplayServiceCallback, IDisposable
    {
        public void OnConnected()
        {
            //Debug.Assert(Thread.CurrentThread.IsThreadPoolThread);
            //Trace.WriteLine("Callback thread = " + Thread.CurrentThread.ManagedThreadId);

            MessageBox.Show("OnConnected()", "Client");
        }


        public void OnServerClosing()
        {
            //Debug.Assert(Thread.CurrentThread.IsThreadPoolThread);
            //Trace.WriteLine("Callback thread = " + Thread.CurrentThread.ManagedThreadId);

            //MessageBox.Show("OnServerClosing()", "Client");
            Program.iMainForm.CloseConnection();
            Program.iMainForm.Close();
        }

        //From IDisposable
        public void Dispose()
        {

        }
    }


    /// <summary>
    ///
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class Client : DuplexClientBase<IDisplayService>
    {
        public string Name { get; set; }
        public string SessionId { get { return InnerChannel.SessionId; } }

        public Client(InstanceContext callbackInstance)
            : base(callbackInstance, new NetTcpBinding(SecurityMode.None, true), new EndpointAddress("net.tcp://localhost:8001/DisplayService"))
        { }

        public void Connect(string aClientName)
        {
            Name = aClientName;
            Channel.Connect(aClientName);
        }

        public void Disconnect()
        {
            Channel.Disconnect(Name);
            Name = "";
        }

        public void SetText(int aLineIndex, string aText)
        {
            Channel.SetText(aLineIndex, aText);
        }


        public void SetTexts(System.Collections.Generic.IList<string> aTexts)
        {
            Channel.SetTexts(aTexts);
        }

        

    }
}
