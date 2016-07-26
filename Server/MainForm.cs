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
//NAudio
using NAudio.CoreAudioApi;
using NAudio.CoreAudioApi.Interfaces;
using System.Runtime.InteropServices;
using CecSharp;
//Network
using NETWORKLIST;
//
using SharpDisplayClient;
using SharpDisplay;
using MiniDisplayInterop;
using SharpLib.Display;
using SharpLib.Ear;

namespace SharpDisplayManager
{
    //Types declarations
    public delegate uint ColorProcessingDelegate(int aX, int aY, uint aPixel);
    public delegate int CoordinateTranslationDelegate(System.Drawing.Bitmap aBmp, int aInt);
    //Delegates are used for our thread safe method
    public delegate void AddClientDelegate(string aSessionId, ICallback aCallback);
    public delegate void RemoveClientDelegate(string aSessionId);
    public delegate void SetFieldDelegate(string SessionId, DataField aField);
    public delegate void SetFieldsDelegate(string SessionId, System.Collections.Generic.IList<DataField> aFields);
    public delegate void SetLayoutDelegate(string SessionId, TableLayout aLayout);
    public delegate void SetClientNameDelegate(string aSessionId, string aName);
    public delegate void SetClientPriorityDelegate(string aSessionId, uint aPriority);
    public delegate void PlainUpdateDelegate();
    public delegate void WndProcDelegate(ref Message aMessage);

    /// <summary>
    /// Our Display manager main form
    /// </summary>
	[System.ComponentModel.DesignerCategory("Form")]
	public partial class MainForm : MainFormHid, IMMNotificationClient
    {
        //public ManagerEventAction iManager = new ManagerEventAction();        
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
		//
		public bool iSkipFrameRendering;
        //Function pointer for pixel color filtering
        ColorProcessingDelegate iColorFx;
        //Function pointer for pixel X coordinate intercept
        CoordinateTranslationDelegate iScreenX;
        //Function pointer for pixel Y coordinate intercept
        CoordinateTranslationDelegate iScreenY;
		//NAudio
		private MMDeviceEnumerator iMultiMediaDeviceEnumerator;
		private MMDevice iMultiMediaDevice;
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
        RichTextBoxTextWriter iWriter;


        /// <summary>
        /// Allow user to receive window messages;
        /// </summary>
        public event WndProcDelegate OnWndProc;

