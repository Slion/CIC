using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Diagnostics;


namespace SharpDisplayClient
{
    public partial class MainForm : Form
    {
        Client iClient;
        Callback iCallback;

        public MainForm()
        {
            InitializeComponent();
        }

        private void buttonSetText_Click(object sender, EventArgs e)
        {
            //iClient.SetText(0,"Top");
            //iClient.SetText(1, "Bottom");
            iClient.SetTexts(new string[] { iClient.Name, iClient.SessionId });
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            iCallback = new Callback(this);
            //Instance context is then managed by our client class
            InstanceContext instanceContext = new InstanceContext(iCallback);
            iClient = new Client(instanceContext);

            //Connect using unique name
            //string name = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt");
            string name = "Client-" + (iClient.ClientCount() - 1);
            iClient.SetName(name);
            //Text = Text + ": " + name;
            Text = "[[" + name + "]]  " + iClient.SessionId;

        }


       
        public delegate void CloseConnectionDelegate();
        public delegate void CloseDelegate();

        /// <summary>
        /// 
        /// </summary>
        public void CloseConnectionThreadSafe()
        {
            if (this.InvokeRequired)
            {
                //Not in the proper thread, invoke ourselves
                CloseConnectionDelegate d = new CloseConnectionDelegate(CloseConnectionThreadSafe);
                this.Invoke(d, new object[] { });
            }
            else
            {
                //We are in the proper thread
                if (IsClientReady())
                {
                    Trace.TraceInformation("Closing client: " + iClient.SessionId);
                    iClient.Close();
                    Trace.TraceInformation("Closed client: " + iClient.SessionId);
                }

                iClient = null;
                iCallback = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void CloseThreadSafe()
        {
            if (this.InvokeRequired)
            {
                //Not in the proper thread, invoke ourselves
                CloseDelegate d = new CloseDelegate(CloseThreadSafe);
                this.Invoke(d, new object[] { });
            }
            else
            {
                //We are in the proper thread
                Close();
            }
        }


        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseConnectionThreadSafe();
        }

        public bool IsClientReady()
        {
            return (iClient != null && iClient.State == CommunicationState.Opened);
        }
    }
}
