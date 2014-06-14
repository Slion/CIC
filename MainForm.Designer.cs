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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageDisplay = new System.Windows.Forms.TabPage();
            this.buttonCapture = new System.Windows.Forms.Button();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.marqueeLabel1 = new SharpDisplayManager.MarqueeLabel();
            this.marqueeLabel2 = new SharpDisplayManager.MarqueeLabel();
            this.buttonFont = new System.Windows.Forms.Button();
            this.tabPageTests = new System.Windows.Forms.TabPage();
            this.fontDialog = new System.Windows.Forms.FontDialog();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
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
            this.tableLayoutPanel.Controls.Add(this.marqueeLabel1, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.marqueeLabel2, 0, 1);
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
            this.tableLayoutPanel.Size = new System.Drawing.Size(256, 64);
            this.tableLayoutPanel.TabIndex = 4;
            // 
            // marqueeLabel1
            // 
            this.marqueeLabel1.BackColor = System.Drawing.Color.Transparent;
            this.marqueeLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.marqueeLabel1.Location = new System.Drawing.Point(1, 1);
            this.marqueeLabel1.Margin = new System.Windows.Forms.Padding(0);
            this.marqueeLabel1.Name = "marqueeLabel1";
            this.marqueeLabel1.PixelsPerSecond = 128;
            this.marqueeLabel1.Size = new System.Drawing.Size(254, 30);
            this.marqueeLabel1.TabIndex = 2;
            this.marqueeLabel1.Text = "ABCDEFGHIJKLMNOPQRST-0123456789";
            this.marqueeLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.marqueeLabel1.UseCompatibleTextRendering = true;
            // 
            // marqueeLabel2
            // 
            this.marqueeLabel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.marqueeLabel2.Location = new System.Drawing.Point(1, 32);
            this.marqueeLabel2.Margin = new System.Windows.Forms.Padding(0);
            this.marqueeLabel2.Name = "marqueeLabel2";
            this.marqueeLabel2.PixelsPerSecond = 64;
            this.marqueeLabel2.Size = new System.Drawing.Size(254, 31);
            this.marqueeLabel2.TabIndex = 3;
            this.marqueeLabel2.Text = "abcdefghijklmnopqrst-0123456789";
            this.marqueeLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.marqueeLabel2.UseCompatibleTextRendering = true;
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
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private MarqueeLabel marqueeLabel1;
        private MarqueeLabel marqueeLabel2;
        private System.Windows.Forms.Button buttonCapture;
    }
}

