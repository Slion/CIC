using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpLib.Display;
using SharpLib.Ear;
using System.Reflection;

namespace SharpDisplayManager
{
    /// <summary>
    /// Action edit dialog form.
    /// </summary>
    public partial class FormEditAction : Form
    {
        public SharpLib.Ear.Action Action = null;

        public FormEditAction()
        {
            InitializeComponent();
        }

        private void FormEditAction_Load(object sender, EventArgs e)
        {
            //Populate registered actions
            foreach (string key in ManagerEventAction.Current.ActionTypes.Keys)
            {
                ItemActionType item = new ItemActionType(ManagerEventAction.Current.ActionTypes[key]);
                comboBoxActionType.Items.Add(item);
            }

            comboBoxActionType.SelectedIndex = 0;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            
        }

        private void FormEditAction_Validating(object sender, CancelEventArgs e)
        {

        }

        private void comboBoxActionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Instantiate an action corresponding to our type
            Action = (SharpLib.Ear.Action)Activator.CreateInstance(((ItemActionType)comboBoxActionType.SelectedItem).Type);

            //Create input fields
            UpdateTableLayoutPanel(Action);
        }

        /// <summary>
        /// Update our table layout.
        /// Will instantiated every field control as defined by our action.
        /// Fields must be specified by rows from the left.
        /// </summary>
        /// <param name="aLayout"></param>
        private void UpdateTableLayoutPanel(SharpLib.Ear.Action aAction)
        {
            toolTip.RemoveAll();
            //Debug.Print("UpdateTableLayoutPanel")
            //First clean our current panel
            iTableLayoutPanel.Controls.Clear();
            iTableLayoutPanel.RowStyles.Clear();
            iTableLayoutPanel.ColumnStyles.Clear();
            iTableLayoutPanel.RowCount = 0;

            //We always want two columns: one for label and one for the field
            iTableLayoutPanel.ColumnCount = 2;
            iTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            iTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));


            if (aAction == null)
            {
                //Just drop it
                return;
            }
            
            //IEnumerable<PropertyInfo> properties = aAction.GetType().GetProperties().Where(
            //    prop => Attribute.IsDefined(prop, typeof(AttributeActionProperty)));


            foreach (PropertyInfo pi in aAction.GetType().GetProperties())
            {
                AttributeActionProperty[] attributes = ((AttributeActionProperty[])pi.GetCustomAttributes(typeof(AttributeActionProperty), true));
                if (attributes.Length != 1)
                {
                    continue;
                }

                AttributeActionProperty attribute = attributes[0];
                //Add a new row
                iTableLayoutPanel.RowCount++;
                iTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                //Create the label
                Label label = new Label();
                label.Text = attribute.Name;
                toolTip.SetToolTip(label, attribute.Description);
                iTableLayoutPanel.Controls.Add(label, 0, iTableLayoutPanel.RowCount-1);


                //Create the editor

            }


            /*
            //Then recreate our rows...
            while (iTableLayoutPanel.RowCount < layout.Rows.Count)
            {
                iTableLayoutPanel.RowCount++;
            }

            // ...and columns 
            while (iTableLayoutPanel.ColumnCount < layout.Columns.Count)
            {
                iTableLayoutPanel.ColumnCount++;
            }

            //For each column
            for (int i = 0; i < iTableLayoutPanel.ColumnCount; i++)
            {
                //Create our column styles
                this.iTableLayoutPanel.ColumnStyles.Add(layout.Columns[i]);

                //For each rows
                for (int j = 0; j < iTableLayoutPanel.RowCount; j++)
                {
                    if (i == 0)
                    {
                        //Create our row styles
                        this.iTableLayoutPanel.RowStyles.Add(layout.Rows[j]);
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            //For each field
            foreach (DataField field in aClient.Fields)
            {
                if (!field.IsTableField)
                {
                    //That field is not taking part in our table layout skip it
                    continue;
                }

                TableField tableField = (TableField)field;

                //Create a control corresponding to the field specified for that cell
                Control control = CreateControlForDataField(tableField);

                //Add newly created control to our table layout at the specified row and column
                iTableLayoutPanel.Controls.Add(control, tableField.Column, tableField.Row);
                //Make sure we specify column and row span for that new control
                iTableLayoutPanel.SetColumnSpan(control, tableField.ColumnSpan);
                iTableLayoutPanel.SetRowSpan(control, tableField.RowSpan);
            }
            */

        }

    }
}
