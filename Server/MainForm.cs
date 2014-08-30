using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using CodeProject.Dialog;
using System.Drawing.Imaging;
using System.ServiceModel;
using System.Threading;
using System.Diagnostics;
//
using SharpDisplayInterface;
using SharpDisplayClient;


namespace SharpDisplayManager
{
    public partial class MainForm : Form
    {
        DateTime LastTickTime;
        Display iDisplay;
        System.Drawing.Bitmap iBmp;
        bool iCreateBitmap; //Workaround render to bitmap issues when minimized
        ServiceHost iServiceHost;
        /// <summary>
        /// Our collection of clients
        /// </summary>
        public Dictionary<string, ClientData> iClients;
        public bool iClosing;

        public MainForm()
        {
            LastTickTime = DateTime.Now;
            iDisplay = new Display();
            iClients = new Dictionary<string, ClientData>();

            InitializeComponent();
            UpdateStatus();

            //Load settings
            marqueeLabelTop.Font = Properties.Settings.Default.DisplayFont;
            marqueeLabelBottom.Font = Properties.Settings.Default.DisplayFont;
            checkBoxShowBorders.Checked = Properties.Settings.Default.DisplayShowBorders;
            checkBoxConnectOnStartup.Checked = Properties.Settings.Default.DisplayConnectOnStartup;
            checkBoxReverseScreen.Checked = Properties.Settings.Default.DisplayReverseScreen;
            comboBoxDisplayType.SelectedIndex = Properties.Settings.Default.DisplayType;
            //
            tableLayoutPanel.CellBorderStyle = (checkBoxShowBorders.Checked ? TableLayoutPanelCellBorderStyle.Single : TableLayoutPanelCellBorderStyle.None);
            //We have a bug when drawing minimized and reusing our bitmap
            iBmp = new System.Drawing.Bitmap(tableLayoutPanel.Width, tableLayoutPanel.Height, PixelFormat.Format32bppArgb);
            iCreateBitmap = false;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            StartServer();

            //
            CheckFontHeight();
            //


            if (Properties.Settings.Default.DisplayConnectOnStartup)
            {
                OpenDisplayConnection();
            }
        }


        private void buttonFont_Click(object sender, EventArgs e)
        {
            //fontDialog.ShowColor = true;
            //fontDialog.ShowApply = true;
            fontDialog.ShowEffects = true;
            fontDialog.Font = marqueeLabelTop.Font;

            fontDialog.FixedPitchOnly = checkBoxFixedPitchFontOnly.Checked;

            //fontDialog.ShowHelp = true;

            //fontDlg.MaxSize = 40;
            //fontDlg.MinSize = 22;

            //fontDialog.Parent = this;
            //fontDialog.StartPosition = FormStartPosition.CenterParent;

            //DlgBox.ShowDialog(fontDialog);

            //if (fontDialog.ShowDialog(this) != DialogResult.Cancel)
            if (DlgBox.ShowDialog(fontDialog) != DialogResult.Cancel)
            {

                //MsgBox.Show("MessageBox MsgBox", "MsgBox caption");

                //MessageBox.Show("Ok");
                marqueeLabelTop.Font = fontDialog.Font;
                marqueeLabelBottom.Font = fontDialog.Font;
                Properties.Settings.Default.DisplayFont = fontDialog.Font;
                Properties.Settings.Default.Save();
                //
                CheckFontHeight();
            }
        }

        /// <summary>
        ///
        /// </summary>
        void CheckFontHeight()
        {
            if (marqueeLabelBottom.Font.Height > marqueeLabelBottom.Height)
            {
                labelWarning.Text = "WARNING: Selected font is too height by " + (marqueeLabelBottom.Font.Height - marqueeLabelBottom.Height) + " pixels!";
                labelWarning.Visible = true;
            }
            else
            {
                labelWarning.Visible = false;
            }

        }

