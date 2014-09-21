﻿using System;
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
            //We have a bug when drawing minimized and reusing our bitmap
            iBmp = new System.Drawing.Bitmap(tableLayoutPanel.Width, tableLayoutPanel.Height, PixelFormat.Format32bppArgb);
            iCreateBitmap = false;
            //
            //this.tableLayoutPanel.CellPaint += new TableLayoutCellPaintEventHandler(tableLayoutPanel_CellPaint);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            StartServer();

            if (Properties.Settings.Default.DisplayConnectOnStartup)
            {
                OpenDisplayConnection();
            }
        }

        //Testing that stuff
        private void tableLayoutPanel_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            var panel = sender as TableLayoutPanel;
            //e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            var rectangle = e.CellBounds;
            using (var pen = new Pen(Color.Black, 1))
            {
                pen.Alignment = System.Drawing.Drawing2D.PenAlignment.Center;
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;

                if (e.Row == (panel.RowCount - 1))
                {
                    rectangle.Height -= 1;
                }

                if (e.Column == (panel.ColumnCount - 1))
                {
                    rectangle.Width -= 1;
                }

                e.Graphics.DrawRectangle(pen, rectangle);
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


        public delegate uint ColorProcessingDelegate(uint aPixel);

        public static uint ColorUntouched(uint aPixel)
        {
            return aPixel;
        }

        public static uint ColorInversed(uint aPixel)
        {
            return ~aPixel;
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


                //Select our pixel processing routine
                ColorProcessingDelegate colorFx;

                if (cds.InverseColors)
                {
                    colorFx = ColorInversed;
                }
                else
                {
                    colorFx = ColorUntouched;
                }

                //Select proper coordinate translation functions
                //We used delegate/function pointer to support reverse screen without doing an extra test on each pixels
                CoordinateTranslationDelegate screenX;
                CoordinateTranslationDelegate screenY;

                if (cds.ReverseScreen)
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
                            //Apply color effects
                            color = colorFx(color);
                            //For some reason when the app is minimized in the task bar only the alpha of our color is set.
                            //Thus that strange test for rendering to work both when the app is in the task bar and when it isn't.
                            //iDisplay.SetPixel(screenX(iBmp, i), screenY(iBmp, j), Convert.ToInt32(!(color != 0xFF000000)));
                            iDisplay.SetPixel(screenX(iBmp, i), screenY(iBmp, j), color);
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
                DisplaysSettings settings = Properties.Settings.Default.DisplaySettings;
                if (settings == null)
                {
                    settings = new DisplaysSettings();
                    settings.Init();
                    Properties.Settings.Default.DisplaySettings = settings;
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
            marqueeLabelTop.Font = cds.Font;
            marqueeLabelBottom.Font = cds.Font;
            CheckFontHeight();
            checkBoxConnectOnStartup.Checked = Properties.Settings.Default.DisplayConnectOnStartup;
            checkBoxReverseScreen.Checked = cds.ReverseScreen;
            checkBoxInverseColors.Checked = cds.InverseColors;
            comboBoxDisplayType.SelectedIndex = cds.DisplayType;
            timer.Interval = cds.TimerInterval;
            maskedTextBoxTimerInterval.Text = cds.TimerInterval.ToString();


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
        }

        private void checkBoxInverseColors_CheckedChanged(object sender, EventArgs e)
        {
            //Save our inverse colors setting
            cds.InverseColors = checkBoxInverseColors.Checked;
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

        //Delegates are used for our thread safe method
        public delegate void AddClientDelegate(string aSessionId, ICallback aCallback);
        public delegate void RemoveClientDelegate(string aSessionId);
        public delegate void SetTextDelegate(string SessionId, TextField aTextField);
        public delegate void SetTextsDelegate(string SessionId, System.Collections.Generic.IList<TextField> aTextFields);
        public delegate void SetClientNameDelegate(string aSessionId, string aName);


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
            Callback = aCallback;
        }

        public string SessionId { get; set; }
        public string Name { get; set; }
        public List<TextField> Texts { get; set; }
        public ICallback Callback { get; set; }
    }
}
