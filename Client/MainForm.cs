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
        ClientOutput iClientOutput;
        InstanceContext iInstanceContext;
        ClientInput iClientInput;

        public MainForm()
        {
            InitializeComponent();
        }

        private void buttonSetText_Click(object sender, EventArgs e)
        {
            //iClient.SetText(0,"Top");
            //iClient.SetText(1, "Bottom");
            iClientOutput.SetTexts(new string[] { "Top", "Bottom" });
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            iClientInput = new ClientInput();
            iInstanceContext = new InstanceContext(iClientInput);
            iClientOutput = new ClientOutput(iInstanceContext);

            iClientOutput.Connect("TestClient");

        }

        public void CloseConnection()
        {
            //If we close the instance context after the client output it hangs
            iInstanceContext.Close();
            iInstanceContext = null;
            iClientOutput.Close();
            iClientOutput = null;
            iClientInput = null;
        }
    }
}
