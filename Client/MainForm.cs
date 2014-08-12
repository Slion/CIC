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
using SharpDisplayManager;

namespace SharpDisplayClient
{
    public partial class MainForm : Form
    {
        ChannelFactory<SharpDisplayManager.IDisplayService> iChannelFactory;
        SharpDisplayManager.IDisplayService iClient;

        public MainForm()
        {
            InitializeComponent();
        }

        private void buttonSetText_Click(object sender, EventArgs e)
        {
            iClient.SetText(0,"Top");
            iClient.SetText(1, "Bottom");
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

            iChannelFactory = new ChannelFactory<SharpDisplayManager.IDisplayService>(
                new NetNamedPipeBinding(),
                new EndpointAddress(
                "net.pipe://localhost/DisplayService"));

            iClient = iChannelFactory.CreateChannel();
        }
    }
}
