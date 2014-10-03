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
using SharpDisplayClient;
using SharpDisplay;

namespace SharpDisplayManager
{
    //Types declarations
    public delegate uint ColorProcessingDelegate(int aX, int aY, uint aPixel);
    public delegate int CoordinateTranslationDelegate(System.Drawing.Bitmap aBmp, int aInt);
    //Delegates are used for our thread safe method
    public delegate void AddClientDelegate(string aSessionId, ICallback aCallback);
    public delegate void RemoveClientDelegate(string aSessionId);
    public delegate void SetTextDelegate(string SessionId, TextField aTextField);
    public delegate void SetLayoutDelegate(string SessionId, TableLayout aLayout);
    public delegate void SetTextsDelegate(string SessionId, System.Collections.Generic.IList<TextField> aTextFields);
    public delegate void SetClientNameDelegate(string aSessionId, string aName);


    /// <summary>
    /// Our Display manager main form
    /// </summary>
    public partial class MainForm : Form
    {

        DateTime LastTickTime;
        Display iDisplay;
        System.Drawing.Bitmap iBmp;
        bool iCreateBitmap; //Workaround render to bitmap issues when minimized
        ServiceHost iServiceHost;
        // Our collection of clients sorted by session id.
        public Dictionary<string, ClientData> iClients;
        // The name of the client which informations are currently displayed.
        public string iCurrentClientSessionId;
        ClientData iCurrentClientData;
        //
        public bool iClosing;
        //Function pointer for pixel color filtering
        ColorProcessingDelegate iColorFx;
        //Function pointer for pixel X coordinate intercept
        CoordinateTranslationDelegate iScreenX;
        //Function pointer for pixel Y coordinate intercept
        CoordinateTranslationDelegate iScreenY;

        public MainForm()
        {
            iCurrentClientSessionId = "";
            iCurrentClientData = null;
            LastTickTime = DateTime.Now;
            iDisplay = new Display();
            iClients = new Dictionary<string, ClientData>();

            InitializeComponent();
            UpdateStatus();
            //We have a bug when drawing minimized and reusing our bitmap
            iBmp = new System.Drawing.Bitmap(tableLayoutPanel.Width, tableLayoutPanel.Height, PixelFormat.Format32bppArgb);
            iCreateBitmap = false;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            StartServer();

            if (Properties.Settings.Default.DisplayConnectOnStartup)
            {
                OpenDisplayConnection();
            }
        }

        /// <summary>
        /// Set our current client.
        /// This will take care of applying our client layout and set data fields.
        /// </summary>
        /// <param name="aSessionId"></param>
        void SetCurrentClient(string aSessionId)
        {
            if (aSessionId == iCurrentClientSessionId)
            {
                //Given client is already the current one.
                //Don't bother changing anything then.
                return;
            }

            //Set current client ID.
            iCurrentClientSessionId = aSessionId;
            //Fetch and set current client data.
            iCurrentClientData = iClients[aSessionId];
            //Apply layout and set data fields.
            UpdateTableLayoutPanel(iCurrentClientData);
        }

        private void buttonFont_Click(object sender, EventArgs e)
        {
            //fontDialog.ShowColor = true;
            //fontDialog.ShowApply = true;
            fontDialog.ShowEffects = true;
            MarqueeLabel label = (MarqueeLabel)tableLayoutPanel.Controls[0];
            fontDialog.Font = label.Font;

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
                foreach (MarqueeLabel ctrl in tableLayoutPanel.Controls)
                {
                    ctrl.Font = fontDialog.Font;
                }
                cds.Font = fontDialog.Font;
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
            //Show font height and width
            labelFontHeight.Text = "Font height: " + cds.Font.Height;
            float charWidth = IsFixedWidth(cds.Font);
            if (charWidth == 0.0f)
            {
                labelFontWidth.Visible = false;
            }
            else
            {
                labelFontWidth.Visible = true;
                labelFontWidth.Text = "Font width: " + charWidth;
            }

            //Now check font height and show a warning if needed.
            MarqueeLabel label = (MarqueeLabel)tableLayoutPanel.Controls[0];
            if (label.Font.Height > label.Height)
            {
                labelWarning.Text = "WARNING: Selected font is too height by " + (label.Font.Height - label.Height) + " pixels!";
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

            ((MarqueeLabel)tableLayoutPanel.Controls[0]).Text = "Captured";

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
                    case Display.TMiniDisplayRequest.EMiniDisplayRequestFirmwareRevision:
                        toolStripStatusLabelConnect.Text += " v" + iDisplay.FirmwareRevision();
                        //Issue next request then
                        iDisplay.RequestPowerSupplyStatus();
                        break;

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
                        //No more request to issue
                        break;
                }
            }
        }

