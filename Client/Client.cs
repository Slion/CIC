﻿using System;
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
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class Callback : IDisplayServiceCallback, IDisposable
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
    public class Client : DuplexClientBase<IDisplayService>
    {
        public string Name { get; set; }
        public string SessionId { get { return InnerChannel.SessionId; } }

        public Client(InstanceContext callbackInstance)
            : base(callbackInstance, new NetTcpBinding(SecurityMode.None, true), new EndpointAddress("net.tcp://localhost:8001/DisplayService"))
        { }

        public void SetName(string aClientName)
        {
            Name = aClientName;
            Channel.SetName(aClientName);
        }

        public void SetText(TextField aTextField)
        {
            Channel.SetText(aTextField);
        }

        public void SetTexts(System.Collections.Generic.IList<TextField> aTextFields)
        {
            Channel.SetTexts(aTextFields);
        }

        public int ClientCount()
        {
            return Channel.ClientCount();
        }
    }
}