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
            iCallback = new Callback();
            //Instance context is then managed by our client class
            InstanceContext instanceContext = new InstanceContext(iCallback);
            iClient = new Client(instanceContext);

            //Connect using unique name
            string name = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt");
            iClient.Connect(name);
            //Text = Text + ": " + name;
            Text = Text + ": " + iClient.SessionId;

        }

        public void CloseConnection()
        {
            if (IsClientReady())
            {
                //iClient.Disconnect();
                iClient.Close();
            }

            iClient = null;
            iCallback = null;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsClientReady()) //Could catch exception instead
            {
                iClient.Disconnect();
            }

            CloseConnection();
        }

        public bool IsClientReady()
        {
            return (iClient != null && iClient.State == CommunicationState.Opened);
        }
    }
}
