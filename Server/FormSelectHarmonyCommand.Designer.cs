namespace SharpDisplayManager
{
    partial class FormSelectHarmonyCommand
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
            this.iTreeViewHarmony = new System.Windows.Forms.TreeView();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.iButtonOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // iTreeViewHarmony
            // 
            this.iTreeViewHarmony.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.iTreeViewHarmony.Location = new System.Drawing.Point(12, 12);
            this.iTreeViewHarmony.Name = "iTreeViewHarmony";
            this.iTreeViewHarmony.Size = new System.Drawing.Size(302, 317);
            this.iTreeViewHarmony.TabIndex = 0;
            this.iTreeViewHarmony.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.iTreeViewHarmony_AfterSelect);
            this.iTreeViewHarmony.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.iTreeViewHarmony_NodeMouseDoubleClick);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(93, 335);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 24;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // iButtonOk
            // 
            this.iButtonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.iButtonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.iButtonOk.Enabled = false;
            this.iButtonOk.Location = new System.Drawing.Point(12, 335);
            this.iButtonOk.Name = "iButtonOk";
            this.iButtonOk.Size = new System.Drawing.Size(75, 23);
            this.iButtonOk.TabIndex = 23;
            this.iButtonOk.Text = "Ok";
            this.iButtonOk.UseVisualStyleBackColor = true;
            // 
            // FormSelectHarmonyCommand
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(326, 370);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.iButtonOk);
            this.Controls.Add(this.iTreeViewHarmony);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSelectHarmonyCommand";
            this.Text = "Select command";
            this.Load += new System.EventHandler(this.FormSelectHarmonyCommand_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView iTreeViewHarmony;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button iButtonOk;
    }
}