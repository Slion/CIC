namespace SharpDisplayClient
{
    partial class FormClientTest
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
            this.buttonSetText = new System.Windows.Forms.Button();
            this.textBoxTop = new System.Windows.Forms.TextBox();
            this.textBoxBottom = new System.Windows.Forms.TextBox();
            this.buttonAlignRight = new System.Windows.Forms.Button();
            this.buttonAlignCenter = new System.Windows.Forms.Button();
            this.buttonAlignLeft = new System.Windows.Forms.Button();
            this.buttonSetTopText = new System.Windows.Forms.Button();
            this.buttonLayoutUpdate = new System.Windows.Forms.Button();
            this.buttonSetBitmap = new System.Windows.Forms.Button();
            this.buttonBitmapLayout = new System.Windows.Forms.Button();
            this.buttonIndicatorsLayout = new System.Windows.Forms.Button();
            this.buttonUpdateTexts = new System.Windows.Forms.Button();
            this.buttonLayoutOneTextField = new System.Windows.Forms.Button();
            this.numericUpDownPriority = new System.Windows.Forms.NumericUpDown();
            this.labelPriority = new System.Windows.Forms.Label();
            this.buttonTriggerEvents = new System.Windows.Forms.Button();
            this.textBoxEventName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPriority)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonSetText
            // 
            this.buttonSetText.Location = new System.Drawing.Point(12, 109);
            this.buttonSetText.Name = "buttonSetText";
            this.buttonSetText.Size = new System.Drawing.Size(75, 23);
            this.buttonSetText.TabIndex = 0;
            this.buttonSetText.Text = "Set Text";
            this.buttonSetText.UseVisualStyleBackColor = true;
            this.buttonSetText.Click += new System.EventHandler(this.buttonSetText_Click);
            // 
            // textBoxTop
            // 
            this.textBoxTop.Location = new System.Drawing.Point(12, 31);
            this.textBoxTop.Name = "textBoxTop";
            this.textBoxTop.Size = new System.Drawing.Size(419, 20);
            this.textBoxTop.TabIndex = 1;
            // 
            // textBoxBottom
            // 
            this.textBoxBottom.Location = new System.Drawing.Point(12, 57);
            this.textBoxBottom.Name = "textBoxBottom";
            this.textBoxBottom.Size = new System.Drawing.Size(419, 20);
            this.textBoxBottom.TabIndex = 2;
            // 
            // buttonAlignRight
            // 
            this.buttonAlignRight.Location = new System.Drawing.Point(175, 137);
            this.buttonAlignRight.Name = "buttonAlignRight";
            this.buttonAlignRight.Size = new System.Drawing.Size(75, 23);
            this.buttonAlignRight.TabIndex = 23;
            this.buttonAlignRight.Text = "Align Right";
            this.buttonAlignRight.UseVisualStyleBackColor = true;
            this.buttonAlignRight.Click += new System.EventHandler(this.buttonAlignRight_Click);
            // 
            // buttonAlignCenter
            // 
            this.buttonAlignCenter.Location = new System.Drawing.Point(93, 138);
            this.buttonAlignCenter.Name = "buttonAlignCenter";
            this.buttonAlignCenter.Size = new System.Drawing.Size(75, 23);
            this.buttonAlignCenter.TabIndex = 22;
            this.buttonAlignCenter.Text = "Align Center";
            this.buttonAlignCenter.UseVisualStyleBackColor = true;
            this.buttonAlignCenter.Click += new System.EventHandler(this.buttonAlignCenter_Click);
            // 
            // buttonAlignLeft
            // 
            this.buttonAlignLeft.Location = new System.Drawing.Point(11, 138);
            this.buttonAlignLeft.Name = "buttonAlignLeft";
            this.buttonAlignLeft.Size = new System.Drawing.Size(75, 23);
            this.buttonAlignLeft.TabIndex = 21;
            this.buttonAlignLeft.Text = "Align Left";
            this.buttonAlignLeft.UseVisualStyleBackColor = true;
            this.buttonAlignLeft.Click += new System.EventHandler(this.buttonAlignLeft_Click);
            // 
            // buttonSetTopText
            // 
            this.buttonSetTopText.Location = new System.Drawing.Point(93, 109);
            this.buttonSetTopText.Name = "buttonSetTopText";
            this.buttonSetTopText.Size = new System.Drawing.Size(75, 23);
            this.buttonSetTopText.TabIndex = 24;
            this.buttonSetTopText.Text = "Set Top Text";
            this.buttonSetTopText.UseVisualStyleBackColor = true;
            this.buttonSetTopText.Click += new System.EventHandler(this.buttonSetTopText_Click);
            // 
            // buttonLayoutUpdate
            // 
            this.buttonLayoutUpdate.Location = new System.Drawing.Point(175, 108);
            this.buttonLayoutUpdate.Name = "buttonLayoutUpdate";
            this.buttonLayoutUpdate.Size = new System.Drawing.Size(156, 23);
            this.buttonLayoutUpdate.TabIndex = 25;
            this.buttonLayoutUpdate.Text = "Layout 2x2 and Recording";
            this.buttonLayoutUpdate.UseVisualStyleBackColor = true;
            this.buttonLayoutUpdate.Click += new System.EventHandler(this.buttonLayoutUpdate_Click);
            // 
            // buttonSetBitmap
            // 
            this.buttonSetBitmap.Location = new System.Drawing.Point(11, 167);
            this.buttonSetBitmap.Name = "buttonSetBitmap";
            this.buttonSetBitmap.Size = new System.Drawing.Size(75, 23);
            this.buttonSetBitmap.TabIndex = 26;
            this.buttonSetBitmap.Text = "Set Bitmap";
            this.buttonSetBitmap.UseVisualStyleBackColor = true;
            this.buttonSetBitmap.Click += new System.EventHandler(this.buttonSetBitmap_Click);
            // 
            // buttonBitmapLayout
            // 
            this.buttonBitmapLayout.Location = new System.Drawing.Point(175, 166);
            this.buttonBitmapLayout.Name = "buttonBitmapLayout";
            this.buttonBitmapLayout.Size = new System.Drawing.Size(75, 35);
            this.buttonBitmapLayout.TabIndex = 27;
            this.buttonBitmapLayout.Text = "Bitmap Layout";
            this.buttonBitmapLayout.UseVisualStyleBackColor = true;
            this.buttonBitmapLayout.Click += new System.EventHandler(this.buttonBitmapLayout_Click);
            // 
            // buttonIndicatorsLayout
            // 
            this.buttonIndicatorsLayout.Location = new System.Drawing.Point(93, 166);
            this.buttonIndicatorsLayout.Name = "buttonIndicatorsLayout";
            this.buttonIndicatorsLayout.Size = new System.Drawing.Size(75, 35);
            this.buttonIndicatorsLayout.TabIndex = 28;
            this.buttonIndicatorsLayout.Text = "Indicators Layout ";
            this.buttonIndicatorsLayout.UseVisualStyleBackColor = true;
            this.buttonIndicatorsLayout.Click += new System.EventHandler(this.buttonIndicatorsLayout_Click);
            // 
            // buttonUpdateTexts
            // 
            this.buttonUpdateTexts.Location = new System.Drawing.Point(256, 166);
            this.buttonUpdateTexts.Name = "buttonUpdateTexts";
            this.buttonUpdateTexts.Size = new System.Drawing.Size(75, 35);
            this.buttonUpdateTexts.TabIndex = 29;
            this.buttonUpdateTexts.Text = "Update Texts";
            this.buttonUpdateTexts.UseVisualStyleBackColor = true;
            this.buttonUpdateTexts.Click += new System.EventHandler(this.buttonUpdateTexts_Click);
            // 
            // buttonLayoutOneTextField
            // 
            this.buttonLayoutOneTextField.Location = new System.Drawing.Point(256, 137);
            this.buttonLayoutOneTextField.Name = "buttonLayoutOneTextField";
            this.buttonLayoutOneTextField.Size = new System.Drawing.Size(75, 23);
            this.buttonLayoutOneTextField.TabIndex = 30;
            this.buttonLayoutOneTextField.Text = "Layout 1x1";
            this.buttonLayoutOneTextField.UseVisualStyleBackColor = true;
            this.buttonLayoutOneTextField.Click += new System.EventHandler(this.buttonLayoutOneTextField_Click);
            // 
            // numericUpDownPriority
            // 
            this.numericUpDownPriority.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownPriority.Location = new System.Drawing.Point(57, 83);
            this.numericUpDownPriority.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownPriority.Name = "numericUpDownPriority";
            this.numericUpDownPriority.Size = new System.Drawing.Size(60, 20);
            this.numericUpDownPriority.TabIndex = 31;
            this.numericUpDownPriority.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numericUpDownPriority.ValueChanged += new System.EventHandler(this.numericUpDownPriority_ValueChanged);
            // 
            // labelPriority
            // 
            this.labelPriority.AutoSize = true;
            this.labelPriority.Location = new System.Drawing.Point(12, 85);
            this.labelPriority.Name = "labelPriority";
            this.labelPriority.Size = new System.Drawing.Size(41, 13);
            this.labelPriority.TabIndex = 32;
            this.labelPriority.Text = "Priority:";
            // 
            // buttonTriggerEvents
            // 
            this.buttonTriggerEvents.Location = new System.Drawing.Point(11, 257);
            this.buttonTriggerEvents.Name = "buttonTriggerEvents";
            this.buttonTriggerEvents.Size = new System.Drawing.Size(75, 23);
            this.buttonTriggerEvents.TabIndex = 33;
            this.buttonTriggerEvents.Text = "Trigger Events";
            this.buttonTriggerEvents.UseVisualStyleBackColor = true;
            this.buttonTriggerEvents.Click += new System.EventHandler(this.buttonTriggerEvents_Click);
            // 
            // textBoxEventName
            // 
            this.textBoxEventName.Location = new System.Drawing.Point(11, 231);
            this.textBoxEventName.Name = "textBoxEventName";
            this.textBoxEventName.Size = new System.Drawing.Size(202, 20);
            this.textBoxEventName.TabIndex = 34;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 212);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 35;
            this.label1.Text = "Event name:";
            // 
            // FormClientTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(499, 292);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxEventName);
            this.Controls.Add(this.buttonTriggerEvents);
            this.Controls.Add(this.labelPriority);
            this.Controls.Add(this.numericUpDownPriority);
            this.Controls.Add(this.buttonLayoutOneTextField);
            this.Controls.Add(this.buttonUpdateTexts);
            this.Controls.Add(this.buttonIndicatorsLayout);
            this.Controls.Add(this.buttonBitmapLayout);
            this.Controls.Add(this.buttonSetBitmap);
            this.Controls.Add(this.buttonLayoutUpdate);
            this.Controls.Add(this.buttonSetTopText);
            this.Controls.Add(this.buttonAlignRight);
            this.Controls.Add(this.buttonAlignCenter);
            this.Controls.Add(this.buttonAlignLeft);
            this.Controls.Add(this.textBoxBottom);
            this.Controls.Add(this.textBoxTop);
            this.Controls.Add(this.buttonSetText);
            this.Name = "FormClientTest";
            this.Text = "Client";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPriority)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonSetText;
        private System.Windows.Forms.TextBox textBoxTop;
        private System.Windows.Forms.TextBox textBoxBottom;
        private System.Windows.Forms.Button buttonAlignRight;
        private System.Windows.Forms.Button buttonAlignCenter;
        private System.Windows.Forms.Button buttonAlignLeft;
        private System.Windows.Forms.Button buttonSetTopText;
        private System.Windows.Forms.Button buttonLayoutUpdate;
        private System.Windows.Forms.Button buttonSetBitmap;
        private System.Windows.Forms.Button buttonBitmapLayout;
        private System.Windows.Forms.Button buttonIndicatorsLayout;
        private System.Windows.Forms.Button buttonUpdateTexts;
		private System.Windows.Forms.Button buttonLayoutOneTextField;
        private System.Windows.Forms.NumericUpDown numericUpDownPriority;
        private System.Windows.Forms.Label labelPriority;
        private System.Windows.Forms.Button buttonTriggerEvents;
        private System.Windows.Forms.TextBox textBoxEventName;
        private System.Windows.Forms.Label label1;
    }
}