        private void buttonCapture_Click(object sender, EventArgs e)
        {
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(tableLayoutPanel.Width, tableLayoutPanel.Height);
            tableLayoutPanel.DrawToBitmap(bmp, tableLayoutPanel.ClientRectangle);
            //Bitmap bmpToSave = new Bitmap(bmp);
            bmp.Save("D:\\capture.png");

            marqueeLabelTop.Text = "Sweet";

            /*
            string outputFileName = "d:\\capture.png";
            using (MemoryStream memory = new MemoryStream())
            {
                using (FileStream fs = new FileStream(outputFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    bmp.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] bytes = memory.ToArray();
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
             */

        }

        private void CheckForRequestResults()
        {
            if (iDisplay.IsRequestPending())
            {
                switch (iDisplay.AttemptRequestCompletion())
                {
                    case Display.TMiniDisplayRequest.EMiniDisplayRequestPowerSupplyStatus:
                        if (iDisplay.PowerSupplyStatus())
                        {
                            toolStripStatusLabelPower.Text = "ON";
                        }
                        else
                        {
                            toolStripStatusLabelPower.Text = "OFF";
                        }
                        //Issue next request then
                        iDisplay.RequestDeviceId();
                        break;

                    case Display.TMiniDisplayRequest.EMiniDisplayRequestDeviceId:
                        toolStripStatusLabelConnect.Text += " - " + iDisplay.DeviceId();
                        //Issue next request then
                        iDisplay.RequestFirmwareRevision();
                        break;

                    case Display.TMiniDisplayRequest.EMiniDisplayRequestFirmwareRevision:
                        toolStripStatusLabelConnect.Text += " v" + iDisplay.FirmwareRevision();
                        //No more request to issue
                        break;
                }
            }
        }


        public delegate int CoordinateTranslationDelegate(System.Drawing.Bitmap aBmp, int aInt);


        public static int ScreenReversedX(System.Drawing.Bitmap aBmp, int aX)
        {
            return aBmp.Width - aX - 1;
        }

        public int ScreenReversedY(System.Drawing.Bitmap aBmp, int aY)
        {
            return iBmp.Height - aY - 1;
        }

        public int ScreenX(System.Drawing.Bitmap aBmp, int aX)
        {
            return aX;
        }

        public int ScreenY(System.Drawing.Bitmap aBmp, int aY)
        {
            return aY;
        }


        //This is our timer tick responsible to perform our render
        private void timer_Tick(object sender, EventArgs e)
        {
            //Update our animations
            DateTime NewTickTime = DateTime.Now;

            marqueeLabelTop.UpdateAnimation(LastTickTime, NewTickTime);
            marqueeLabelBottom.UpdateAnimation(LastTickTime, NewTickTime);

            //Update our display
            if (iDisplay.IsOpen())
            {
                CheckForRequestResults();

                //Draw to bitmap
                if (iCreateBitmap)
                {
                    iBmp = new System.Drawing.Bitmap(tableLayoutPanel.Width, tableLayoutPanel.Height, PixelFormat.Format32bppArgb);
                }
                tableLayoutPanel.DrawToBitmap(iBmp, tableLayoutPanel.ClientRectangle);
                //iBmp.Save("D:\\capture.png");

                //Select proper coordinate translation functions
                //We used delegate/function pointer to support reverse screen without doing an extra test on each pixels
                CoordinateTranslationDelegate screenX;
                CoordinateTranslationDelegate screenY;

                if (Properties.Settings.Default.DisplayReverseScreen)
                {
                    screenX = ScreenReversedX;
                    screenY = ScreenReversedY;
                }
                else
                {
                    screenX = ScreenX;
                    screenY = ScreenY;
                }

                //Send it to our display
                for (int i = 0; i < iBmp.Width; i++)
                {
                    for (int j = 0; j < iBmp.Height; j++)
                    {
                        unchecked
                        {
                            uint color = (uint)iBmp.GetPixel(i, j).ToArgb();
                            //For some reason when the app is minimized in the task bar only the alpha of our color is set.
                            //Thus that strange test for rendering to work both when the app is in the task bar and when it isn't.
                            iDisplay.SetPixel(screenX(iBmp, i), screenY(iBmp, j), Convert.ToInt32(!(color != 0xFF000000)));
                        }
                    }
                }

                iDisplay.SwapBuffers();

            }

            //Compute instant FPS
            toolStripStatusLabelFps.Text = (1.0/NewTickTime.Subtract(LastTickTime).TotalSeconds).ToString("F0") + " FPS";

            LastTickTime = NewTickTime;

        }

