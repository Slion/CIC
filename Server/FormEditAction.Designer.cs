namespace SharpDisplayManager
{
    partial class FormEditAction
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
            this.comboBoxActionType = new System.Windows.Forms.ComboBox();
            this.labelActionType = new System.Windows.Forms.Label();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.iTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // comboBoxActionType
            // 
            this.comboBoxActionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxActionType.FormattingEnabled = true;
            this.comboBoxActionType.Location = new System.Drawing.Point(55, 12);
            this.comboBoxActionType.Name = "comboBoxActionType";
            this.comboBoxActionType.Size = new System.Drawing.Size(333, 21);
            this.comboBoxActionType.Sorted = true;
            this.comboBoxActionType.TabIndex = 18;
            this.comboBoxActionType.SelectedIndexChanged += new System.EventHandler(this.comboBoxActionType_SelectedIndexChanged);
            // 
            // labelActionType
            // 
            this.labelActionType.AutoSize = true;
            this.labelActionType.Location = new System.Drawing.Point(12, 15);
            this.labelActionType.Name = "labelActionType";
            this.labelActionType.Size = new System.Drawing.Size(37, 13);
            this.labelActionType.TabIndex = 20;
            this.labelActionType.Text = "Type :";
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Location = new System.Drawing.Point(12, 72);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 21;
            this.buttonOk.Text = "Ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(93, 72);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 22;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // iTableLayoutPanel
            // 
            this.iTableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.iTableLayoutPanel.AutoSize = true;
            this.iTableLayoutPanel.ColumnCount = 2;
            this.iTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.iTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.iTableLayoutPanel.Location = new System.Drawing.Point(15, 50);
            this.iTableLayoutPanel.Name = "iTableLayoutPanel";
            this.iTableLayoutPanel.RowCount = 2;
            this.iTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.iTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.iTableLayoutPanel.Size = new System.Drawing.Size(375, 16);
            this.iTableLayoutPanel.TabIndex = 23;
            // 
            // FormEditAction
            // 
            this.AcceptButton = this.buttonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(402, 107);
            this.Controls.Add(this.iTableLayoutPanel);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.labelActionType);
            this.Controls.Add(this.comboBoxActionType);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormEditAction";
            this.Text = "Edit action";
            this.Load += new System.EventHandler(this.FormEditAction_Load);
            this.Validating += new System.ComponentModel.CancelEventHandler(this.FormEditAction_Validating);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxActionType;
        private System.Windows.Forms.Label labelActionType;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.TableLayoutPanel iTableLayoutPanel;
        private System.Windows.Forms.ToolTip toolTip;
    }
}