        public static uint ColorWhiteIsOn(int aX, int aY, uint aPixel)
        {
            if ((aPixel & 0x00FFFFFF) == 0x00FFFFFF)
            {
                return 0xFFFFFFFF;
            }
            return 0x00000000;
        }

        public static uint ColorUntouched(int aX, int aY, uint aPixel)
        {
            return aPixel;
        }

        public static uint ColorInversed(int aX, int aY, uint aPixel)
        {
            return ~aPixel;
        }

        public static uint ColorChessboard(int aX, int aY, uint aPixel)
        {
            if ((aX % 2 == 0) && (aY % 2 == 0))
            {
                return ~aPixel;
            }
            else if ((aX % 2 != 0) && (aY % 2 != 0))
            {
                return ~aPixel;
            }
            return 0x00000000;
        }


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

        /// <summary>
        /// Select proper pixel delegates according to our current settings.
        /// </summary>
        private void SetupPixelDelegates()
        {
            //Select our pixel processing routine
            if (cds.InverseColors)
            {
                //iColorFx = ColorChessboard;
                iColorFx = ColorInversed;
            }
            else
            {
                iColorFx = ColorWhiteIsOn;
            }

            //Select proper coordinate translation functions
            //We used delegate/function pointer to support reverse screen without doing an extra test on each pixels
            if (cds.ReverseScreen)
            {
                iScreenX = ScreenReversedX;
                iScreenY = ScreenReversedY;
            }
            else
            {
                iScreenX = ScreenX;
                iScreenY = ScreenY;
            }

        }

        //This is our timer tick responsible to perform our render
        private void timer_Tick(object sender, EventArgs e)
        {
            //Update our animations
            DateTime NewTickTime = DateTime.Now;

            //Update animation for all our marquees
            foreach (MarqueeLabel ctrl in tableLayoutPanel.Controls)
            {
                ctrl.UpdateAnimation(LastTickTime, NewTickTime);
            }


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

                //Send it to our display
                for (int i = 0; i < iBmp.Width; i++)
                {
                    for (int j = 0; j < iBmp.Height; j++)
                    {
                        unchecked
                        {
                            //Get our processed pixel coordinates
                            int x = iScreenX(iBmp, i);
                            int y = iScreenY(iBmp, j);
                            //Get pixel color
                            uint color = (uint)iBmp.GetPixel(i, j).ToArgb();
                            //Apply color effects
                            color = iColorFx(x,y,color);
                            //Now set our pixel
                            iDisplay.SetPixel(x, y, color);
                        }
                    }
                }

                iDisplay.SwapBuffers();

            }

            //Compute instant FPS
            toolStripStatusLabelFps.Text = (1.0/NewTickTime.Subtract(LastTickTime).TotalSeconds).ToString("F0") + " / " + (1000/timer.Interval).ToString() + " FPS";

