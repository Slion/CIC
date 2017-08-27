//
// Copyright (C) 2014-2015 Stéphane Lenclud.
//
// This file is part of SharpDisplayManager.
//
// SharpDisplayManager is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// SharpDisplayManager is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with SharpDisplayManager.  If not, see <http://www.gnu.org/licenses/>.
//

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
using System.Deployment.Application;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Configuration;
//CSCore
using CSCore;
using CSCore.Win32;
using CSCore.DSP;
using CSCore.Streams;
using CSCore.CoreAudioAPI;
using CSCore.SoundIn;
// Visualization
using Visualization;
// CEC
using CecSharp;
//Network
using NETWORKLIST;
//
using SharpDisplayClient;
using SharpDisplay;
using SharpLib.MiniDisplay;
using SharpLib.Display;
using Ear = SharpLib.Ear;
using SharpLib.Win32;
using Squirrel;
using SmartHome = SharpLib.FritzBox.SmartHome;

namespace SharpDisplayManager
{
    //Types declarations
    public delegate uint ColorProcessingDelegate(int aX, int aY, uint aPixel);

    public delegate int CoordinateTranslationDelegate(System.Drawing.Bitmap aBmp, int aInt);

    //Delegates are used for our thread safe method
    public delegate void AddClientDelegate(string aSessionId, ICallback aCallback);

    public delegate void RemoveClientDelegate(string aSessionId);

    public delegate void WndProcDelegate(ref Message aMessage);

    /// <summary>
    /// Our Display manager main form
    /// </summary>
    [System.ComponentModel.DesignerCategory("Form")]
    public partial class FormMain : FormMainHid
    {
        //public Manager iManager = new Manager();        
        DateTime iLastTickTime;
        Display iDisplay;
        System.Drawing.Bitmap iBitmap;
        Rectangle iBitmapRect;
        //TODO: Align that with what we did from Audio Visualizers bitmaps?
        bool iCreateBitmap; //Workaround render to bitmap issues when minimized
        ServiceHost iServiceHost;
        // Our collection of clients sorted by session id.
        public Dictionary<string, ClientData> iClients;
        // The name of the client which informations are currently displayed.
        public string iCurrentClientSessionId;
        ClientData iCurrentClientData;
        /// <summary>
        /// Define our display view including layout and fields.
        /// Display view should include one ClientField that will show on client View.
        /// </summary>
        SharpLib.Display.View iDisplayView;

        bool iHasNewDisplayLayout;
        //
        public bool iClosing;
        // True if we are currently loading settings into our UI
        private bool iUpdatingStatus = false;
        //
        public bool iSkipFrameRendering;
        //Function pointer for pixel color filtering
        ColorProcessingDelegate iColorFx;
        //Function pointer for pixel X coordinate intercept
        CoordinateTranslationDelegate iScreenX;
        //Function pointer for pixel Y coordinate intercept
        CoordinateTranslationDelegate iScreenY;
        // Audio
        private AudioManager iAudioManager;
        // Kinect
        public SpeechManager iSpeechManager;

        //Network
        private NetworkManager iNetworkManager;

        /// <summary>
        /// CEC - Consumer Electronic Control.
        /// Notably used to turn TV on and off as Windows broadcast monitor on and off notifications.
        /// </summary>
        private ConsumerElectronicControl iCecManager;

        /// <summary>
        /// Manage run when Windows startup option
        /// </summary>
        private StartupManager iStartupManager;

        /// <summary>
        /// System notification icon used to hide our application from the task bar.
        /// </summary>
        private SharpLib.Notification.Control iNotifyIcon;

        /// <summary>
        /// System recording notification icon.
        /// </summary>
        private SharpLib.Notification.Control iRecordingNotification;

        /// <summary>
        /// 
        /// </summary>
        RichTextBoxTraceListener iWriter;


        /// <summary>
        /// Allow user to receive window messages;
        /// </summary>
        public event WndProcDelegate OnWndProc;

        public FormMain()
        {
            if (Properties.Settings.Default.EarManager == null)
            {
                //No actions in our settings yet
                Properties.Settings.Default.EarManager = new EarManager();
            }
            else
            {
                // We loaded events and actions from our settings
                // Internalizer apparently skips constructor so we need to initialize it here
                // Though I reckon that should only be needed when loading an empty EAR manager I guess.
                Properties.Settings.Default.EarManager.Construct();
            }

            // Register for property change event
            Properties.Settings.Default.PropertyChanged += PropertyChangedEventHandler;


            iSkipFrameRendering = false;
            iClosing = false;
            iCurrentClientSessionId = "";
            iCurrentClientData = null;
            iLastTickTime = DateTime.Now;
            //Instantiate our display and register for events notifications
            iDisplay = new Display();
            iDisplay.OnOpened += OnDisplayOpened;
            iDisplay.OnClosed += OnDisplayClosed;
            //
            iClients = new Dictionary<string, ClientData>();
            iStartupManager = new StartupManager();
            iNotifyIcon = new SharpLib.Notification.Control();
            iRecordingNotification = new SharpLib.Notification.Control();
            iDisplayView = new SharpLib.Display.View();
            // Default to a single field showing our client
            iDisplayView.Layout = new TableLayout(1, 1);
            iDisplayView.Fields.Add(new ClientField());

            //Have our designer initialize its controls
            InitializeComponent();

            //Redirect console output
            iWriter = new RichTextBoxTraceListener(richTextBoxLogs);
            Trace.Listeners.Add(iWriter);

            //Populate device types
            PopulateDeviceTypes();

            //Initial status update 
            UpdateStatus();

            //We have a bug when drawing minimized and reusing our bitmap
            //Though I could not reproduce it on Windows 10
            iBitmap = new System.Drawing.Bitmap(iTableLayoutPanelDisplay.Width, iTableLayoutPanelDisplay.Height, PixelFormat.Format32bppArgb);
            iBitmapRect = new Rectangle(new Point(0, 0), iBitmap.Size);
            iCreateBitmap = false;

            //Minimize our window if desired
            if (Properties.Settings.Default.StartMinimized)
            {
                WindowState = FormWindowState.Minimized;
            }            
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void MainForm_Load(object sender, EventArgs e)
        {
            //Check if we are running a Click Once deployed application
            //TODO: remove ClickOnce stuff 
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                //This is a proper Click Once installation, fetch and show our version number
                this.Text += " - v" + ApplicationDeployment.CurrentDeployment.CurrentVersion;
            }
            else
            {
                //Not a proper Click Once installation, assuming development build then
                var assembly = Assembly.GetExecutingAssembly();
                var versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
                this.Text += " - v" + versionInfo.ProductVersion;
                // Update not supported for non Click Once installation
                //buttonUpdate.Visible = false;
                //this.Text += " - development";
            }

            //CSCore
            CreateAudioManager();

            //Kinect
            CreateSpeechManagerIfNeeded();

            //Network
            iNetworkManager = new NetworkManager();
            iNetworkManager.OnConnectivityChanged += OnConnectivityChanged;
            UpdateNetworkStatus();

            //CEC
            iCecManager = new ConsumerElectronicControl();
            OnWndProc += iCecManager.OnWndProc;
            ResetCec();

            //Harmony
            ResetHarmonyAsync();

            //FRITZ!Box
            await CreateFritzBoxClient();

            //Setup Events
            PopulateTreeViewEvents();

            //Setup notification icon
            SetupTrayIcon();

            //Setup recording notification
            SetupRecordingNotification();

            // To make sure start up with minimize to tray works
            if (WindowState == FormWindowState.Minimized && Properties.Settings.Default.MinimizeToTray)
            {
                Visible = false;
            }

#if !DEBUG
            // When not debugging we want the screen to be empty until a client takes over
			ClearLayout(iTableLayoutPanelCurrentClient);

            // Display layout should be empty too
            // TODO: Eventually we will need to dynamically create and destroy our client table layout
            DoClearLayout(iTableLayoutPanelDisplay);
            iTableLayoutPanelDisplay.ColumnCount = 1;
            iTableLayoutPanelDisplay.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            iTableLayoutPanelDisplay.RowCount = 1;
            iTableLayoutPanelDisplay.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            //
            iTableLayoutPanelDisplay.SetRowSpan(iTableLayoutPanelCurrentClient, 1);
            iTableLayoutPanelDisplay.SetColumnSpan(iTableLayoutPanelCurrentClient, 1);
            iTableLayoutPanelDisplay.Controls.Add(iTableLayoutPanelCurrentClient,0,0);

            // Is that needed? Why just in release?
            iCurrentClientData = null;
#else
            //When developing we want at least one client for testing
            StartNewClient("abcdefghijklmnopqrst-0123456789", "ABCDEFGHIJKLMNOPQRST-0123456789");
#endif

            //Open display connection on start-up if needed
            if (Properties.Settings.Default.DisplayConnectOnStartup)
            {
                OpenDisplayConnection();
            }

            //Start our server so that we can get client requests
            StartServer();

            //Register for HID events
            RegisterHidDevices();

            //Start Idle client if needed
            if (Properties.Settings.Default.StartIdleClient)
            {
                StartIdleClient();
            }
        }

        /// <summary>
        /// Check for application update and ask the user to proceed if any.
        /// </summary>
        async void SquirrelUpdate(bool aAutoCheck=false)
        {
            // Check for Squirrel application update
#if !DEBUG
            // Prevent user from starting an update while one is already running
            buttonUpdate.Enabled = false; 

            ReleaseEntry release = null;
            using (var mgr = new UpdateManager(Program.KSquirrelUpdateUrl))
            {
                // We have an update ask our user if he wants it
                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                //
                UpdateInfo updateInfo = await mgr.CheckForUpdate();
                if (updateInfo.ReleasesToApply.Any()) // Check if we have any update
                {

                    string msg = "New version available!" +
                                    "\n\nCurrent version: " + updateInfo.CurrentlyInstalledVersion.Version +
                                    "\nNew version: " + updateInfo.FutureReleaseEntry.Version +
                                    "\n\nUpdate application now?";
                    DialogResult dialogResult = MessageBox.Show(msg, fvi.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        // User wants it, do the update
                        release = await mgr.UpdateApp();
                        // Backup our users settings
                        Program.BackupSettings();
                    }
                    else
                    {
                        // User cancel an update enable manual update option
                        //iToolStripMenuItemUpdate.Visible = true;
                    }
                }
                else
                {
                    // Don't display intrusive message for auto checks
                    if (!aAutoCheck)
                    {
                        MessageBox.Show("You are already running the latest version.", fvi.ProductName);
                    }
                    
                }
            }

            // Restart the app
            if (release != null)
            {
                UpdateManager.RestartApp();
            }

            // Our update is completed re-enable the update button then
            buttonUpdate.Enabled = true;
#endif
        }


        private async void PropertyChangedEventHandler(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Trace.WriteLine($"Settings: property changed {e.PropertyName}");

            if (e.PropertyName.Equals("SpeechEnabled") || e.PropertyName.Equals("UseMicrosoftSpeech"))
            {
                CreateSpeechManagerIfNeeded();
            }
            else if (e.PropertyName.Equals("CecEnabled"))
            {
                ResetCec();
            }
            else if (e.PropertyName.Equals("FritzBoxEnabled"))
            {
                await CreateFritzBoxClient();
            }
        }


    
        /// <summary>
        /// 
        /// </summary>
        private void CreateAudioManager()
        {
            iAudioManager = new AudioManager();
            iAudioManager.Open(OnDefaultMultiMediaDeviceChanged, OnVolumeNotification);
            UpdateAudioDeviceAndMasterVolumeThreadSafe();
        }

