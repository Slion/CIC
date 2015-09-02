namespace SharpDisplayManager
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.panelDisplay = new System.Windows.Forms.Panel();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
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
            this.buttonCloseClients = new System.Windows.Forms.Button();
            this.buttonStartClient = new System.Windows.Forms.Button();
            this.treeViewClients = new System.Windows.Forms.TreeView();
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
            this.pictureBoxGreenStart = new System.Windows.Forms.PictureBox();
            this.labelStartFileName = new System.Windows.Forms.Label();
            this.buttonSelectFile = new System.Windows.Forms.Button();
            this.tabPageApp = new System.Windows.Forms.TabPage();
            this.checkBoxStartMinimized = new System.Windows.Forms.CheckBox();
            this.checkBoxMinimizeToTray = new System.Windows.Forms.CheckBox();
            this.checkBoxAutoStart = new System.Windows.Forms.CheckBox();
            this.buttonUpdate = new System.Windows.Forms.Button();
            this.labelFontWidth = new System.Windows.Forms.Label();
            this.labelFontHeight = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.pictureBoxDemo = new System.Windows.Forms.PictureBox();
            this.labelOpticalDriveEject = new System.Windows.Forms.Label();
            this.comboBoxOpticalDrives = new System.Windows.Forms.ComboBox();
            this.panelDisplay.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
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
            this.tabPageApp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDemo)).BeginInit();
            this.SuspendLayout();
            // 
            // panelDisplay
            // 
            this.panelDisplay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelDisplay.Controls.Add(this.tableLayoutPanel);
            this.panelDisplay.Location = new System.Drawing.Point(173, 40);
            this.panelDisplay.Margin = new System.Windows.Forms.Padding(0);
            this.panelDisplay.Name = "panelDisplay";
            this.panelDisplay.Size = new System.Drawing.Size(258, 66);
            this.panelDisplay.TabIndex = 12;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.tableLayoutPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.marqueeLabelTop, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.marqueeLabelBottom, 0, 1);
            this.tableLayoutPanel.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(256, 64);
            this.tableLayoutPanel.TabIndex = 5;
            this.tableLayoutPanel.SizeChanged += new System.EventHandler(this.tableLayoutPanel_SizeChanged);
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
            this.timer.Enabled = true;
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
            this.statusStrip.Location = new System.Drawing.Point(0, 420);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.statusStrip.Size = new System.Drawing.Size(624, 22);
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
            this.toolStripStatusLabelSpring.Size = new System.Drawing.Size(473, 17);
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
            this.tabPageClients.Controls.Add(this.buttonCloseClients);
            this.tabPageClients.Controls.Add(this.buttonStartClient);
            this.tabPageClients.Controls.Add(this.treeViewClients);
            this.tabPageClients.Location = new System.Drawing.Point(4, 22);
            this.tabPageClients.Name = "tabPageClients";
            this.tabPageClients.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageClients.Size = new System.Drawing.Size(592, 242);
            this.tabPageClients.TabIndex = 2;
            this.tabPageClients.Text = "Clients";
            this.tabPageClients.UseVisualStyleBackColor = true;
            // 
            // buttonCloseClients
            // 
            this.buttonCloseClients.Location = new System.Drawing.Point(6, 35);
            this.buttonCloseClients.Name = "buttonCloseClients";
            this.buttonCloseClients.Size = new System.Drawing.Size(75, 23);
            this.buttonCloseClients.TabIndex = 20;
            this.buttonCloseClients.Text = "Close Clients";
            this.buttonCloseClients.UseVisualStyleBackColor = true;
            this.buttonCloseClients.Click += new System.EventHandler(this.buttonCloseClients_Click);
            // 
            // buttonStartClient
            // 
            this.buttonStartClient.Location = new System.Drawing.Point(6, 6);
            this.buttonStartClient.Name = "buttonStartClient";
            this.buttonStartClient.Size = new System.Drawing.Size(75, 23);
            this.buttonStartClient.TabIndex = 19;
            this.buttonStartClient.Text = "Start Client";
            this.buttonStartClient.UseVisualStyleBackColor = true;
            this.buttonStartClient.Click += new System.EventHandler(this.buttonStartClient_Click);
            // 
            // treeViewClients
            // 
            this.treeViewClients.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewClients.Location = new System.Drawing.Point(87, 6);
            this.treeViewClients.Name = "treeViewClients";
            this.treeViewClients.Size = new System.Drawing.Size(499, 233);
            this.treeViewClients.TabIndex = 0;
            this.treeViewClients.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewClients_AfterSelect);
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
            this.tabPageDisplay.Size = new System.Drawing.Size(592, 242);
            this.tabPageDisplay.TabIndex = 0;
            this.tabPageDisplay.Text = "Display";
            this.tabPageDisplay.UseVisualStyleBackColor = true;
            // 
            // buttonShowClock
            // 
            this.buttonShowClock.Location = new System.Drawing.Point(293, 124);
            this.buttonShowClock.Name = "buttonShowClock";
            this.buttonShowClock.Size = new System.Drawing.Size(75, 23);
            this.buttonShowClock.TabIndex = 23;
            this.buttonShowClock.Text = "Show Clock";
            this.buttonShowClock.UseVisualStyleBackColor = true;
            this.buttonShowClock.Click += new System.EventHandler(this.buttonShowClock_Click);
            // 
            // buttonHideClock
            // 
            this.buttonHideClock.Location = new System.Drawing.Point(293, 153);
            this.buttonHideClock.Name = "buttonHideClock";
            this.buttonHideClock.Size = new System.Drawing.Size(75, 23);
            this.buttonHideClock.TabIndex = 22;
            this.buttonHideClock.Text = "Hide Clock";
            this.buttonHideClock.UseVisualStyleBackColor = true;
            this.buttonHideClock.Click += new System.EventHandler(this.buttonHideClock_Click);
            // 
            // buttonPowerOff
            // 
            this.buttonPowerOff.Location = new System.Drawing.Point(293, 211);
            this.buttonPowerOff.Name = "buttonPowerOff";
            this.buttonPowerOff.Size = new System.Drawing.Size(75, 23);
            this.buttonPowerOff.TabIndex = 21;
            this.buttonPowerOff.Text = "OFF";
            this.buttonPowerOff.UseVisualStyleBackColor = true;
            this.buttonPowerOff.Click += new System.EventHandler(this.buttonPowerOff_Click);
            // 
            // buttonPowerOn
            // 
            this.buttonPowerOn.Location = new System.Drawing.Point(293, 182);
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
            this.buttonSuspend.Location = new System.Drawing.Point(6, 184);
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
            this.checkBoxConnectOnStartup.Location = new System.Drawing.Point(113, 217);
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
            this.trackBarBrightness.Location = new System.Drawing.Point(544, 9);
            this.trackBarBrightness.Name = "trackBarBrightness";
            this.trackBarBrightness.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBarBrightness.Size = new System.Drawing.Size(45, 225);
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
            this.buttonCapture.Location = new System.Drawing.Point(6, 213);
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
            this.checkBoxFixedPitchFontOnly.Location = new System.Drawing.Point(87, 217);
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
            this.checkBoxShowBorders.Location = new System.Drawing.Point(485, 29);
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
            this.buttonFont.Location = new System.Drawing.Point(6, 213);
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
            this.tabControl.Controls.Add(this.tabPageApp);
            this.tabControl.Location = new System.Drawing.Point(12, 139);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(600, 268);
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
            this.tabPageDesign.Size = new System.Drawing.Size(592, 242);
            this.tabPageDesign.TabIndex = 3;
            this.tabPageDesign.Text = "Design";
            this.tabPageDesign.UseVisualStyleBackColor = true;
            // 
            // labelScrollingSpeed
            // 
            this.labelScrollingSpeed.AutoSize = true;
            this.labelScrollingSpeed.Location = new System.Drawing.Point(84, 119);
            this.labelScrollingSpeed.Name = "labelScrollingSpeed";
            this.labelScrollingSpeed.Size = new System.Drawing.Size(115, 13);
            this.labelScrollingSpeed.TabIndex = 28;
            this.labelScrollingSpeed.Text = "Scrolling speed (px/s) :";
            // 
            // maskedTextBoxScrollingSpeed
            // 
            this.maskedTextBoxScrollingSpeed.Location = new System.Drawing.Point(205, 116);
            this.maskedTextBoxScrollingSpeed.Mask = "000";
            this.maskedTextBoxScrollingSpeed.Name = "maskedTextBoxScrollingSpeed";
            this.maskedTextBoxScrollingSpeed.PromptChar = ' ';
            this.maskedTextBoxScrollingSpeed.Size = new System.Drawing.Size(24, 20);
            this.maskedTextBoxScrollingSpeed.TabIndex = 27;
            this.maskedTextBoxScrollingSpeed.TextChanged += new System.EventHandler(this.maskedTextBoxScrollingSpeed_TextChanged);
            // 
            // labelScrollLoopSeparator
            // 
            this.labelScrollLoopSeparator.AutoSize = true;
            this.labelScrollLoopSeparator.Location = new System.Drawing.Point(90, 145);
            this.labelScrollLoopSeparator.Name = "labelScrollLoopSeparator";
            this.labelScrollLoopSeparator.Size = new System.Drawing.Size(109, 13);
            this.labelScrollLoopSeparator.TabIndex = 26;
            this.labelScrollLoopSeparator.Text = "Scroll loop separator :";
            // 
            // textBoxScrollLoopSeparator
            // 
            this.textBoxScrollLoopSeparator.Location = new System.Drawing.Point(205, 142);
            this.textBoxScrollLoopSeparator.Name = "textBoxScrollLoopSeparator";
            this.textBoxScrollLoopSeparator.Size = new System.Drawing.Size(74, 20);
            this.textBoxScrollLoopSeparator.TabIndex = 25;
            this.textBoxScrollLoopSeparator.TextChanged += new System.EventHandler(this.textBoxScrollLoopSeparator_TextChanged);
            // 
            // labelMinFontSize
            // 
            this.labelMinFontSize.AutoSize = true;
            this.labelMinFontSize.Location = new System.Drawing.Point(80, 194);
            this.labelMinFontSize.Name = "labelMinFontSize";
            this.labelMinFontSize.Size = new System.Drawing.Size(119, 13);
            this.labelMinFontSize.TabIndex = 24;
            this.labelMinFontSize.Text = "Minimum font size (pts) :";
            // 
            // maskedTextBoxMinFontSize
            // 
            this.maskedTextBoxMinFontSize.Location = new System.Drawing.Point(205, 191);
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
            this.checkBoxScaleToFit.Location = new System.Drawing.Point(87, 168);
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
            this.checkBoxInverseColors.Location = new System.Drawing.Point(485, 52);
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
            this.checkBoxReverseScreen.Location = new System.Drawing.Point(485, 6);
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
            this.tabPageAudio.Size = new System.Drawing.Size(592, 242);
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
            this.checkBoxShowVolumeLabel.Location = new System.Drawing.Point(3, 196);
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
            this.checkBoxMute.Location = new System.Drawing.Point(3, 219);
            this.checkBoxMute.Name = "checkBoxMute";
            this.checkBoxMute.Size = new System.Drawing.Size(50, 17);
            this.checkBoxMute.TabIndex = 17;
            this.checkBoxMute.Text = "Mute";
            this.checkBoxMute.UseVisualStyleBackColor = true;
            this.checkBoxMute.CheckedChanged += new System.EventHandler(this.checkBoxMute_CheckedChanged);
            // 
            // trackBarMasterVolume
            // 
            this.trackBarMasterVolume.BackColor = System.Drawing.SystemColors.Window;
            this.trackBarMasterVolume.Location = new System.Drawing.Point(541, 6);
            this.trackBarMasterVolume.Maximum = 100;
            this.trackBarMasterVolume.Name = "trackBarMasterVolume";
            this.trackBarMasterVolume.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBarMasterVolume.Size = new System.Drawing.Size(45, 230);
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
            this.tabPageInput.Size = new System.Drawing.Size(592, 242);
            this.tabPageInput.TabIndex = 6;
            this.tabPageInput.Text = "Input";
            this.tabPageInput.UseVisualStyleBackColor = true;
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
            // tabPageApp
            // 
            this.tabPageApp.Controls.Add(this.checkBoxStartMinimized);
            this.tabPageApp.Controls.Add(this.checkBoxMinimizeToTray);
            this.tabPageApp.Controls.Add(this.checkBoxAutoStart);
            this.tabPageApp.Controls.Add(this.buttonUpdate);
            this.tabPageApp.Location = new System.Drawing.Point(4, 22);
            this.tabPageApp.Name = "tabPageApp";
            this.tabPageApp.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageApp.Size = new System.Drawing.Size(592, 242);
            this.tabPageApp.TabIndex = 4;
            this.tabPageApp.Text = "Application";
            this.tabPageApp.UseVisualStyleBackColor = true;
            // 
            // checkBoxStartMinimized
            // 
            this.checkBoxStartMinimized.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxStartMinimized.AutoSize = true;
            this.checkBoxStartMinimized.Location = new System.Drawing.Point(8, 144);
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
            this.checkBoxMinimizeToTray.Location = new System.Drawing.Point(8, 167);
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
            this.checkBoxAutoStart.Location = new System.Drawing.Point(8, 190);
            this.checkBoxAutoStart.Name = "checkBoxAutoStart";
            this.checkBoxAutoStart.Size = new System.Drawing.Size(143, 17);
            this.checkBoxAutoStart.TabIndex = 14;
            this.checkBoxAutoStart.Text = "Run on Windows startup";
            this.checkBoxAutoStart.UseVisualStyleBackColor = true;
            this.checkBoxAutoStart.CheckedChanged += new System.EventHandler(this.checkBoxAutoStart_CheckedChanged);
            // 
            // buttonUpdate
            // 
            this.buttonUpdate.Location = new System.Drawing.Point(6, 213);
            this.buttonUpdate.Name = "buttonUpdate";
            this.buttonUpdate.Size = new System.Drawing.Size(75, 23);
            this.buttonUpdate.TabIndex = 0;
            this.buttonUpdate.Text = "Update";
            this.buttonUpdate.UseVisualStyleBackColor = true;
            this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click);
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
            // pictureBoxDemo
            // 
            this.pictureBoxDemo.Location = new System.Drawing.Point(478, 54);
            this.pictureBoxDemo.Name = "pictureBoxDemo";
            this.pictureBoxDemo.Size = new System.Drawing.Size(100, 50);
            this.pictureBoxDemo.TabIndex = 21;
            this.pictureBoxDemo.TabStop = false;
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
            // comboBoxOpticalDrives
            // 
            this.comboBoxOpticalDrives.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxOpticalDrives.FormattingEnabled = true;
            this.comboBoxOpticalDrives.Location = new System.Drawing.Point(113, 53);
            this.comboBoxOpticalDrives.Name = "comboBoxOpticalDrives";
            this.comboBoxOpticalDrives.Size = new System.Drawing.Size(44, 21);
            this.comboBoxOpticalDrives.TabIndex = 18;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 442);
            this.Controls.Add(this.pictureBoxDemo);
            this.Controls.Add(this.labelFontHeight);
            this.Controls.Add(this.labelFontWidth);
            this.Controls.Add(this.labelWarning);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.panelDisplay);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(640, 480);
            this.Name = "MainForm";
            this.Text = "Sharp Display Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.panelDisplay.ResumeLayout(false);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.tabPageClients.ResumeLayout(false);
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
            this.tabPageApp.ResumeLayout(false);
            this.tabPageApp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDemo)).EndInit();
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
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private MarqueeLabel marqueeLabelTop;
        private MarqueeLabel marqueeLabelBottom;
        private System.Windows.Forms.TabPage tabPageClients;
        private System.Windows.Forms.TreeView treeViewClients;
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
        private System.Windows.Forms.PictureBox pictureBoxDemo;
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
    }
}

