namespace SharpDisplayManager
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (iNotifyIcon != null)
                {
                    iNotifyIcon.Dispose();
                    iNotifyIcon = null;
                }
                
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.panelDisplay = new System.Windows.Forms.Panel();
            this.iTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.marqueeLabelTop = new SharpDisplayManager.MarqueeLabel();
            this.marqueeLabelBottom = new SharpDisplayManager.MarqueeLabel();
            this.fontDialog = new System.Windows.Forms.FontDialog();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelConnect = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelSpring = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelPower = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelFps = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabPageClients = new System.Windows.Forms.TabPage();
            this.iCheckBoxStartIdleClient = new System.Windows.Forms.CheckBox();
            this.iButtonStartIdleClient = new System.Windows.Forms.Button();
            this.buttonCloseClients = new System.Windows.Forms.Button();
            this.buttonStartClient = new System.Windows.Forms.Button();
            this.iTreeViewClients = new System.Windows.Forms.TreeView();
            this.tabPageDisplay = new System.Windows.Forms.TabPage();
            this.buttonShowClock = new System.Windows.Forms.Button();
            this.buttonHideClock = new System.Windows.Forms.Button();
            this.buttonPowerOff = new System.Windows.Forms.Button();
            this.buttonPowerOn = new System.Windows.Forms.Button();
            this.labelTimerInterval = new System.Windows.Forms.Label();
            this.maskedTextBoxTimerInterval = new System.Windows.Forms.MaskedTextBox();
            this.comboBoxDisplayType = new System.Windows.Forms.ComboBox();
            this.buttonSuspend = new System.Windows.Forms.Button();
            this.checkBoxConnectOnStartup = new System.Windows.Forms.CheckBox();
            this.trackBarBrightness = new System.Windows.Forms.TrackBar();
            this.buttonFill = new System.Windows.Forms.Button();
            this.buttonClear = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonOpen = new System.Windows.Forms.Button();
            this.buttonCapture = new System.Windows.Forms.Button();
            this.labelWarning = new System.Windows.Forms.Label();
            this.checkBoxFixedPitchFontOnly = new System.Windows.Forms.CheckBox();
            this.checkBoxShowBorders = new System.Windows.Forms.CheckBox();
            this.buttonFont = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageDesign = new System.Windows.Forms.TabPage();
            this.labelScrollingSpeed = new System.Windows.Forms.Label();
            this.maskedTextBoxScrollingSpeed = new System.Windows.Forms.MaskedTextBox();
            this.labelScrollLoopSeparator = new System.Windows.Forms.Label();
            this.textBoxScrollLoopSeparator = new System.Windows.Forms.TextBox();
            this.labelMinFontSize = new System.Windows.Forms.Label();
            this.maskedTextBoxMinFontSize = new System.Windows.Forms.MaskedTextBox();
            this.checkBoxScaleToFit = new System.Windows.Forms.CheckBox();
            this.checkBoxInverseColors = new System.Windows.Forms.CheckBox();
            this.checkBoxReverseScreen = new System.Windows.Forms.CheckBox();
            this.tabPageAudio = new System.Windows.Forms.TabPage();
            this.labelDefaultAudioDevice = new System.Windows.Forms.Label();
            this.checkBoxShowVolumeLabel = new System.Windows.Forms.CheckBox();
            this.checkBoxMute = new System.Windows.Forms.CheckBox();
            this.trackBarMasterVolume = new System.Windows.Forms.TrackBar();
            this.tabPageInput = new System.Windows.Forms.TabPage();
            this.comboBoxOpticalDrives = new System.Windows.Forms.ComboBox();
            this.labelOpticalDriveEject = new System.Windows.Forms.Label();
            this.pictureBoxGreenStart = new System.Windows.Forms.PictureBox();
            this.labelStartFileName = new System.Windows.Forms.Label();
            this.buttonSelectFile = new System.Windows.Forms.Button();
            this.tabPageCec = new System.Windows.Forms.TabPage();
            this.groupBoxCecLogOptions = new System.Windows.Forms.GroupBox();
            this.checkBoxCecLogNoPoll = new System.Windows.Forms.CheckBox();
            this.checkBoxCecLogTraffic = new System.Windows.Forms.CheckBox();
            this.checkBoxCecLogDebug = new System.Windows.Forms.CheckBox();
            this.checkBoxCecLogNotice = new System.Windows.Forms.CheckBox();
            this.checkBoxCecLogError = new System.Windows.Forms.CheckBox();
            this.checkBoxCecLogWarning = new System.Windows.Forms.CheckBox();
            this.checkBoxCecEnabled = new System.Windows.Forms.CheckBox();
            this.labelHdmiPort = new System.Windows.Forms.Label();
            this.comboBoxHdmiPort = new System.Windows.Forms.ComboBox();
            this.tabPageHarmony = new System.Windows.Forms.TabPage();
            this.iCheckBoxHarmonyEnabled = new System.Windows.Forms.CheckBox();
            this.iTreeViewHarmony = new System.Windows.Forms.TreeView();
            this.iButtonHarmonyConnect = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.iTextBoxLogitechPassword = new System.Windows.Forms.TextBox();
            this.labelLogitechUserName = new System.Windows.Forms.Label();
            this.iTextBoxLogitechUserName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.iTextBoxHarmonyHubAddress = new System.Windows.Forms.TextBox();
            this.tabPageEvent = new System.Windows.Forms.TabPage();
            this.buttonEventEdit = new System.Windows.Forms.Button();
            this.buttonEventDelete = new System.Windows.Forms.Button();
            this.buttonEventAdd = new System.Windows.Forms.Button();
            this.buttonEventTest = new System.Windows.Forms.Button();
            this.buttonActionEdit = new System.Windows.Forms.Button();
            this.buttonActionMoveUp = new System.Windows.Forms.Button();
            this.buttonActionMoveDown = new System.Windows.Forms.Button();
            this.buttonActionTest = new System.Windows.Forms.Button();
            this.buttonActionDelete = new System.Windows.Forms.Button();
            this.buttonActionAdd = new System.Windows.Forms.Button();
            this.iTreeViewEvents = new System.Windows.Forms.TreeView();
            this.tabPageApp = new System.Windows.Forms.TabPage();
            this.checkBoxStartMinimized = new System.Windows.Forms.CheckBox();
            this.checkBoxMinimizeToTray = new System.Windows.Forms.CheckBox();
            this.checkBoxAutoStart = new System.Windows.Forms.CheckBox();
            this.buttonUpdate = new System.Windows.Forms.Button();
            this.tabPageLogs = new System.Windows.Forms.TabPage();
            this.buttonClearLogs = new System.Windows.Forms.Button();
            this.richTextBoxLogs = new System.Windows.Forms.RichTextBox();
            this.labelFontWidth = new System.Windows.Forms.Label();
            this.labelFontHeight = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.panelDisplay.SuspendLayout();
            this.iTableLayoutPanel.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.tabPageClients.SuspendLayout();
            this.tabPageDisplay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBrightness)).BeginInit();
            this.tabControl.SuspendLayout();
            this.tabPageDesign.SuspendLayout();
            this.tabPageAudio.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMasterVolume)).BeginInit();
            this.tabPageInput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxGreenStart)).BeginInit();
            this.tabPageCec.SuspendLayout();
            this.groupBoxCecLogOptions.SuspendLayout();
            this.tabPageHarmony.SuspendLayout();
            this.tabPageEvent.SuspendLayout();
            this.tabPageApp.SuspendLayout();
            this.tabPageLogs.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelDisplay
            // 
            this.panelDisplay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelDisplay.Controls.Add(this.iTableLayoutPanel);
            this.panelDisplay.Location = new System.Drawing.Point(173, 40);
            this.panelDisplay.Margin = new System.Windows.Forms.Padding(0);
            this.panelDisplay.Name = "panelDisplay";
            this.panelDisplay.Size = new System.Drawing.Size(258, 66);
            this.panelDisplay.TabIndex = 12;
            // 
            // iTableLayoutPanel
            // 
            this.iTableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.iTableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.iTableLayoutPanel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.iTableLayoutPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.iTableLayoutPanel.ColumnCount = 1;
            this.iTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.iTableLayoutPanel.Controls.Add(this.marqueeLabelTop, 0, 0);
            this.iTableLayoutPanel.Controls.Add(this.marqueeLabelBottom, 0, 1);
            this.iTableLayoutPanel.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.iTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.iTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.iTableLayoutPanel.Name = "iTableLayoutPanel";
            this.iTableLayoutPanel.RowCount = 2;
            this.iTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.iTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.iTableLayoutPanel.Size = new System.Drawing.Size(256, 64);
            this.iTableLayoutPanel.TabIndex = 5;
            this.iTableLayoutPanel.SizeChanged += new System.EventHandler(this.tableLayoutPanel_SizeChanged);
            // 
            // marqueeLabelTop
            // 
            this.marqueeLabelTop.AutoEllipsis = true;
            this.marqueeLabelTop.AutoSize = true;
            this.marqueeLabelTop.BackColor = System.Drawing.Color.Transparent;
            this.marqueeLabelTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.marqueeLabelTop.Location = new System.Drawing.Point(1, 1);
            this.marqueeLabelTop.Margin = new System.Windows.Forms.Padding(0);
            this.marqueeLabelTop.MinFontSize = 15F;
            this.marqueeLabelTop.Name = "marqueeLabelTop";
            this.marqueeLabelTop.OwnTimer = false;
            this.marqueeLabelTop.PixelsPerSecond = 64;
            this.marqueeLabelTop.ScaleToFit = true;
            this.marqueeLabelTop.Separator = "|";
            this.marqueeLabelTop.Size = new System.Drawing.Size(254, 30);
            this.marqueeLabelTop.TabIndex = 2;
            this.marqueeLabelTop.Text = "ABCDEFGHIJKLMNOPQRST-0123456789";
            this.marqueeLabelTop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.marqueeLabelTop.UseCompatibleTextRendering = true;
            // 
            // marqueeLabelBottom
            // 
            this.marqueeLabelBottom.AutoEllipsis = true;
            this.marqueeLabelBottom.AutoSize = true;
            this.marqueeLabelBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.marqueeLabelBottom.Location = new System.Drawing.Point(1, 32);
            this.marqueeLabelBottom.Margin = new System.Windows.Forms.Padding(0);
            this.marqueeLabelBottom.MinFontSize = 15F;
            this.marqueeLabelBottom.Name = "marqueeLabelBottom";
            this.marqueeLabelBottom.OwnTimer = false;
            this.marqueeLabelBottom.PixelsPerSecond = 64;
            this.marqueeLabelBottom.ScaleToFit = true;
            this.marqueeLabelBottom.Separator = "|";
            this.marqueeLabelBottom.Size = new System.Drawing.Size(254, 31);
            this.marqueeLabelBottom.TabIndex = 3;
            this.marqueeLabelBottom.Text = "abcdefghijklmnopqrst-0123456789";
            this.marqueeLabelBottom.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.marqueeLabelBottom.UseCompatibleTextRendering = true;
            // 
            // timer
            // 
            this.timer.Interval = 50;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelConnect,
            this.toolStripStatusLabelSpring,
            this.toolStripStatusLabelPower,
            this.toolStripStatusLabelFps});
            this.statusStrip.Location = new System.Drawing.Point(0, 539);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.statusStrip.Size = new System.Drawing.Size(784, 22);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip";
            // 
            // toolStripStatusLabelConnect
            // 
            this.toolStripStatusLabelConnect.Name = "toolStripStatusLabelConnect";
            this.toolStripStatusLabelConnect.Size = new System.Drawing.Size(86, 17);
            this.toolStripStatusLabelConnect.Text = "Not connected";
            // 
            // toolStripStatusLabelSpring
            // 
            this.toolStripStatusLabelSpring.Name = "toolStripStatusLabelSpring";
            this.toolStripStatusLabelSpring.Size = new System.Drawing.Size(633, 17);
            this.toolStripStatusLabelSpring.Spring = true;
            // 
            // toolStripStatusLabelPower
            // 
            this.toolStripStatusLabelPower.Name = "toolStripStatusLabelPower";
            this.toolStripStatusLabelPower.Size = new System.Drawing.Size(24, 17);
            this.toolStripStatusLabelPower.Text = "NA";
            // 
            // toolStripStatusLabelFps
            // 
            this.toolStripStatusLabelFps.Name = "toolStripStatusLabelFps";
            this.toolStripStatusLabelFps.Size = new System.Drawing.Size(26, 17);
            this.toolStripStatusLabelFps.Text = "FPS";
            // 
            // tabPageClients
            // 
            this.tabPageClients.Controls.Add(this.iCheckBoxStartIdleClient);
            this.tabPageClients.Controls.Add(this.iButtonStartIdleClient);
            this.tabPageClients.Controls.Add(this.buttonCloseClients);
            this.tabPageClients.Controls.Add(this.buttonStartClient);
            this.tabPageClients.Controls.Add(this.iTreeViewClients);
            this.tabPageClients.Location = new System.Drawing.Point(4, 22);
            this.tabPageClients.Name = "tabPageClients";
            this.tabPageClients.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageClients.Size = new System.Drawing.Size(752, 385);
            this.tabPageClients.TabIndex = 2;
            this.tabPageClients.Text = "Clients";
            this.tabPageClients.UseVisualStyleBackColor = true;
            // 
            // iCheckBoxStartIdleClient
            // 
            this.iCheckBoxStartIdleClient.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.iCheckBoxStartIdleClient.AutoSize = true;
            this.iCheckBoxStartIdleClient.Location = new System.Drawing.Point(108, 340);
            this.iCheckBoxStartIdleClient.Name = "iCheckBoxStartIdleClient";
            this.iCheckBoxStartIdleClient.Size = new System.Drawing.Size(145, 17);
            this.iCheckBoxStartIdleClient.TabIndex = 22;
            this.iCheckBoxStartIdleClient.Text = "Start idle client on startup";
            this.iCheckBoxStartIdleClient.UseVisualStyleBackColor = true;
            this.iCheckBoxStartIdleClient.CheckedChanged += new System.EventHandler(this.checkBoxStartIdleClient_CheckedChanged);
            // 
            // iButtonStartIdleClient
            // 
            this.iButtonStartIdleClient.Location = new System.Drawing.Point(6, 35);
            this.iButtonStartIdleClient.Name = "iButtonStartIdleClient";
            this.iButtonStartIdleClient.Size = new System.Drawing.Size(96, 23);
            this.iButtonStartIdleClient.TabIndex = 21;
            this.iButtonStartIdleClient.Text = "Start Idle Client";
            this.iButtonStartIdleClient.UseVisualStyleBackColor = true;
            this.iButtonStartIdleClient.Click += new System.EventHandler(this.ButtonStartIdleClient_Click);
            // 
            // buttonCloseClients
            // 
            this.buttonCloseClients.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCloseClients.Location = new System.Drawing.Point(6, 336);
            this.buttonCloseClients.Name = "buttonCloseClients";
            this.buttonCloseClients.Size = new System.Drawing.Size(96, 23);
            this.buttonCloseClients.TabIndex = 20;
            this.buttonCloseClients.Text = "Close Clients";
            this.buttonCloseClients.UseVisualStyleBackColor = true;
            this.buttonCloseClients.Click += new System.EventHandler(this.buttonCloseClients_Click);
            // 
            // buttonStartClient
            // 
            this.buttonStartClient.Location = new System.Drawing.Point(6, 6);
            this.buttonStartClient.Name = "buttonStartClient";
            this.buttonStartClient.Size = new System.Drawing.Size(96, 23);
            this.buttonStartClient.TabIndex = 19;
            this.buttonStartClient.Text = "Start Test Client";
            this.buttonStartClient.UseVisualStyleBackColor = true;
            this.buttonStartClient.Click += new System.EventHandler(this.buttonStartClient_Click);
            // 
            // iTreeViewClients
            // 
            this.iTreeViewClients.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.iTreeViewClients.Location = new System.Drawing.Point(108, 6);
            this.iTreeViewClients.Name = "iTreeViewClients";
            this.iTreeViewClients.Size = new System.Drawing.Size(638, 328);
            this.iTreeViewClients.TabIndex = 0;
            this.iTreeViewClients.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewClients_AfterSelect);
            // 
            // tabPageDisplay
            // 
            this.tabPageDisplay.Controls.Add(this.buttonShowClock);
            this.tabPageDisplay.Controls.Add(this.buttonHideClock);
            this.tabPageDisplay.Controls.Add(this.buttonPowerOff);
            this.tabPageDisplay.Controls.Add(this.buttonPowerOn);
            this.tabPageDisplay.Controls.Add(this.labelTimerInterval);
            this.tabPageDisplay.Controls.Add(this.maskedTextBoxTimerInterval);
            this.tabPageDisplay.Controls.Add(this.comboBoxDisplayType);
            this.tabPageDisplay.Controls.Add(this.buttonSuspend);
            this.tabPageDisplay.Controls.Add(this.checkBoxConnectOnStartup);
            this.tabPageDisplay.Controls.Add(this.trackBarBrightness);
            this.tabPageDisplay.Controls.Add(this.buttonFill);
            this.tabPageDisplay.Controls.Add(this.buttonClear);
            this.tabPageDisplay.Controls.Add(this.buttonClose);
            this.tabPageDisplay.Controls.Add(this.buttonOpen);
            this.tabPageDisplay.Controls.Add(this.buttonCapture);
            this.tabPageDisplay.Location = new System.Drawing.Point(4, 22);
            this.tabPageDisplay.Name = "tabPageDisplay";
            this.tabPageDisplay.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDisplay.Size = new System.Drawing.Size(752, 385);
            this.tabPageDisplay.TabIndex = 0;
            this.tabPageDisplay.Text = "Display";
            this.tabPageDisplay.UseVisualStyleBackColor = true;
            // 
            // buttonShowClock
            // 
            this.buttonShowClock.Location = new System.Drawing.Point(6, 122);
            this.buttonShowClock.Name = "buttonShowClock";
            this.buttonShowClock.Size = new System.Drawing.Size(75, 23);
            this.buttonShowClock.TabIndex = 23;
            this.buttonShowClock.Text = "Show Clock";
            this.buttonShowClock.UseVisualStyleBackColor = true;
            this.buttonShowClock.Click += new System.EventHandler(this.buttonShowClock_Click);
            // 
            // buttonHideClock
            // 
            this.buttonHideClock.Location = new System.Drawing.Point(6, 151);
            this.buttonHideClock.Name = "buttonHideClock";
            this.buttonHideClock.Size = new System.Drawing.Size(75, 23);
            this.buttonHideClock.TabIndex = 22;
            this.buttonHideClock.Text = "Hide Clock";
            this.buttonHideClock.UseVisualStyleBackColor = true;
            this.buttonHideClock.Click += new System.EventHandler(this.buttonHideClock_Click);
            // 
            // buttonPowerOff
            // 
            this.buttonPowerOff.Location = new System.Drawing.Point(6, 209);
            this.buttonPowerOff.Name = "buttonPowerOff";
            this.buttonPowerOff.Size = new System.Drawing.Size(75, 23);
            this.buttonPowerOff.TabIndex = 21;
            this.buttonPowerOff.Text = "OFF";
            this.buttonPowerOff.UseVisualStyleBackColor = true;
            this.buttonPowerOff.Click += new System.EventHandler(this.buttonPowerOff_Click);
            // 
            // buttonPowerOn
            // 
            this.buttonPowerOn.Location = new System.Drawing.Point(6, 180);
            this.buttonPowerOn.Name = "buttonPowerOn";
            this.buttonPowerOn.Size = new System.Drawing.Size(75, 23);
            this.buttonPowerOn.TabIndex = 20;
            this.buttonPowerOn.Text = "ON";
            this.buttonPowerOn.UseVisualStyleBackColor = true;
            this.buttonPowerOn.Click += new System.EventHandler(this.buttonPowerOn_Click);
            // 
            // labelTimerInterval
            // 
            this.labelTimerInterval.AutoSize = true;
            this.labelTimerInterval.Location = new System.Drawing.Point(184, 45);
            this.labelTimerInterval.Name = "labelTimerInterval";
            this.labelTimerInterval.Size = new System.Drawing.Size(98, 13);
            this.labelTimerInterval.TabIndex = 19;
            this.labelTimerInterval.Text = "Timer interval (ms) :";
            // 
            // maskedTextBoxTimerInterval
            // 
            this.maskedTextBoxTimerInterval.Location = new System.Drawing.Point(288, 42);
            this.maskedTextBoxTimerInterval.Mask = "000";
            this.maskedTextBoxTimerInterval.Name = "maskedTextBoxTimerInterval";
            this.maskedTextBoxTimerInterval.PromptChar = ' ';
            this.maskedTextBoxTimerInterval.Size = new System.Drawing.Size(24, 20);
            this.maskedTextBoxTimerInterval.TabIndex = 18;
            this.maskedTextBoxTimerInterval.TextChanged += new System.EventHandler(this.maskedTextBoxTimerInterval_TextChanged);
            // 
            // comboBoxDisplayType
            // 
            this.comboBoxDisplayType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDisplayType.FormattingEnabled = true;
            this.comboBoxDisplayType.Location = new System.Drawing.Point(187, 9);
            this.comboBoxDisplayType.Name = "comboBoxDisplayType";
            this.comboBoxDisplayType.Size = new System.Drawing.Size(181, 21);
            this.comboBoxDisplayType.TabIndex = 17;
            this.comboBoxDisplayType.SelectedIndexChanged += new System.EventHandler(this.comboBoxDisplayType_SelectedIndexChanged);
            // 
            // buttonSuspend
            // 
            this.buttonSuspend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSuspend.Location = new System.Drawing.Point(6, 317);
            this.buttonSuspend.Name = "buttonSuspend";
            this.buttonSuspend.Size = new System.Drawing.Size(75, 23);
            this.buttonSuspend.TabIndex = 16;
            this.buttonSuspend.Text = "Pause";
            this.buttonSuspend.UseVisualStyleBackColor = true;
            this.buttonSuspend.Click += new System.EventHandler(this.buttonSuspend_Click);
            // 
            // checkBoxConnectOnStartup
            // 
            this.checkBoxConnectOnStartup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxConnectOnStartup.AutoSize = true;
            this.checkBoxConnectOnStartup.Location = new System.Drawing.Point(113, 350);
            this.checkBoxConnectOnStartup.Name = "checkBoxConnectOnStartup";
            this.checkBoxConnectOnStartup.Size = new System.Drawing.Size(119, 17);
            this.checkBoxConnectOnStartup.TabIndex = 13;
            this.checkBoxConnectOnStartup.Text = "Connect on stratup ";
            this.checkBoxConnectOnStartup.UseVisualStyleBackColor = true;
            this.checkBoxConnectOnStartup.CheckedChanged += new System.EventHandler(this.checkBoxConnectOnStartup_CheckedChanged);
            // 
            // trackBarBrightness
            // 
            this.trackBarBrightness.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarBrightness.BackColor = System.Drawing.SystemColors.Window;
            this.trackBarBrightness.Location = new System.Drawing.Point(704, 9);
            this.trackBarBrightness.Name = "trackBarBrightness";
            this.trackBarBrightness.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBarBrightness.Size = new System.Drawing.Size(45, 358);
            this.trackBarBrightness.TabIndex = 10;
            this.trackBarBrightness.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.toolTip.SetToolTip(this.trackBarBrightness, "Brightness adjustment");
            this.trackBarBrightness.Scroll += new System.EventHandler(this.trackBarBrightness_Scroll);
            // 
            // buttonFill
            // 
            this.buttonFill.Location = new System.Drawing.Point(6, 93);
            this.buttonFill.Name = "buttonFill";
            this.buttonFill.Size = new System.Drawing.Size(75, 23);
            this.buttonFill.TabIndex = 9;
            this.buttonFill.Text = "Fill";
            this.buttonFill.UseVisualStyleBackColor = true;
            this.buttonFill.Click += new System.EventHandler(this.buttonFill_Click);
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(6, 64);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(75, 23);
            this.buttonClear.TabIndex = 8;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(6, 35);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 7;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonOpen
            // 
            this.buttonOpen.Location = new System.Drawing.Point(6, 6);
            this.buttonOpen.Name = "buttonOpen";
            this.buttonOpen.Size = new System.Drawing.Size(75, 23);
            this.buttonOpen.TabIndex = 6;
            this.buttonOpen.Text = "Open";
            this.buttonOpen.UseVisualStyleBackColor = true;
            this.buttonOpen.Click += new System.EventHandler(this.buttonOpen_Click);
            // 
            // buttonCapture
            // 
            this.buttonCapture.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCapture.Location = new System.Drawing.Point(6, 346);
            this.buttonCapture.Name = "buttonCapture";
            this.buttonCapture.Size = new System.Drawing.Size(75, 23);
            this.buttonCapture.TabIndex = 5;
            this.buttonCapture.Text = "Capture";
            this.buttonCapture.UseVisualStyleBackColor = true;
            this.buttonCapture.Click += new System.EventHandler(this.buttonCapture_Click);
            // 
            // labelWarning
            // 
            this.labelWarning.AutoSize = true;
            this.labelWarning.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelWarning.ForeColor = System.Drawing.Color.Red;
            this.labelWarning.Location = new System.Drawing.Point(9, 9);
            this.labelWarning.Name = "labelWarning";
            this.labelWarning.Size = new System.Drawing.Size(80, 16);
            this.labelWarning.TabIndex = 18;
            this.labelWarning.Text = "WARNING";
            this.labelWarning.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelWarning.Visible = false;
            // 
            // checkBoxFixedPitchFontOnly
            // 
            this.checkBoxFixedPitchFontOnly.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxFixedPitchFontOnly.AutoSize = true;
            this.checkBoxFixedPitchFontOnly.Location = new System.Drawing.Point(87, 362);
            this.checkBoxFixedPitchFontOnly.Name = "checkBoxFixedPitchFontOnly";
            this.checkBoxFixedPitchFontOnly.Size = new System.Drawing.Size(120, 17);
            this.checkBoxFixedPitchFontOnly.TabIndex = 17;
            this.checkBoxFixedPitchFontOnly.Text = "Fixed pitch font only";
            this.checkBoxFixedPitchFontOnly.UseVisualStyleBackColor = true;
            // 
            // checkBoxShowBorders
            // 
            this.checkBoxShowBorders.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxShowBorders.AutoSize = true;
            this.checkBoxShowBorders.Location = new System.Drawing.Point(645, 29);
            this.checkBoxShowBorders.Name = "checkBoxShowBorders";
            this.checkBoxShowBorders.Size = new System.Drawing.Size(91, 17);
            this.checkBoxShowBorders.TabIndex = 11;
            this.checkBoxShowBorders.Text = "Show borders";
            this.checkBoxShowBorders.UseVisualStyleBackColor = true;
            this.checkBoxShowBorders.CheckedChanged += new System.EventHandler(this.checkBoxShowBorders_CheckedChanged);
            // 
            // buttonFont
            // 
            this.buttonFont.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonFont.Location = new System.Drawing.Point(6, 356);
            this.buttonFont.Name = "buttonFont";
            this.buttonFont.Size = new System.Drawing.Size(75, 23);
            this.buttonFont.TabIndex = 0;
            this.buttonFont.Text = "Select Font";
            this.buttonFont.UseVisualStyleBackColor = true;
            this.buttonFont.Click += new System.EventHandler(this.buttonFont_Click);
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabPageDisplay);
            this.tabControl.Controls.Add(this.tabPageClients);
            this.tabControl.Controls.Add(this.tabPageDesign);
            this.tabControl.Controls.Add(this.tabPageAudio);
            this.tabControl.Controls.Add(this.tabPageInput);
            this.tabControl.Controls.Add(this.tabPageCec);
            this.tabControl.Controls.Add(this.tabPageHarmony);
            this.tabControl.Controls.Add(this.tabPageEvent);
            this.tabControl.Controls.Add(this.tabPageApp);
            this.tabControl.Controls.Add(this.tabPageLogs);
            this.tabControl.Location = new System.Drawing.Point(12, 125);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(760, 411);
            this.tabControl.TabIndex = 0;
            // 
            // tabPageDesign
            // 
            this.tabPageDesign.Controls.Add(this.labelScrollingSpeed);
            this.tabPageDesign.Controls.Add(this.maskedTextBoxScrollingSpeed);
            this.tabPageDesign.Controls.Add(this.labelScrollLoopSeparator);
            this.tabPageDesign.Controls.Add(this.textBoxScrollLoopSeparator);
            this.tabPageDesign.Controls.Add(this.labelMinFontSize);
            this.tabPageDesign.Controls.Add(this.maskedTextBoxMinFontSize);
            this.tabPageDesign.Controls.Add(this.checkBoxScaleToFit);
            this.tabPageDesign.Controls.Add(this.checkBoxInverseColors);
            this.tabPageDesign.Controls.Add(this.checkBoxFixedPitchFontOnly);
            this.tabPageDesign.Controls.Add(this.buttonFont);
            this.tabPageDesign.Controls.Add(this.checkBoxReverseScreen);
            this.tabPageDesign.Controls.Add(this.checkBoxShowBorders);
            this.tabPageDesign.Location = new System.Drawing.Point(4, 22);
            this.tabPageDesign.Name = "tabPageDesign";
            this.tabPageDesign.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDesign.Size = new System.Drawing.Size(752, 385);
            this.tabPageDesign.TabIndex = 3;
            this.tabPageDesign.Text = "Design";
            this.tabPageDesign.UseVisualStyleBackColor = true;
            // 
            // labelScrollingSpeed
            // 
            this.labelScrollingSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelScrollingSpeed.AutoSize = true;
            this.labelScrollingSpeed.Location = new System.Drawing.Point(92, 248);
            this.labelScrollingSpeed.Name = "labelScrollingSpeed";
            this.labelScrollingSpeed.Size = new System.Drawing.Size(115, 13);
            this.labelScrollingSpeed.TabIndex = 28;
            this.labelScrollingSpeed.Text = "Scrolling speed (px/s) :";
            // 
            // maskedTextBoxScrollingSpeed
            // 
            this.maskedTextBoxScrollingSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.maskedTextBoxScrollingSpeed.Location = new System.Drawing.Point(213, 245);
            this.maskedTextBoxScrollingSpeed.Mask = "000";
            this.maskedTextBoxScrollingSpeed.Name = "maskedTextBoxScrollingSpeed";
            this.maskedTextBoxScrollingSpeed.PromptChar = ' ';
            this.maskedTextBoxScrollingSpeed.Size = new System.Drawing.Size(24, 20);
            this.maskedTextBoxScrollingSpeed.TabIndex = 27;
            this.maskedTextBoxScrollingSpeed.TextChanged += new System.EventHandler(this.maskedTextBoxScrollingSpeed_TextChanged);
            // 
            // labelScrollLoopSeparator
            // 
            this.labelScrollLoopSeparator.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelScrollLoopSeparator.AutoSize = true;
            this.labelScrollLoopSeparator.Location = new System.Drawing.Point(98, 274);
            this.labelScrollLoopSeparator.Name = "labelScrollLoopSeparator";
            this.labelScrollLoopSeparator.Size = new System.Drawing.Size(109, 13);
            this.labelScrollLoopSeparator.TabIndex = 26;
            this.labelScrollLoopSeparator.Text = "Scroll loop separator :";
            // 
            // textBoxScrollLoopSeparator
            // 
            this.textBoxScrollLoopSeparator.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxScrollLoopSeparator.Location = new System.Drawing.Point(213, 271);
            this.textBoxScrollLoopSeparator.Name = "textBoxScrollLoopSeparator";
            this.textBoxScrollLoopSeparator.Size = new System.Drawing.Size(74, 20);
            this.textBoxScrollLoopSeparator.TabIndex = 25;
            this.textBoxScrollLoopSeparator.TextChanged += new System.EventHandler(this.textBoxScrollLoopSeparator_TextChanged);
            // 
            // labelMinFontSize
            // 
            this.labelMinFontSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelMinFontSize.AutoSize = true;
            this.labelMinFontSize.Location = new System.Drawing.Point(88, 323);
            this.labelMinFontSize.Name = "labelMinFontSize";
            this.labelMinFontSize.Size = new System.Drawing.Size(119, 13);
            this.labelMinFontSize.TabIndex = 24;
            this.labelMinFontSize.Text = "Minimum font size (pts) :";
            // 
            // maskedTextBoxMinFontSize
            // 
            this.maskedTextBoxMinFontSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.maskedTextBoxMinFontSize.Location = new System.Drawing.Point(213, 320);
            this.maskedTextBoxMinFontSize.Mask = "000";
            this.maskedTextBoxMinFontSize.Name = "maskedTextBoxMinFontSize";
            this.maskedTextBoxMinFontSize.PromptChar = ' ';
            this.maskedTextBoxMinFontSize.Size = new System.Drawing.Size(24, 20);
            this.maskedTextBoxMinFontSize.TabIndex = 23;
            this.maskedTextBoxMinFontSize.TextChanged += new System.EventHandler(this.maskedTextBoxMinFontSize_TextChanged);
            // 
            // checkBoxScaleToFit
            // 
            this.checkBoxScaleToFit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxScaleToFit.AutoSize = true;
            this.checkBoxScaleToFit.Location = new System.Drawing.Point(86, 297);
            this.checkBoxScaleToFit.Name = "checkBoxScaleToFit";
            this.checkBoxScaleToFit.Size = new System.Drawing.Size(201, 17);
            this.checkBoxScaleToFit.TabIndex = 22;
            this.checkBoxScaleToFit.Text = "Try scale font down to avoid scrolling";
            this.checkBoxScaleToFit.UseVisualStyleBackColor = true;
            this.checkBoxScaleToFit.CheckedChanged += new System.EventHandler(this.checkBoxScaleToFit_CheckedChanged);
            // 
            // checkBoxInverseColors
            // 
            this.checkBoxInverseColors.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxInverseColors.AutoSize = true;
            this.checkBoxInverseColors.Location = new System.Drawing.Point(645, 52);
            this.checkBoxInverseColors.Name = "checkBoxInverseColors";
            this.checkBoxInverseColors.Size = new System.Drawing.Size(92, 17);
            this.checkBoxInverseColors.TabIndex = 21;
            this.checkBoxInverseColors.Text = "Inverse colors";
            this.checkBoxInverseColors.UseVisualStyleBackColor = true;
            this.checkBoxInverseColors.CheckedChanged += new System.EventHandler(this.checkBoxInverseColors_CheckedChanged);
            // 
            // checkBoxReverseScreen
            // 
            this.checkBoxReverseScreen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxReverseScreen.AutoSize = true;
            this.checkBoxReverseScreen.Location = new System.Drawing.Point(645, 6);
            this.checkBoxReverseScreen.Name = "checkBoxReverseScreen";
            this.checkBoxReverseScreen.Size = new System.Drawing.Size(101, 17);
            this.checkBoxReverseScreen.TabIndex = 14;
            this.checkBoxReverseScreen.Text = "Reverse screen";
            this.checkBoxReverseScreen.UseVisualStyleBackColor = true;
            this.checkBoxReverseScreen.CheckedChanged += new System.EventHandler(this.checkBoxReverseScreen_CheckedChanged);
            // 
            // tabPageAudio
            // 
            this.tabPageAudio.Controls.Add(this.labelDefaultAudioDevice);
            this.tabPageAudio.Controls.Add(this.checkBoxShowVolumeLabel);
            this.tabPageAudio.Controls.Add(this.checkBoxMute);
            this.tabPageAudio.Controls.Add(this.trackBarMasterVolume);
            this.tabPageAudio.Location = new System.Drawing.Point(4, 22);
            this.tabPageAudio.Name = "tabPageAudio";
            this.tabPageAudio.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAudio.Size = new System.Drawing.Size(752, 385);
            this.tabPageAudio.TabIndex = 5;
            this.tabPageAudio.Text = "Audio";
            this.tabPageAudio.UseVisualStyleBackColor = true;
            // 
            // labelDefaultAudioDevice
            // 
            this.labelDefaultAudioDevice.AutoSize = true;
            this.labelDefaultAudioDevice.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDefaultAudioDevice.Location = new System.Drawing.Point(3, 6);
            this.labelDefaultAudioDevice.Name = "labelDefaultAudioDevice";
            this.labelDefaultAudioDevice.Size = new System.Drawing.Size(120, 13);
            this.labelDefaultAudioDevice.TabIndex = 19;
            this.labelDefaultAudioDevice.Text = "Audio Device Unknown";
            // 
            // checkBoxShowVolumeLabel
            // 
            this.checkBoxShowVolumeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxShowVolumeLabel.AutoSize = true;
            this.checkBoxShowVolumeLabel.Location = new System.Drawing.Point(3, 329);
            this.checkBoxShowVolumeLabel.Name = "checkBoxShowVolumeLabel";
            this.checkBoxShowVolumeLabel.Size = new System.Drawing.Size(115, 17);
            this.checkBoxShowVolumeLabel.TabIndex = 18;
            this.checkBoxShowVolumeLabel.Text = "Show volume label";
            this.checkBoxShowVolumeLabel.UseVisualStyleBackColor = true;
            this.checkBoxShowVolumeLabel.CheckedChanged += new System.EventHandler(this.checkBoxShowVolumeLabel_CheckedChanged);
            // 
            // checkBoxMute
            // 
            this.checkBoxMute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxMute.AutoSize = true;
            this.checkBoxMute.Location = new System.Drawing.Point(3, 352);
            this.checkBoxMute.Name = "checkBoxMute";
            this.checkBoxMute.Size = new System.Drawing.Size(50, 17);
            this.checkBoxMute.TabIndex = 17;
            this.checkBoxMute.Text = "Mute";
            this.checkBoxMute.UseVisualStyleBackColor = true;
            this.checkBoxMute.CheckedChanged += new System.EventHandler(this.checkBoxMute_CheckedChanged);
            // 
            // trackBarMasterVolume
            // 
            this.trackBarMasterVolume.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarMasterVolume.BackColor = System.Drawing.SystemColors.Window;
            this.trackBarMasterVolume.Location = new System.Drawing.Point(701, 6);
            this.trackBarMasterVolume.Maximum = 100;
            this.trackBarMasterVolume.Name = "trackBarMasterVolume";
            this.trackBarMasterVolume.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBarMasterVolume.Size = new System.Drawing.Size(45, 373);
            this.trackBarMasterVolume.TabIndex = 0;
            this.trackBarMasterVolume.TickFrequency = 10;
            this.trackBarMasterVolume.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.toolTip.SetToolTip(this.trackBarMasterVolume, "Master Volume");
            this.trackBarMasterVolume.Scroll += new System.EventHandler(this.trackBarMasterVolume_Scroll);
            // 
            // tabPageInput
            // 
            this.tabPageInput.Controls.Add(this.comboBoxOpticalDrives);
            this.tabPageInput.Controls.Add(this.labelOpticalDriveEject);
            this.tabPageInput.Controls.Add(this.pictureBoxGreenStart);
            this.tabPageInput.Controls.Add(this.labelStartFileName);
            this.tabPageInput.Controls.Add(this.buttonSelectFile);
            this.tabPageInput.Location = new System.Drawing.Point(4, 22);
            this.tabPageInput.Name = "tabPageInput";
            this.tabPageInput.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageInput.Size = new System.Drawing.Size(752, 385);
            this.tabPageInput.TabIndex = 6;
            this.tabPageInput.Text = "Input";
            this.tabPageInput.UseVisualStyleBackColor = true;
            // 
            // comboBoxOpticalDrives
            // 
            this.comboBoxOpticalDrives.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxOpticalDrives.FormattingEnabled = true;
            this.comboBoxOpticalDrives.Location = new System.Drawing.Point(113, 53);
            this.comboBoxOpticalDrives.Name = "comboBoxOpticalDrives";
            this.comboBoxOpticalDrives.Size = new System.Drawing.Size(55, 21);
            this.comboBoxOpticalDrives.TabIndex = 18;
            this.comboBoxOpticalDrives.SelectedIndexChanged += new System.EventHandler(this.comboBoxOpticalDrives_SelectedIndexChanged);
            // 
            // labelOpticalDriveEject
            // 
            this.labelOpticalDriveEject.AutoSize = true;
            this.labelOpticalDriveEject.Location = new System.Drawing.Point(0, 56);
            this.labelOpticalDriveEject.Name = "labelOpticalDriveEject";
            this.labelOpticalDriveEject.Size = new System.Drawing.Size(107, 13);
            this.labelOpticalDriveEject.TabIndex = 3;
            this.labelOpticalDriveEject.Text = "Optical drive to eject:";
            // 
            // pictureBoxGreenStart
            // 
            this.pictureBoxGreenStart.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxGreenStart.Image")));
            this.pictureBoxGreenStart.Location = new System.Drawing.Point(3, 6);
            this.pictureBoxGreenStart.Name = "pictureBoxGreenStart";
            this.pictureBoxGreenStart.Size = new System.Drawing.Size(32, 32);
            this.pictureBoxGreenStart.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxGreenStart.TabIndex = 2;
            this.pictureBoxGreenStart.TabStop = false;
            // 
            // labelStartFileName
            // 
            this.labelStartFileName.AutoSize = true;
            this.labelStartFileName.Location = new System.Drawing.Point(72, 16);
            this.labelStartFileName.Name = "labelStartFileName";
            this.labelStartFileName.Size = new System.Drawing.Size(33, 13);
            this.labelStartFileName.TabIndex = 1;
            this.labelStartFileName.Text = "None";
            // 
            // buttonSelectFile
            // 
            this.buttonSelectFile.Location = new System.Drawing.Point(41, 11);
            this.buttonSelectFile.Name = "buttonSelectFile";
            this.buttonSelectFile.Size = new System.Drawing.Size(25, 23);
            this.buttonSelectFile.TabIndex = 0;
            this.buttonSelectFile.Text = "...";
            this.buttonSelectFile.UseVisualStyleBackColor = true;
            this.buttonSelectFile.Click += new System.EventHandler(this.buttonSelectFile_Click);
            // 
            // tabPageCec
            // 
            this.tabPageCec.Controls.Add(this.groupBoxCecLogOptions);
            this.tabPageCec.Controls.Add(this.checkBoxCecEnabled);
            this.tabPageCec.Controls.Add(this.labelHdmiPort);
            this.tabPageCec.Controls.Add(this.comboBoxHdmiPort);
            this.tabPageCec.Location = new System.Drawing.Point(4, 22);
            this.tabPageCec.Name = "tabPageCec";
            this.tabPageCec.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCec.Size = new System.Drawing.Size(752, 385);
            this.tabPageCec.TabIndex = 7;
            this.tabPageCec.Text = "CEC";
            this.tabPageCec.UseVisualStyleBackColor = true;
            // 
            // groupBoxCecLogOptions
            // 
            this.groupBoxCecLogOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxCecLogOptions.Controls.Add(this.checkBoxCecLogNoPoll);
            this.groupBoxCecLogOptions.Controls.Add(this.checkBoxCecLogTraffic);
            this.groupBoxCecLogOptions.Controls.Add(this.checkBoxCecLogDebug);
            this.groupBoxCecLogOptions.Controls.Add(this.checkBoxCecLogNotice);
            this.groupBoxCecLogOptions.Controls.Add(this.checkBoxCecLogError);
            this.groupBoxCecLogOptions.Controls.Add(this.checkBoxCecLogWarning);
            this.groupBoxCecLogOptions.Location = new System.Drawing.Point(6, 239);
            this.groupBoxCecLogOptions.Name = "groupBoxCecLogOptions";
            this.groupBoxCecLogOptions.Size = new System.Drawing.Size(165, 140);
            this.groupBoxCecLogOptions.TabIndex = 25;
            this.groupBoxCecLogOptions.TabStop = false;
            this.groupBoxCecLogOptions.Text = "Log options";
            // 
            // checkBoxCecLogNoPoll
            // 
            this.checkBoxCecLogNoPoll.AutoSize = true;
            this.checkBoxCecLogNoPoll.Checked = true;
            this.checkBoxCecLogNoPoll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxCecLogNoPoll.Location = new System.Drawing.Point(76, 24);
            this.checkBoxCecLogNoPoll.Name = "checkBoxCecLogNoPoll";
            this.checkBoxCecLogNoPoll.Size = new System.Drawing.Size(59, 17);
            this.checkBoxCecLogNoPoll.TabIndex = 30;
            this.checkBoxCecLogNoPoll.Text = "No poll";
            this.checkBoxCecLogNoPoll.UseVisualStyleBackColor = true;
            this.checkBoxCecLogNoPoll.CheckedChanged += new System.EventHandler(this.checkBoxCecLogs_CheckedChanged);
            // 
            // checkBoxCecLogTraffic
            // 
            this.checkBoxCecLogTraffic.AutoSize = true;
            this.checkBoxCecLogTraffic.Location = new System.Drawing.Point(6, 93);
            this.checkBoxCecLogTraffic.Name = "checkBoxCecLogTraffic";
            this.checkBoxCecLogTraffic.Size = new System.Drawing.Size(56, 17);
            this.checkBoxCecLogTraffic.TabIndex = 29;
            this.checkBoxCecLogTraffic.Text = "Traffic";
            this.checkBoxCecLogTraffic.UseVisualStyleBackColor = true;
            this.checkBoxCecLogTraffic.CheckedChanged += new System.EventHandler(this.checkBoxCecLogs_CheckedChanged);
            // 
            // checkBoxCecLogDebug
            // 
            this.checkBoxCecLogDebug.AutoSize = true;
            this.checkBoxCecLogDebug.Location = new System.Drawing.Point(6, 116);
            this.checkBoxCecLogDebug.Name = "checkBoxCecLogDebug";
            this.checkBoxCecLogDebug.Size = new System.Drawing.Size(58, 17);
            this.checkBoxCecLogDebug.TabIndex = 28;
            this.checkBoxCecLogDebug.Text = "Debug";
            this.checkBoxCecLogDebug.UseVisualStyleBackColor = true;
            this.checkBoxCecLogDebug.CheckedChanged += new System.EventHandler(this.checkBoxCecLogs_CheckedChanged);
            // 
            // checkBoxCecLogNotice
            // 
            this.checkBoxCecLogNotice.AutoSize = true;
            this.checkBoxCecLogNotice.Checked = true;
            this.checkBoxCecLogNotice.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxCecLogNotice.Location = new System.Drawing.Point(6, 70);
            this.checkBoxCecLogNotice.Name = "checkBoxCecLogNotice";
            this.checkBoxCecLogNotice.Size = new System.Drawing.Size(57, 17);
            this.checkBoxCecLogNotice.TabIndex = 27;
            this.checkBoxCecLogNotice.Text = "Notice";
            this.checkBoxCecLogNotice.UseVisualStyleBackColor = true;
            this.checkBoxCecLogNotice.CheckedChanged += new System.EventHandler(this.checkBoxCecLogs_CheckedChanged);
            // 
            // checkBoxCecLogError
            // 
            this.checkBoxCecLogError.AutoSize = true;
            this.checkBoxCecLogError.Checked = true;
            this.checkBoxCecLogError.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxCecLogError.Location = new System.Drawing.Point(6, 24);
            this.checkBoxCecLogError.Name = "checkBoxCecLogError";
            this.checkBoxCecLogError.Size = new System.Drawing.Size(48, 17);
            this.checkBoxCecLogError.TabIndex = 26;
            this.checkBoxCecLogError.Text = "Error";
            this.checkBoxCecLogError.UseVisualStyleBackColor = true;
            this.checkBoxCecLogError.CheckedChanged += new System.EventHandler(this.checkBoxCecLogs_CheckedChanged);
            // 
            // checkBoxCecLogWarning
            // 
            this.checkBoxCecLogWarning.AutoSize = true;
            this.checkBoxCecLogWarning.Checked = true;
            this.checkBoxCecLogWarning.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxCecLogWarning.Location = new System.Drawing.Point(6, 47);
            this.checkBoxCecLogWarning.Name = "checkBoxCecLogWarning";
            this.checkBoxCecLogWarning.Size = new System.Drawing.Size(66, 17);
            this.checkBoxCecLogWarning.TabIndex = 25;
            this.checkBoxCecLogWarning.Text = "Warning";
            this.checkBoxCecLogWarning.UseVisualStyleBackColor = true;
            this.checkBoxCecLogWarning.CheckedChanged += new System.EventHandler(this.checkBoxCecLogs_CheckedChanged);
            // 
            // checkBoxCecEnabled
            // 
            this.checkBoxCecEnabled.AutoSize = true;
            this.checkBoxCecEnabled.Location = new System.Drawing.Point(6, 6);
            this.checkBoxCecEnabled.Name = "checkBoxCecEnabled";
            this.checkBoxCecEnabled.Size = new System.Drawing.Size(83, 17);
            this.checkBoxCecEnabled.TabIndex = 21;
            this.checkBoxCecEnabled.Text = "Enable CEC";
            this.checkBoxCecEnabled.UseVisualStyleBackColor = true;
            this.checkBoxCecEnabled.CheckedChanged += new System.EventHandler(this.checkBoxCecEnabled_CheckedChanged);
            // 
            // labelHdmiPort
            // 
            this.labelHdmiPort.AutoSize = true;
            this.labelHdmiPort.Location = new System.Drawing.Point(3, 26);
            this.labelHdmiPort.Name = "labelHdmiPort";
            this.labelHdmiPort.Size = new System.Drawing.Size(182, 13);
            this.labelHdmiPort.TabIndex = 20;
            this.labelHdmiPort.Text = "TV HDMI port connected to your PC:";
            // 
            // comboBoxHdmiPort
            // 
            this.comboBoxHdmiPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxHdmiPort.FormattingEnabled = true;
            this.comboBoxHdmiPort.Items.AddRange(new object[] {
            "HDMI 1",
            "HDMI 2",
            "HDMI 3",
            "HDMI 4",
            "HDMI 5",
            "HDMI 6",
            "HDMI 7",
            "HDMI 8",
            "HDMI 9"});
            this.comboBoxHdmiPort.Location = new System.Drawing.Point(6, 42);
            this.comboBoxHdmiPort.Name = "comboBoxHdmiPort";
            this.comboBoxHdmiPort.Size = new System.Drawing.Size(87, 21);
            this.comboBoxHdmiPort.TabIndex = 19;
            this.comboBoxHdmiPort.SelectedIndexChanged += new System.EventHandler(this.comboBoxHdmiPort_SelectedIndexChanged);
            // 
            // tabPageHarmony
            // 
            this.tabPageHarmony.Controls.Add(this.iCheckBoxHarmonyEnabled);
            this.tabPageHarmony.Controls.Add(this.iTreeViewHarmony);
            this.tabPageHarmony.Controls.Add(this.iButtonHarmonyConnect);
            this.tabPageHarmony.Controls.Add(this.label2);
            this.tabPageHarmony.Controls.Add(this.iTextBoxLogitechPassword);
            this.tabPageHarmony.Controls.Add(this.labelLogitechUserName);
            this.tabPageHarmony.Controls.Add(this.iTextBoxLogitechUserName);
            this.tabPageHarmony.Controls.Add(this.label1);
            this.tabPageHarmony.Controls.Add(this.iTextBoxHarmonyHubAddress);
            this.tabPageHarmony.Location = new System.Drawing.Point(4, 22);
            this.tabPageHarmony.Name = "tabPageHarmony";
            this.tabPageHarmony.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageHarmony.Size = new System.Drawing.Size(752, 385);
            this.tabPageHarmony.TabIndex = 10;
            this.tabPageHarmony.Text = "Harmony";
            this.tabPageHarmony.UseVisualStyleBackColor = true;
            // 
            // iCheckBoxHarmonyEnabled
            // 
            this.iCheckBoxHarmonyEnabled.AutoSize = true;
            this.iCheckBoxHarmonyEnabled.Location = new System.Drawing.Point(6, 6);
            this.iCheckBoxHarmonyEnabled.Name = "iCheckBoxHarmonyEnabled";
            this.iCheckBoxHarmonyEnabled.Size = new System.Drawing.Size(104, 17);
            this.iCheckBoxHarmonyEnabled.TabIndex = 22;
            this.iCheckBoxHarmonyEnabled.Text = "Enable Harmony";
            this.iCheckBoxHarmonyEnabled.UseVisualStyleBackColor = true;
            this.iCheckBoxHarmonyEnabled.CheckedChanged += new System.EventHandler(this.iCheckBoxHarmonyEnabled_CheckedChanged);
            // 
            // iTreeViewHarmony
            // 
            this.iTreeViewHarmony.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.iTreeViewHarmony.Location = new System.Drawing.Point(84, 49);
            this.iTreeViewHarmony.Name = "iTreeViewHarmony";
            this.iTreeViewHarmony.Size = new System.Drawing.Size(662, 330);
            this.iTreeViewHarmony.TabIndex = 15;
            this.iTreeViewHarmony.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.iTreeViewHarmony_NodeMouseDoubleClick);
            // 
            // iButtonHarmonyConnect
            // 
            this.iButtonHarmonyConnect.Location = new System.Drawing.Point(3, 49);
            this.iButtonHarmonyConnect.Name = "iButtonHarmonyConnect";
            this.iButtonHarmonyConnect.Size = new System.Drawing.Size(75, 23);
            this.iButtonHarmonyConnect.TabIndex = 14;
            this.iButtonHarmonyConnect.Text = "Connect";
            this.iButtonHarmonyConnect.UseVisualStyleBackColor = true;
            this.iButtonHarmonyConnect.Click += new System.EventHandler(this.iButtonHarmonyConnect_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(597, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Logitech password:";
            // 
            // iTextBoxLogitechPassword
            // 
            this.iTextBoxLogitechPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.iTextBoxLogitechPassword.Location = new System.Drawing.Point(600, 23);
            this.iTextBoxLogitechPassword.Name = "iTextBoxLogitechPassword";
            this.iTextBoxLogitechPassword.PasswordChar = '*';
            this.iTextBoxLogitechPassword.Size = new System.Drawing.Size(134, 20);
            this.iTextBoxLogitechPassword.TabIndex = 12;
            // 
            // labelLogitechUserName
            // 
            this.labelLogitechUserName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelLogitechUserName.AutoSize = true;
            this.labelLogitechUserName.Location = new System.Drawing.Point(433, 7);
            this.labelLogitechUserName.Name = "labelLogitechUserName";
            this.labelLogitechUserName.Size = new System.Drawing.Size(141, 13);
            this.labelLogitechUserName.TabIndex = 11;
            this.labelLogitechUserName.Text = "Logitech user name / e-mail:";
            // 
            // iTextBoxLogitechUserName
            // 
            this.iTextBoxLogitechUserName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.iTextBoxLogitechUserName.Location = new System.Drawing.Point(436, 23);
            this.iTextBoxLogitechUserName.Name = "iTextBoxLogitechUserName";
            this.iTextBoxLogitechUserName.Size = new System.Drawing.Size(134, 20);
            this.iTextBoxLogitechUserName.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(279, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Harmony Hub Address:";
            // 
            // iTextBoxHarmonyHubAddress
            // 
            this.iTextBoxHarmonyHubAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.iTextBoxHarmonyHubAddress.Location = new System.Drawing.Point(282, 23);
            this.iTextBoxHarmonyHubAddress.Name = "iTextBoxHarmonyHubAddress";
            this.iTextBoxHarmonyHubAddress.Size = new System.Drawing.Size(100, 20);
            this.iTextBoxHarmonyHubAddress.TabIndex = 8;
            this.iTextBoxHarmonyHubAddress.Text = "HarmonyHub";
            // 
            // tabPageEvent
            // 
            this.tabPageEvent.Controls.Add(this.buttonEventEdit);
            this.tabPageEvent.Controls.Add(this.buttonEventDelete);
            this.tabPageEvent.Controls.Add(this.buttonEventAdd);
            this.tabPageEvent.Controls.Add(this.buttonEventTest);
            this.tabPageEvent.Controls.Add(this.buttonActionEdit);
            this.tabPageEvent.Controls.Add(this.buttonActionMoveUp);
            this.tabPageEvent.Controls.Add(this.buttonActionMoveDown);
            this.tabPageEvent.Controls.Add(this.buttonActionTest);
            this.tabPageEvent.Controls.Add(this.buttonActionDelete);
            this.tabPageEvent.Controls.Add(this.buttonActionAdd);
            this.tabPageEvent.Controls.Add(this.iTreeViewEvents);
            this.tabPageEvent.Location = new System.Drawing.Point(4, 22);
            this.tabPageEvent.Name = "tabPageEvent";
            this.tabPageEvent.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageEvent.Size = new System.Drawing.Size(752, 385);
            this.tabPageEvent.TabIndex = 9;
            this.tabPageEvent.Text = "Events";
            this.tabPageEvent.UseVisualStyleBackColor = true;
            // 
            // buttonEventEdit
            // 
            this.buttonEventEdit.Enabled = false;
            this.buttonEventEdit.Location = new System.Drawing.Point(6, 35);
            this.buttonEventEdit.Name = "buttonEventEdit";
            this.buttonEventEdit.Size = new System.Drawing.Size(96, 23);
            this.buttonEventEdit.TabIndex = 29;
            this.buttonEventEdit.Text = "Edit Event";
            this.buttonEventEdit.UseVisualStyleBackColor = true;
            this.buttonEventEdit.Click += new System.EventHandler(this.buttonEventEdit_Click);
            // 
            // buttonEventDelete
            // 
            this.buttonEventDelete.Enabled = false;
            this.buttonEventDelete.Location = new System.Drawing.Point(6, 64);
            this.buttonEventDelete.Name = "buttonEventDelete";
            this.buttonEventDelete.Size = new System.Drawing.Size(96, 23);
            this.buttonEventDelete.TabIndex = 28;
            this.buttonEventDelete.Text = "Delete Event";
            this.buttonEventDelete.UseVisualStyleBackColor = true;
            this.buttonEventDelete.Click += new System.EventHandler(this.buttonEventDelete_Click);
            // 
            // buttonEventAdd
            // 
            this.buttonEventAdd.Location = new System.Drawing.Point(6, 6);
            this.buttonEventAdd.Name = "buttonEventAdd";
            this.buttonEventAdd.Size = new System.Drawing.Size(96, 23);
            this.buttonEventAdd.TabIndex = 27;
            this.buttonEventAdd.Text = "Add Event";
            this.buttonEventAdd.UseVisualStyleBackColor = true;
            this.buttonEventAdd.Click += new System.EventHandler(this.buttonEventAdd_Click);
            // 
            // buttonEventTest
            // 
            this.buttonEventTest.Enabled = false;
            this.buttonEventTest.Location = new System.Drawing.Point(6, 93);
            this.buttonEventTest.Name = "buttonEventTest";
            this.buttonEventTest.Size = new System.Drawing.Size(96, 23);
            this.buttonEventTest.TabIndex = 26;
            this.buttonEventTest.Text = "Test Event";
            this.buttonEventTest.UseVisualStyleBackColor = true;
            this.buttonEventTest.Click += new System.EventHandler(this.buttonEventTest_Click);
            // 
            // buttonActionEdit
            // 
            this.buttonActionEdit.Enabled = false;
            this.buttonActionEdit.Location = new System.Drawing.Point(6, 190);
            this.buttonActionEdit.Name = "buttonActionEdit";
            this.buttonActionEdit.Size = new System.Drawing.Size(96, 23);
            this.buttonActionEdit.TabIndex = 25;
            this.buttonActionEdit.Text = "Edit Action";
            this.buttonActionEdit.UseVisualStyleBackColor = true;
            this.buttonActionEdit.Click += new System.EventHandler(this.buttonActionEdit_Click);
            // 
            // buttonActionMoveUp
            // 
            this.buttonActionMoveUp.Enabled = false;
            this.buttonActionMoveUp.Location = new System.Drawing.Point(6, 327);
            this.buttonActionMoveUp.Name = "buttonActionMoveUp";
            this.buttonActionMoveUp.Size = new System.Drawing.Size(96, 23);
            this.buttonActionMoveUp.TabIndex = 24;
            this.buttonActionMoveUp.Text = "Move Up";
            this.buttonActionMoveUp.UseVisualStyleBackColor = true;
            this.buttonActionMoveUp.Click += new System.EventHandler(this.buttonActionMoveUp_Click);
            // 
            // buttonActionMoveDown
            // 
            this.buttonActionMoveDown.Enabled = false;
            this.buttonActionMoveDown.Location = new System.Drawing.Point(6, 356);
            this.buttonActionMoveDown.Name = "buttonActionMoveDown";
            this.buttonActionMoveDown.Size = new System.Drawing.Size(96, 23);
            this.buttonActionMoveDown.TabIndex = 23;
            this.buttonActionMoveDown.Text = "Move Down";
            this.buttonActionMoveDown.UseVisualStyleBackColor = true;
            this.buttonActionMoveDown.Click += new System.EventHandler(this.buttonActionMoveDown_Click);
            // 
            // buttonActionTest
            // 
            this.buttonActionTest.Enabled = false;
            this.buttonActionTest.Location = new System.Drawing.Point(6, 248);
            this.buttonActionTest.Name = "buttonActionTest";
            this.buttonActionTest.Size = new System.Drawing.Size(96, 23);
            this.buttonActionTest.TabIndex = 22;
            this.buttonActionTest.Text = "Test Action";
            this.buttonActionTest.UseVisualStyleBackColor = true;
            this.buttonActionTest.Click += new System.EventHandler(this.buttonActionTest_Click);
            // 
            // buttonActionDelete
            // 
            this.buttonActionDelete.Enabled = false;
            this.buttonActionDelete.Location = new System.Drawing.Point(6, 219);
            this.buttonActionDelete.Name = "buttonActionDelete";
            this.buttonActionDelete.Size = new System.Drawing.Size(96, 23);
            this.buttonActionDelete.TabIndex = 21;
            this.buttonActionDelete.Text = "Delete Action";
            this.buttonActionDelete.UseVisualStyleBackColor = true;
            this.buttonActionDelete.Click += new System.EventHandler(this.buttonActionDelete_Click);
            // 
            // buttonActionAdd
            // 
            this.buttonActionAdd.Enabled = false;
            this.buttonActionAdd.Location = new System.Drawing.Point(6, 157);
            this.buttonActionAdd.Name = "buttonActionAdd";
            this.buttonActionAdd.Size = new System.Drawing.Size(96, 27);
            this.buttonActionAdd.TabIndex = 20;
            this.buttonActionAdd.Text = "Add Action";
            this.buttonActionAdd.UseVisualStyleBackColor = true;
            this.buttonActionAdd.Click += new System.EventHandler(this.buttonActionAdd_Click);
            // 
            // iTreeViewEvents
            // 
            this.iTreeViewEvents.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.iTreeViewEvents.HideSelection = false;
            this.iTreeViewEvents.Location = new System.Drawing.Point(111, 3);
            this.iTreeViewEvents.Name = "iTreeViewEvents";
            this.iTreeViewEvents.Size = new System.Drawing.Size(638, 376);
            this.iTreeViewEvents.TabIndex = 1;
            this.iTreeViewEvents.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.iTreeViewEvents_AfterSelect);
            this.iTreeViewEvents.Leave += new System.EventHandler(this.iTreeViewEvents_Leave);
            // 
            // tabPageApp
            // 
            this.tabPageApp.Controls.Add(this.checkBoxStartMinimized);
            this.tabPageApp.Controls.Add(this.checkBoxMinimizeToTray);
            this.tabPageApp.Controls.Add(this.checkBoxAutoStart);
            this.tabPageApp.Controls.Add(this.buttonUpdate);
            this.tabPageApp.Location = new System.Drawing.Point(4, 22);
            this.tabPageApp.Name = "tabPageApp";
            this.tabPageApp.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageApp.Size = new System.Drawing.Size(752, 385);
            this.tabPageApp.TabIndex = 4;
            this.tabPageApp.Text = "Application";
            this.tabPageApp.UseVisualStyleBackColor = true;
            // 
            // checkBoxStartMinimized
            // 
            this.checkBoxStartMinimized.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxStartMinimized.AutoSize = true;
            this.checkBoxStartMinimized.Location = new System.Drawing.Point(6, 285);
            this.checkBoxStartMinimized.Name = "checkBoxStartMinimized";
            this.checkBoxStartMinimized.Size = new System.Drawing.Size(96, 17);
            this.checkBoxStartMinimized.TabIndex = 16;
            this.checkBoxStartMinimized.Text = "Start minimized";
            this.checkBoxStartMinimized.UseVisualStyleBackColor = true;
            this.checkBoxStartMinimized.CheckedChanged += new System.EventHandler(this.checkBoxStartMinimized_CheckedChanged);
            // 
            // checkBoxMinimizeToTray
            // 
            this.checkBoxMinimizeToTray.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxMinimizeToTray.AutoSize = true;
            this.checkBoxMinimizeToTray.Location = new System.Drawing.Point(6, 308);
            this.checkBoxMinimizeToTray.Name = "checkBoxMinimizeToTray";
            this.checkBoxMinimizeToTray.Size = new System.Drawing.Size(133, 17);
            this.checkBoxMinimizeToTray.TabIndex = 15;
            this.checkBoxMinimizeToTray.Text = "Minimize to system tray";
            this.checkBoxMinimizeToTray.UseVisualStyleBackColor = true;
            this.checkBoxMinimizeToTray.CheckedChanged += new System.EventHandler(this.checkBoxMinimizeToTray_CheckedChanged);
            // 
            // checkBoxAutoStart
            // 
            this.checkBoxAutoStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxAutoStart.AutoSize = true;
            this.checkBoxAutoStart.Location = new System.Drawing.Point(6, 331);
            this.checkBoxAutoStart.Name = "checkBoxAutoStart";
            this.checkBoxAutoStart.Size = new System.Drawing.Size(143, 17);
            this.checkBoxAutoStart.TabIndex = 14;
            this.checkBoxAutoStart.Text = "Run on Windows startup";
            this.checkBoxAutoStart.UseVisualStyleBackColor = true;
            this.checkBoxAutoStart.CheckedChanged += new System.EventHandler(this.checkBoxAutoStart_CheckedChanged);
            // 
            // buttonUpdate
            // 
            this.buttonUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonUpdate.Location = new System.Drawing.Point(6, 354);
            this.buttonUpdate.Name = "buttonUpdate";
            this.buttonUpdate.Size = new System.Drawing.Size(75, 23);
            this.buttonUpdate.TabIndex = 0;
            this.buttonUpdate.Text = "Update";
            this.buttonUpdate.UseVisualStyleBackColor = true;
            this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click);
            // 
            // tabPageLogs
            // 
            this.tabPageLogs.Controls.Add(this.buttonClearLogs);
            this.tabPageLogs.Controls.Add(this.richTextBoxLogs);
            this.tabPageLogs.Location = new System.Drawing.Point(4, 22);
            this.tabPageLogs.Name = "tabPageLogs";
            this.tabPageLogs.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageLogs.Size = new System.Drawing.Size(752, 385);
            this.tabPageLogs.TabIndex = 8;
            this.tabPageLogs.Text = "Logs";
            this.tabPageLogs.UseVisualStyleBackColor = true;
            // 
            // buttonClearLogs
            // 
            this.buttonClearLogs.Location = new System.Drawing.Point(671, 6);
            this.buttonClearLogs.Name = "buttonClearLogs";
            this.buttonClearLogs.Size = new System.Drawing.Size(75, 23);
            this.buttonClearLogs.TabIndex = 2;
            this.buttonClearLogs.Text = "Clear";
            this.buttonClearLogs.UseVisualStyleBackColor = true;
            this.buttonClearLogs.Click += new System.EventHandler(this.buttonClearLogs_Click);
            // 
            // richTextBoxLogs
            // 
            this.richTextBoxLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxLogs.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBoxLogs.DetectUrls = false;
            this.richTextBoxLogs.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxLogs.Location = new System.Drawing.Point(6, 6);
            this.richTextBoxLogs.Name = "richTextBoxLogs";
            this.richTextBoxLogs.ReadOnly = true;
            this.richTextBoxLogs.Size = new System.Drawing.Size(740, 373);
            this.richTextBoxLogs.TabIndex = 1;
            this.richTextBoxLogs.Text = "";
            this.richTextBoxLogs.WordWrap = false;
            // 
            // labelFontWidth
            // 
            this.labelFontWidth.AutoSize = true;
            this.labelFontWidth.Location = new System.Drawing.Point(13, 29);
            this.labelFontWidth.Name = "labelFontWidth";
            this.labelFontWidth.Size = new System.Drawing.Size(56, 13);
            this.labelFontWidth.TabIndex = 19;
            this.labelFontWidth.Text = "Font width";
            // 
            // labelFontHeight
            // 
            this.labelFontHeight.AutoSize = true;
            this.labelFontHeight.Location = new System.Drawing.Point(13, 46);
            this.labelFontHeight.Name = "labelFontHeight";
            this.labelFontHeight.Size = new System.Drawing.Size(60, 13);
            this.labelFontHeight.TabIndex = 20;
            this.labelFontHeight.Text = "Font height";
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "EXE files (*.exe)|*.exe|All files (*.*)|*.*";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.labelFontHeight);
            this.Controls.Add(this.labelFontWidth);
            this.Controls.Add(this.labelWarning);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.panelDisplay);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "FormMain";
            this.Text = "Sharp Display Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.panelDisplay.ResumeLayout(false);
            this.iTableLayoutPanel.ResumeLayout(false);
            this.iTableLayoutPanel.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.tabPageClients.ResumeLayout(false);
            this.tabPageClients.PerformLayout();
            this.tabPageDisplay.ResumeLayout(false);
            this.tabPageDisplay.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBrightness)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.tabPageDesign.ResumeLayout(false);
            this.tabPageDesign.PerformLayout();
            this.tabPageAudio.ResumeLayout(false);
            this.tabPageAudio.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMasterVolume)).EndInit();
            this.tabPageInput.ResumeLayout(false);
            this.tabPageInput.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxGreenStart)).EndInit();
            this.tabPageCec.ResumeLayout(false);
            this.tabPageCec.PerformLayout();
            this.groupBoxCecLogOptions.ResumeLayout(false);
            this.groupBoxCecLogOptions.PerformLayout();
            this.tabPageHarmony.ResumeLayout(false);
            this.tabPageHarmony.PerformLayout();
            this.tabPageEvent.ResumeLayout(false);
            this.tabPageApp.ResumeLayout(false);
            this.tabPageApp.PerformLayout();
            this.tabPageLogs.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FontDialog fontDialog;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelConnect;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelFps;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelSpring;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelPower;
        private System.Windows.Forms.Panel panelDisplay;
        private System.Windows.Forms.TableLayoutPanel iTableLayoutPanel;
        private MarqueeLabel marqueeLabelTop;
        private MarqueeLabel marqueeLabelBottom;
        private System.Windows.Forms.TabPage tabPageClients;
        private System.Windows.Forms.TreeView iTreeViewClients;
        private System.Windows.Forms.TabPage tabPageDisplay;
        private System.Windows.Forms.CheckBox checkBoxFixedPitchFontOnly;
        private System.Windows.Forms.Button buttonSuspend;
        private System.Windows.Forms.CheckBox checkBoxConnectOnStartup;
        private System.Windows.Forms.CheckBox checkBoxShowBorders;
        private System.Windows.Forms.TrackBar trackBarBrightness;
        private System.Windows.Forms.Button buttonFill;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonOpen;
        private System.Windows.Forms.Button buttonCapture;
        private System.Windows.Forms.Button buttonFont;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.Button buttonCloseClients;
        private System.Windows.Forms.Button buttonStartClient;
        private System.Windows.Forms.Label labelWarning;
		private System.Windows.Forms.TabPage tabPageDesign;
		private System.Windows.Forms.CheckBox checkBoxReverseScreen;
        private System.Windows.Forms.ComboBox comboBoxDisplayType;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxTimerInterval;
        private System.Windows.Forms.Label labelTimerInterval;
        private System.Windows.Forms.Button buttonPowerOff;
        private System.Windows.Forms.Button buttonPowerOn;
        private System.Windows.Forms.Button buttonShowClock;
        private System.Windows.Forms.Button buttonHideClock;
        private System.Windows.Forms.Label labelFontWidth;
        private System.Windows.Forms.Label labelFontHeight;
        private System.Windows.Forms.CheckBox checkBoxInverseColors;
        private System.Windows.Forms.TabPage tabPageApp;
        private System.Windows.Forms.Button buttonUpdate;
		private System.Windows.Forms.CheckBox checkBoxAutoStart;
		private System.Windows.Forms.CheckBox checkBoxStartMinimized;
		private System.Windows.Forms.CheckBox checkBoxMinimizeToTray;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxMinFontSize;
        private System.Windows.Forms.CheckBox checkBoxScaleToFit;
        private System.Windows.Forms.Label labelMinFontSize;
        private System.Windows.Forms.Label labelScrollLoopSeparator;
        private System.Windows.Forms.TextBox textBoxScrollLoopSeparator;
		private System.Windows.Forms.Label labelScrollingSpeed;
		private System.Windows.Forms.MaskedTextBox maskedTextBoxScrollingSpeed;
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.TabPage tabPageAudio;
		private System.Windows.Forms.TrackBar trackBarMasterVolume;
		private System.Windows.Forms.CheckBox checkBoxMute;
		private System.Windows.Forms.CheckBox checkBoxShowVolumeLabel;
		private System.Windows.Forms.Label labelDefaultAudioDevice;
		private System.Windows.Forms.TabPage tabPageInput;
		private System.Windows.Forms.Button buttonSelectFile;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.Label labelStartFileName;
		private System.Windows.Forms.PictureBox pictureBoxGreenStart;
        private System.Windows.Forms.ComboBox comboBoxOpticalDrives;
        private System.Windows.Forms.Label labelOpticalDriveEject;
        private System.Windows.Forms.TabPage tabPageCec;
        private System.Windows.Forms.CheckBox checkBoxCecEnabled;
        private System.Windows.Forms.Label labelHdmiPort;
        private System.Windows.Forms.ComboBox comboBoxHdmiPort;
        private System.Windows.Forms.Button iButtonStartIdleClient;
        private System.Windows.Forms.CheckBox iCheckBoxStartIdleClient;
        private System.Windows.Forms.TabPage tabPageLogs;
        private System.Windows.Forms.RichTextBox richTextBoxLogs;
        private System.Windows.Forms.Button buttonClearLogs;
        private System.Windows.Forms.GroupBox groupBoxCecLogOptions;
        private System.Windows.Forms.CheckBox checkBoxCecLogWarning;
        private System.Windows.Forms.CheckBox checkBoxCecLogError;
        private System.Windows.Forms.CheckBox checkBoxCecLogDebug;
        private System.Windows.Forms.CheckBox checkBoxCecLogNotice;
        private System.Windows.Forms.CheckBox checkBoxCecLogTraffic;
        private System.Windows.Forms.CheckBox checkBoxCecLogNoPoll;
        private System.Windows.Forms.TabPage tabPageEvent;
        private System.Windows.Forms.TreeView iTreeViewEvents;
        private System.Windows.Forms.Button buttonActionDelete;
        private System.Windows.Forms.Button buttonActionAdd;
        private System.Windows.Forms.Button buttonActionMoveUp;
        private System.Windows.Forms.Button buttonActionMoveDown;
        private System.Windows.Forms.Button buttonActionTest;
        private System.Windows.Forms.Button buttonActionEdit;
        private System.Windows.Forms.Button buttonEventTest;
        private System.Windows.Forms.Button buttonEventDelete;
        private System.Windows.Forms.Button buttonEventAdd;
        private System.Windows.Forms.Button buttonEventEdit;
        private System.Windows.Forms.TabPage tabPageHarmony;
        private System.Windows.Forms.TreeView iTreeViewHarmony;
        private System.Windows.Forms.Button iButtonHarmonyConnect;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox iTextBoxLogitechPassword;
        private System.Windows.Forms.Label labelLogitechUserName;
        private System.Windows.Forms.TextBox iTextBoxLogitechUserName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox iTextBoxHarmonyHubAddress;
        private System.Windows.Forms.CheckBox iCheckBoxHarmonyEnabled;
    }
}

