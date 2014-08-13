﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpDisplayManager;
using System.ServiceModel;
using System.ServiceModel.Channels;


namespace SharpDisplayClient
{
    public partial class ClientInput : SharpDisplayManager.IDisplayServiceCallback
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
        }
    }



    public partial class ClientOutput : DuplexClientBase<IDisplayService>, IDisplayService
    {
        public ClientOutput(InstanceContext callbackInstance)
            : base(callbackInstance, new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:8001/DisplayService"))
        { }

        public void Connect(string aClientName)
        {
            Channel.Connect(aClientName);
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