        public MainForm()
        {
            ManagerEventAction.Current = Properties.Settings.Default.Actions;
            if (ManagerEventAction.Current == null)
            {
                //No actions in our settings yet
                ManagerEventAction.Current = new ManagerEventAction();
                Properties.Settings.Default.Actions = ManagerEventAction.Current;
            }
            else
            {
                //We loaded actions from our settings
                //We need to hook them with corresponding events
                ManagerEventAction.Current.Init();
            }
            iSkipFrameRendering = false;
			iClosing = false;
            iCurrentClientSessionId = "";
            iCurrentClientData = null;
            LastTickTime = DateTime.Now;
			//Instantiate our display and register for events notifications
            iDisplay = new Display();
			iDisplay.OnOpened += OnDisplayOpened;
			iDisplay.OnClosed += OnDisplayClosed;
			//
			iClients = new Dictionary<string, ClientData>();
			iStartupManager = new StartupManager();
			iNotifyIcon = new SharpLib.Notification.Control();
            iRecordingNotification = new SharpLib.Notification.Control();

            //Have our designer initialize its controls
            InitializeComponent();

            //Redirect console output
            iWriter = new RichTextBoxTextWriter(richTextBoxLogs);
            Console.SetOut(iWriter);

            //Populate device types
            PopulateDeviceTypes();

            //Populate optical drives
            PopulateOpticalDrives();

			//Initial status update 
            UpdateStatus();

            //We have a bug when drawing minimized and reusing our bitmap
            //Though I could not reproduce it on Windows 10
            iBmp = new System.Drawing.Bitmap(iTableLayoutPanel.Width, iTableLayoutPanel.Height, PixelFormat.Format32bppArgb);
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
        private void MainForm_Load(object sender, EventArgs e)
        {
			//Check if we are running a Click Once deployed application
			if (ApplicationDeployment.IsNetworkDeployed)
			{
				//This is a proper Click Once installation, fetch and show our version number
				this.Text += " - v" + ApplicationDeployment.CurrentDeployment.CurrentVersion;
			}
			else
			{
				//Not a proper Click Once installation, assuming development build then
				this.Text += " - development";
			}

			//NAudio
			iMultiMediaDeviceEnumerator = new MMDeviceEnumerator();
			iMultiMediaDeviceEnumerator.RegisterEndpointNotificationCallback(this);			
			UpdateAudioDeviceAndMasterVolumeThreadSafe();

			//Network
			iNetworkManager = new NetworkManager();
			iNetworkManager.OnConnectivityChanged += OnConnectivityChanged;
			UpdateNetworkStatus();

            //CEC
            iCecManager = new ConsumerElectronicControl();
            OnWndProc += iCecManager.OnWndProc;
            ResetCec();

            //Setup Events
            PopulateEventsTreeView();

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
			//When not debugging we want the screen to be empty until a client takes over
			ClearLayout();
#else
			//When developing we want at least one client for testing
			StartNewClient("abcdefghijklmnopqrst-0123456789","ABCDEFGHIJKLMNOPQRST-0123456789");
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
			panelDisplay.Size = size;

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

        /// <summary>
        /// Populate tree view with events and actions
        /// </summary>
        private void PopulateEventsTreeView()
        {
            //Disable action buttons
            buttonAddAction.Enabled = false;
            buttonDeleteAction.Enabled = false;

            Event currentEvent = CurrentEvent();

            //Reset our tree
            iTreeViewEvents.Nodes.Clear();
            //Populate registered events
            foreach (string key in ManagerEventAction.Current.Events.Keys)
            {
                Event e = ManagerEventAction.Current.Events[key];
                TreeNode eventNode = iTreeViewEvents.Nodes.Add(key,e.Name);
                eventNode.Tag = e;
                eventNode.Nodes.Add(key + ".Description", e.Description);
                TreeNode actionsNodes = eventNode.Nodes.Add(key + ".Actions", "Actions");

                // Add our actions for that event
                foreach (SharpLib.Ear.Action a in e.Actions)
                {
                    TreeNode actionNode = actionsNodes.Nodes.Add(a.Brief());
                    actionNode.Tag = a;
                }
            }

            iTreeViewEvents.ExpandAll();
            SelectEvent(currentEvent);
            //Select the last action if any 
            if (iTreeViewEvents.SelectedNode!= null && iTreeViewEvents.SelectedNode.Nodes[1].GetNodeCount(false) > 0)
            {
                iTreeViewEvents.SelectedNode = iTreeViewEvents.SelectedNode.Nodes[1].Nodes[iTreeViewEvents.SelectedNode.Nodes[1].GetNodeCount(false)-1];
            }

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
                iDisplay.SetIconOnOff(MiniDisplay.IconType.Internet, iNetworkManager.NetworkListManager.IsConnectedToInternet);
                iDisplay.SetIconOnOff(MiniDisplay.IconType.NetworkSignal, iNetworkManager.NetworkListManager.IsConnected);
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
			iUpdateCountSinceLastNetworkAnimation = iUpdateCountSinceLastNetworkAnimation % 4;

			if (iDisplay.IsOpen() && iNetworkManager.NetworkListManager.IsConnected && iUpdateCountSinceLastNetworkAnimation==0)
			{
                int iconCount = iDisplay.IconCount(MiniDisplay.IconType.NetworkSignal);
				if (iconCount <= 0)
				{
					//Prevents div by zero and other undefined behavior
					return;
				}
				iLastNetworkIconIndex++;
				iLastNetworkIconIndex = iLastNetworkIconIndex % (iconCount*2);
				for (int i=0;i<iconCount;i++)
				{
					if (i < iLastNetworkIconIndex && !(i == 0 && iLastNetworkIconIndex > 3) && !(i == 1 && iLastNetworkIconIndex > 4))
					{
                        iDisplay.SetIconOn(MiniDisplay.IconType.NetworkSignal, i);
					}
					else
					{
                        iDisplay.SetIconOff(MiniDisplay.IconType.NetworkSignal, i);
					}
				}				
			}
		}



        /// <summary>
        /// Receive volume change notification and reflect changes on our slider.
        /// </summary>
        /// <param name="data"></param>
        public void OnVolumeNotificationThreadSafe(AudioVolumeNotificationData data)
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
			iMultiMediaDevice.AudioEndpointVolume.Mute = false;
			//Set volume level according to our volume slider new position
			iMultiMediaDevice.AudioEndpointVolume.MasterVolumeLevelScalar = trackBarMasterVolume.Value / 100.0f;
        }


		/// <summary>
		/// Mute check box changed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void checkBoxMute_CheckedChanged(object sender, EventArgs e)
		{
			iMultiMediaDevice.AudioEndpointVolume.Mute = checkBoxMute.Checked;
		}

        /// <summary>
        /// Device State Changed
        /// </summary>
        public void OnDeviceStateChanged([MarshalAs(UnmanagedType.LPWStr)] string deviceId, [MarshalAs(UnmanagedType.I4)] DeviceState newState){}

        /// <summary>
        /// Device Added
        /// </summary>
        public void OnDeviceAdded([MarshalAs(UnmanagedType.LPWStr)] string pwstrDeviceId) { }

        /// <summary>
        /// Device Removed
        /// </summary>
        public void OnDeviceRemoved([MarshalAs(UnmanagedType.LPWStr)] string deviceId) { }

        /// <summary>
        /// Default Device Changed
        /// </summary>
        public void OnDefaultDeviceChanged(DataFlow flow, Role role, [MarshalAs(UnmanagedType.LPWStr)] string defaultDeviceId)
        {
            if (role == Role.Multimedia && flow == DataFlow.Render)
            {
                UpdateAudioDeviceAndMasterVolumeThreadSafe();
            }
        }

        /// <summary>
        /// Property Value Changed
        /// </summary>
        /// <param name="pwstrDeviceId"></param>
        /// <param name="key"></param>
        public void OnPropertyValueChanged([MarshalAs(UnmanagedType.LPWStr)] string pwstrDeviceId, PropertyKey key){}


        

		/// <summary>
		/// Update master volume indicators based our current system states.
		/// This typically includes volume levels and mute status.
		/// </summary>
		private void UpdateMasterVolumeThreadSafe()
		{
			if (this.InvokeRequired)
			{
				//Not in the proper thread, invoke ourselves
				PlainUpdateDelegate d = new PlainUpdateDelegate(UpdateMasterVolumeThreadSafe);
				this.Invoke(d, new object[] { });
				return;
			}

			//Update volume slider
			float volumeLevelScalar = iMultiMediaDevice.AudioEndpointVolume.MasterVolumeLevelScalar;
			trackBarMasterVolume.Value = Convert.ToInt32(volumeLevelScalar * 100);
			//Update mute checkbox
			checkBoxMute.Checked = iMultiMediaDevice.AudioEndpointVolume.Mute;

			//If our display connection is open we need to update its icons
			if (iDisplay.IsOpen())
			{
				//First take care our our volume level icons
                int volumeIconCount = iDisplay.IconCount(MiniDisplay.IconType.Volume);
				if (volumeIconCount > 0)
				{					
					//Compute current volume level from system level and the number of segments in our display volume bar.
					//That tells us how many segments in our volume bar needs to be turned on.
					float currentVolume = volumeLevelScalar * volumeIconCount;
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
                                iDisplay.SetIconStatus(MiniDisplay.IconType.Volume, i, (iDisplay.IconStatusCount(MiniDisplay.IconType.Volume) - 1) / 2);
							}
							else
							{
								//Full brightness
                                iDisplay.SetIconStatus(MiniDisplay.IconType.Volume, i, iDisplay.IconStatusCount(MiniDisplay.IconType.Volume) - 1);
							}
						}
						else
						{
                            iDisplay.SetIconStatus(MiniDisplay.IconType.Volume, i, 0);
						}
					}
				}

