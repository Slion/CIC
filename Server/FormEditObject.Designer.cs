namespace SharpDisplayManager
{
    partial class FormEditObject<T>
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
            this.iComboBoxObjectType = new System.Windows.Forms.ComboBox();
            this.labelActionType = new System.Windows.Forms.Label();
            this.iButtonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.iTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.iButtonTest = new System.Windows.Forms.Button();
            this.iLabelBrief = new System.Windows.Forms.Label();
            this.iLabelDescription = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // iComboBoxObjectType
            // 
            this.iComboBoxObjectType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.iComboBoxObjectType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.iComboBoxObjectType.FormattingEnabled = true;
            this.iComboBoxObjectType.Location = new System.Drawing.Point(55, 52);
            this.iComboBoxObjectType.Name = "iComboBoxObjectType";
            this.iComboBoxObjectType.Size = new System.Drawing.Size(272, 21);
            this.iComboBoxObjectType.Sorted = true;
            this.iComboBoxObjectType.TabIndex = 18;
            this.iComboBoxObjectType.SelectedIndexChanged += new System.EventHandler(this.comboBoxActionType_SelectedIndexChanged);
            this.iComboBoxObjectType.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.iComboBoxObjectType_KeyPress);
            // 
            // labelActionType
            // 
            this.labelActionType.AutoSize = true;
            this.labelActionType.Location = new System.Drawing.Point(12, 55);
            this.labelActionType.Name = "labelActionType";
            this.labelActionType.Size = new System.Drawing.Size(37, 13);
            this.labelActionType.TabIndex = 20;
            this.labelActionType.Text = "Type :";
            // 
            // buttonOk
            // 
            this.iButtonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.iButtonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.iButtonOk.Location = new System.Drawing.Point(12, 157);
            this.iButtonOk.Name = "buttonOk";
            this.iButtonOk.Size = new System.Drawing.Size(75, 23);
            this.iButtonOk.TabIndex = 21;
            this.iButtonOk.Text = "Ok";
            this.iButtonOk.UseVisualStyleBackColor = true;
            this.iButtonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(93, 157);
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
            this.iTableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.iTableLayoutPanel.ColumnCount = 2;
            this.iTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.iTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.iTableLayoutPanel.Location = new System.Drawing.Point(15, 91);
            this.iTableLayoutPanel.Name = "iTableLayoutPanel";
            this.iTableLayoutPanel.RowCount = 2;
            this.iTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.iTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.iTableLayoutPanel.Size = new System.Drawing.Size(312, 46);
            this.iTableLayoutPanel.TabIndex = 23;
            // 
            // buttonTest
            // 
            this.iButtonTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.iButtonTest.Location = new System.Drawing.Point(252, 157);
            this.iButtonTest.Name = "buttonTest";
            this.iButtonTest.Size = new System.Drawing.Size(75, 23);
            this.iButtonTest.TabIndex = 24;
            this.iButtonTest.Text = "Test";
            this.iButtonTest.UseVisualStyleBackColor = true;
            this.iButtonTest.Click += new System.EventHandler(this.buttonTest_Click);
            // 
            // iLabelBrief
            // 
            this.iLabelBrief.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.iLabelBrief.AutoSize = true;
            this.iLabelBrief.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iLabelBrief.Location = new System.Drawing.Point(12, 9);
            this.iLabelBrief.Name = "iLabelBrief";
            this.iLabelBrief.Size = new System.Drawing.Size(33, 13);
            this.iLabelBrief.TabIndex = 25;
            this.iLabelBrief.Text = "Brief";
            // 
            // iLabelDescription
            // 
            this.iLabelDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.iLabelDescription.AutoSize = true;
            this.iLabelDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iLabelDescription.Location = new System.Drawing.Point(12, 31);
            this.iLabelDescription.Name = "iLabelDescription";
            this.iLabelDescription.Size = new System.Drawing.Size(60, 13);
            this.iLabelDescription.TabIndex = 26;
            this.iLabelDescription.Text = "Description";
            // 
            // FormEditObject
            // 
            this.AcceptButton = this.iButtonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(339, 192);
            this.Controls.Add(this.iLabelDescription);
            this.Controls.Add(this.iLabelBrief);
            this.Controls.Add(this.iButtonTest);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.iButtonOk);
            this.Controls.Add(this.labelActionType);
            this.Controls.Add(this.iComboBoxObjectType);
            this.Controls.Add(this.iTableLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormEditObject";
            this.Text = "Edit action";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormEditObject_FormClosing);
            this.Load += new System.EventHandler(this.FormEditAction_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox iComboBoxObjectType;
        private System.Windows.Forms.Label labelActionType;
        private System.Windows.Forms.Button iButtonOk;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.TableLayoutPanel iTableLayoutPanel;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button iButtonTest;
        private System.Windows.Forms.Label iLabelBrief;
        private System.Windows.Forms.Label iLabelDescription;
    }
}