        private void OpenDisplayConnection()
        {
            CloseDisplayConnection();

            if (iDisplay.Open((Display.TMiniDisplayType)Properties.Settings.Default.DisplayType))
            {
                UpdateStatus();
                iDisplay.RequestPowerSupplyStatus();
            }
            else
            {
                UpdateStatus();
                toolStripStatusLabelConnect.Text = "Connection error";
            }
        }

        private void CloseDisplayConnection()
        {
            iDisplay.Close();
            UpdateStatus();
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            OpenDisplayConnection();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            CloseDisplayConnection();
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            iDisplay.Clear();
            iDisplay.SwapBuffers();
        }

        private void buttonFill_Click(object sender, EventArgs e)
        {
            iDisplay.Fill();
            iDisplay.SwapBuffers();
        }

        private void trackBarBrightness_Scroll(object sender, EventArgs e)
        {
            Properties.Settings.Default.DisplayBrightness = trackBarBrightness.Value;
            Properties.Settings.Default.Save();
            iDisplay.SetBrightness(trackBarBrightness.Value);

        }

        private void UpdateStatus()
        {
            if (iDisplay.IsOpen())
            {
                buttonFill.Enabled = true;
                buttonClear.Enabled = true;
                buttonOpen.Enabled = false;
                buttonClose.Enabled = true;
                trackBarBrightness.Enabled = true;
                trackBarBrightness.Minimum = iDisplay.MinBrightness();
                trackBarBrightness.Maximum = iDisplay.MaxBrightness();
                trackBarBrightness.Value = Properties.Settings.Default.DisplayBrightness;
                trackBarBrightness.LargeChange = Math.Max(1,(iDisplay.MaxBrightness() - iDisplay.MinBrightness())/5);
                trackBarBrightness.SmallChange = 1;
                iDisplay.SetBrightness(Properties.Settings.Default.DisplayBrightness);

                toolStripStatusLabelConnect.Text = "Connected - " + iDisplay.Vendor() + " - " + iDisplay.Product();
                //+ " - " + iDisplay.SerialNumber();
            }
            else
            {
                buttonFill.Enabled = false;
                buttonClear.Enabled = false;
                buttonOpen.Enabled = true;
                buttonClose.Enabled = false;
                trackBarBrightness.Enabled = false;
                toolStripStatusLabelConnect.Text = "Disconnected";
            }
        }



        private void checkBoxShowBorders_CheckedChanged(object sender, EventArgs e)
        {
            //Save our show borders setting
            tableLayoutPanel.CellBorderStyle = (checkBoxShowBorders.Checked ? TableLayoutPanelCellBorderStyle.Single : TableLayoutPanelCellBorderStyle.None);
            Properties.Settings.Default.DisplayShowBorders = checkBoxShowBorders.Checked;
            Properties.Settings.Default.Save();
        }

        private void checkBoxConnectOnStartup_CheckedChanged(object sender, EventArgs e)
        {
            //Save our connect on startup setting
            Properties.Settings.Default.DisplayConnectOnStartup = checkBoxConnectOnStartup.Checked;
            Properties.Settings.Default.Save();
        }

        private void checkBoxReverseScreen_CheckedChanged(object sender, EventArgs e)
        {
            //Save our reverse screen setting
            Properties.Settings.Default.DisplayReverseScreen = checkBoxReverseScreen.Checked;
            Properties.Settings.Default.Save();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                // Do some stuff
                //iBmp = new System.Drawing.Bitmap(tableLayoutPanel.Width, tableLayoutPanel.Height, PixelFormat.Format32bppArgb);
                iCreateBitmap = true;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopServer();
            e.Cancel = iClosing;
        }

