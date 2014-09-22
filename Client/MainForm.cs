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
using SharpDisplay;


namespace SharpDisplayClient
{
    public partial class MainForm : Form
    {
        Client iClient;
        Callback iCallback;
        ContentAlignment Alignment;
        TextField iTextFieldTop;

        public MainForm()
        {
            InitializeComponent();
            Alignment = ContentAlignment.MiddleLeft;
            iTextFieldTop = new TextField(0);
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            iCallback = new Callback(this);
            iClient = new Client(iCallback);

            //Connect using unique name
            //string name = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt");
            string name = "Client-" + (iClient.ClientCount() - 1);
            iClient.SetName(name);
            //Text = Text + ": " + name;
            Text = "[[" + name + "]]  " + iClient.SessionId;

            //
            textBoxTop.Text = iClient.Name;
            textBoxBottom.Text = iClient.SessionId;

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

        private void buttonAlignLeft_Click(object sender, EventArgs e)
        {
            Alignment = ContentAlignment.MiddleLeft;
            textBoxTop.TextAlign = HorizontalAlignment.Left;
            textBoxBottom.TextAlign = HorizontalAlignment.Left;
        }

        private void buttonAlignCenter_Click(object sender, EventArgs e)
        {
            Alignment = ContentAlignment.MiddleCenter;
            textBoxTop.TextAlign = HorizontalAlignment.Center;
            textBoxBottom.TextAlign = HorizontalAlignment.Center;
        }

        private void buttonAlignRight_Click(object sender, EventArgs e)
        {
            Alignment = ContentAlignment.MiddleRight;
            textBoxTop.TextAlign = HorizontalAlignment.Right;
            textBoxBottom.TextAlign = HorizontalAlignment.Right;
        }

        private void buttonSetTopText_Click(object sender, EventArgs e)
        {
            //TextField top = new TextField(0, textBoxTop.Text, ContentAlignment.MiddleLeft);
            iTextFieldTop.Text = textBoxTop.Text;
            iTextFieldTop.Alignment = Alignment;
            iClient.SetText(iTextFieldTop);
        }

        private void buttonSetText_Click(object sender, EventArgs e)
        {
            //iClient.SetText(0,"Top");
            //iClient.SetText(1, "Bottom");
            //TextField top = new TextField(0, textBoxTop.Text, ContentAlignment.MiddleLeft);

            iClient.SetTexts(new TextField[]
            {
                new TextField(0, textBoxTop.Text, Alignment),
                new TextField(1, textBoxBottom.Text, Alignment)
            });
        }

        private void buttonLayoutUpdate_Click(object sender, EventArgs e)
        {
            TableLayout layout = new TableLayout(2,2);
            layout.Columns[1].Width = 25F;
            iClient.SetLayout(layout);
        }
    }
}