				//Take care of our mute icon
                iDisplay.SetIconOnOff(MiniDisplay.IconType.Mute, iMultiMediaDevice.AudioEndpointVolume.Mute);
			}

		}

        /// <summary>
        /// 
        /// </summary>
        private void UpdateAudioDeviceAndMasterVolumeThreadSafe()
        {
            if (this.InvokeRequired)
            {
                //Not in the proper thread, invoke ourselves
				PlainUpdateDelegate d = new PlainUpdateDelegate(UpdateAudioDeviceAndMasterVolumeThreadSafe);
                this.Invoke(d, new object[] { });
                return;
            }
            
            //We are in the correct thread just go ahead.
            try
            {                
                //Get our master volume            
				iMultiMediaDevice = iMultiMediaDeviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
				//Update our label
				labelDefaultAudioDevice.Text = iMultiMediaDevice.FriendlyName;

                //Show our volume in our track bar
				UpdateMasterVolumeThreadSafe();

                //Register to get volume modifications
				iMultiMediaDevice.AudioEndpointVolume.OnVolumeNotification += OnVolumeNotificationThreadSafe;
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
				comboBoxDisplayType.Items.Add(Display.TypeName((MiniDisplay.Type)i));
			}
		}

        /// <summary>
        /// 
        /// </summary>
        private void PopulateOpticalDrives()
        {
            //Reset our list of drives
            comboBoxOpticalDrives.Items.Clear();
            comboBoxOpticalDrives.Items.Add("None");

            //Go through each drives on our system and collected the optical ones in our list
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            foreach (DriveInfo d in allDrives)
            {
                Debug.WriteLine("Drive " + d.Name);
                Debug.WriteLine("  Drive type: {0}", d.DriveType);

                if (d.DriveType==DriveType.CDRom)
                {
                    //This is an optical drive, add it now
                    comboBoxOpticalDrives.Items.Add(d.Name.Substring(0,2));
                }                
            }           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string OpticalDriveToEject()
        {
            return comboBoxOpticalDrives.SelectedItem.ToString();
        }



		/// <summary>
		///
		/// </summary>
		private void SetupTrayIcon()
		{
			iNotifyIcon.Icon = GetIcon("vfd.ico");
			iNotifyIcon.Text = "Sharp Display Manager";
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
			string[] names =  Assembly.GetExecutingAssembly().GetManifestResourceNames();
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
        void SetCurrentClient(string aSessionId, bool aForce=false)
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
                    requestedClientData.Priority == iCurrentClientData.Priority && //Time sharing is only if clients have the same priority
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
            UpdateTableLayoutPanel(iCurrentClientData);
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
                //Set the fonts to all our labels in our layout
                foreach (Control ctrl in iTableLayoutPanel.Controls)
                {
                    if (ctrl is MarqueeLabel)
                    {
                        ((MarqueeLabel)ctrl).Font = fontDialog.Font;
                    }
                }

                //Save font settings
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

            MarqueeLabel label = null;
            //Get the first label control we can find
            foreach (Control ctrl in iTableLayoutPanel.Controls)
            {
                if (ctrl is MarqueeLabel)
                {
                    label = (MarqueeLabel)ctrl;
                    break;
                }
            }

            //Now check font height and show a warning if needed.
            if (label != null && label.Font.Height > label.Height)
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
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(iTableLayoutPanel.Width, iTableLayoutPanel.Height);
            iTableLayoutPanel.DrawToBitmap(bmp, iTableLayoutPanel.ClientRectangle);
            //Bitmap bmpToSave = new Bitmap(bmp);
            bmp.Save("D:\\capture.png");

            ((MarqueeLabel)iTableLayoutPanel.Controls[0]).Text = "Captured";

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
                    case MiniDisplay.Request.FirmwareRevision:
                        toolStripStatusLabelConnect.Text += " v" + iDisplay.FirmwareRevision();
                        //Issue next request then
                        iDisplay.RequestPowerSupplyStatus();
                        break;

                    case MiniDisplay.Request.PowerSupplyStatus:
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

                    case MiniDisplay.Request.DeviceId:
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
            //Not ideal cause this has nothing to do with display render
            LogsUpdate();

            //Update our animations
            DateTime NewTickTime = DateTime.Now;

			UpdateNetworkSignal(LastTickTime, NewTickTime);

            //Update animation for all our marquees
            foreach (Control ctrl in iTableLayoutPanel.Controls)
            {
                if (ctrl is MarqueeLabel)
                {
                    ((MarqueeLabel)ctrl).UpdateAnimation(LastTickTime, NewTickTime);
                }
            }

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
                        iBmp = new System.Drawing.Bitmap(iTableLayoutPanel.Width, iTableLayoutPanel.Height, PixelFormat.Format32bppArgb);
                        iCreateBitmap = false;
                    }
					iTableLayoutPanel.DrawToBitmap(iBmp, iTableLayoutPanel.ClientRectangle);
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
								color = iColorFx(x, y, color);
								//Now set our pixel
								iDisplay.SetPixel(x, y, color);
							}
						}
					}

					iDisplay.SwapBuffers();
				}
            }

            //Compute instant FPS
            toolStripStatusLabelFps.Text = (1.0/NewTickTime.Subtract(LastTickTime).TotalSeconds).ToString("F0") + " / " + (1000/timer.Interval).ToString() + " FPS";

            LastTickTime = NewTickTime;

        }

		/// <summary>
		/// Attempt to establish connection with our display hardware.
		/// </summary>
        private void OpenDisplayConnection()
        {
            CloseDisplayConnection();

            if (!iDisplay.Open((MiniDisplay.Type)cds.DisplayType))
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

		/// <summary>
		/// Synchronize UI with settings
		/// </summary>
        private void UpdateStatus()
        {            
            //Load settings
            checkBoxShowBorders.Checked = cds.ShowBorders;
            iTableLayoutPanel.CellBorderStyle = (cds.ShowBorders ? TableLayoutPanelCellBorderStyle.Single : TableLayoutPanelCellBorderStyle.None);

            //Set the proper font to each of our labels
            foreach (MarqueeLabel ctrl in iTableLayoutPanel.Controls)
            {
                ctrl.Font = cds.Font;
            }

            CheckFontHeight();
			//Check if "run on Windows startup" is enabled
			checkBoxAutoStart.Checked = iStartupManager.Startup;
			//
            checkBoxConnectOnStartup.Checked = Properties.Settings.Default.DisplayConnectOnStartup;
			checkBoxMinimizeToTray.Checked = Properties.Settings.Default.MinimizeToTray;
			checkBoxStartMinimized.Checked = Properties.Settings.Default.StartMinimized;
            iCheckBoxStartIdleClient.Checked = Properties.Settings.Default.StartIdleClient;
            labelStartFileName.Text = Properties.Settings.Default.StartFileName;


            //Try find our drive in our drive list
            int opticalDriveItemIndex=0;
            bool driveNotFound = true;
            string opticalDriveToEject=Properties.Settings.Default.OpticalDriveToEject;
            foreach (object item in comboBoxOpticalDrives.Items)
            {
                if (opticalDriveToEject == item.ToString())
                {
                    comboBoxOpticalDrives.SelectedIndex = opticalDriveItemIndex;
                    driveNotFound = false;
                    break;
                }
                opticalDriveItemIndex++;
            }

            if (driveNotFound)
            {
                //We could not find the drive we had saved.
                //Select "None" then.
                comboBoxOpticalDrives.SelectedIndex = 0;
            }

            //CEC settings
            checkBoxCecEnabled.Checked = Properties.Settings.Default.CecEnabled;
            comboBoxHdmiPort.SelectedIndex = Properties.Settings.Default.CecHdmiPort - 1;

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
            timer.Interval = cds.TimerInterval;
            maskedTextBoxTimerInterval.Text = cds.TimerInterval.ToString();
            textBoxScrollLoopSeparator.Text = cds.Separator;
            //
            SetupPixelDelegates();

            if (iDisplay.IsOpen())
            {
                //We have a display connection
                //Reflect that in our UI
                StartTimer();

				iTableLayoutPanel.Enabled = true;
				panelDisplay.Enabled = true;

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
                trackBarBrightness.LargeChange = Math.Max(1, (iDisplay.MaxBrightness() - iDisplay.MinBrightness()) / 5);
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
				checkBoxShowVolumeLabel.Enabled = iDisplay.IconCount(MiniDisplay.IconType.VolumeLabel)>0;

				if (cds.ShowVolumeLabel)
				{
                    iDisplay.SetIconOn(MiniDisplay.IconType.VolumeLabel);
				}
				else
				{
                    iDisplay.SetIconOff(MiniDisplay.IconType.VolumeLabel);
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
				iTableLayoutPanel.Enabled = false;
				panelDisplay.Enabled = false;
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
            iTableLayoutPanel.CellBorderStyle = (checkBoxShowBorders.Checked ? TableLayoutPanelCellBorderStyle.Single : TableLayoutPanelCellBorderStyle.None);
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

		private void checkBoxMinimizeToTray_CheckedChanged(object sender, EventArgs e)
		{
			//Save our "Minimize to tray" setting
			Properties.Settings.Default.MinimizeToTray = checkBoxMinimizeToTray.Checked;
			Properties.Settings.Default.Save();

		}

		private void checkBoxStartMinimized_CheckedChanged(object sender, EventArgs e)
		{
			//Save our "Start minimized" setting
			Properties.Settings.Default.StartMinimized = checkBoxStartMinimized.Checked;
			Properties.Settings.Default.Save();
		}

        private void checkBoxStartIdleClient_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.StartIdleClient = iCheckBoxStartIdleClient.Checked;
            Properties.Settings.Default.Save();
        }

        private void checkBoxAutoStart_CheckedChanged(object sender, EventArgs e)
		{
			iStartupManager.Startup = checkBoxAutoStart.Checked;
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

        private void checkBoxScaleToFit_CheckedChanged(object sender, EventArgs e)
        {
            //Save our scale to fit setting
            cds.ScaleToFit = checkBoxScaleToFit.Checked;
            Properties.Settings.Default.Save();
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

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            iCecManager.Stop();
			iNetworkManager.Dispose();
			CloseDisplayConnection();
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
                    Program.iMainForm.iTreeViewClients.Nodes.Remove(Program.iMainForm.iTreeViewClients.Nodes.Find(client, false)[0]);
                }
            }

			if (iClients.Count==0)
			{
				ClearLayout();
			}
        }

		/// <summary>
		/// Just remove all our fields.
		/// </summary>
		private void ClearLayout()
		{
			iTableLayoutPanel.Controls.Clear();
			iTableLayoutPanel.RowStyles.Clear();
			iTableLayoutPanel.ColumnStyles.Clear();
			iCurrentClientData = null;
		}

		/// <summary>
		/// Just launch a demo client.
		/// </summary>
		private void StartNewClient(string aTopText = "", string aBottomText = "")
		{
			Thread clientThread = new Thread(SharpDisplayClient.Program.MainWithParams);
			SharpDisplayClient.StartParams myParams = new SharpDisplayClient.StartParams(new Point(this.Right, this.Top),aTopText,aBottomText);
			clientThread.Start(myParams);
			BringToFront();
		}

        /// <summary>
        /// Just launch our idle client.
        /// </summary>
        private void StartIdleClient(string aTopText = "", string aBottomText = "")
        {
            Thread clientThread = new Thread(SharpDisplayIdleClient.Program.MainWithParams);
            SharpDisplayIdleClient.StartParams myParams = new SharpDisplayIdleClient.StartParams(new Point(this.Right, this.Top), aTopText, aBottomText);
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
            LastTickTime = DateTime.Now; //Reset timer to prevent jump
            timer.Enabled = true;
            UpdateSuspendButton();
        }

        private void StopTimer()
        {
            LastTickTime = DateTime.Now; //Reset timer to prevent jump
            timer.Enabled = false;
            UpdateSuspendButton();
        }

        private void ToggleTimer()
        {
            LastTickTime = DateTime.Now; //Reset timer to prevent jump
            timer.Enabled = !timer.Enabled;
            UpdateSuspendButton();
        }

        private void UpdateSuspendButton()
        {
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
            //Root node must have at least one child
            if (e.Node.Nodes.Count == 0)
            {
                return;
            }

            //If the selected node is the root node of a client then switch to it
            string sessionId=e.Node.Nodes[0].Text; //First child of a root node is the sessionId
            if (iClients.ContainsKey(sessionId)) //Check that's actually what we are looking at
            {
                //We have a valid session just switch to that client
                SetCurrentClient(sessionId,true);
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
                this.Invoke(d, new object[] { aSessionId });
            }
            else
            {
                //We are in the proper thread
                //Remove this session from both client collection and UI tree view
                if (Program.iMainForm.iClients.Keys.Contains(aSessionId))
                {
                    Program.iMainForm.iClients.Remove(aSessionId);
                    Program.iMainForm.iTreeViewClients.Nodes.Remove(Program.iMainForm.iTreeViewClients.Nodes.Find(aSessionId, false)[0]);
                    //Update recording status too whenever a client is removed
                    UpdateRecordingNotification();
                }

                if (iCurrentClientSessionId == aSessionId)
                {
                    //The current client is closing
                    iCurrentClientData = null;
                    //Find the client with the highest priority and set it as current
                    ClientData newCurrentClient = FindHighestPriorityClient();
                    if (newCurrentClient!=null)
                    {
                        SetCurrentClient(newCurrentClient.SessionId, true);
                    }                    
                }

                if (iClients.Count == 0)
				{
					//Clear our screen when last client disconnects
					ClearLayout();

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
                SetLayoutDelegate d = new SetLayoutDelegate(SetClientLayoutThreadSafe);
                this.Invoke(d, new object[] { aSessionId, aLayout });
            }
            else
            {
                ClientData client = iClients[aSessionId];
                if (client != null)
                {
                    //Don't change a thing if the layout is the same
                    if (!client.Layout.IsSameAs(aLayout))
                    {
                        Debug.Print("SetClientLayoutThreadSafe: Layout updated.");
                        //Set our client layout then
                        client.Layout = aLayout;
                        //So that next time we update all our fields at ones
                        client.HasNewLayout = true;
                        //Layout has changed clear our fields then
                        client.Fields.Clear();
                        //
                        UpdateClientTreeViewNode(client);
                    }
                    else
                    {
                        Debug.Print("SetClientLayoutThreadSafe: Layout has not changed.");
                    }
                }
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
                SetFieldDelegate d = new SetFieldDelegate(SetClientFieldThreadSafe);
                this.Invoke(d, new object[] { aSessionId, aField });
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
            int fieldIndex = client.FindSameFieldIndex(aField);

            if (fieldIndex < 0)
            {
                //No corresponding field, just bail out
                return;
            }

            //Keep our previous field in there
            DataField previousField = client.Fields[fieldIndex];
            //Just update that field then 
            client.Fields[fieldIndex] = aField;

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
                    Control ctrl=iTableLayoutPanel.GetControlFromPosition(tableField.Column, tableField.Row);
                    if (aField.IsTextField && ctrl is MarqueeLabel)
                    {
                        TextField textField=(TextField)aField;
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
                        UpdateTableLayoutPanel(iCurrentClientData);
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
                SetFieldsDelegate d = new SetFieldsDelegate(SetClientFieldsThreadSafe);
                this.Invoke(d, new object[] { aSessionId, aFields });
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
                    client.Fields.AddRange(aFields);
                    //Try switch to that client
                    SetCurrentClient(aSessionId);

                    //If we are updating the current client update our panel
                    if (aSessionId == iCurrentClientSessionId)
                    {
                        //Apply layout and set data fields.
                        UpdateTableLayoutPanel(iCurrentClientData);
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

        ///
        public void SetClientPriorityThreadSafe(string aSessionId, uint aPriority)
        {
            if (this.InvokeRequired)
            {
                //Not in the proper thread, invoke ourselves
                SetClientPriorityDelegate d = new SetClientPriorityDelegate(SetClientPriorityThreadSafe);
                this.Invoke(d, new object[] { aSessionId, aPriority });
            }
            else
            {
                //We are in the proper thread
                //Get our client
                ClientData client = iClients[aSessionId];
                if (client != null)
                {
                    //Set its name
                    client.Priority = aPriority;
                    //Update our tree-view
                    UpdateClientTreeViewNode(client);
                    //Change our current client as per new priority
                    ClientData newCurrentClient = FindHighestPriorityClient();
                    if (newCurrentClient!=null)
                    {
                        SetCurrentClient(newCurrentClient.SessionId);
                    }
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
            return value.Length <= maxChars ? value : value.Substring(0, maxChars-3) + "...";
        }

        /// <summary>
        /// Update our recording notification.
        /// </summary>
        private void UpdateRecordingNotification()
        {
            //Go through each 
            bool activeRecording = false;
            string text="";
            RecordingField recField=new RecordingField();
            foreach (var client in iClients)
            {
                RecordingField rec=(RecordingField)client.Value.FindSameFieldAs(recField);
                if (rec!=null && rec.IsActive)
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
            iRecordingNotification.Text = Truncate(text,63);

            //Change visibility of notification if needed
            if (iRecordingNotification.Visible != activeRecording)
            {                
                iRecordingNotification.Visible = activeRecording;
                //Assuming the notification icon is in sync with our display icon
                //Take care of our REC icon
                if (iDisplay.IsOpen())
                {
                    iDisplay.SetIconOnOff(MiniDisplay.IconType.Recording, activeRecording);
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

                if (aClient.Fields.Count > 0)
                {
                    //Create root node for our texts
                    TreeNode textsRoot = new TreeNode("Fields");
                    node.Nodes.Add(textsRoot);
                    //For each text add a new entry
                    foreach (DataField field in aClient.Fields)
                    {
                        if (field.IsTextField)
                        {
                            TextField textField = (TextField)field;
                            textsRoot.Nodes.Add(new TreeNode("[Text]" + textField.Text));
                        }
                        else if (field.IsBitmapField)
                        {
                            textsRoot.Nodes.Add(new TreeNode("[Bitmap]"));
                        }
                        else if (field.IsRecordingField)
                        {
                            RecordingField recordingField = (RecordingField)field;
                            textsRoot.Nodes.Add(new TreeNode("[Recording]" + recordingField.IsActive));
                        }
                    }
                }

                node.ExpandAll();
            }
        }

        /// <summary>
        /// Update our table layout row styles to make sure each rows have similar height
        /// </summary>
        private void UpdateTableLayoutRowStyles()
        {
            foreach (RowStyle rowStyle in iTableLayoutPanel.RowStyles)
            {
                rowStyle.SizeType = SizeType.Percent;
                rowStyle.Height = 100 / iTableLayoutPanel.RowCount;
            }
        }

        /// <summary>
        /// Update our display table layout.
        /// Will instanciated every field control as defined by our client.
        /// Fields must be specified by rows from the left.
        /// </summary>
        /// <param name="aLayout"></param>
        private void UpdateTableLayoutPanel(ClientData aClient)
        {
            Debug.Print("UpdateTableLayoutPanel");

			if (aClient == null)
			{
				//Just drop it
				return;
			}


            TableLayout layout = aClient.Layout;

            //First clean our current panel
            iTableLayoutPanel.Controls.Clear();
            iTableLayoutPanel.RowStyles.Clear();
            iTableLayoutPanel.ColumnStyles.Clear();
            iTableLayoutPanel.RowCount = 0;
            iTableLayoutPanel.ColumnCount = 0;

            //Then recreate our rows...
            while (iTableLayoutPanel.RowCount < layout.Rows.Count)
            {
                iTableLayoutPanel.RowCount++;
            }

            // ...and columns 
            while (iTableLayoutPanel.ColumnCount < layout.Columns.Count)
            {
                iTableLayoutPanel.ColumnCount++;
            }

            //For each column
            for (int i = 0; i < iTableLayoutPanel.ColumnCount; i++)
            {
                //Create our column styles
                this.iTableLayoutPanel.ColumnStyles.Add(layout.Columns[i]);

                //For each rows
                for (int j = 0; j < iTableLayoutPanel.RowCount; j++)
                {
                    if (i == 0)
                    {
                        //Create our row styles
                        this.iTableLayoutPanel.RowStyles.Add(layout.Rows[j]);
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            //For each field
            foreach (DataField field in aClient.Fields)
            {
                if (!field.IsTableField)
                {
                    //That field is not taking part in our table layout skip it
                    continue;
                }

                TableField tableField = (TableField)field;

                //Create a control corresponding to the field specified for that cell
                Control control = CreateControlForDataField(tableField);

                //Add newly created control to our table layout at the specified row and column
                iTableLayoutPanel.Controls.Add(control, tableField.Column, tableField.Row);
                //Make sure we specify column and row span for that new control
                iTableLayoutPanel.SetColumnSpan(control, tableField.ColumnSpan);
                iTableLayoutPanel.SetRowSpan(control, tableField.RowSpan);
            }


            CheckFontHeight();
        }

        /// <summary>
        /// Check our type of data field and create corresponding control
        /// </summary>
        /// <param name="aField"></param>
        private Control CreateControlForDataField(DataField aField)
        {
            Control control=null;
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
            Properties.Settings.Default.CurrentDisplayIndex = comboBoxDisplayType.SelectedIndex;
            cds.DisplayType = comboBoxDisplayType.SelectedIndex;
            Properties.Settings.Default.Save();

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
                    timer.Interval = interval;
                    cds.TimerInterval = timer.Interval;
                    Properties.Settings.Default.Save();
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
                    cds.MinFontSize = minFontSize;
                    Properties.Settings.Default.Save();
					//We need to recreate our layout for that change to take effect
					UpdateTableLayoutPanel(iCurrentClientData);
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
					cds.ScrollingSpeedInPixelsPerSecond = scrollingSpeed;
					Properties.Settings.Default.Save();
					//We need to recreate our layout for that change to take effect
					UpdateTableLayoutPanel(iCurrentClientData);
				}
			}
		}

        private void textBoxScrollLoopSeparator_TextChanged(object sender, EventArgs e)
        {
            cds.Separator = textBoxScrollLoopSeparator.Text;
            Properties.Settings.Default.Save();

			//Update our text fields
			foreach (MarqueeLabel ctrl in iTableLayoutPanel.Controls)
			{
				ctrl.Separator = cds.Separator;
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
			ShowClock();
        }

        private void buttonHideClock_Click(object sender, EventArgs e)
        {
			HideClock();
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            InstallUpdateSyncWithInfo();
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
                    MessageBox.Show("The new version of the application cannot be downloaded at this time. \n\nPlease check your network connection, or try again later. Error: " + dde.Message);
                    return;
                }
                catch (InvalidDeploymentException ide)
                {
                    MessageBox.Show("Cannot check for a new version of the application. The ClickOnce deployment is corrupt. Please redeploy the application and try again. Error: " + ide.Message);
                    return;
                }
                catch (InvalidOperationException ioe)
                {
                    MessageBox.Show("This application cannot be updated. It is likely not a ClickOnce application. Error: " + ioe.Message);
                    return;
                }

				if (info.UpdateAvailable)
				{
					Boolean doUpdate = true;

					if (!info.IsUpdateRequired)
					{
						DialogResult dr = MessageBox.Show("An update is available. Would you like to update the application now?", "Update Available", MessageBoxButtons.OKCancel);
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
							MessageBox.Show("Cannot install the latest version of the application. \n\nPlease check your network connection, or try again later. Error: " + dde);
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
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonSelectFile_Click(object sender, EventArgs e)
		{
			//openFileDialog1.InitialDirectory = "c:\\";
			//openFileDialog.Filter = "EXE files (*.exe)|*.exe|All files (*.*)|*.*";
			//openFileDialog.FilterIndex = 1;
			openFileDialog.RestoreDirectory = true;

			if (DlgBox.ShowDialog(openFileDialog) == DialogResult.OK)
			{
				labelStartFileName.Text = openFileDialog.FileName;
				Properties.Settings.Default.StartFileName = openFileDialog.FileName;
				Properties.Settings.Default.Save();
			}
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxOpticalDrives_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Save the optical drive the user selected for ejection
            Properties.Settings.Default.OpticalDriveToEject = comboBoxOpticalDrives.SelectedItem.ToString();
            Properties.Settings.Default.Save();
        }


        /// <summary>
        /// 
        /// </summary>
        private void LogsUpdate()
        {
            if (iWriter != null)
            {
                iWriter.Flush();
            }

        }

        /// <summary>
        /// Broadcast messages to subscribers.
        /// </summary>
        /// <param name="message"></param>
        protected override void WndProc(ref Message aMessage)
        {
            LogsUpdate();

            if (OnWndProc!=null)
            {
                OnWndProc(ref aMessage);
            }
            
            base.WndProc(ref aMessage);
        }

        private void checkBoxCecEnabled_CheckedChanged(object sender, EventArgs e)
        {
            //Save CEC enabled status
            Properties.Settings.Default.CecEnabled = checkBoxCecEnabled.Checked;
            Properties.Settings.Default.Save();
            //
            ResetCec();
        }

        private void comboBoxHdmiPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Save CEC HDMI port
            Properties.Settings.Default.CecHdmiPort = Convert.ToByte(comboBoxHdmiPort.SelectedIndex);
            Properties.Settings.Default.CecHdmiPort++;
            Properties.Settings.Default.Save();
            //
            ResetCec();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ResetCec()
        {
            if (iCecManager==null)
            {
                //Thus skipping initial UI setup
                return;
            }

            iCecManager.Stop();
            //
            if (Properties.Settings.Default.CecEnabled)
            {
                iCecManager.Start(Handle, "CEC",
                Properties.Settings.Default.CecHdmiPort);

                SetupCecLogLevel();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetupCecLogLevel()
        {
            //Setup log level
            iCecManager.Client.LogLevel = 0;

            if (checkBoxCecLogError.Checked)
                iCecManager.Client.LogLevel |= (int)CecLogLevel.Error;

            if (checkBoxCecLogWarning.Checked)
                iCecManager.Client.LogLevel |= (int)CecLogLevel.Warning;

            if (checkBoxCecLogNotice.Checked)
                iCecManager.Client.LogLevel |= (int)CecLogLevel.Notice;

            if (checkBoxCecLogTraffic.Checked)
                iCecManager.Client.LogLevel |= (int)CecLogLevel.Traffic;

            if (checkBoxCecLogDebug.Checked)
                iCecManager.Client.LogLevel |= (int)CecLogLevel.Debug;

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
        /// 
        /// </summary>
        /// <param name="aEvent"></param>
        private void SelectEvent(Event aEvent)
        {
            if (aEvent == null)
            {
                return;
            }

            string key = aEvent.GetType().Name;
            TreeNode[] res=iTreeViewEvents.Nodes.Find(key, false);
            if (res.Length > 0)
            {                
                iTreeViewEvents.SelectedNode = res[0];
                iTreeViewEvents.Focus();
            }
        }



        /// <summary>
        /// Get the current event based on event tree view selection.
        /// </summary>
        /// <returns></returns>
        private Event CurrentEvent()
        {
            //Walk up the tree from the selected node to find our event
            TreeNode node = iTreeViewEvents.SelectedNode;
            Event selectedEvent = null;
            while (node != null)
            {
                if (node.Tag is Event)
                {
                    selectedEvent = (Event)node.Tag;
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
        private SharpLib.Ear.Action CurrentAction()
        {
            TreeNode node = iTreeViewEvents.SelectedNode;
            if (node != null && node.Tag is SharpLib.Ear.Action)
            {
                return (SharpLib.Ear.Action) node.Tag;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAddAction_Click(object sender, EventArgs e)
        {
            Event selectedEvent = CurrentEvent();
            if (selectedEvent == null)
            {
                //We did not find a corresponding event
                return;
            }

            FormEditAction ea = new FormEditAction();
            DialogResult res = CodeProject.Dialog.DlgBox.ShowDialog(ea);
            if (res == DialogResult.OK)
            {
                selectedEvent.Actions.Add(ea.Action);                
                Properties.Settings.Default.Actions = ManagerEventAction.Current;
                Properties.Settings.Default.Save();
                PopulateEventsTreeView();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDeleteAction_Click(object sender, EventArgs e)
        {

            SharpLib.Ear.Action action = CurrentAction();
            if (action == null)
            {
                //Must select action node
                return;
            }

            ManagerEventAction.Current.RemoveAction(action);
            Properties.Settings.Default.Actions = ManagerEventAction.Current;
            Properties.Settings.Default.Save();
            PopulateEventsTreeView();
        }

        private void iTreeViewEvents_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //Enable buttons according to selected item
            buttonAddAction.Enabled = CurrentEvent() != null;
            buttonDeleteAction.Enabled = CurrentAction() != null;
        }
    }
}
