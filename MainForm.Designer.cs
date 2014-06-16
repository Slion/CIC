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
            this.buttonCapture = new System.Windows.Forms.Button();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.buttonFont = new System.Windows.Forms.Button();
            this.tabPageTests = new System.Windows.Forms.TabPage();
            this.fontDialog = new System.Windows.Forms.FontDialog();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.marqueeLabelTop = new SharpDisplayManager.MarqueeLabel();
            this.marqueeLabelBottom = new SharpDisplayManager.MarqueeLabel();
            this.tabControl.SuspendLayout();
            this.tabPageDisplay.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
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
            this.tabControl.Size = new System.Drawing.Size(529, 362);
            this.tabControl.TabIndex = 0;
            // 
            // tabPageDisplay
            // 
            this.tabPageDisplay.Controls.Add(this.buttonCapture);
            this.tabPageDisplay.Controls.Add(this.tableLayoutPanel);
            this.tabPageDisplay.Controls.Add(this.buttonFont);
            this.tabPageDisplay.Location = new System.Drawing.Point(4, 22);
            this.tabPageDisplay.Name = "tabPageDisplay";
            this.tabPageDisplay.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDisplay.Size = new System.Drawing.Size(521, 336);
            this.tabPageDisplay.TabIndex = 0;
            this.tabPageDisplay.Text = "Display";
            this.tabPageDisplay.UseVisualStyleBackColor = true;
            // 
            // buttonCapture
            // 
            this.buttonCapture.Location = new System.Drawing.Point(6, 278);
            this.buttonCapture.Name = "buttonCapture";
            this.buttonCapture.Size = new System.Drawing.Size(75, 23);
            this.buttonCapture.TabIndex = 5;
            this.buttonCapture.Text = "Capture";
            this.buttonCapture.UseVisualStyleBackColor = true;
            this.buttonCapture.Click += new System.EventHandler(this.buttonCapture_Click);
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Controls.Add(this.marqueeLabelTop, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.marqueeLabelBottom, 0, 1);
            this.tableLayoutPanel.Location = new System.Drawing.Point(215, 165);
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
            this.tableLayoutPanel.Size = new System.Drawing.Size(256, 64);
            this.tableLayoutPanel.TabIndex = 4;
            // 
            // buttonFont
            // 
            this.buttonFont.Location = new System.Drawing.Point(6, 307);
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
            this.tabPageTests.Size = new System.Drawing.Size(521, 336);
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
            // marqueeLabelTop
            // 
            this.marqueeLabelTop.BackColor = System.Drawing.Color.Transparent;
            this.marqueeLabelTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.marqueeLabelTop.Location = new System.Drawing.Point(1, -187);
            this.marqueeLabelTop.Margin = new System.Windows.Forms.Padding(0);
            this.marqueeLabelTop.Name = "marqueeLabelTop";
            this.marqueeLabelTop.OwnTimer = false;
            this.marqueeLabelTop.PixelsPerSecond = 64;
            this.marqueeLabelTop.Size = new System.Drawing.Size(254, 20);
            this.marqueeLabelTop.TabIndex = 2;
            this.marqueeLabelTop.Text = "ABCDEFGHIJKLMNOPQRST-0123456789";
            this.marqueeLabelTop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.marqueeLabelTop.UseCompatibleTextRendering = true;
            // 
            // marqueeLabelBottom
            // 
            this.marqueeLabelBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.marqueeLabelBottom.Location = new System.Drawing.Point(1, -61);
            this.marqueeLabelBottom.Margin = new System.Windows.Forms.Padding(0);
            this.marqueeLabelBottom.Name = "marqueeLabelBottom";
            this.marqueeLabelBottom.OwnTimer = false;
            this.marqueeLabelBottom.PixelsPerSecond = 64;
            this.marqueeLabelBottom.Size = new System.Drawing.Size(254, 20);
            this.marqueeLabelBottom.TabIndex = 3;
            this.marqueeLabelBottom.Text = "abcdefghijklmnopqrst-0123456789";
            this.marqueeLabelBottom.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.marqueeLabelBottom.UseCompatibleTextRendering = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(553, 386);
            this.Controls.Add(this.tabControl);
            this.Name = "MainForm";
            this.Text = "Sharp Display Manager";
            this.tabControl.ResumeLayout(false);
            this.tabPageDisplay.ResumeLayout(false);
            this.tableLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageDisplay;
        private System.Windows.Forms.TabPage tabPageTests;
        private System.Windows.Forms.Button buttonFont;
        private System.Windows.Forms.FontDialog fontDialog;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private MarqueeLabel marqueeLabelTop;
        private MarqueeLabel marqueeLabelBottom;
        private System.Windows.Forms.Button buttonCapture;
        private System.Windows.Forms.Timer timer;
    }
}

