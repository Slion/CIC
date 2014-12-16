namespace SharpDisplayClient
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
            this.SuspendLayout();
            // 
            // buttonSetText
            // 
            this.buttonSetText.Location = new System.Drawing.Point(13, 132);
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
            this.buttonAlignRight.Location = new System.Drawing.Point(176, 160);
            this.buttonAlignRight.Name = "buttonAlignRight";
            this.buttonAlignRight.Size = new System.Drawing.Size(75, 23);
            this.buttonAlignRight.TabIndex = 23;
            this.buttonAlignRight.Text = "Align Right";
            this.buttonAlignRight.UseVisualStyleBackColor = true;
            this.buttonAlignRight.Click += new System.EventHandler(this.buttonAlignRight_Click);
            // 
            // buttonAlignCenter
            // 
            this.buttonAlignCenter.Location = new System.Drawing.Point(94, 161);
            this.buttonAlignCenter.Name = "buttonAlignCenter";
            this.buttonAlignCenter.Size = new System.Drawing.Size(75, 23);
            this.buttonAlignCenter.TabIndex = 22;
            this.buttonAlignCenter.Text = "Align Center";
            this.buttonAlignCenter.UseVisualStyleBackColor = true;
            this.buttonAlignCenter.Click += new System.EventHandler(this.buttonAlignCenter_Click);
            // 
            // buttonAlignLeft
            // 
            this.buttonAlignLeft.Location = new System.Drawing.Point(12, 161);
            this.buttonAlignLeft.Name = "buttonAlignLeft";
            this.buttonAlignLeft.Size = new System.Drawing.Size(75, 23);
            this.buttonAlignLeft.TabIndex = 21;
            this.buttonAlignLeft.Text = "Align Left";
            this.buttonAlignLeft.UseVisualStyleBackColor = true;
            this.buttonAlignLeft.Click += new System.EventHandler(this.buttonAlignLeft_Click);
            // 
            // buttonSetTopText
            // 
            this.buttonSetTopText.Location = new System.Drawing.Point(94, 132);
            this.buttonSetTopText.Name = "buttonSetTopText";
            this.buttonSetTopText.Size = new System.Drawing.Size(75, 23);
            this.buttonSetTopText.TabIndex = 24;
            this.buttonSetTopText.Text = "Set Top Text";
            this.buttonSetTopText.UseVisualStyleBackColor = true;
            this.buttonSetTopText.Click += new System.EventHandler(this.buttonSetTopText_Click);
            // 
            // buttonLayoutUpdate
            // 
            this.buttonLayoutUpdate.Location = new System.Drawing.Point(176, 131);
            this.buttonLayoutUpdate.Name = "buttonLayoutUpdate";
            this.buttonLayoutUpdate.Size = new System.Drawing.Size(75, 23);
            this.buttonLayoutUpdate.TabIndex = 25;
            this.buttonLayoutUpdate.Text = "Layout 2x2";
            this.buttonLayoutUpdate.UseVisualStyleBackColor = true;
            this.buttonLayoutUpdate.Click += new System.EventHandler(this.buttonLayoutUpdate_Click);
            // 
            // buttonSetBitmap
            // 
            this.buttonSetBitmap.Location = new System.Drawing.Point(12, 190);
            this.buttonSetBitmap.Name = "buttonSetBitmap";
            this.buttonSetBitmap.Size = new System.Drawing.Size(75, 23);
            this.buttonSetBitmap.TabIndex = 26;
            this.buttonSetBitmap.Text = "Set Bitmap";
            this.buttonSetBitmap.UseVisualStyleBackColor = true;
            this.buttonSetBitmap.Click += new System.EventHandler(this.buttonSetBitmap_Click);
            // 
            // buttonBitmapLayout
            // 
            this.buttonBitmapLayout.Location = new System.Drawing.Point(176, 189);
            this.buttonBitmapLayout.Name = "buttonBitmapLayout";
            this.buttonBitmapLayout.Size = new System.Drawing.Size(75, 35);
            this.buttonBitmapLayout.TabIndex = 27;
            this.buttonBitmapLayout.Text = "Bitmap Layout";
            this.buttonBitmapLayout.UseVisualStyleBackColor = true;
            this.buttonBitmapLayout.Click += new System.EventHandler(this.buttonBitmapLayout_Click);
            // 
            // buttonIndicatorsLayout
            // 
            this.buttonIndicatorsLayout.Location = new System.Drawing.Point(94, 189);
            this.buttonIndicatorsLayout.Name = "buttonIndicatorsLayout";
            this.buttonIndicatorsLayout.Size = new System.Drawing.Size(75, 35);
            this.buttonIndicatorsLayout.TabIndex = 28;
            this.buttonIndicatorsLayout.Text = "Indicators Layout ";
            this.buttonIndicatorsLayout.UseVisualStyleBackColor = true;
            this.buttonIndicatorsLayout.Click += new System.EventHandler(this.buttonIndicatorsLayout_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(443, 252);
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
            this.Name = "MainForm";
            this.Text = "Client";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
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
    }
}

