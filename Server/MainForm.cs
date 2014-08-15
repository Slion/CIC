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
        public Dictionary<string, IDisplayServiceCallback> iClients;
        public bool iClosing;

        public MainForm()
        {
            LastTickTime = DateTime.Now;
            iDisplay = new Display();
            iClients = new Dictionary<string, IDisplayServiceCallback>();

            InitializeComponent();
            UpdateStatus();

            //Load settings
            marqueeLabelTop.Font = Properties.Settings.Default.DisplayFont;
            marqueeLabelBottom.Font = Properties.Settings.Default.DisplayFont;
            checkBoxShowBorders.Checked = Properties.Settings.Default.DisplayShowBorders;
            checkBoxConnectOnStartup.Checked = Properties.Settings.Default.DisplayConnectOnStartup;
            checkBoxReverseScreen.Checked = Properties.Settings.Default.DisplayReverseScreen;
            //
            tableLayoutPanel.CellBorderStyle = (checkBoxShowBorders.Checked ? TableLayoutPanelCellBorderStyle.Single : TableLayoutPanelCellBorderStyle.None);
            //We have a bug when drawing minimized and reusing our bitmap
            iBmp = new System.Drawing.Bitmap(tableLayoutPanel.Width, tableLayoutPanel.Height, PixelFormat.Format32bppArgb);
            iCreateBitmap = false;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            StartServer();

            if (Properties.Settings.Default.DisplayConnectOnStartup)
            {
                iDisplay.Open();
                UpdateStatus();
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
                //label1.Font = fontDlg.Font;
                //textBox1.BackColor = fontDlg.Color;
                //label1.ForeColor = fontDlg.Color;
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

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            if (iDisplay.Open())
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

        private void buttonClose_Click(object sender, EventArgs e)
        {
            iDisplay.Close();
            UpdateStatus();
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
            //Tell connected client first? Is that possible?

            if (iClients.Count>0)
            {
                //Tell our clients
                BroadcastCloseEvent();
            }

            //iServiceHost.Close();

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
                        client.Value.OnServerClosing(/*eventData*/);
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

        public delegate void AddClientDelegate(string aSessionId, IDisplayServiceCallback aCallback);
        public delegate void RemoveClientDelegate(string aSessionId);
        public delegate void SetTextDelegate(int aLineIndex, string aText);
        public delegate void SetTextsDelegate(System.Collections.Generic.IList<string> aTexts);

       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aSessionId"></param>
        /// <param name="aCallback"></param>
        public void AddClientThreadSafe(string aSessionId, IDisplayServiceCallback aCallback)
        {
            if (this.treeViewClients.InvokeRequired)
            {
                //Not in the proper thread, invoke ourselves
                AddClientDelegate d = new AddClientDelegate(AddClientThreadSafe);
                this.Invoke(d, new object[] { aSessionId, aCallback });
            }
            else
            {
                //We are in the proper thread
                //Add this session to our collection of clients
                Program.iMainForm.iClients.Add(aSessionId, aCallback);
                //Add this session to our UI
                Program.iMainForm.treeViewClients.Nodes.Add(aSessionId, aSessionId);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aSessionId"></param>
        public void RemoveClientThreadSafe(string aSessionId)
        {
            if (this.treeViewClients.InvokeRequired)
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
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aLineIndex"></param>
        /// <param name="aText"></param>
        public void SetTextThreadSafe(int aLineIndex, string aText)
        {
            if (this.treeViewClients.InvokeRequired)
            {
                //Not in the proper thread, invoke ourselves
                SetTextDelegate d = new SetTextDelegate(SetTextThreadSafe);
                this.Invoke(d, new object[] { aLineIndex, aText });
            }
            else
            {
                //We are in the proper thread
                //Only support two lines for now
                if (aLineIndex == 0)
                {
                    Program.iMainForm.marqueeLabelTop.Text = aText;
                }
                else if (aLineIndex == 1)
                {
                    Program.iMainForm.marqueeLabelBottom.Text = aText;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aTexts"></param>
        public void SetTextsThreadSafe(System.Collections.Generic.IList<string> aTexts)
        {
            if (this.treeViewClients.InvokeRequired)
            {
                //Not in the proper thread, invoke ourselves
                SetTextsDelegate d = new SetTextsDelegate(SetTextsThreadSafe);
                this.Invoke(d, new object[] { aTexts });
            }
            else
            {
                //We are in the proper thread
                //Only support two lines for now
                for (int i = 0; i < aTexts.Count; i++)
                {
                    if (i == 0)
                    {
                        Program.iMainForm.marqueeLabelTop.Text = aTexts[i];
                    }
                    else if (i == 1)
                    {
                        Program.iMainForm.marqueeLabelBottom.Text = aTexts[i];
                    }
                }
            }

        }

    }
}