        public void StartServer()
        {
            iServiceHost = new ServiceHost
                (
                    typeof(DisplayServer),
                    new Uri[] { new Uri("net.tcp://localhost:8001/") }
                );

            iServiceHost.AddServiceEndpoint(typeof(IDisplayService), new NetTcpBinding(SecurityMode.None,true), "DisplayService");
            iServiceHost.Open();
        }

        public void StopServer()
        {
            if (iClients.Count > 0 && !iClosing)
            {
                //Tell our clients
                iClosing = true;
                BroadcastCloseEvent();
            }
            else if (iClosing)
            {
                if (MessageBox.Show("Force exit?", "Waiting for clients...", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    iClosing = false; //We make sure we force close if asked twice
                }
            }
            else
            {
                //We removed that as it often lags for some reason
                //iServiceHost.Close();
            }
        }

        public void BroadcastCloseEvent()
        {
            Trace.TraceInformation("BroadcastCloseEvent - start");

            var inactiveClients = new List<string>();
            foreach (var client in iClients)
            {
                //if (client.Key != eventData.ClientName)
                {
                    try
                    {
                        Trace.TraceInformation("BroadcastCloseEvent - " + client.Key);
                        client.Value.Callback.OnCloseOrder(/*eventData*/);
                    }
                    catch (Exception ex)
                    {
                        inactiveClients.Add(client.Key);
                    }
                }
            }

            if (inactiveClients.Count > 0)
            {
                foreach (var client in inactiveClients)
                {
                    iClients.Remove(client);
                    Program.iMainForm.treeViewClients.Nodes.Remove(Program.iMainForm.treeViewClients.Nodes.Find(client, false)[0]);
                }
            }
        }

        private void buttonStartClient_Click(object sender, EventArgs e)
        {
            Thread clientThread = new Thread(SharpDisplayClient.Program.Main);
            clientThread.Start();
            BringToFront();
        }

        private void buttonSuspend_Click(object sender, EventArgs e)
        {
            timer.Enabled = !timer.Enabled;
            if (!timer.Enabled)
            {
                buttonSuspend.Text = "Suspend";
            }
            else
            {
                buttonSuspend.Text = "Pause";
            }
        }

        private void buttonCloseClients_Click(object sender, EventArgs e)
        {
            BroadcastCloseEvent();
        }

        private void treeViewClients_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        //Delegates are used for our thread safe method
        public delegate void AddClientDelegate(string aSessionId, IDisplayServiceCallback aCallback);
        public delegate void RemoveClientDelegate(string aSessionId);
        public delegate void SetTextDelegate(string SessionId, TextField aTextField);
        public delegate void SetTextsDelegate(string SessionId, System.Collections.Generic.IList<TextField> aTextFields);
        public delegate void SetClientNameDelegate(string aSessionId, string aName);


        /// <summary>
        ///
        /// </summary>
        /// <param name="aSessionId"></param>
        /// <param name="aCallback"></param>
        public void AddClientThreadSafe(string aSessionId, IDisplayServiceCallback aCallback)
        {
            if (this.InvokeRequired)
            {
                //Not in the proper thread, invoke ourselves
                AddClientDelegate d = new AddClientDelegate(AddClientThreadSafe);
                this.Invoke(d, new object[] { aSessionId, aCallback });
            }
            else
            {
                //We are in the proper thread
                //Add this session to our collection of clients
                ClientData newClient = new ClientData(aSessionId, aCallback);
                Program.iMainForm.iClients.Add(aSessionId, newClient);
                //Add this session to our UI
                UpdateClientTreeViewNode(newClient);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="aSessionId"></param>
        public void RemoveClientThreadSafe(string aSessionId)
        {
            if (this.InvokeRequired)
            {
                //Not in the proper thread, invoke ourselves
                RemoveClientDelegate d = new RemoveClientDelegate(RemoveClientThreadSafe);
                this.Invoke(d, new object[] { aSessionId });
            }
            else
            {
                //We are in the proper thread
                //Remove this session from both client collection and UI tree view
                if (Program.iMainForm.iClients.Keys.Contains(aSessionId))
                {
                    Program.iMainForm.iClients.Remove(aSessionId);
                    Program.iMainForm.treeViewClients.Nodes.Remove(Program.iMainForm.treeViewClients.Nodes.Find(aSessionId, false)[0]);
                }

                if (iClosing && iClients.Count == 0)
                {
                    //We were closing our form
                    //All clients are now closed
                    //Just resume our close operation
                    iClosing = false;
                    Close();
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="aLineIndex"></param>
        /// <param name="aText"></param>
        public void SetTextThreadSafe(string aSessionId, TextField aTextField)
        {
            if (this.InvokeRequired)
            {
                //Not in the proper thread, invoke ourselves
                SetTextDelegate d = new SetTextDelegate(SetTextThreadSafe);
                this.Invoke(d, new object[] { aSessionId, aTextField });
            }
            else
            {
                ClientData client = iClients[aSessionId];
                if (client != null)
                {
                    //Make sure all our texts are in place
                    while (client.Texts.Count < (aTextField.Index + 1))
                    {
                        //Add a text field with proper index
                        client.Texts.Add(new TextField(client.Texts.Count));
                    }
                    client.Texts[aTextField.Index] = aTextField;

                    //We are in the proper thread
                    //Only support two lines for now
                    if (aTextField.Index == 0)
                    {
                        marqueeLabelTop.Text = aTextField.Text;
                        marqueeLabelTop.TextAlign = aTextField.Alignment;
                    }
                    else if (aTextField.Index == 1)
                    {
                        marqueeLabelBottom.Text = aTextField.Text;
                        marqueeLabelBottom.TextAlign = aTextField.Alignment;
                    }


                    UpdateClientTreeViewNode(client);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="aTexts"></param>
        public void SetTextsThreadSafe(string aSessionId, System.Collections.Generic.IList<TextField> aTextFields)
        {
            if (this.InvokeRequired)
            {
                //Not in the proper thread, invoke ourselves
                SetTextsDelegate d = new SetTextsDelegate(SetTextsThreadSafe);
                this.Invoke(d, new object[] { aSessionId, aTextFields });
            }
            else
            {
                //We are in the proper thread
                ClientData client = iClients[aSessionId];
                if (client != null)
                {
                    //Populate our client with the given text fields
                    int j = 0;
                    foreach (TextField textField in aTextFields)
                    {
                        if (client.Texts.Count < (j + 1))
                        {
                            client.Texts.Add(textField);
                        }
                        else
                        {
                            client.Texts[j] = textField;
                        }
                        j++;
                    }                    
                    //Only support two lines for now
                    for (int i = 0; i < aTextFields.Count; i++)
                    {
                        if (aTextFields[i].Index == 0)
                        {
                            marqueeLabelTop.Text = aTextFields[i].Text;
                            marqueeLabelTop.TextAlign = aTextFields[i].Alignment;
                        }
                        else if (aTextFields[i].Index == 1)
                        {
                            marqueeLabelBottom.Text = aTextFields[i].Text;
                            marqueeLabelBottom.TextAlign = aTextFields[i].Alignment;
                        }
                    }


                    UpdateClientTreeViewNode(client);
                }
            }
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="aSessionId"></param>
        /// <param name="aName"></param>
        public void SetClientNameThreadSafe(string aSessionId, string aName)
        {
            if (this.InvokeRequired)
            {
                //Not in the proper thread, invoke ourselves
                SetClientNameDelegate d = new SetClientNameDelegate(SetClientNameThreadSafe);
                this.Invoke(d, new object[] { aSessionId, aName });
            }
            else
            {
                //We are in the proper thread
                //Get our client
                ClientData client = iClients[aSessionId];
                if (client != null)
                {
                    //Set its name
                    client.Name = aName;
                    //Update our tree-view
                    UpdateClientTreeViewNode(client);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="aClient"></param>
        private void UpdateClientTreeViewNode(ClientData aClient)
        {
            if (aClient == null)
            {
                return;
            }

            TreeNode node = null;
            //Check that our client node already exists
            //Get our client root node using its key which is our session ID
            TreeNode[] nodes = treeViewClients.Nodes.Find(aClient.SessionId, false);
            if (nodes.Count()>0)
            {
                //We already have a node for that client
                node = nodes[0];
                //Clear children as we are going to recreate them below
                node.Nodes.Clear();
            }
            else
            {
                //Client node does not exists create a new one
                treeViewClients.Nodes.Add(aClient.SessionId, aClient.SessionId);
                node = treeViewClients.Nodes.Find(aClient.SessionId, false)[0];
            }

            if (node != null)
            {
                //Change its name
                if (aClient.Name != "")
                {
                    //We have a name, us it as text for our root node
                    node.Text = aClient.Name;
                    //Add a child with SessionId
                    node.Nodes.Add(new TreeNode(aClient.SessionId));
                }
                else
                {
                    //No name, use session ID instead
                    node.Text = aClient.SessionId;
                }

                if (aClient.Texts.Count > 0)
                {
                    //Create root node for our texts
                    TreeNode textsRoot = new TreeNode("Text");
                    node.Nodes.Add(textsRoot);
                    //For each text add a new entry
                    foreach (TextField field in aClient.Texts)
                    {
                        textsRoot.Nodes.Add(new TreeNode(field.Text));
                    }
                }

                node.ExpandAll();
            }
        }

        private void buttonAddRow_Click(object sender, EventArgs e)
        {
            if (tableLayoutPanel.RowCount < 6)
            {
                tableLayoutPanel.RowCount++;
                CheckFontHeight();
            }
        }

        private void buttonRemoveRow_Click(object sender, EventArgs e)
        {
            if (tableLayoutPanel.RowCount > 1)
            {
                tableLayoutPanel.RowCount--;
                CheckFontHeight();
            }
        }

        private void buttonAddColumn_Click(object sender, EventArgs e)
        {
            if (tableLayoutPanel.ColumnCount < 8)
            {
                tableLayoutPanel.ColumnCount++;
                //CheckFontHeight();
            }
        }

        private void buttonRemoveColumn_Click(object sender, EventArgs e)
        {
            if (tableLayoutPanel.ColumnCount > 1)
            {
                tableLayoutPanel.ColumnCount--;
                //CheckFontHeight();
            }
        }

        private void buttonAlignLeft_Click(object sender, EventArgs e)
        {
            marqueeLabelTop.TextAlign = ContentAlignment.MiddleLeft;
            marqueeLabelBottom.TextAlign = ContentAlignment.MiddleLeft;
        }

        private void buttonAlignCenter_Click(object sender, EventArgs e)
        {
            marqueeLabelTop.TextAlign = ContentAlignment.MiddleCenter;
            marqueeLabelBottom.TextAlign = ContentAlignment.MiddleCenter;
        }

        private void buttonAlignRight_Click(object sender, EventArgs e)
        {
            marqueeLabelTop.TextAlign = ContentAlignment.MiddleRight;
            marqueeLabelBottom.TextAlign = ContentAlignment.MiddleRight;
        }

        private void comboBoxDisplayType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.DisplayType = comboBoxDisplayType.SelectedIndex;
            Properties.Settings.Default.Save();
            OpenDisplayConnection();
        }

    }

    /// <summary>
    /// A UI thread copy of a client relevant data.
    /// Keeping this copy in the UI thread helps us deal with threading issues.
    /// </summary>
    public class ClientData
    {
        public ClientData(string aSessionId, IDisplayServiceCallback aCallback)
        {
            SessionId = aSessionId;
            Name = "";
            Texts = new List<TextField>();
            Callback = aCallback;
        }

        public string SessionId { get; set; }
        public string Name { get; set; }
        public List<TextField> Texts { get; set; }
        public IDisplayServiceCallback Callback { get; set; }
    }
}