            LastTickTime = NewTickTime;

        }

        private void OpenDisplayConnection()
        {
            CloseDisplayConnection();

            if (iDisplay.Open((Display.TMiniDisplayType)cds.DisplayType))
            {
                UpdateStatus();
                iDisplay.RequestFirmwareRevision();
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
            cds.Brightness = trackBarBrightness.Value;
            Properties.Settings.Default.Save();
            iDisplay.SetBrightness(trackBarBrightness.Value);

        }


        /// <summary>
        /// CDS stands for Current Display Settings
        /// </summary>
        private DisplaySettings cds
        {
            get
            {
                DisplaysSettings settings = Properties.Settings.Default.DisplaysSettings;
                if (settings == null)
                {
                    settings = new DisplaysSettings();
                    settings.Init();
                    Properties.Settings.Default.DisplaysSettings = settings;
                }

                //Make sure all our settings have been created
                while (settings.Displays.Count <= Properties.Settings.Default.CurrentDisplayIndex)
                {
                    settings.Displays.Add(new DisplaySettings());
                }

                DisplaySettings displaySettings = settings.Displays[Properties.Settings.Default.CurrentDisplayIndex];
                return displaySettings;
            }
        }

        /// <summary>
        /// Check if the given font has a fixed character pitch.
        /// </summary>
        /// <param name="ft"></param>
        /// <returns>0.0f if this is not a monospace font, otherwise returns the character width.</returns>
        public float IsFixedWidth(Font ft)
        {
            Graphics g = CreateGraphics();
            char[] charSizes = new char[] { 'i', 'a', 'Z', '%', '#', 'a', 'B', 'l', 'm', ',', '.' };
            float charWidth = g.MeasureString("I", ft, Int32.MaxValue, StringFormat.GenericTypographic).Width;

            bool fixedWidth = true;

            foreach (char c in charSizes)
                if (g.MeasureString(c.ToString(), ft, Int32.MaxValue, StringFormat.GenericTypographic).Width != charWidth)
                    fixedWidth = false;

            if (fixedWidth)
            {
                return charWidth;
            }

            return 0.0f;
        }

        private void UpdateStatus()
        {
            //Synchronize UI with settings
            //Load settings
            checkBoxShowBorders.Checked = cds.ShowBorders;
            tableLayoutPanel.CellBorderStyle = (cds.ShowBorders ? TableLayoutPanelCellBorderStyle.Single : TableLayoutPanelCellBorderStyle.None);

            //Set the proper font to each of our labels
            foreach (MarqueeLabel ctrl in tableLayoutPanel.Controls)
            {
                ctrl.Font = cds.Font;
            }

            CheckFontHeight();
            checkBoxConnectOnStartup.Checked = Properties.Settings.Default.DisplayConnectOnStartup;
            checkBoxReverseScreen.Checked = cds.ReverseScreen;
            checkBoxInverseColors.Checked = cds.InverseColors;
            comboBoxDisplayType.SelectedIndex = cds.DisplayType;
            timer.Interval = cds.TimerInterval;
            maskedTextBoxTimerInterval.Text = cds.TimerInterval.ToString();
            //
            SetupPixelDelegates();

            if (iDisplay.IsOpen())
            {
                //Only setup brightness if display is open
                trackBarBrightness.Minimum = iDisplay.MinBrightness();
                trackBarBrightness.Maximum = iDisplay.MaxBrightness();
                trackBarBrightness.Value = cds.Brightness;
                trackBarBrightness.LargeChange = Math.Max(1, (iDisplay.MaxBrightness() - iDisplay.MinBrightness()) / 5);
                trackBarBrightness.SmallChange = 1;
                iDisplay.SetBrightness(cds.Brightness);
                //
                buttonFill.Enabled = true;
                buttonClear.Enabled = true;
                buttonOpen.Enabled = false;
                buttonClose.Enabled = true;
                trackBarBrightness.Enabled = true;
                toolStripStatusLabelConnect.Text = "Connected - " + iDisplay.Vendor() + " - " + iDisplay.Product();
                //+ " - " + iDisplay.SerialNumber();

                if (iDisplay.SupportPowerOnOff())
                {
                    buttonPowerOn.Enabled = true;
                    buttonPowerOff.Enabled = true;
                }
                else
                {
                    buttonPowerOn.Enabled = false;
                    buttonPowerOff.Enabled = false;
                }

                if (iDisplay.SupportClock())
                {
                    buttonShowClock.Enabled = true;
                    buttonHideClock.Enabled = true;
                }
                else
                {
                    buttonShowClock.Enabled = false;
                    buttonHideClock.Enabled = false;
                }
            }
            else
            {
                buttonFill.Enabled = false;
                buttonClear.Enabled = false;
                buttonOpen.Enabled = true;
                buttonClose.Enabled = false;
                trackBarBrightness.Enabled = false;
                buttonPowerOn.Enabled = false;
                buttonPowerOff.Enabled = false;
                buttonShowClock.Enabled = false;
                buttonHideClock.Enabled = false;
                toolStripStatusLabelConnect.Text = "Disconnected";
                toolStripStatusLabelPower.Text = "N/A";
            }
        }



        private void checkBoxShowBorders_CheckedChanged(object sender, EventArgs e)
        {
            //Save our show borders setting
            tableLayoutPanel.CellBorderStyle = (checkBoxShowBorders.Checked ? TableLayoutPanelCellBorderStyle.Single : TableLayoutPanelCellBorderStyle.None);
            cds.ShowBorders = checkBoxShowBorders.Checked;
            Properties.Settings.Default.Save();
            CheckFontHeight();
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
            cds.ReverseScreen = checkBoxReverseScreen.Checked;
            Properties.Settings.Default.Save();
            SetupPixelDelegates();
        }

        private void checkBoxInverseColors_CheckedChanged(object sender, EventArgs e)
        {
            //Save our inverse colors setting
            cds.InverseColors = checkBoxInverseColors.Checked;
            Properties.Settings.Default.Save();
            SetupPixelDelegates();
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
                    typeof(Session),
                    new Uri[] { new Uri("net.tcp://localhost:8001/") }
                );

            iServiceHost.AddServiceEndpoint(typeof(IService), new NetTcpBinding(SecurityMode.None, true), "DisplayService");
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
            LastTickTime = DateTime.Now; //Reset timer to prevent jump
            timer.Enabled = !timer.Enabled;
            if (!timer.Enabled)
            {
                buttonSuspend.Text = "Run";
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


        /// <summary>
        ///
        /// </summary>
        /// <param name="aSessionId"></param>
        /// <param name="aCallback"></param>
        public void AddClientThreadSafe(string aSessionId, ICallback aCallback)
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
        /// <param name="aSessionId"></param>
        /// <param name="aTextField"></param>
        public void SetClientLayoutThreadSafe(string aSessionId, TableLayout aLayout)
        {
            if (this.InvokeRequired)
            {
                //Not in the proper thread, invoke ourselves
                SetLayoutDelegate d = new SetLayoutDelegate(SetClientLayoutThreadSafe);
                this.Invoke(d, new object[] { aSessionId, aLayout });
            }
            else
            {
                ClientData client = iClients[aSessionId];
                if (client != null)
                {
                    client.Layout = aLayout;
                    UpdateTableLayoutPanel(client);
                    //
                    UpdateClientTreeViewNode(client);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="aLineIndex"></param>
        /// <param name="aText"></param>
        public void SetClientTextThreadSafe(string aSessionId, TextField aTextField)
        {
            if (this.InvokeRequired)
            {
                //Not in the proper thread, invoke ourselves
                SetTextDelegate d = new SetTextDelegate(SetClientTextThreadSafe);
                this.Invoke(d, new object[] { aSessionId, aTextField });
            }
            else
            {
                SetCurrentClient(aSessionId);
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
                    MarqueeLabel label = (MarqueeLabel)tableLayoutPanel.Controls[aTextField.Index];
                    label.Text = aTextField.Text;
                    label.TextAlign = aTextField.Alignment;
                    //
                    UpdateClientTreeViewNode(client);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="aTexts"></param>
        public void SetClientTextsThreadSafe(string aSessionId, System.Collections.Generic.IList<TextField> aTextFields)
        {
            if (this.InvokeRequired)
            {
                //Not in the proper thread, invoke ourselves
                SetTextsDelegate d = new SetTextsDelegate(SetClientTextsThreadSafe);
                this.Invoke(d, new object[] { aSessionId, aTextFields });
            }
            else
            {
                SetCurrentClient(aSessionId);
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
                    //Put each our text fields in a label control
                    for (int i = 0; i < aTextFields.Count; i++)
                    {
                        MarqueeLabel label = (MarqueeLabel)tableLayoutPanel.Controls[aTextFields[i].Index];
                        label.Text = aTextFields[i].Text;
                        label.TextAlign = aTextFields[i].Alignment;
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
                UpdateTableLayoutPanel(tableLayoutPanel.ColumnCount, tableLayoutPanel.RowCount + 1);
            }
        }

        private void buttonRemoveRow_Click(object sender, EventArgs e)
        {
            if (tableLayoutPanel.RowCount > 1)
            {
                UpdateTableLayoutPanel(tableLayoutPanel.ColumnCount, tableLayoutPanel.RowCount - 1);
            }

            UpdateTableLayoutRowStyles();
        }

        private void buttonAddColumn_Click(object sender, EventArgs e)
        {
            if (tableLayoutPanel.ColumnCount < 8)
            {
                UpdateTableLayoutPanel(tableLayoutPanel.ColumnCount + 1, tableLayoutPanel.RowCount);
            }
        }

        private void buttonRemoveColumn_Click(object sender, EventArgs e)
        {
            if (tableLayoutPanel.ColumnCount > 1)
            {
                UpdateTableLayoutPanel(tableLayoutPanel.ColumnCount - 1, tableLayoutPanel.RowCount);
            }
        }


        /// <summary>
        /// Update our table layout row styles to make sure each rows have similar height
        /// </summary>
        private void UpdateTableLayoutRowStyles()
        {
            foreach (RowStyle rowStyle in tableLayoutPanel.RowStyles)
            {
                rowStyle.SizeType = SizeType.Percent;
                rowStyle.Height = 100 / tableLayoutPanel.RowCount;
            }
        }

        /// <summary>
        /// Empty and recreate our table layout with the given number of columns and rows.
        /// Sizes of rows and columns are uniform.
        /// </summary>
        /// <param name="aColumn"></param>
        /// <param name="aRow"></param>
        private void UpdateTableLayoutPanel(int aColumn, int aRow)
        {
            tableLayoutPanel.Controls.Clear();
            tableLayoutPanel.RowStyles.Clear();
            tableLayoutPanel.ColumnStyles.Clear();
            tableLayoutPanel.RowCount = 0;
            tableLayoutPanel.ColumnCount = 0;

            while (tableLayoutPanel.RowCount < aRow)
            {
                tableLayoutPanel.RowCount++;
            }

            while (tableLayoutPanel.ColumnCount < aColumn)
            {
                tableLayoutPanel.ColumnCount++;
            }

            for (int i = 0; i < tableLayoutPanel.ColumnCount; i++)
            {
                //Create our column styles
                this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100 / tableLayoutPanel.ColumnCount));

                for (int j = 0; j < tableLayoutPanel.RowCount; j++)
                {
                    if (i == 0)
                    {
                        //Create our row styles
                        this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100 / tableLayoutPanel.RowCount));
                    }

                    MarqueeLabel control = new SharpDisplayManager.MarqueeLabel();
                    control.AutoEllipsis = true;
                    control.AutoSize = true;
                    control.BackColor = System.Drawing.Color.Transparent;
                    control.Dock = System.Windows.Forms.DockStyle.Fill;
                    control.Location = new System.Drawing.Point(1, 1);
                    control.Margin = new System.Windows.Forms.Padding(0);
                    control.Name = "marqueeLabelCol" + aColumn + "Row" + aRow;
                    control.OwnTimer = false;
                    control.PixelsPerSecond = 64;
                    control.Separator = "|";
                    //control.Size = new System.Drawing.Size(254, 30);
                    //control.TabIndex = 2;
                    control.Font = cds.Font;
                    control.Text = "ABCDEFGHIJKLMNOPQRST-0123456789";
                    control.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                    control.UseCompatibleTextRendering = true;
                    //
                    tableLayoutPanel.Controls.Add(control, i, j);
                }
            }

            CheckFontHeight();
        }


        /// <summary>
        /// Update our display table layout.
        /// </summary>
        /// <param name="aLayout"></param>
        private void UpdateTableLayoutPanel(ClientData aClient)
        {
            TableLayout layout = aClient.Layout;
            
            tableLayoutPanel.Controls.Clear();
            tableLayoutPanel.RowStyles.Clear();
            tableLayoutPanel.ColumnStyles.Clear();
            tableLayoutPanel.RowCount = 0;
            tableLayoutPanel.ColumnCount = 0;

            while (tableLayoutPanel.RowCount < layout.Rows.Count)
            {
                tableLayoutPanel.RowCount++;
            }

            while (tableLayoutPanel.ColumnCount < layout.Columns.Count)
            {
                tableLayoutPanel.ColumnCount++;
            }

            for (int i = 0; i < tableLayoutPanel.ColumnCount; i++)
            {
                //Create our column styles
                this.tableLayoutPanel.ColumnStyles.Add(layout.Columns[i]);

                for (int j = 0; j < tableLayoutPanel.RowCount; j++)
                {
                    if (i == 0)
                    {
                        //Create our row styles
                        this.tableLayoutPanel.RowStyles.Add(layout.Rows[j]);
                    }

                    MarqueeLabel control = new SharpDisplayManager.MarqueeLabel();
                    control.AutoEllipsis = true;
                    control.AutoSize = true;
                    control.BackColor = System.Drawing.Color.Transparent;
                    control.Dock = System.Windows.Forms.DockStyle.Fill;
                    control.Location = new System.Drawing.Point(1, 1);
                    control.Margin = new System.Windows.Forms.Padding(0);
                    control.Name = "marqueeLabelCol" + layout.Columns.Count + "Row" + layout.Rows.Count;
                    control.OwnTimer = false;
                    control.PixelsPerSecond = 64;
                    control.Separator = "|";
                    //control.Size = new System.Drawing.Size(254, 30);
                    //control.TabIndex = 2;
                    control.Font = cds.Font;
                    control.Text = "";
                    //If we already have a text for that field
                    if (aClient.Texts.Count > tableLayoutPanel.Controls.Count)
                    {
                        control.Text = aClient.Texts[tableLayoutPanel.Controls.Count].Text;
                    }
                    
                    control.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                    control.UseCompatibleTextRendering = true;
                    //
                    tableLayoutPanel.Controls.Add(control, i, j);
                }
            }

            CheckFontHeight();
        }


        private void buttonAlignLeft_Click(object sender, EventArgs e)
        {
            foreach (MarqueeLabel ctrl in tableLayoutPanel.Controls)
            {
                ctrl.TextAlign = ContentAlignment.MiddleLeft;
            }
        }

        private void buttonAlignCenter_Click(object sender, EventArgs e)
        {
            foreach (MarqueeLabel ctrl in tableLayoutPanel.Controls)
            {
                ctrl.TextAlign = ContentAlignment.MiddleCenter;
            }
        }

        private void buttonAlignRight_Click(object sender, EventArgs e)
        {
            foreach (MarqueeLabel ctrl in tableLayoutPanel.Controls)
            {
                ctrl.TextAlign = ContentAlignment.MiddleRight;
            }
        }

        private void comboBoxDisplayType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.CurrentDisplayIndex = comboBoxDisplayType.SelectedIndex;
            cds.DisplayType = comboBoxDisplayType.SelectedIndex;
            Properties.Settings.Default.Save();
            if (iDisplay.IsOpen())
            {
                OpenDisplayConnection();
            }
            else
            {
                UpdateStatus();
            }
        }


        private void maskedTextBoxTimerInterval_TextChanged(object sender, EventArgs e)
        {
            if (maskedTextBoxTimerInterval.Text != "")
            {
                int interval = Convert.ToInt32(maskedTextBoxTimerInterval.Text);

                if (interval > 0)
                {
                    timer.Interval = interval;
                    cds.TimerInterval = timer.Interval;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private void buttonPowerOn_Click(object sender, EventArgs e)
        {
            iDisplay.PowerOn();
        }

        private void buttonPowerOff_Click(object sender, EventArgs e)
        {
            iDisplay.PowerOff();
        }

        private void buttonShowClock_Click(object sender, EventArgs e)
        {
            iDisplay.ShowClock();
        }

        private void buttonHideClock_Click(object sender, EventArgs e)
        {
            iDisplay.HideClock();
        }
    }

    /// <summary>
    /// A UI thread copy of a client relevant data.
    /// Keeping this copy in the UI thread helps us deal with threading issues.
    /// </summary>
    public class ClientData
    {
        public ClientData(string aSessionId, ICallback aCallback)
        {
            SessionId = aSessionId;
            Name = "";
            Texts = new List<TextField>();
            Layout = new TableLayout(1, 2); //Default to one column and two rows
            Callback = aCallback;
        }

        public string SessionId { get; set; }
        public string Name { get; set; }
        public List<TextField> Texts { get; set; }
        public TableLayout Layout { get; set; }
        public ICallback Callback { get; set; }
    }
}
