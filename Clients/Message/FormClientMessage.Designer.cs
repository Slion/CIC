namespace SharpDisplayClientMessage
{
    partial class FormClientMessage
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
            this.iTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // iTimer
            // 
            this.iTimer.Tick += new System.EventHandler(this.iTimer_Tick);
            // 
            // FormClientMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "FormClientMessage";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormClientMessage_FormClosing);
            this.Load += new System.EventHandler(this.FormClientMessage_Load);
            this.Shown += new System.EventHandler(this.FormClientMessage_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer iTimer;
    }
}

