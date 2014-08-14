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
            if (disposing && (components != null))
            {
                components.Dispose();
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageDisplay = new System.Windows.Forms.TabPage();
            this.buttonSuspend = new System.Windows.Forms.Button();
            this.buttonStartClient = new System.Windows.Forms.Button();
            this.checkBoxReverseScreen = new System.Windows.Forms.CheckBox();
            this.checkBoxConnectOnStartup = new System.Windows.Forms.CheckBox();
            this.panelDisplay = new System.Windows.Forms.Panel();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.marqueeLabelTop = new SharpDisplayManager.MarqueeLabel();
            this.marqueeLabelBottom = new SharpDisplayManager.MarqueeLabel();
            this.checkBoxShowBorders = new System.Windows.Forms.CheckBox();
            this.trackBarBrightness = new System.Windows.Forms.TrackBar();
            this.buttonFill = new System.Windows.Forms.Button();
            this.buttonClear = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonOpen = new System.Windows.Forms.Button();
            this.buttonCapture = new System.Windows.Forms.Button();
            this.buttonFont = new System.Windows.Forms.Button();
            this.tabPageTests = new System.Windows.Forms.TabPage();
            this.fontDialog = new System.Windows.Forms.FontDialog();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelConnect = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelSpring = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelPower = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelFps = new System.Windows.Forms.ToolStripStatusLabel();
            this.checkBoxFixedPitchFontOnly = new System.Windows.Forms.CheckBox();
            this.tabControl.SuspendLayout();
            this.tabPageDisplay.SuspendLayout();
            this.panelDisplay.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBrightness)).BeginInit();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabPageDisplay);
            this.tabControl.Controls.Add(this.tabPageTests);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(600, 395);
            this.tabControl.TabIndex = 0;
            // 
            // tabPageDisplay
            // 
            this.tabPageDisplay.Controls.Add(this.checkBoxFixedPitchFontOnly);
            this.tabPageDisplay.Controls.Add(this.buttonSuspend);
            this.tabPageDisplay.Controls.Add(this.buttonStartClient);
            this.tabPageDisplay.Controls.Add(this.checkBoxReverseScreen);
            this.tabPageDisplay.Controls.Add(this.checkBoxConnectOnStartup);
            this.tabPageDisplay.Controls.Add(this.panelDisplay);
            this.tabPageDisplay.Controls.Add(this.checkBoxShowBorders);
            this.tabPageDisplay.Controls.Add(this.trackBarBrightness);
            this.tabPageDisplay.Controls.Add(this.buttonFill);
            this.tabPageDisplay.Controls.Add(this.buttonClear);
            this.tabPageDisplay.Controls.Add(this.buttonClose);
            this.tabPageDisplay.Controls.Add(this.buttonOpen);
            this.tabPageDisplay.Controls.Add(this.buttonCapture);
            this.tabPageDisplay.Controls.Add(this.buttonFont);
            this.tabPageDisplay.Location = new System.Drawing.Point(4, 22);
            this.tabPageDisplay.Name = "tabPageDisplay";
            this.tabPageDisplay.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDisplay.Size = new System.Drawing.Size(592, 369);
            this.tabPageDisplay.TabIndex = 0;
            this.tabPageDisplay.Text = "Display";
            this.tabPageDisplay.UseVisualStyleBackColor = true;
            // 
            // buttonSuspend
            // 
            this.buttonSuspend.Location = new System.Drawing.Point(7, 253);
            this.buttonSuspend.Name = "buttonSuspend";
            this.buttonSuspend.Size = new System.Drawing.Size(75, 23);
            this.buttonSuspend.TabIndex = 16;
            this.buttonSuspend.Text = "Suspend";
            this.buttonSuspend.UseVisualStyleBackColor = true;
            this.buttonSuspend.Click += new System.EventHandler(this.buttonSuspend_Click);
            // 
            // buttonStartClient
            // 
            this.buttonStartClient.Location = new System.Drawing.Point(6, 282);
            this.buttonStartClient.Name = "buttonStartClient";
            this.buttonStartClient.Size = new System.Drawing.Size(75, 23);
            this.buttonStartClient.TabIndex = 15;
            this.buttonStartClient.Text = "Start Client";
            this.buttonStartClient.UseVisualStyleBackColor = true;
            this.buttonStartClient.Click += new System.EventHandler(this.buttonStartClient_Click);
            // 
            // checkBoxReverseScreen
            // 
            this.checkBoxReverseScreen.AutoSize = true;
            this.checkBoxReverseScreen.Location = new System.Drawing.Point(113, 298);
            this.checkBoxReverseScreen.Name = "checkBoxReverseScreen";
            this.checkBoxReverseScreen.Size = new System.Drawing.Size(101, 17);
            this.checkBoxReverseScreen.TabIndex = 14;
            this.checkBoxReverseScreen.Text = "Reverse screen";
            this.checkBoxReverseScreen.UseVisualStyleBackColor = true;
            this.checkBoxReverseScreen.CheckedChanged += new System.EventHandler(this.checkBoxReverseScreen_CheckedChanged);
            // 
            // checkBoxConnectOnStartup
            // 
            this.checkBoxConnectOnStartup.AutoSize = true;
            this.checkBoxConnectOnStartup.Location = new System.Drawing.Point(113, 321);
            this.checkBoxConnectOnStartup.Name = "checkBoxConnectOnStartup";
            this.checkBoxConnectOnStartup.Size = new System.Drawing.Size(119, 17);
            this.checkBoxConnectOnStartup.TabIndex = 13;
            this.checkBoxConnectOnStartup.Text = "Connect on stratup ";
            this.checkBoxConnectOnStartup.UseVisualStyleBackColor = true;
            this.checkBoxConnectOnStartup.CheckedChanged += new System.EventHandler(this.checkBoxConnectOnStartup_CheckedChanged);
            // 
            // panelDisplay
            // 
            this.panelDisplay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelDisplay.Controls.Add(this.tableLayoutPanel);
            this.panelDisplay.Location = new System.Drawing.Point(181, 151);
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
            this.tableLayoutPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Controls.Add(this.marqueeLabelTop, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.marqueeLabelBottom, 0, 1);
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(256, 64);
            this.tableLayoutPanel.TabIndex = 5;
            // 
            // marqueeLabelTop
            // 
            this.marqueeLabelTop.AutoEllipsis = true;
            this.marqueeLabelTop.BackColor = System.Drawing.Color.Transparent;
            this.marqueeLabelTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.marqueeLabelTop.Location = new System.Drawing.Point(1, 1);
            this.marqueeLabelTop.Margin = new System.Windows.Forms.Padding(0);
            this.marqueeLabelTop.Name = "marqueeLabelTop";
            this.marqueeLabelTop.OwnTimer = false;
            this.marqueeLabelTop.PixelsPerSecond = 64;
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
            this.marqueeLabelBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.marqueeLabelBottom.Location = new System.Drawing.Point(1, 32);
            this.marqueeLabelBottom.Margin = new System.Windows.Forms.Padding(0);
            this.marqueeLabelBottom.Name = "marqueeLabelBottom";
            this.marqueeLabelBottom.OwnTimer = false;
            this.marqueeLabelBottom.PixelsPerSecond = 64;
            this.marqueeLabelBottom.Separator = null;
            this.marqueeLabelBottom.Size = new System.Drawing.Size(254, 31);
            this.marqueeLabelBottom.TabIndex = 3;
            this.marqueeLabelBottom.Text = "abcdefghijklmnopqrst-0123456789";
            this.marqueeLabelBottom.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.marqueeLabelBottom.UseCompatibleTextRendering = true;
            // 
            // checkBoxShowBorders
            // 
            this.checkBoxShowBorders.AutoSize = true;
            this.checkBoxShowBorders.Location = new System.Drawing.Point(113, 344);
            this.checkBoxShowBorders.Name = "checkBoxShowBorders";
            this.checkBoxShowBorders.Size = new System.Drawing.Size(91, 17);
            this.checkBoxShowBorders.TabIndex = 11;
            this.checkBoxShowBorders.Text = "Show borders";
            this.checkBoxShowBorders.UseVisualStyleBackColor = true;
            this.checkBoxShowBorders.CheckedChanged += new System.EventHandler(this.checkBoxShowBorders_CheckedChanged);
            // 
            // trackBarBrightness
            // 
            this.trackBarBrightness.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarBrightness.BackColor = System.Drawing.SystemColors.Window;
            this.trackBarBrightness.Location = new System.Drawing.Point(544, 9);
            this.trackBarBrightness.Name = "trackBarBrightness";
            this.trackBarBrightness.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBarBrightness.Size = new System.Drawing.Size(45, 357);
            this.trackBarBrightness.TabIndex = 10;
            this.trackBarBrightness.TickStyle = System.Windows.Forms.TickStyle.Both;
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
            this.buttonCapture.Location = new System.Drawing.Point(6, 311);
            this.buttonCapture.Name = "buttonCapture";
            this.buttonCapture.Size = new System.Drawing.Size(75, 23);
            this.buttonCapture.TabIndex = 5;
            this.buttonCapture.Text = "Capture";
            this.buttonCapture.UseVisualStyleBackColor = true;
            this.buttonCapture.Click += new System.EventHandler(this.buttonCapture_Click);
            // 
            // buttonFont
            // 
            this.buttonFont.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonFont.Location = new System.Drawing.Point(6, 340);
            this.buttonFont.Name = "buttonFont";
            this.buttonFont.Size = new System.Drawing.Size(75, 23);
            this.buttonFont.TabIndex = 0;
            this.buttonFont.Text = "Select Font";
            this.buttonFont.UseVisualStyleBackColor = true;
            this.buttonFont.Click += new System.EventHandler(this.buttonFont_Click);
            // 
            // tabPageTests
            // 
            this.tabPageTests.Location = new System.Drawing.Point(4, 22);
            this.tabPageTests.Name = "tabPageTests";
            this.tabPageTests.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTests.Size = new System.Drawing.Size(592, 369);
            this.tabPageTests.TabIndex = 1;
            this.tabPageTests.Text = "Test";
            this.tabPageTests.UseVisualStyleBackColor = true;
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
            // checkBoxFixedPitchFontOnly
            // 
            this.checkBoxFixedPitchFontOnly.AutoSize = true;
            this.checkBoxFixedPitchFontOnly.Location = new System.Drawing.Point(113, 275);
            this.checkBoxFixedPitchFontOnly.Name = "checkBoxFixedPitchFontOnly";
            this.checkBoxFixedPitchFontOnly.Size = new System.Drawing.Size(120, 17);
            this.checkBoxFixedPitchFontOnly.TabIndex = 17;
            this.checkBoxFixedPitchFontOnly.Text = "Fixed pitch font only";
            this.checkBoxFixedPitchFontOnly.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 442);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.tabControl);
            this.MinimumSize = new System.Drawing.Size(640, 480);
            this.Name = "MainForm";
            this.Text = "Sharp Display Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.tabControl.ResumeLayout(false);
            this.tabPageDisplay.ResumeLayout(false);
            this.tabPageDisplay.PerformLayout();
            this.panelDisplay.ResumeLayout(false);
            this.tableLayoutPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBrightness)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageDisplay;
        private System.Windows.Forms.TabPage tabPageTests;
        private System.Windows.Forms.Button buttonFont;
        private System.Windows.Forms.FontDialog fontDialog;
        private System.Windows.Forms.Button buttonCapture;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Button buttonFill;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonOpen;
        private System.Windows.Forms.TrackBar trackBarBrightness;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelConnect;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelFps;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelSpring;
        private System.Windows.Forms.CheckBox checkBoxShowBorders;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelPower;
        private System.Windows.Forms.Panel panelDisplay;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        public MarqueeLabel marqueeLabelTop;
        public MarqueeLabel marqueeLabelBottom;
        private System.Windows.Forms.CheckBox checkBoxConnectOnStartup;
        private System.Windows.Forms.CheckBox checkBoxReverseScreen;
        private System.Windows.Forms.Button buttonStartClient;
        private System.Windows.Forms.Button buttonSuspend;
        private System.Windows.Forms.CheckBox checkBoxFixedPitchFontOnly;
    }
}