        /// <summary>
        /// 
        /// </summary>
        private void DestroyAudioManager()
        {
            if (iAudioManager != null)
            {
                iAudioManager.Close();
                iAudioManager = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void CreateSpeechManagerIfNeeded()
        {
            try
            {
                // First clean things up
                DestroySpeechManager();

                if (Properties.Settings.Default.SpeechEnabled)
                {
                    // Instanciate the proper Speech Manager
                    if (Properties.Settings.Default.UseMicrosoftSpeech)
                    {
                        // Typically used for Kinect
                        // Needs user to install it on its machine
                        iSpeechManager = new SpeechManagerMicrosoft();
                    }
                    else
                    {
                        // Default, provided with Windows 
                        iSpeechManager = new SpeechManagerSystem();
                    }

                    iSpeechManager.StartSpeechRecognition();
                    iLabelSpeechRecognizerCulture.Text = "Culture: " + iSpeechManager.Culture.Name;
                    if (iSpeechManager.IsKinectRecognizer)
                    {
                        iLabelSpeechRecognizerCulture.Text += " - Kinect";
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Print(ex.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void DestroySpeechManager()
        {
            try
            {
                if (iSpeechManager != null)
                {
                    iSpeechManager.StopSpeechRecognition();
                }
            }
            catch (Exception ex)
            {
                Debug.Print(ex.ToString());
            }

            iLabelSpeechRecognizerCulture.Text = "Culture: none";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="aEvent"></param>
        public void OnDefaultMultiMediaDeviceChanged(object sender, DefaultDeviceChangedEventArgs aEvent)
        {
            if (aEvent.DataFlow == DataFlow.Render && aEvent.Role == Role.Multimedia)
            {
                ResetAudioManagerThreadSafe();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ResetAudioManagerThreadSafe()
        {
            if (InvokeRequired)
            {
                //Not in the proper thread, invoke ourselves
                BeginInvoke(new Action<FormMain>((sender) => { ResetAudioManagerThreadSafe(); }), this);
                return;
            }

            //Proper thread, go ahead
            DestroyAudioManager();
            CreateAudioManager();

        }

        /// <summary>
        /// Called when our display is opened.
        /// </summary>
        /// <param name="aDisplay"></param>
        private void OnDisplayOpened(Display aDisplay)
        {
            //Make sure we resume frame rendering
            iSkipFrameRendering = false;

            //Set our screen size now that our display is connected
            //Our panelDisplay is the container of our tableLayoutPanel
            //tableLayoutPanel will resize itself to fit the client size of our panelDisplay
            //panelDisplay needs an extra 2 pixels for borders on each sides
            //tableLayoutPanel will eventually be the exact size of our display
            Size size = new Size(iDisplay.WidthInPixels() + 2, iDisplay.HeightInPixels() + 2);
            iPanelDisplay.Size = size;

            //Our display was just opened, update our UI
            UpdateStatus();
            //Initiate asynchronous request
            iDisplay.RequestFirmwareRevision();

            //Audio
            UpdateMasterVolumeThreadSafe();
            //Network
            UpdateNetworkStatus();

#if DEBUG
            //Testing icon in debug, no arm done if icon not supported
            //iDisplay.SetIconStatus(Display.TMiniDisplayIconType.EMiniDisplayIconRecording, 0, 1);
            //iDisplay.SetAllIconsStatus(2);
#endif

        }


        private static void AddActionsToTreeNode(TreeNode aParentNode, Ear.Object aObject)
        {
            foreach (Ear.Action a in aObject.Objects.OfType<Ear.Action>())
            {
                //Create action node
                TreeNode actionNode = aParentNode.Nodes.Add(a.Brief());
                actionNode.Tag = a;
                //Use color from parent unless our action itself is disabled
                actionNode.ForeColor = a.Enabled ? aParentNode.ForeColor : Color.DimGray;
                //Go recursive
                AddActionsToTreeNode(actionNode,a);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="aObject"></param>
        /// <param name="aNode"></param>
        private static TreeNode FindTreeNodeForEarObject(Ear.Object aObject, TreeNode aNode)
        {
            if (aNode.Tag == aObject)
            {
                return aNode;
            }

            foreach (TreeNode n in aNode.Nodes)
            {
                TreeNode found = FindTreeNodeForEarObject(aObject,n);
                if (found != null)
                {
                    return found;
                }
            }

            return null;
        }

        /// <summary>
        /// Recursive function to expand paht to a node
        /// </summary>
        /// <param name="aNode"></param>
        static void TreeViewExpandPathTo(TreeNode aNode)
        {
            if (aNode.Parent!=null)
            {
                TreeViewExpandPathTo(aNode.Parent);
            }
            aNode.Expand();
        }

        /// <summary>
        /// Expand the given node and all its children to the given depth.
        /// </summary>
        /// <param name="aDepth"></param>
        static void TreeViewExpandAll(TreeNode aNode, int aDepth)
        {
            if (aDepth<0)
            {
                return;
            }

            aNode.Expand();

            foreach (TreeNode n in aNode.Nodes)
            {                
                TreeViewExpandAll(n, aDepth - 1);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aObject"></param>
        private void SelectEarObject(Ear.Object aObject)
        {
            foreach (TreeNode n in iTreeViewEvents.Nodes)
            {
                TreeNode found = FindTreeNodeForEarObject(aObject, n);
                if (found != null)
                {
                    // Expand all parents on our path
                    TreeViewExpandPathTo(found);
                    // Show all our children and their first level of children.
                    // That works well for events node thus we show the first level of actions.
                    // It could be improved for nested actions I guess but we can't bother for now.
                    TreeViewExpandAll(found, 1);
                    // Select and focus it
                    iTreeViewEvents.SelectedNode=found;
                    iTreeViewEvents.Focus();
                    return;
                }
            }
        }

        /// <summary>
        /// Populate tree view with events and actions
        /// </summary>
        private void PopulateTreeViewEvents(Ear.Object aSelectedObject=null)
        {
            //Reset our tree
            iTreeViewEvents.Nodes.Clear();
            //Populate registered events
            foreach (Ear.Event e in Properties.Settings.Default.EarManager.Events)
            {
                //Create our event node
                //Work out the name of our node
                string eventNodeName = "";
                if (!string.IsNullOrEmpty(e.Name))
                {
                    //That event has a proper name, use it then
                    eventNodeName = $"{e.Name} - {e.Brief()}";
                }
                else
                {
                    //Unnamed events just use brief
                    eventNodeName = e.Brief();
                }
                
                TreeNode eventNode = iTreeViewEvents.Nodes.Add(eventNodeName);
                eventNode.Tag = e; //For easy access to our event
                if (!e.Enabled)
                {
                    //Dim our nodes if disabled
                    eventNode.ForeColor = Color.DimGray;
                }

                //Add event description as child node
                eventNode.Nodes.Add(e.AttributeDescription).ForeColor = eventNode.ForeColor; 
                //Create child node for actions root
                TreeNode actionsNode = eventNode.Nodes.Add("Actions");
                actionsNode.ForeColor = eventNode.ForeColor;

                // Recursively add our actions for that event
                AddActionsToTreeNode(actionsNode,e);
            }

            //iTreeViewEvents.ExpandAll();

            if (aSelectedObject != null)
            {
                SelectEarObject(aSelectedObject);
            }            

            // Just to be safe in case the selection did not work
            UpdateEventView();
        }

        /// <summary>
        /// Called when our display is closed.
        /// </summary>
        /// <param name="aDisplay"></param>
        private void OnDisplayClosed(Display aDisplay)
        {
            //Our display was just closed, update our UI consequently
            UpdateStatus();
        }

        public void OnConnectivityChanged(NetworkManager aNetwork, NLM_CONNECTIVITY newConnectivity)
        {
            //Update network status
            UpdateNetworkStatus();
        }

        /// <summary>
        /// Update our Network Status
        /// </summary>
        private void UpdateNetworkStatus()
        {
            if (iDisplay.IsOpen())
            {
                iDisplay.SetIconOnOff(IconType.Internet,
                    iNetworkManager.NetworkListManager.IsConnectedToInternet);
                iDisplay.SetIconOnOff(IconType.NetworkSignal, iNetworkManager.NetworkListManager.IsConnected);
            }
        }


        int iLastNetworkIconIndex = 0;
        int iUpdateCountSinceLastNetworkAnimation = 0;

        /// <summary>
        /// 
        /// </summary>
        private void UpdateNetworkSignal(DateTime aLastTickTime, DateTime aNewTickTime)
        {
            iUpdateCountSinceLastNetworkAnimation++;
            iUpdateCountSinceLastNetworkAnimation = iUpdateCountSinceLastNetworkAnimation%4;

            if (iDisplay.IsOpen() && iNetworkManager.NetworkListManager.IsConnected &&
                iUpdateCountSinceLastNetworkAnimation == 0)
            {
                int iconCount = iDisplay.IconCount(IconType.NetworkSignal);
                if (iconCount <= 0)
                {
                    //Prevents div by zero and other undefined behavior
                    return;
                }
                iLastNetworkIconIndex++;
                iLastNetworkIconIndex = iLastNetworkIconIndex%(iconCount*2);
                for (int i = 0; i < iconCount; i++)
                {
                    if (i < iLastNetworkIconIndex && !(i == 0 && iLastNetworkIconIndex > 3) &&
                        !(i == 1 && iLastNetworkIconIndex > 4))
                    {
                        iDisplay.SetIconOn(IconType.NetworkSignal, i);
                    }
                    else
                    {
                        iDisplay.SetIconOff(IconType.NetworkSignal, i);
                    }
                }
            }
        }



        /// <summary>
        /// Receive volume change notification and reflect changes on our slider.
        /// </summary>
        /// <param name="data"></param>
        public void OnVolumeNotification(object sender, AudioEndpointVolumeCallbackEventArgs aEvent)
        {
            UpdateMasterVolumeThreadSafe();
        }

        /// <summary>
        /// Update master volume when user moves our slider.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trackBarMasterVolume_Scroll(object sender, EventArgs e)
        {
            //Just like Windows Volume Mixer we unmute if the volume is adjusted
            iAudioManager.Volume.IsMuted = false;
            //Set volume level according to our volume slider new position
            iAudioManager.Volume.MasterVolumeLevelScalar = trackBarMasterVolume.Value/100.0f;
        }


        /// <summary>
        /// Mute check box changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxMute_CheckedChanged(object sender, EventArgs e)
        {
            iAudioManager.Volume.IsMuted = checkBoxMute.Checked;
        }


        /// <summary>
        /// Update master volume indicators based our current system states.
        /// This typically includes volume levels and mute status.
        /// </summary>
        private void UpdateMasterVolumeThreadSafe()
        {
            if (InvokeRequired)
            {
                //Not in the proper thread, invoke ourselves
                BeginInvoke(new Action<FormMain>((sender) => { UpdateMasterVolumeThreadSafe(); }), this);
                return;
            }

            //Update volume slider
            float volumeLevelScalar = iAudioManager.Volume.MasterVolumeLevelScalar;
            trackBarMasterVolume.Value = Convert.ToInt32(volumeLevelScalar*100);
            //Update mute checkbox
            checkBoxMute.Checked = iAudioManager.Volume.IsMuted;

            //If our display connection is open we need to update its icons
            if (iDisplay.IsOpen())
            {
                //First take care our our volume level icons
                int volumeIconCount = iDisplay.IconCount(IconType.Volume);
                if (volumeIconCount > 0)
                {
                    //Compute current volume level from system level and the number of segments in our display volume bar.
                    //That tells us how many segments in our volume bar needs to be turned on.
                    float currentVolume = volumeLevelScalar*volumeIconCount;
                    int segmentOnCount = Convert.ToInt32(currentVolume);
                    //Check if our segment count was rounded up, this will later be used for half brightness segment
                    bool roundedUp = segmentOnCount > currentVolume;

                    for (int i = 0; i < volumeIconCount; i++)
                    {
                        if (i < segmentOnCount)
                        {
                            //If we are dealing with our last segment and our segment count was rounded up then we will use half brightness.
                            if (i == segmentOnCount - 1 && roundedUp)
                            {
                                //Half brightness
                                iDisplay.SetIconStatus(IconType.Volume, i,
                                    (iDisplay.IconStatusCount(IconType.Volume) - 1)/2);
                            }
                            else
                            {
                                //Full brightness
                                iDisplay.SetIconStatus(IconType.Volume, i,
                                    iDisplay.IconStatusCount(IconType.Volume) - 1);
                            }
                        }
                        else
                        {
                            iDisplay.SetIconStatus(IconType.Volume, i, 0);
                        }
                    }
                }

                //Take care of our mute icon
                iDisplay.SetIconOnOff(IconType.Mute, iAudioManager.Volume.IsMuted);
            }

        }



        /// <summary>
        /// 
        /// </summary>
        private void UpdateAudioVisualization()
        {
            // No point if we don't have a current client
            if (iCurrentClientData == null)
            {
                return;
            }

            // Update our math
            if (iAudioManager==null || iAudioManager.Spectrum==null || !iAudioManager.Spectrum.Update())
            {
                //Nothing changed no need to render
                return;
            }

            // Check if our current client has an Audio Visualizer field
            // and render them as needed
            foreach (DataField f in iCurrentClientData.View.Fields)
            {
                if (f is AudioVisualizerField)
                {
                    AudioVisualizerField avf = (AudioVisualizerField)f;
                    Control ctrl = iTableLayoutPanelCurrentClient.GetControlFromPosition(avf.Column, avf.Row);

                    if (ctrl is PictureBox)
                    {
                        PictureBox pb = (PictureBox)ctrl;
                        if (iAudioManager.Spectrum.Render(pb.Image, Color.Black, Color.Black, Color.White, false))
                        {
                            pb.Invalidate();
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void UpdateAudioDeviceAndMasterVolumeThreadSafe()
        {            
            if (InvokeRequired)
            {
                //Not in the proper thread, invoke ourselves
                BeginInvoke(new Action<FormMain>((sender) => { UpdateAudioDeviceAndMasterVolumeThreadSafe(); }), this);
                return;
            }

            //We are in the correct thread just go ahead.
            try
            {
                  
                //Update our label
                iLabelDefaultAudioDevice.Text = iAudioManager.DefaultDevice.FriendlyName;

                //Show our volume in our track bar
                UpdateMasterVolumeThreadSafe();

                //
                trackBarMasterVolume.Enabled = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception thrown in UpdateAudioDeviceAndMasterVolume");
                Debug.WriteLine(ex.ToString());
                //Something went wrong S/PDIF device ca throw exception I guess
                trackBarMasterVolume.Enabled = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopulateDeviceTypes()
        {
            int count = Display.TypeCount();

            for (int i = 0; i < count; i++)
            {
                comboBoxDisplayType.Items.Add(Display.TypeName((SharpLib.MiniDisplay.Type) i));
            }
        }

        /// <summary>
        ///
        /// </summary>
        private void SetupTrayIcon()
        {
            iNotifyIcon.Icon = GetIcon("cic.ico");
            iNotifyIcon.Text = "CIC";
            iNotifyIcon.Visible = true;

            //Double click toggles visibility - typically brings up the application
            iNotifyIcon.DoubleClick += delegate(object obj, EventArgs args)
            {
                SysTrayHideShow();
            };

            //Adding a context menu, useful to be able to exit the application
            ContextMenu contextMenu = new ContextMenu();
            //Context menu item to toggle visibility
            MenuItem hideShowItem = new MenuItem("Hide/Show");
            hideShowItem.Click += delegate(object obj, EventArgs args)
            {
                SysTrayHideShow();
            };
            contextMenu.MenuItems.Add(hideShowItem);

            //Context menu item separator
            contextMenu.MenuItems.Add(new MenuItem("-"));

            //Context menu exit item
            MenuItem exitItem = new MenuItem("Exit");
            exitItem.Click += delegate(object obj, EventArgs args)
            {
                Application.Exit();
            };
            contextMenu.MenuItems.Add(exitItem);

            iNotifyIcon.ContextMenu = contextMenu;
        }

        /// <summary>
        ///
        /// </summary>
        private void SetupRecordingNotification()
        {
            iRecordingNotification.Icon = GetIcon("record.ico");
            iRecordingNotification.Text = "No recording";
            iRecordingNotification.Visible = false;
        }

        /// <summary>
        /// Access icons from embedded resources.
        /// </summary>
        /// <param name="aName"></param>
        /// <returns></returns>
        public static Icon GetIcon(string aName)
        {
            string[] names = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            foreach (string name in names)
            {
                //Find a resource name that ends with the given name
                if (name.EndsWith(aName))
                {
                    using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name))
                    {
                        return new Icon(stream);
                    }
                }
            }

            return null;
        }


        /// <summary>
        /// Set our current client.
        /// This will take care of applying our client layout and set data fields.
        /// </summary>
        /// <param name="aSessionId"></param>
        void SetCurrentClient(string aSessionId, bool aForce = false)
        {
            if (aSessionId == iCurrentClientSessionId)
            {
                //Given client is already the current one.
                //Don't bother changing anything then.
                return;
            }

            ClientData requestedClientData = iClients[aSessionId];

            //Check when was the last time we switched to that client
            if (iCurrentClientData != null)
            {
                //Do not switch client if priority of current client is higher 
                if (!aForce && requestedClientData.Priority < iCurrentClientData.Priority)
                {
                    return;
                }


                double lastSwitchToClientSecondsAgo = (DateTime.Now - iCurrentClientData.LastSwitchTime).TotalSeconds;
                //TODO: put that hard coded value as a client property
                //Clients should be able to define how often they can be interrupted
                //Thus a background client can set this to zero allowing any other client to interrupt at any time
                //We could also compute this delay by looking at the requests frequencies?
                if (!aForce &&
                    requestedClientData.Priority == iCurrentClientData.Priority &&
                    //Time sharing is only if clients have the same priority
                    (lastSwitchToClientSecondsAgo < 30)) //Make sure a client is on for at least 30 seconds
                {
                    //Don't switch clients too often
                    return;
                }
            }

            //Set current client ID.
            iCurrentClientSessionId = aSessionId;
            //Set the time we last switched to that client
            iClients[aSessionId].LastSwitchTime = DateTime.Now;
            //Fetch and set current client data.
            iCurrentClientData = requestedClientData;
            //Apply layout and set data fields.
            UpdateTableLayoutPanel(iCurrentClientData.View, iTableLayoutPanelCurrentClient);
        }

        private void buttonFont_Click(object sender, EventArgs e)
        {
            //fontDialog.ShowColor = true;
            //fontDialog.ShowApply = true;
            fontDialog.ShowEffects = true;
            fontDialog.Font = cds.Font;

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
                //Save font settings
                cds.Font = fontDialog.Font;
                Properties.Settings.Default.Save();
                //
                //Set the fonts to all our labels in our layout
                UpdateFonts(iTableLayoutPanelDisplay);
                //
                CheckFontHeight();
            }
        }

        /// <summary>
        /// TODO: review this in respect to our logical font feature when we get there.
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

            MarqueeLabel label = null;
            //Get the first label control we can find
            foreach (Control ctrl in iTableLayoutPanelCurrentClient.Controls)
            {
                if (ctrl is MarqueeLabel)
                {
                    label = (MarqueeLabel) ctrl;
                    break;
                }
            }

            //Now check font height and show a warning if needed.
            if (label != null && label.Font.Height > label.Height)
            {
                labelWarning.Text = "WARNING: Selected font is too height by " + (label.Font.Height - label.Height) +
                                    " pixels!";
                labelWarning.Visible = true;
            }
            else
            {
                labelWarning.Visible = false;
            }

        }

        private void buttonCapture_Click(object sender, EventArgs e)
        {
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(iTableLayoutPanelDisplay.Width, iTableLayoutPanelDisplay.Height);
            iTableLayoutPanelDisplay.DrawToBitmap(bmp, iTableLayoutPanelDisplay.ClientRectangle);
            //Bitmap bmpToSave = new Bitmap(bmp);
            bmp.Save("D:\\capture.png");

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
                    case Request.FirmwareRevision:
                        toolStripStatusLabelConnect.Text += " v" + iDisplay.FirmwareRevision();
                        //Issue next request then
                        iDisplay.RequestPowerSupplyStatus();
                        break;

                    case Request.PowerSupplyStatus:
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

                    case Request.DeviceId:
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
            if ((aX%2 == 0) && (aY%2 == 0))
            {
                return ~aPixel;
            }
            else if ((aX%2 != 0) && (aY%2 != 0))
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
            return iBitmap.Height - aY - 1;
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

        /// <summary>
        /// This is our timer tick responsible to perform our render
        /// TODO: Use a threading timer instead of a Windows form timer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            //Update our animations
            DateTime newTickTime = DateTime.Now;

            UpdateNetworkSignal(iLastTickTime, newTickTime);

            // Update animation for all our marquees
            UpdateMarqueesAnimations(iTableLayoutPanelDisplay, iLastTickTime, newTickTime);

            // Update audio visualization
            UpdateAudioVisualization();

            //Update our display
            if (iDisplay.IsOpen())
            {
                CheckForRequestResults();

                //Check if frame rendering is needed
                //Typically used when showing clock
                if (!iSkipFrameRendering)
                {
                    //Draw to bitmap
                    if (iCreateBitmap)
                    {
                        iBitmap = new System.Drawing.Bitmap(iTableLayoutPanelDisplay.Width, iTableLayoutPanelDisplay.Height, PixelFormat.Format32bppArgb);
                        iBitmapRect = new Rectangle(new Point(0, 0), iBitmap.Size);
                        iCreateBitmap = false;
                    }
                    iTableLayoutPanelDisplay.DrawToBitmap(iBitmap, iTableLayoutPanelDisplay.ClientRectangle);
                    //iBmp.Save("D:\\capture.png");

                    unsafe
                    {
                        BitmapData bitmap = iBitmap.LockBits(iBitmapRect, ImageLockMode.ReadOnly, iBitmap.PixelFormat);
                        uint* pixels = (uint*)bitmap.Scan0.ToPointer(); // Assuming 4 bytes per pixel since we specified the format ourselves

                        //Send it to our display
                        for (int i = 0; i < bitmap.Width; i++)
                        {
                            for (int j = 0; j < bitmap.Height; j++)
                            {
                                //Get our processed pixel coordinates
                                int x = iScreenX(iBitmap, i);
                                int y = iScreenY(iBitmap, j);
                                //Get pixel color
                                uint color = pixels[j*bitmap.Width+i];
                                //Apply color effects
                                color = iColorFx(x, y, color);
                                //Now set our pixel
                                iDisplay.SetPixel(x, y, color);
                            }
                        }
                        iBitmap.UnlockBits(bitmap);
                    }

                    iDisplay.SwapBuffers();
                }
            }

            DateTime afterRenderTime = DateTime.Now;

            //Compute instant FPS
            toolStripStatusLabelFps.Text = (1.0/newTickTime.Subtract(iLastTickTime).TotalSeconds).ToString("F0") + " / " +
                                           (1000/iTimerDisplay.Interval).ToString() + " FPS - " +
                                           afterRenderTime.Subtract(newTickTime).TotalMilliseconds.ToString("F0") + " ms";

            iLastTickTime = newTickTime;

        }

        /// <summary>
        /// Update marquee animation children of the given control.
        /// </summary>
        static private void UpdateMarqueesAnimations(Control aControl, DateTime aLastTickTime, DateTime aNewTickTime)
        {
            // For each our children
            foreach (Control ctrl in aControl.Controls)
            {
                // Update animation if it is a marquee control
                if (ctrl is MarqueeLabel)
                {
                    ((MarqueeLabel)ctrl).UpdateAnimation(aLastTickTime, aNewTickTime);
                }

                // Go one level deeper by recursion
                UpdateMarqueesAnimations(ctrl, aLastTickTime, aNewTickTime);
            }
        }

        /// <summary>
        /// Attempt to establish connection with our display hardware.
        /// </summary>
        private void OpenDisplayConnection()
        {
            CloseDisplayConnection();

            if (!iDisplay.Open((SharpLib.MiniDisplay.Type) cds.DisplayType))
            {
                UpdateStatus();
                toolStripStatusLabelConnect.Text = "Connection error";
            }
        }

        private void CloseDisplayConnection()
        {
            //Status will be updated upon receiving the closed event

            if (iDisplay == null || !iDisplay.IsOpen())
            {
                return;
            }

            //Do not clear if we gave up on rendering already.
            //This means we will keep on displaying clock on MDM166AA for instance.
            if (!iSkipFrameRendering)
            {
                iDisplay.Clear();
                iDisplay.SwapBuffers();
            }

            iDisplay.SetAllIconsStatus(0); //Turn off all icons
            iDisplay.Close();
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
            char[] charSizes = new char[] {'i', 'a', 'Z', '%', '#', 'a', 'B', 'l', 'm', ',', '.'};
            float charWidth = g.MeasureString("I", ft, Int32.MaxValue, StringFormat.GenericTypographic).Width;

            bool fixedWidth = true;

            foreach (char c in charSizes)
                if (g.MeasureString(c.ToString(), ft, Int32.MaxValue, StringFormat.GenericTypographic).Width !=
                    charWidth)
                    fixedWidth = false;

            if (fixedWidth)
            {
                return charWidth;
            }

            return 0.0f;
        }

        /// <summary>
        /// Update fonts in our control tree.
        /// </summary>
        /// <param name="aControls"></param>
        private void UpdateFonts(Control aControl)
        {
            foreach (Control ctrl in aControl.Controls)
            {
                if (ctrl is MarqueeLabel)
                {
                    MarqueeLabel marquee = (MarqueeLabel)ctrl;
                    marquee.Font = cds.Font;
                }

                // Recurse
                UpdateFonts(ctrl);
            }
        }

        /// <summary>
        /// Update marquees separator in our control tree.
        /// </summary>
        /// <param name="aControls"></param>
        private void UpdateMarqueesSeparator(Control aControl)
        {
            foreach (Control ctrl in aControl.Controls)
            {
                if (ctrl is MarqueeLabel)
                {
                    MarqueeLabel marquee = (MarqueeLabel)ctrl;
                    marquee.Separator = cds.Separator;
                }

                // Recurse
                UpdateMarqueesSeparator(ctrl);
            }
        }

        /// <summary>
        /// Synchronize UI with settings
        /// </summary>
        private void UpdateStatus()
        {
            Debug.WriteLine("Status update - being");
            // Set that flag to avoid feedback loop
            iUpdatingStatus = true;
            //Load settings
            checkBoxShowBorders.Checked = cds.ShowBorders;
            iTableLayoutPanelCurrentClient.CellBorderStyle = (cds.ShowBorders
                ? TableLayoutPanelCellBorderStyle.Single
                : TableLayoutPanelCellBorderStyle.None);

            //Set the proper font to each of our labels
            UpdateFonts(iTableLayoutPanelDisplay);

            CheckFontHeight();
            //Check if "run on Windows startup" is enabled
            checkBoxAutoStart.Checked = iStartupManager.Startup;
            
            //CEC settings
            iComboBoxHdmiPort.SelectedIndex = Properties.Settings.Default.CecHdmiPort - 1;

            //Mini Display settings
            checkBoxReverseScreen.Checked = cds.ReverseScreen;
            checkBoxInverseColors.Checked = cds.InverseColors;
            checkBoxShowVolumeLabel.Checked = cds.ShowVolumeLabel;
            checkBoxScaleToFit.Checked = cds.ScaleToFit;
            maskedTextBoxMinFontSize.Enabled = cds.ScaleToFit;
            labelMinFontSize.Enabled = cds.ScaleToFit;
            maskedTextBoxMinFontSize.Text = cds.MinFontSize.ToString();
            maskedTextBoxScrollingSpeed.Text = cds.ScrollingSpeedInPixelsPerSecond.ToString();
            comboBoxDisplayType.SelectedIndex = cds.DisplayType;
            iTimerDisplay.Interval = cds.TimerInterval;
            maskedTextBoxTimerInterval.Text = cds.TimerInterval.ToString();
            textBoxScrollLoopSeparator.Text = cds.Separator;
            //
            SetupPixelDelegates();

            if (iDisplay.IsOpen())
            {
                //We have a display connection
                //Reflect that in our UI
                StartTimer();

                iTableLayoutPanelDisplay.Enabled = true;
                iTableLayoutPanelCurrentClient.Enabled = true;
                iPanelDisplay.Enabled = true;

                //Only setup brightness if display is open
                trackBarBrightness.Minimum = iDisplay.MinBrightness();
                trackBarBrightness.Maximum = iDisplay.MaxBrightness();
                if (cds.Brightness < iDisplay.MinBrightness() || cds.Brightness > iDisplay.MaxBrightness())
                {
                    //Brightness out of range, this can occur when using auto-detect
                    //Use max brightness instead
                    trackBarBrightness.Value = iDisplay.MaxBrightness();
                    iDisplay.SetBrightness(iDisplay.MaxBrightness());
                }
                else
                {
                    trackBarBrightness.Value = cds.Brightness;
                    iDisplay.SetBrightness(cds.Brightness);
                }

                //Try compute the steps to something that makes sense
                trackBarBrightness.LargeChange = Math.Max(1, (iDisplay.MaxBrightness() - iDisplay.MinBrightness())/5);
                trackBarBrightness.SmallChange = 1;

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


                //Check if Volume Label is supported. To date only MDM166AA supports that crap :)
                checkBoxShowVolumeLabel.Enabled = iDisplay.IconCount(IconType.VolumeLabel) > 0;

                if (cds.ShowVolumeLabel)
                {
                    iDisplay.SetIconOn(IconType.VolumeLabel);
                }
                else
                {
                    iDisplay.SetIconOff(IconType.VolumeLabel);
                }
            }
            else
            {
                //Display connection not available
                //Reflect that in our UI
#if DEBUG
                //In debug start our timer even if we don't have a display connection
                StartTimer();
#else
    //In production environment we don't need our timer if no display connection
                StopTimer();
#endif
                checkBoxShowVolumeLabel.Enabled = false;
                iTableLayoutPanelDisplay.Enabled = false;
                iTableLayoutPanelCurrentClient.Enabled = false;
                iPanelDisplay.Enabled = false;
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

            //
            iUpdatingStatus = false;
            Debug.WriteLine("Status update - end");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxShowVolumeLabel_CheckedChanged(object sender, EventArgs e)
        {
            cds.ShowVolumeLabel = checkBoxShowVolumeLabel.Checked;
            Properties.Settings.Default.Save();
            UpdateStatus();
        }

        private void checkBoxShowBorders_CheckedChanged(object sender, EventArgs e)
        {
            //Save our show borders setting
            if (!iUpdatingStatus)
            {
                iTableLayoutPanelCurrentClient.CellBorderStyle = (checkBoxShowBorders.Checked
                ? TableLayoutPanelCellBorderStyle.Single
                : TableLayoutPanelCellBorderStyle.None);
                cds.ShowBorders = checkBoxShowBorders.Checked;
                Properties.Settings.Default.Save();
            }
            CheckFontHeight();
        }

        private void checkBoxAutoStart_CheckedChanged(object sender, EventArgs e)
        {
            iStartupManager.Startup = checkBoxAutoStart.Checked;
        }


        private void checkBoxReverseScreen_CheckedChanged(object sender, EventArgs e)
        {
            //Save our reverse screen setting
            if (!iUpdatingStatus)
            {
                cds.ReverseScreen = checkBoxReverseScreen.Checked;
                Properties.Settings.Default.Save();
            }
            SetupPixelDelegates();
        }

        private void checkBoxInverseColors_CheckedChanged(object sender, EventArgs e)
        {
            //Save our inverse colors setting
            if (!iUpdatingStatus)
            {
                cds.InverseColors = checkBoxInverseColors.Checked;
                Properties.Settings.Default.Save();
            }
            SetupPixelDelegates();
        }

        private void checkBoxScaleToFit_CheckedChanged(object sender, EventArgs e)
        {
            //Save our scale to fit setting
            if (!iUpdatingStatus)
            {
                cds.ScaleToFit = checkBoxScaleToFit.Checked;
                Properties.Settings.Default.Save();
            }
            //
            labelMinFontSize.Enabled = cds.ScaleToFit;
            maskedTextBoxMinFontSize.Enabled = cds.ScaleToFit;
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                // To workaround our empty bitmap bug on Windows 7 we need to recreate our bitmap when the application is minimized
                // That's apparently not needed on Windows 10 but we better leave it in place.
                iCreateBitmap = true;
            }
        }

        /// <summary>
        /// Usually called twice due to our client shutdown process.
        /// The first one is canceled to give time for our clients to shutdown.
        /// When all clients are shutdown we Close our form again.
        /// TODO: This is messy and should be reworked.
        /// Should StopServer come first?
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            iCecManager.Stop();
            iNetworkManager.Dispose();
            DestroyAudioManager();
            DestroySpeechManager();
            CloseDisplayConnection();
            StopServer();
            e.Cancel = iClosing;
            if (e.Cancel)
            {
                // TODO: Have a function
                Properties.Settings.Default.PropertyChanged += PropertyChangedEventHandler;
            }
        }

        public void StartServer()
        {
            iServiceHost = new ServiceHost
                (
                typeof(Session),
                new Uri[] {new Uri("net.tcp://localhost:8001/")}
                );

            iServiceHost.AddServiceEndpoint(typeof(IService), new NetTcpBinding(SecurityMode.None, true),
                "DisplayService");
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
                if (
                    MessageBox.Show("Force exit?", "Waiting for clients...", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning) == DialogResult.Yes)
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

        /// <summary>
        /// 
        /// </summary>
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
                        client.Value.Callback.OnCloseOrder( /*eventData*/);
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
                    Program.iFormMain.iTreeViewClients.Nodes.Remove(
                        Program.iFormMain.iTreeViewClients.Nodes.Find(client, false)[0]);
                }
            }

            if (iClients.Count == 0)
            {
                ClearLayout(iTableLayoutPanelCurrentClient);
                iCurrentClientData = null;
            }
        }

        /// <summary>
        /// Just remove all our fields.
        /// </summary>
        static private void ClearLayout(TableLayoutPanel aPanel)
        {
            // For each loop did not work as calling Dispose on a control removes it from the collection.
            // We make sure every control are disposed of notably to turn off visualizer when no more needed.
            // That's the only way we found to make sure Control.Disposed is called in a timely fashion.
            // Though that loop is admetitly dangerous as if one of the control does not removes itself from the list we end up with infinite loop.
            // That's what happened with our MarqueeLabel until we fixed it's Dispose override.
            while (aPanel.Controls.Count>0)
            {
                // Dispose our last item
                aPanel.Controls[aPanel.Controls.Count - 1].Dispose();
            }

            DoClearLayout(aPanel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aPanel"></param>
        static private void DoClearLayout(TableLayoutPanel aPanel)
        {
            aPanel.Controls.Clear();
            aPanel.RowStyles.Clear();
            aPanel.ColumnStyles.Clear();
            aPanel.RowCount = 0;
            aPanel.ColumnCount = 0;
        }

        /// <summary>
        /// Just launch a demo client.
        /// </summary>
        private void StartNewClient(string aTopText = "", string aBottomText = "")
        {
            Thread clientThread = new Thread(SharpDisplayClient.Program.MainWithParams);
            SharpDisplayClient.StartParams myParams = new SharpDisplayClient.StartParams(
                new Point(this.Right, this.Top), aTopText, aBottomText);
            clientThread.Start(myParams);
            BringToFront();
        }

        /// <summary>
        /// Just launch our idle client.
        /// </summary>
        private void StartIdleClient(string aTopText = "", string aBottomText = "")
        {
            Thread clientThread = new Thread(SharpDisplayClientIdle.Program.MainWithParams);
            SharpDisplayClientIdle.StartParams myParams =
                new SharpDisplayClientIdle.StartParams(new Point(this.Right, this.Top), aTopText, aBottomText);
            clientThread.Start(myParams);
            BringToFront();
        }


        private void buttonStartClient_Click(object sender, EventArgs e)
        {
            StartNewClient();
        }

        private void buttonSuspend_Click(object sender, EventArgs e)
        {
            ToggleTimer();
        }

        private void StartTimer()
        {
            iLastTickTime = DateTime.Now; //Reset timer to prevent jump
            iTimerDisplay.Enabled = true;
            UpdateSuspendButton();
        }

        private void StopTimer()
        {
            iLastTickTime = DateTime.Now; //Reset timer to prevent jump
            iTimerDisplay.Enabled = false;
            UpdateSuspendButton();
        }

        private void ToggleTimer()
        {
            iLastTickTime = DateTime.Now; //Reset timer to prevent jump
            iTimerDisplay.Enabled = !iTimerDisplay.Enabled;
            UpdateSuspendButton();
        }

        private void UpdateSuspendButton()
        {
            if (!iTimerDisplay.Enabled)
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
            //Root node must have at least one child
            if (e.Node.Nodes.Count == 0)
            {
                return;
            }

            //If the selected node is the root node of a client then switch to it
            string sessionId = e.Node.Nodes[0].Text; //First child of a root node is the sessionId
            if (iClients.ContainsKey(sessionId)) //Check that's actually what we are looking at
            {
                //We have a valid session just switch to that client
                SetCurrentClient(sessionId, true);
            }

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
                this.Invoke(d, new object[] {aSessionId, aCallback});
            }
            else
            {
                //We are in the proper thread
                //Add this session to our collection of clients
                ClientData newClient = new ClientData(aSessionId, aCallback);
                Program.iFormMain.iClients.Add(aSessionId, newClient);
                //Add this session to our UI
                UpdateClientTreeViewNode(newClient);
            }
        }


        /// <summary>
        /// Find the client with the highest priority if any.
        /// </summary>
        /// <returns>Our highest priority client or null if not a single client is connected.</returns>
        public ClientData FindHighestPriorityClient()
        {
            ClientData highestPriorityClient = null;
            foreach (var client in iClients)
            {
                if (highestPriorityClient == null || client.Value.Priority > highestPriorityClient.Priority)
                {
                    highestPriorityClient = client.Value;
                }
            }

            return highestPriorityClient;
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
                this.Invoke(d, new object[] {aSessionId});
            }
            else
            {
                //We are in the proper thread
                //Remove this session from both client collection and UI tree view
                if (Program.iFormMain.iClients.Keys.Contains(aSessionId))
                {
                    Program.iFormMain.iClients.Remove(aSessionId);
                    Program.iFormMain.iTreeViewClients.Nodes.Remove(
                        Program.iFormMain.iTreeViewClients.Nodes.Find(aSessionId, false)[0]);
                    //Update recording status too whenever a client is removed
                    UpdateRecordingNotification();
                }

                if (iCurrentClientSessionId == aSessionId)
                {
                    //The current client is closing
                    iCurrentClientData = null;
                    //Find the client with the highest priority and set it as current
                    ClientData newCurrentClient = FindHighestPriorityClient();
                    if (newCurrentClient != null)
                    {
                        SetCurrentClient(newCurrentClient.SessionId, true);
                    }
                }

                if (iClients.Count == 0)
                {
                    //Clear our screen when last client disconnects
                    ClearLayout(iTableLayoutPanelCurrentClient);
                    iCurrentClientData = null;

                    if (iClosing)
                    {
                        //We were closing our form
                        //All clients are now closed
                        //Just resume our close operation
                        iClosing = false;
                        Close();
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="aSessionId"></param>
        /// <param name="aLayout"></param>
        public void SetClientLayoutThreadSafe(string aSessionId, TableLayout aLayout)
        {
            if (this.InvokeRequired)
            {
                //Not in the proper thread, invoke ourselves
                Invoke(new Action<FormMain>((sender) => { SetClientLayoutThreadSafe(aSessionId, aLayout); }), this);
                return;
            }

            ClientData client = iClients[aSessionId];
            if (client == null)
            {
                //TODO: logs
                return;
            }

            // If we have a matching client and we want to change the client layout
            if (client.Target == Target.Client)
            {
                //Don't change a thing if the layout is the same
                if (!client.View.Layout.IsSameAs(aLayout))
                {
                    Debug.Print("SetClientLayoutThreadSafe: Layout updated.");
                    //Set our client layout then
                    client.View.Layout = aLayout;
                    //So that next time we update all our fields at ones
                    client.HasNewLayout = true;
                    //Layout has changed clear our fields then
                    client.View.Fields.Clear();
                    //
                    UpdateClientTreeViewNode(client);
                }
                else
                {
                    Debug.Print("SetClientLayoutThreadSafe: Layout has not changed.");
                }
            }
            else if (client.Target == Target.Display)
            {
                // Mark our display layout has updated and wait for the fields.
                // Is display layout a property from our client?
                //iTableLayoutPanelDisplay. = aLayout;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="aSessionId"></param>
        /// <param name="aField"></param>
        public void SetClientFieldThreadSafe(string aSessionId, DataField aField)
        {
            if (this.InvokeRequired)
            {
                //Not in the proper thread, invoke ourselves
                Invoke(new Action<FormMain>((sender) => { SetClientFieldThreadSafe(aSessionId, aField); }), this);
            }
            else
            {
                //We are in the proper thread
                //Call the non-thread-safe variant
                SetClientField(aSessionId, aField);
            }
        }




        /// <summary>
        /// Set a data field in the given client.
        /// </summary>
        /// <param name="aSessionId"></param>
        /// <param name="aField"></param>
        private void SetClientField(string aSessionId, DataField aField)
        {
            //TODO: should check if the field actually changed?

            ClientData client = iClients[aSessionId];
            bool layoutChanged = false;
            bool contentChanged = true;

            //Fetch our field index
            int fieldIndex = client.View.FindSameFieldIndex(aField);

            if (fieldIndex < 0)
            {
                //No corresponding field, just bail out
                return;
            }

            //Keep our previous field in there
            DataField previousField = client.View.Fields[fieldIndex];
            //Just update that field then 
            client.View.Fields[fieldIndex] = aField;

            if (!aField.IsTableField)
            {
                //We are done then if that field is not in our table layout
                return;
            }

            TableField tableField = (TableField) aField;

            if (previousField.IsSameLayout(aField))
            {
                //If we are updating a field in our current client we need to update it in our panel
                if (aSessionId == iCurrentClientSessionId)
                {
                    Control ctrl = iTableLayoutPanelCurrentClient.GetControlFromPosition(tableField.Column, tableField.Row);
                    if (aField.IsTextField && ctrl is MarqueeLabel)
                    {
                        TextField textField = (TextField)aField;
                        //Text field control already in place, just change the text
                        MarqueeLabel label = (MarqueeLabel)ctrl;
                        contentChanged = (label.Text != textField.Text || label.TextAlign != textField.Alignment);
                        label.Text = textField.Text;
                        label.TextAlign = textField.Alignment;
                    }
                    else if (aField.IsBitmapField && ctrl is PictureBox)
                    {
                        BitmapField bitmapField = (BitmapField)aField;
                        contentChanged = true; //TODO: Bitmap comp or should we leave that to clients?
                        //Bitmap field control already in place just change the bitmap
                        PictureBox pictureBox = (PictureBox)ctrl;
                        pictureBox.Image = bitmapField.Bitmap;
                    }
                    else if (aField is AudioVisualizerField && ctrl is PictureBox)
                    {
                        contentChanged = false; // Since nothing was changed
                    }
                    else
                    {
                        layoutChanged = true;
                    }
                }
            }
            else
            {
                layoutChanged = true;
            }

            //If either content or layout changed we need to update our tree view to reflect the changes
            if (contentChanged || layoutChanged)
            {
                UpdateClientTreeViewNode(client);
                //
                if (layoutChanged)
                {
                    Debug.Print("Layout changed");
                    //Our layout has changed, if we are already the current client we need to update our panel
                    if (aSessionId == iCurrentClientSessionId)
                    {
                        //Apply layout and set data fields.
                        UpdateTableLayoutPanel(iCurrentClientData.View, iTableLayoutPanelCurrentClient);
                    }
                }
                else
                {
                    Debug.Print("Layout has not changed.");
                }
            }
            else
            {
                Debug.Print("WARNING: content and layout have not changed!");
            }

            //When a client field is set we try switching to this client to present the new information to our user
            SetCurrentClient(aSessionId);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="aTexts"></param>
        public void SetClientFieldsThreadSafe(string aSessionId, System.Collections.Generic.IList<DataField> aFields)
        {
            if (this.InvokeRequired)
            {
                //Not in the proper thread, invoke ourselves
                Invoke(new Action<FormMain>((sender) => { SetClientFieldsThreadSafe(aSessionId, aFields); }), this);
            }
            else
            {
                ClientData client = iClients[aSessionId];

                if (client.HasNewLayout)
                {
                    //TODO: Assert client.Count == 0
                    //Our layout was just changed
                    //Do some special handling to avoid re-creating our panel N times, once for each fields
                    client.HasNewLayout = false;
                    //Just set all our fields then
                    client.View.Fields.AddRange(aFields);
                    //Try switch to that client
                    SetCurrentClient(aSessionId);

                    //If we are updating the current client update our panel
                    if (aSessionId == iCurrentClientSessionId)
                    {
                        //Apply layout and set data fields.
                        UpdateTableLayoutPanel(iCurrentClientData.View, iTableLayoutPanelCurrentClient);
                    }

                    UpdateClientTreeViewNode(client);
                }
                else
                {
                    //Put each our text fields in a label control
                    foreach (DataField field in aFields)
                    {
                        SetClientField(aSessionId, field);
                    }
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
                Invoke(new Action<FormMain>((sender) => { SetClientNameThreadSafe(aSessionId, aName); }), this);
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

        ///
        public void SetClientPriorityThreadSafe(string aSessionId, uint aPriority)
        {
            if (this.InvokeRequired)
            {
                //Not in the proper thread, invoke ourselves
                Invoke(new Action<FormMain>((sender) => { SetClientPriorityThreadSafe(aSessionId, aPriority); }), this);
            }
            else
            {
                //We are in the proper thread
                //Get our client
                ClientData client = iClients[aSessionId];
                if (client != null)
                {
                    //Set its priority
                    client.Priority = aPriority;
                    //Update our tree-view
                    UpdateClientTreeViewNode(client);
                    //Change our current client as per new priority
                    ClientData newCurrentClient = FindHighestPriorityClient();
                    if (newCurrentClient != null)
                    {
                        SetCurrentClient(newCurrentClient.SessionId);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aSessionId"></param>
        /// <param name="aPriority"></param>
        public void SetClientTargetThreadSafe(string aSessionId, Target aTarget)
        {
            if (this.InvokeRequired)
            {
                //Not in the proper thread, invoke ourselves
                Invoke(new Action<FormMain>((sender) => { SetClientTargetThreadSafe(aSessionId, aTarget); }), this);
            }
            else
            {
                //We are in the proper thread
                //Get our client
                ClientData client = iClients[aSessionId];
                if (client != null)
                {
                    //Set its priority
                    client.Target = aTarget;
                    //Update our tree-view
                    //UpdateClientTreeViewNode(client);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="maxChars"></param>
        /// <returns></returns>
        public static string Truncate(string value, int maxChars)
        {
            return value.Length <= maxChars ? value : value.Substring(0, maxChars - 3) + "...";
        }

        /// <summary>
        /// Update our recording notification.
        /// </summary>
        private void UpdateRecordingNotification()
        {
            //Go through each 
            bool activeRecording = false;
            string text = "";
            RecordingField recField = new RecordingField();
            foreach (var client in iClients)
            {
                RecordingField rec = (RecordingField) client.Value.View.FindSameFieldAs(recField);
                if (rec != null && rec.IsActive)
                {
                    activeRecording = true;
                    //Don't break cause we are collecting the names/texts.
                    if (!String.IsNullOrEmpty(rec.Text))
                    {
                        text += (rec.Text + "\n");
                    }
                    else
                    {
                        //Not text for that recording, use client name instead
                        text += client.Value.Name + " recording\n";
                    }

                }
            }

            //Update our text no matter what, can't have more than 63 characters otherwise it throws an exception.
            iRecordingNotification.Text = Truncate(text, 63);

            //Change visibility of notification if needed
            if (iRecordingNotification.Visible != activeRecording)
            {
                iRecordingNotification.Visible = activeRecording;
                //Assuming the notification icon is in sync with our display icon
                //Take care of our REC icon
                if (iDisplay.IsOpen())
                {
                    iDisplay.SetIconOnOff(IconType.Recording, activeRecording);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="aClient"></param>
        private void UpdateClientTreeViewNode(ClientData aClient)
        {
            Debug.Print("UpdateClientTreeViewNode");

            if (aClient == null)
            {
                return;
            }

            //Hook in record icon update too
            UpdateRecordingNotification();

            TreeNode node = null;
            //Check that our client node already exists
            //Get our client root node using its key which is our session ID
            TreeNode[] nodes = iTreeViewClients.Nodes.Find(aClient.SessionId, false);
            if (nodes.Count() > 0)
            {
                //We already have a node for that client
                node = nodes[0];
                //Clear children as we are going to recreate them below
                node.Nodes.Clear();
            }
            else
            {
                //Client node does not exists create a new one
                iTreeViewClients.Nodes.Add(aClient.SessionId, aClient.SessionId);
                node = iTreeViewClients.Nodes.Find(aClient.SessionId, false)[0];
            }

            if (node != null)
            {
                //Change its name
                if (!String.IsNullOrEmpty(aClient.Name))
                {
                    //We have a name, use it as text for our root node
                    node.Text = aClient.Name;
                    //Add a child with SessionId
                    node.Nodes.Add(new TreeNode(aClient.SessionId));
                }
                else
                {
                    //No name, use session ID instead
                    node.Text = aClient.SessionId;
                }

                //Display client priority
                node.Nodes.Add(new TreeNode("Priority: " + aClient.Priority));

                if (aClient.View.Fields.Count > 0)
                {
                    //Create root node for our texts
                    TreeNode textsRoot = new TreeNode("Fields");
                    node.Nodes.Add(textsRoot);
                    //For each text add a new entry
                    foreach (DataField field in aClient.View.Fields)
                    {
                        if (field.IsTextField)
                        {
                            TextField textField = (TextField) field;
                            textsRoot.Nodes.Add(new TreeNode("[Text]" + textField.Text));
                        }
                        else if (field.IsBitmapField)
                        {
                            textsRoot.Nodes.Add(new TreeNode("[Bitmap]"));
                        }
                        else if (field is AudioVisualizerField)
                        {
                            textsRoot.Nodes.Add(new TreeNode("[Audio Visualizer]"));
                        }
                        else if (field.IsRecordingField)
                        {
                            RecordingField recordingField = (RecordingField) field;
                            textsRoot.Nodes.Add(new TreeNode("[Recording]" + recordingField.IsActive));
                        }
                    }
                }

                node.ExpandAll();
            }
        }

        /// <summary>
        /// Update our display table layout.
        /// Will instantiate every field control as defined by our client.
        /// Fields must be specified by rows from the left.
        /// </summary>
        /// <param name="aLayout"></param>
        private void UpdateTableLayoutPanel(SharpLib.Display.View aView, TableLayoutPanel aPanel)
        {
            Debug.Print("UpdateTableLayoutPanel");

            if (aView == null)
            {
                //Just drop it
                return;
            }


            TableLayout layout = aView.Layout;

            //First clean our current panel
            ClearLayout(aPanel);

            //Then recreate our rows...
            while (aPanel.RowCount < layout.Rows.Count)
            {
                aPanel.RowCount++;
            }

            // ...and columns 
            while (aPanel.ColumnCount < layout.Columns.Count)
            {
                aPanel.ColumnCount++;
            }

            //For each column
            for (int i = 0; i < aPanel.ColumnCount; i++)
            {
                //Create our column styles
                aPanel.ColumnStyles.Add(layout.Columns[i]);

                //For each rows
                for (int j = 0; j < aPanel.RowCount; j++)
                {
                    if (i == 0)
                    {
                        //Create our row styles
                        aPanel.RowStyles.Add(layout.Rows[j]);
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            //For each field
            foreach (DataField field in aView.Fields)
            {
                if (!field.IsTableField)
                {
                    //That field is not taking part in our table layout skip it
                    continue;
                }

                TableField tableField = (TableField) field;

                //Create a control corresponding to the field specified for that cell
                Control control = CreateControlForDataField(tableField);

                //Add newly created control to our table layout at the specified row and column
                aPanel.Controls.Add(control, tableField.Column, tableField.Row);
                //Make sure we specify column and row span for that new control
                aPanel.SetColumnSpan(control, tableField.ColumnSpan);
                aPanel.SetRowSpan(control, tableField.RowSpan);
            }


            CheckFontHeight();
        }

        /// <summary>
        /// Check our type of data field and create corresponding control
        /// </summary>
        /// <param name="aField"></param>
        private Control CreateControlForDataField(DataField aField)
        {
            Control control = null;
            if (aField.IsTextField)
            {
                MarqueeLabel label = new SharpDisplayManager.MarqueeLabel();
                label.AutoEllipsis = true;
                label.AutoSize = true;
                label.BackColor = System.Drawing.Color.Transparent;
                label.Dock = System.Windows.Forms.DockStyle.Fill;
                label.Location = new System.Drawing.Point(1, 1);
                label.Margin = new System.Windows.Forms.Padding(0);
                label.Name = "marqueeLabel" + aField;
                label.OwnTimer = false;
                label.PixelsPerSecond = cds.ScrollingSpeedInPixelsPerSecond;
                label.Separator = cds.Separator;
                label.MinFontSize = cds.MinFontSize;
                label.ScaleToFit = cds.ScaleToFit;
                //control.Size = new System.Drawing.Size(254, 30);
                //control.TabIndex = 2;
                label.Font = cds.Font;

                TextField field = (TextField)aField;
                label.TextAlign = field.Alignment;
                label.UseCompatibleTextRendering = true;
                label.Text = field.Text;
                //
                control = label;
            }
            else if (aField.IsBitmapField)
            {
                //Create picture box
                PictureBox picture = new PictureBox();
                picture.AutoSize = true;
                picture.BackColor = System.Drawing.Color.Transparent;
                picture.Dock = System.Windows.Forms.DockStyle.Fill;
                picture.Location = new System.Drawing.Point(1, 1);
                picture.Margin = new System.Windows.Forms.Padding(0);
                picture.Name = "pictureBox" + aField;
                //Set our image
                BitmapField field = (BitmapField)aField;
                picture.Image = field.Bitmap;
                //
                control = picture;
            }
            else if (aField is AudioVisualizerField)
            {
                //Create picture box
                PictureBox picture = new PictureBox();
                picture.AutoSize = true;
                picture.BackColor = System.Drawing.Color.Transparent;
                picture.Dock = System.Windows.Forms.DockStyle.Fill;
                picture.Location = new System.Drawing.Point(1, 1);
                picture.Margin = new System.Windows.Forms.Padding(0);
                picture.Name = "pictureBox" + aField;

                // Make sure visualization is running                
                if (iAudioManager != null) // When closing main form with multiple client audio manager can be null. I guess we should fix the core issue instead.
                {
                    iAudioManager.AddVisualizer();
                }

                // Notify audio manager when we don't need audio visualizer anymore
                picture.Disposed += (sender, e) =>
                {
                    if (iAudioManager != null)
                    {
                        // Make sure we stop visualization when not needed
                        iAudioManager.RemoveVisualizer();
                    }
                };

                // Create a new bitmap when control size changes
                picture.SizeChanged += (sender, e) =>
                {
                    // Somehow bitmap created when our from is invisible are not working
                    // Mark our form visibility status
                    bool visible = Visible;
                    // Make sure it's visible
                    Visible = true;
                    // Adjust our bitmap size when control size changes
                    picture.Image = new System.Drawing.Bitmap(picture.Width, picture.Height, PixelFormat.Format32bppArgb);
                    // Restore our form visibility
                    Visible = visible;
                };
                
                control = picture;
            }

            //TODO: Handle recording field?

            return control;
        }

        /// <summary>
        /// Called when the user selected a new display type.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxDisplayType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Store the selected display type in our settings
            if (!iUpdatingStatus)
            {
                Properties.Settings.Default.CurrentDisplayIndex = comboBoxDisplayType.SelectedIndex;
                cds.DisplayType = comboBoxDisplayType.SelectedIndex;
                Properties.Settings.Default.Save();
            }
            //Try re-opening the display connection if we were already connected.
            //Otherwise just update our status to reflect display type change.
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
                    iTimerDisplay.Interval = interval;
                    if (!iUpdatingStatus)
                    {
                        cds.TimerInterval = iTimerDisplay.Interval;
                        Properties.Settings.Default.Save();
                    }
                }
            }
        }

        private void maskedTextBoxMinFontSize_TextChanged(object sender, EventArgs e)
        {
            if (maskedTextBoxMinFontSize.Text != "")
            {
                int minFontSize = Convert.ToInt32(maskedTextBoxMinFontSize.Text);

                if (minFontSize > 0)
                {
                    if (!iUpdatingStatus)
                    {
                        cds.MinFontSize = minFontSize;
                        Properties.Settings.Default.Save();
                    }
                    //We need to recreate our layout for that change to take effect
                    if (iCurrentClientData != null)
                    {
                        UpdateTableLayoutPanel(iCurrentClientData.View, iTableLayoutPanelCurrentClient);
                    }
                }
            }
        }


        private void maskedTextBoxScrollingSpeed_TextChanged(object sender, EventArgs e)
        {
            if (maskedTextBoxScrollingSpeed.Text != "")
            {
                int scrollingSpeed = Convert.ToInt32(maskedTextBoxScrollingSpeed.Text);

                if (scrollingSpeed > 0)
                {
                    if (!iUpdatingStatus)
                    {
                        cds.ScrollingSpeedInPixelsPerSecond = scrollingSpeed;
                        Properties.Settings.Default.Save();
                    }
                    //We need to recreate our layout for that change to take effect
                    if (iCurrentClientData != null)
                    {
                        UpdateTableLayoutPanel(iCurrentClientData.View, iTableLayoutPanelCurrentClient);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxScrollLoopSeparator_TextChanged(object sender, EventArgs e)
        {
            if (!iUpdatingStatus)
            {
                cds.Separator = textBoxScrollLoopSeparator.Text;
                Properties.Settings.Default.Save();
            }

            //Update our text fields
            UpdateMarqueesSeparator(iTableLayoutPanelDisplay);
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
            ShowClock();
        }

        private void buttonHideClock_Click(object sender, EventArgs e)
        {
            HideClock();
        }

        async private void buttonUpdate_Click(object sender, EventArgs e)
        {
            //InstallUpdateSyncWithInfo();
            SquirrelUpdate();
        }

        /// <summary>
        /// 
        /// </summary>
        void ShowClock()
        {
            if (!iDisplay.IsOpen())
            {
                return;
            }

            //Devices like MDM166AA don't support windowing and frame rendering must be stopped while showing our clock
            iSkipFrameRendering = true;
            //Clear our screen 
            iDisplay.Clear();
            iDisplay.SwapBuffers();
            //Then show our clock
            iDisplay.ShowClock();
        }

        /// <summary>
        /// 
        /// </summary>
        void HideClock()
        {
            if (!iDisplay.IsOpen())
            {
                return;
            }

            //Devices like MDM166AA don't support windowing and frame rendering must be stopped while showing our clock
            iSkipFrameRendering = false;
            iDisplay.HideClock();
        }

        ///
        /// ClickOnce update trigger.
        /// TODO: Remove this now that we migrated to Squirrel.
        ///
        private void InstallUpdateSyncWithInfo()
        {
            UpdateCheckInfo info = null;

            if (ApplicationDeployment.IsNetworkDeployed)
            {
                ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;

                try
                {
                    info = ad.CheckForDetailedUpdate();

                }
                catch (DeploymentDownloadException dde)
                {
                    MessageBox.Show(
                        "The new version of the application cannot be downloaded at this time. \n\nPlease check your network connection, or try again later. Error: " +
                        dde.Message);
                    return;
                }
                catch (InvalidDeploymentException ide)
                {
                    MessageBox.Show(
                        "Cannot check for a new version of the application. The ClickOnce deployment is corrupt. Please redeploy the application and try again. Error: " +
                        ide.Message);
                    return;
                }
                catch (InvalidOperationException ioe)
                {
                    MessageBox.Show(
                        "This application cannot be updated. It is likely not a ClickOnce application. Error: " +
                        ioe.Message);
                    return;
                }

                if (info.UpdateAvailable)
                {
                    Boolean doUpdate = true;

                    if (!info.IsUpdateRequired)
                    {
                        DialogResult dr =
                            MessageBox.Show("An update is available. Would you like to update the application now?",
                                "Update Available", MessageBoxButtons.OKCancel);
                        if (!(DialogResult.OK == dr))
                        {
                            doUpdate = false;
                        }
                    }
                    else
                    {
                        // Display a message that the application MUST reboot. Display the minimum required version.
                        MessageBox.Show("This application has detected a mandatory update from your current " +
                                        "version to version " + info.MinimumRequiredVersion.ToString() +
                                        ". The application will now install the update and restart.",
                            "Update Available", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }

                    if (doUpdate)
                    {
                        try
                        {
                            ad.Update();
                            MessageBox.Show("The application has been upgraded, and will now restart.");
                            Application.Restart();
                        }
                        catch (DeploymentDownloadException dde)
                        {
                            MessageBox.Show(
                                "Cannot install the latest version of the application. \n\nPlease check your network connection, or try again later. Error: " +
                                dde);
                            return;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("You are already running the latest version.", "Application up-to-date");
                }
            }
        }


        /// <summary>
        /// Used to
        /// </summary>
        private void SysTrayHideShow()
        {
            Visible = !Visible;
            if (Visible)
            {
                Activate();
                WindowState = FormWindowState.Normal;
            }
        }

        /// <summary>
        /// Use to handle minimize events.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized && Properties.Settings.Default.MinimizeToTray)
            {
                if (Visible)
                {
                    SysTrayHideShow();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tableLayoutPanel_SizeChanged(object sender, EventArgs e)
        {
            //Our table layout size has changed which means our display size has changed.
            //We need to re-create our bitmap.
            iCreateBitmap = true;
        }

        /// <summary>
        /// Broadcast messages to subscribers.
        /// </summary>
        /// <param name="message"></param>
        protected override void WndProc(ref Message aMessage)
        {
            if (OnWndProc != null)
            {
                OnWndProc(ref aMessage);
            }

            if (aMessage.Msg==Const.WM_CLOSE)
            {
                // If a settings was changed durring this session for some reason the chnaged handler will be called again when closing
                // To work around this we remove our handler now.
                // If our closing is cancel we put it back on see _FormClosing
                Properties.Settings.Default.PropertyChanged -= PropertyChangedEventHandler;
            }

            base.WndProc(ref aMessage);


        }


        private void comboBoxHdmiPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Save CEC HDMI port
            if (!iUpdatingStatus)
            {
                byte index = Convert.ToByte(iComboBoxHdmiPort.SelectedIndex);
                index++;
                Properties.Settings.Default.CecHdmiPort = index;
            }
            //
            ResetCec();
        }


        /// <summary>
        /// 
        /// </summary>
        private void ResetCec()
        {
            if (iCecManager == null)
            {
                //Thus skipping initial UI setup
                return;
            }

            iCecManager.Stop();
            //
            if (Properties.Settings.Default.CecEnabled)
            {
                iCecManager.Start(Handle, "CEC", Properties.Settings.Default.CecHdmiPort);

                SetupCecLogLevel();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private async void ResetHarmonyAsync(bool aForceAuth=false)
        {
            // ConnectAsync already if we have an existing session cookie
            if (Properties.Settings.Default.HarmonyEnabled)
            {
                try
                {
                    iButtonHarmonyConnect.Enabled = false;
                    await ConnectHarmonyAsync(aForceAuth);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine("Exception thrown by ConnectHarmonyAsync");
                    Trace.WriteLine(ex.ToString());
                }
                finally
                {
                    iButtonHarmonyConnect.Enabled = true;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iButtonHarmonyConnect_Click(object sender, EventArgs e)
        {
            // User is explicitly trying to connect
            //Reset Harmony Hub connection forcing authentication
            ResetHarmonyAsync(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iCheckBoxHarmonyEnabled_CheckedChanged(object sender, EventArgs e)
        {
            iButtonHarmonyConnect.Enabled = iCheckBoxHarmonyEnabled.Checked;
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetupCecLogLevel()
        {
            //Setup log level
            iCecManager.Client.LogLevel = 0;

            if (checkBoxCecLogError.Checked)
                iCecManager.Client.LogLevel |= (int) CecLogLevel.Error;

            if (checkBoxCecLogWarning.Checked)
                iCecManager.Client.LogLevel |= (int) CecLogLevel.Warning;

            if (checkBoxCecLogNotice.Checked)
                iCecManager.Client.LogLevel |= (int) CecLogLevel.Notice;

            if (checkBoxCecLogTraffic.Checked)
                iCecManager.Client.LogLevel |= (int) CecLogLevel.Traffic;

            if (checkBoxCecLogDebug.Checked)
                iCecManager.Client.LogLevel |= (int) CecLogLevel.Debug;

            iCecManager.Client.FilterOutPollLogs = checkBoxCecLogNoPoll.Checked;

        }

        private void ButtonStartIdleClient_Click(object sender, EventArgs e)
        {
            StartIdleClient();
        }

        private void buttonClearLogs_Click(object sender, EventArgs e)
        {
            richTextBoxLogs.Clear();
        }

        private void checkBoxCecLogs_CheckedChanged(object sender, EventArgs e)
        {
            SetupCecLogLevel();
        }





        /// <summary>
        /// Get the current event based on event tree view selection.
        /// </summary>
        /// <returns></returns>
        private Ear.Event CurrentEvent()
        {
            //Walk up the tree from the selected node to find our event
            TreeNode node = iTreeViewEvents.SelectedNode;
            Ear.Event selectedEvent = null;
            while (node != null)
            {
                if (node.Tag is Ear.Event)
                {
                    selectedEvent = (Ear.Event) node.Tag;
                    break;
                }
                node = node.Parent;
            }
            return selectedEvent;
        }

        /// <summary>
        /// Get the current action based on event tree view selection
        /// </summary>
        /// <returns></returns>
        private Ear.Action CurrentAction()
        {
            TreeNode node = iTreeViewEvents.SelectedNode;
            if (node != null && node.Tag is Ear.Action)
            {
                return (Ear.Action) node.Tag;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Ear.Object CurrentEarObject()
        {
            Ear.Action a = CurrentAction();
            Ear.Event e = CurrentEvent();

            if (a != null)
            {
                return a;
            }

            return e;
        }

        /// <summary>
        /// Get the current action based on event tree view selection
        /// </summary>
        /// <returns></returns>
        private Ear.Object CurrentEarParent()
        {
            TreeNode node = iTreeViewEvents.SelectedNode;
            if (node == null || node.Parent == null)
            {
                return null;
            }

            if (node.Parent.Tag is Ear.Object)
            {
                return (Ear.Object)node.Parent.Tag;
            }

            if (node.Parent.Parent != null && node.Parent.Parent.Tag is Ear.Object)
            {
                //Can be the case for events
                return (Ear.Object)node.Parent.Parent.Tag;
            }

            return null;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonActionAdd_Click(object sender, EventArgs e)
        {
            Ear.Object parent = CurrentEarObject();
            if (parent == null )
            {
                //We did not find a corresponding event or action
                return;
            }

            FormEditObject<Ear.Action> ea = new FormEditObject<Ear.Action>();
            ea.Text = "Add action";
            DialogResult res = CodeProject.Dialog.DlgBox.ShowDialog(ea);
            if (res == DialogResult.OK)
            {
                parent.Objects.Add(ea.Object);               
                Properties.Settings.Default.Save();
                // We want to select the parent so that one can easily add another action to the same collection
                PopulateTreeViewEvents(parent);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonActionEdit_Click(object sender, EventArgs e)
        {
            Ear.Action selectedAction = CurrentAction();
            Ear.Object parent = CurrentEarParent()
            ;
            if (parent == null || selectedAction == null)
            {
                //We did not find a corresponding parent
                return;
            }

            FormEditObject<Ear.Action> ea = new FormEditObject<Ear.Action>();
            ea.Text = "Edit action";
            ea.Object = selectedAction;
            // Find our action within its parent so that we can update it later
            int actionIndex = parent.Objects.FindIndex(x => x == selectedAction);
            DialogResult res = CodeProject.Dialog.DlgBox.ShowDialog(ea);
            if (res == DialogResult.OK)
            {
                //Make sure we keep the same children as before
                ea.Object.Objects = parent.Objects[actionIndex].Objects;
                //Update our action
                parent.Objects[actionIndex]=ea.Object;
                //Save and rebuild our event tree view
                Properties.Settings.Default.Save();
                PopulateTreeViewEvents(ea.Object);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonActionDelete_Click(object sender, EventArgs e)
        {
            Ear.Action action = CurrentAction();
            if (action == null)
            {
                //Must select action node
                return;
            }

            Properties.Settings.Default.EarManager.RemoveAction(action);
            Properties.Settings.Default.Save();
            PopulateTreeViewEvents();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonActionTest_Click(object sender, EventArgs e)
        {
            Ear.Action a = CurrentAction();
            if (a != null)
            {
                a.Test();
            }
            iTreeViewEvents.Focus();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonActionMoveUp_Click(object sender, EventArgs e)
        {
            Ear.Action a = CurrentAction();
            if (a == null || 
                //Action already at the top of the list
                iTreeViewEvents.SelectedNode.Index == 0)
            {
                return;
            }

            //Swap actions in event's action list
            Ear.Object parent = CurrentEarParent();
            int currentIndex = iTreeViewEvents.SelectedNode.Index;
            Ear.Action movingUp = parent.Objects[currentIndex] as Ear.Action;
            Ear.Action movingDown = parent.Objects[currentIndex-1] as Ear.Action;
            parent.Objects[currentIndex] = movingDown;
            parent.Objects[currentIndex-1] = movingUp;

            //Save and populate our tree again
            Properties.Settings.Default.Save();
            PopulateTreeViewEvents(a);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonActionMoveDown_Click(object sender, EventArgs e)
        {
            Ear.Action a = CurrentAction();
            if (a == null ||
                //Action already at the bottom of the list
                iTreeViewEvents.SelectedNode.Index == iTreeViewEvents.SelectedNode.Parent.Nodes.Count - 1)
            {
                return;
            }

            //Swap actions in event's action list
            Ear.Object parent = CurrentEarParent();
            int currentIndex = iTreeViewEvents.SelectedNode.Index;
            Ear.Action movingDown = parent.Objects[currentIndex] as Ear.Action;
            Ear.Action movingUp = parent.Objects[currentIndex + 1] as Ear.Action;
            parent.Objects[currentIndex] = movingUp;
            parent.Objects[currentIndex + 1] = movingDown;

            //Save and populate our tree again
            Properties.Settings.Default.Save();
            PopulateTreeViewEvents(a);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonEventTest_Click(object sender, EventArgs e)
        {
            Ear.Event earEvent = CurrentEvent();
            if (earEvent != null)
            {
                earEvent.Test();
            }
        }

        /// <summary>
        /// Manages events and actions buttons according to selected item in event tree.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iTreeViewEvents_AfterSelect(object sender, TreeViewEventArgs e)
        {
            UpdateEventView();
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateEventView()
        {
            //One can always add an event
            buttonEventAdd.Enabled = true;

            //Enable buttons according to selected item
            buttonActionAdd.Enabled =
            buttonEventTest.Enabled =
            buttonEventDelete.Enabled =
            buttonEventEdit.Enabled =
                CurrentEvent() != null;

            Ear.Action currentAction = CurrentAction();
            //If an action is selected enable the following buttons
            buttonActionTest.Enabled =
            buttonActionDelete.Enabled =
            buttonActionMoveUp.Enabled =
            buttonActionMoveDown.Enabled =
            buttonActionEdit.Enabled =
                    currentAction != null;

            if (currentAction != null)
            {
                //If an action is selected enable move buttons if needed
                buttonActionMoveUp.Enabled = iTreeViewEvents.SelectedNode.Index != 0;
                buttonActionMoveDown.Enabled = iTreeViewEvents.SelectedNode.Index <
                                               iTreeViewEvents.SelectedNode.Parent.Nodes.Count - 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonEventAdd_Click(object sender, EventArgs e)
        {
            FormEditObject<Ear.Event> ea = new FormEditObject<Ear.Event>();
            ea.Text = "Add event";
            DialogResult res = CodeProject.Dialog.DlgBox.ShowDialog(ea);
            if (res == DialogResult.OK)
            {
                Properties.Settings.Default.EarManager.Events.Add(ea.Object);
                Properties.Settings.Default.Save();
                PopulateTreeViewEvents(ea.Object);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonEventDelete_Click(object sender, EventArgs e)
        {
            Ear.Event currentEvent = CurrentEvent();
            if (currentEvent == null)
            {
                //Must select action node
                return;
            }

            Properties.Settings.Default.EarManager.Events.Remove(currentEvent);
            Properties.Settings.Default.Save();
            PopulateTreeViewEvents();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonEventEdit_Click(object sender, EventArgs e)
        {
            Ear.Event selectedEvent = CurrentEvent();
            if (selectedEvent == null)
            {
                //We did not find a corresponding event
                return;
            }

            FormEditObject<Ear.Event> ea = new FormEditObject<Ear.Event>();
            ea.Text = "Edit event";
            ea.Object = selectedEvent;
            // Get the index of our event so that we can update it below
            int index = Properties.Settings.Default.EarManager.Events.FindIndex(x => x == selectedEvent);
            DialogResult res = CodeProject.Dialog.DlgBox.ShowDialog(ea);
            if (res == DialogResult.OK)
            {                
                //Make sure we keep the same actions as before
                ea.Object.Objects = Properties.Settings.Default.EarManager.Events[index].Objects;
                //Update our event
                Properties.Settings.Default.EarManager.Events[index] = ea.Object;
                //Save and rebuild our event tree view
                Properties.Settings.Default.Save();
                PopulateTreeViewEvents(ea.Object);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iTreeViewEvents_Leave(object sender, EventArgs e)
        {
            //Make sure our event tree never looses focus
            ((TreeView) sender).Focus();
        }


        /// <summary>
        /// 
        /// </summary>
        private async Task CreateFritzBoxClient()
        {
            DestroyFritzBoxClient();

            if (!Properties.Settings.Default.FritzBoxEnabled)
            {
                return;
            }
            
            Program.FritzBoxClient = new SmartHome.Client(Properties.Settings.Default.FritzBoxUrl);
            iTextBoxFritzBoxUrl.Enabled = false; // Can't change URL of existing client
            await Program.FritzBoxClient.Authenticate(Properties.Settings.Default.FritzBoxLogin, Properties.Settings.Default.FritzBoxPassword);
            await PopulateFritzBoxTreeView();
        }

        /// <summary>
        /// 
        /// </summary>
        private void DestroyFritzBoxClient()
        {
            if (Program.FritzBoxClient != null)
            {
                Program.FritzBoxClient.Dispose();
                Program.FritzBoxClient = null;
            }

            //Allow updating the URL again
            iTextBoxFritzBoxUrl.Enabled = true;
            iTreeViewFritzBox.Nodes.Clear();
        }

        private async Task PopulateFritzBoxTreeView()
        {
            SmartHome.DeviceList deviceList = await Program.FritzBoxClient.GetDeviceList();
            PopulateDevicesTree(deviceList);
        }


        /// <summary>
        /// Populate our tree view with our devices information.
        /// </summary>
        /// <param name="aDeviceList"></param>
        void PopulateDevicesTree(SmartHome.DeviceList aDeviceList)
        {
            iTreeViewFritzBox.Nodes.Clear();

            // For each device
            foreach (SmartHome.Device device in aDeviceList.Devices)
            {
                // Add a new node
                TreeNode deviceNode = iTreeViewFritzBox.Nodes.Add(device.Id, $"{device.Name} - {device.ProductName} by {device.Manufacturer}");
                deviceNode.Tag = device;

                // Check the functions of that device
                foreach (SmartHome.Function f in Enum.GetValues(typeof(SmartHome.Function)))
                {
                    if (device.Has(f))
                    {
                        // Add a new node for each supported function
                        TreeNode functionNode = deviceNode.Nodes.Add(f.ToString());
                        if (f == SmartHome.Function.TemperatureSensor)
                        {
                            // Add temperature sensor data
                            functionNode.Nodes.Add($"{device.Temperature.Reading} °C");
                            functionNode.Nodes.Add($"Offset: {device.Temperature.OffsetReading} °C");
                        }
                        else if (f == SmartHome.Function.SwitchSocket)
                        {
                            // Add switch socket data
                            functionNode.Nodes.Add($"Mode: {device.Switch.Mode.ToString()}");
                            functionNode.Nodes.Add($"Switched {device.Switch.State.ToString()}");
                            functionNode.Nodes.Add($"Lock: {device.Switch.Lock.ToString()}");
                            functionNode.Nodes.Add($"Device lock: {device.Switch.DeviceLock.ToString()}");
                        }
                        else if (f == SmartHome.Function.RadiatorThermostat)
                        {
                            // Add radiator thermostat data                            
                            functionNode.Nodes.Add($"Comfort temperature: {device.Thermostat.ComfortTemperatureInCelsius.ToString()} °C");
                            functionNode.Nodes.Add($"Economy temperature: {device.Thermostat.EconomyTemperatureInCelsius.ToString()} °C");
                            functionNode.Nodes.Add($"Current temperature: {device.Thermostat.CurrentTemperatureInCelsius.ToString()} °C");
                            functionNode.Nodes.Add($"Target temperature: {device.Thermostat.TargetTemperatureInCelsius.ToString()} °C");
                            functionNode.Nodes.Add($"Battery {device.Thermostat.Battery.ToString()}");
                            functionNode.Nodes.Add($"Lock: {device.Thermostat.Lock.ToString()}");
                            functionNode.Nodes.Add($"Device lock: {device.Thermostat.DeviceLock.ToString()}");
                        }
                        else if (f == SmartHome.Function.PowerMeter)
                        {
                            // Add power meter data
                            functionNode.Nodes.Add($"Power: {device.PowerMeter.PowerInWatt}W");
                            functionNode.Nodes.Add($"Energy: {device.PowerMeter.EnergyInKiloWattPerHour}kWh");
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Called whenever we loose connection with our HarmonyHub.
        /// </summary>
        /// <param name="aRequestWasCancelled"></param>
        private void HarmonyConnectionClosed(object aSender, bool aClosedByServer)
        {
            if (aClosedByServer)
            {
                //Try reconnect then
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                BeginInvoke(new MethodInvoker(delegate () { ResetHarmonyAsync(); }));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }
        }



        int iHarmonyReconnectTries = 0;
        const int KHarmonyMaxReconnectTries = 10;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task ConnectHarmonyAsync(bool aForceAuth=false)
        {
            if (Program.HarmonyClient != null)
            {
                await Program.HarmonyClient.CloseAsync();
            }

            bool success = false;

            //Reset Harmony client & config
            Program.HarmonyClient = null;
            Program.HarmonyConfig = null;
            iTreeViewHarmony.Nodes.Clear();

            Trace.WriteLine("Harmony: Connecting... ");
            //First create our client and login
            //Tip: Set keep-alive to false when testing reconnection process
            Program.HarmonyClient = new HarmonyHub.Client(iTextBoxHarmonyHubAddress.Text, true);
            Program.HarmonyClient.OnConnectionClosed += HarmonyConnectionClosed;

            string authToken = Properties.Settings.Default.LogitechAuthToken;
            if (!string.IsNullOrEmpty(authToken) && !aForceAuth)
            {
                Trace.WriteLine("Harmony: Reusing token: {0}", authToken);
                success = await Program.HarmonyClient.TryOpenAsync(authToken);
            }

            if (!Program.HarmonyClient.IsReady || !success 
                // Only first failure triggers new Harmony server AUTH
                // That's to avoid calling upon Logitech servers too often
                && iHarmonyReconnectTries == 0 )
            {
                //We failed to connect using our token
                //Delete it then
                Trace.WriteLine("Harmony: Reseting authentication token!");
                Properties.Settings.Default.LogitechAuthToken = "";

                Trace.WriteLine("Harmony: Authenticating with Logitech servers...");
                success = await Program.HarmonyClient.TryOpenAsync();
                //Persist our authentication token in our setting
                if (success)
                {
                    Trace.WriteLine("Harmony: Saving authentication token.");
                    Properties.Settings.Default.LogitechAuthToken = Program.HarmonyClient.Token;
                }
            }
           
            // I've seen this failing with "Policy lookup failed on server".
            Program.HarmonyConfig = await Program.HarmonyClient.TryGetConfigAsync();
            if (Program.HarmonyConfig == null)
            {
                success = false;
            }
            else
            {
                // So we now have our Harmony Configuration
                PopulateTreeViewHarmony(Program.HarmonyConfig);
                // Make sure harmony command actions are showing device name instead of device id
                PopulateTreeViewEvents(CurrentEarObject());
            }

            // TODO: Consider putting the retry logic one level higher in ResetHarmonyAsync
            if (!success)
            {
                // See if we need to keep trying 
                if (iHarmonyReconnectTries < KHarmonyMaxReconnectTries)
                {
                    iHarmonyReconnectTries++;
                    Trace.WriteLine("Harmony: Failed to connect, try again: " + iHarmonyReconnectTries);
                    await ConnectHarmonyAsync();
                }
                else
                {
                    Trace.WriteLine("Harmony: Failed to connect, giving up!");
                    iHarmonyReconnectTries = 0;
                    // TODO: Could use a data member as timer rather than a new instance.
                    // Try that again in 5 minutes then.
                    // Using Windows Form timer to make sure we run in the UI thread.
                    System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
                    timer.Tick += async delegate (object sender, EventArgs e)
                    {
                        // Stop our timer first as we won't need it anymore
                        (sender as System.Windows.Forms.Timer).Stop();
                        // Then try to connect again
                        await ConnectHarmonyAsync();
                    };
                    timer.Interval = 300000;
                    timer.Start();
                }
            }
            else
            {
                // We are connected with a valid Harmony Configuration
                // Reset our tries counter then    
                iHarmonyReconnectTries = 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aConfig"></param>
        private void PopulateTreeViewHarmony(HarmonyHub.Config aConfig)
        {
            iTreeViewHarmony.Nodes.Clear();
            //Add our devices
            foreach (HarmonyHub.Device device in aConfig.Devices)
            {
                TreeNode deviceNode = iTreeViewHarmony.Nodes.Add(device.Id, $"{device.Label} ({device.DeviceTypeDisplayName}/{device.Model})");
                deviceNode.Tag = device;

                foreach (HarmonyHub.ControlGroup cg in device.ControlGroups)
                {
                    TreeNode cgNode = deviceNode.Nodes.Add(cg.Name);
                    cgNode.Tag = cg;

                    foreach (HarmonyHub.Function f in cg.Functions)
                    {
                        TreeNode fNode = cgNode.Nodes.Add(f.Label);
                        fNode.Tag = f;
                    }
                }
            }

            //treeViewConfig.ExpandAll();
        }

        private async void iTreeViewHarmony_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //Upon function node double click we execute it
            var tag = e.Node.Tag as HarmonyHub.Function;
            if (tag != null && e.Node.Parent.Parent.Tag is HarmonyHub.Device)
            {
                HarmonyHub.Function f = tag;
                HarmonyHub.Device d = (HarmonyHub.Device)e.Node.Parent.Parent.Tag;

                Trace.WriteLine($"Harmony: Sending {f.Label} to {d.Label}...");

                await Program.HarmonyClient.TrySendKeyPressAsync(d.Id, f.Action.Command);
            }
        }

        private void iButtonOpenDataFolder_Click(object sender, EventArgs e)
        {
            string directory = Path.GetDirectoryName(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath);
            Process.Start(directory);
        }

        private void iButtonOpenExeFolder_Click(object sender, EventArgs e)
        {
            string directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Process.Start(directory);
        }

        private void FormMain_Shown(object sender, EventArgs e)
        {
            // Check for update
            // I reckon this happens only once per session.
            SquirrelUpdate(true);
        }
    }
}
