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
using Microsoft.VisualBasic.CompilerServices;
using SharpLib.Utils;

namespace SharpDisplayManager
{
    /// <summary>
    /// Object edit dialog form.
    /// </summary>
    public partial class FormEditObject<T> : Form where T : class
    {
        public T Object = null;

        public FormEditObject()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormEditAction_Load(object sender, EventArgs e)
        {
            // Populate registered object types
            IEnumerable < Type > types = Reflection.GetConcreteClassesDerivedFrom<T>();
            foreach (Type type in types)
            {
                ItemObjectType item = new ItemObjectType(type);
                comboBoxActionType.Items.Add(item);
            }

            if (Object == null)
            {
                // Creating new issue, select our first item
                comboBoxActionType.SelectedIndex = 0;
            }
            else
            {
                // Editing existing object
                // Look up our item in our combobox 
                foreach (ItemObjectType item in comboBoxActionType.Items)
                {
                    if (item.Type == Object.GetType())
                    {
                        comboBoxActionType.SelectedItem = item;
                    }
                }
            }            
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            FetchPropertiesValue(Object);
        }

        private void FormEditAction_Validating(object sender, CancelEventArgs e)
        {

        }

        private void comboBoxActionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Instantiate an action corresponding to our type
            Type actionType = ((ItemObjectType) comboBoxActionType.SelectedItem).Type;
            //Create another type of action only if needed
            if (Object == null || Object.GetType() != actionType)
            {
                Object = (T)Activator.CreateInstance(actionType);
            }
            
            //Create input fields
            UpdateTableLayoutPanel(Object);
        }


        /// <summary>
        /// Get properties values from our generated input fields
        /// </summary>
        private void FetchPropertiesValue(T aObject)
        {
            int ctrlIndex = 0;
            foreach (PropertyInfo pi in aObject.GetType().GetProperties())
            {
                AttributeObjectProperty[] attributes =
                    ((AttributeObjectProperty[]) pi.GetCustomAttributes(typeof(AttributeObjectProperty), true));
                if (attributes.Length != 1)
                {
                    continue;
                }

                AttributeObjectProperty attribute = attributes[0];

                if (!IsPropertyTypeSupported(pi))
                {
                    continue;
                }

                GetPropertyValueFromControl(iTableLayoutPanel.Controls[ctrlIndex+1], pi, aObject); //+1 otherwise we get the label

                ctrlIndex+=2; //Jump over the label too
            }
        }

        /// <summary>
        /// Extend this function to support reading new types of properties.
        /// </summary>
        /// <param name="aObject"></param>
        private void GetPropertyValueFromControl(Control aControl, PropertyInfo aInfo, T aObject)
        {
            if (aInfo.PropertyType == typeof(int))
            {
                NumericUpDown ctrl=(NumericUpDown)aControl;
                aInfo.SetValue(aObject,(int)ctrl.Value);
            }
            else if (aInfo.PropertyType.IsEnum)
            {
                // Instantiate our enum
                object enumValue= Activator.CreateInstance(aInfo.PropertyType);
                // Parse our enum from combo box
                enumValue = Enum.Parse(aInfo.PropertyType,((ComboBox)aControl).SelectedItem.ToString());
                //enumValue = ((ComboBox)aControl).SelectedValue;
                // Set enum value
                aInfo.SetValue(aObject, enumValue);
            }
            else if (aInfo.PropertyType == typeof(bool))
            {
                CheckBox ctrl = (CheckBox)aControl;
                aInfo.SetValue(aObject, ctrl.Checked);
            }
            else if (aInfo.PropertyType == typeof(string))
            {
                TextBox ctrl = (TextBox)aControl;
                aInfo.SetValue(aObject, ctrl.Text);
            }
            //TODO: add support for other types here
        }


        /// <summary>
        /// Create a control for the given property.
        /// </summary>
        /// <param name="aInfo"></param>
        /// <param name="aAttribute"></param>
        /// <param name="aObject"></param>
        /// <returns></returns>
        private Control CreateControlForProperty(PropertyInfo aInfo, AttributeObjectProperty aAttribute, T aObject)
        {
            if (aInfo.PropertyType == typeof(int))
            {
                //Integer properties are using numeric editor
                NumericUpDown ctrl = new NumericUpDown();
                ctrl.AutoSize = true;
                ctrl.Minimum = Int32.Parse(aAttribute.Minimum);
                ctrl.Maximum = Int32.Parse(aAttribute.Maximum);
                ctrl.Increment = Int32.Parse(aAttribute.Increment);
                ctrl.Value = (int)aInfo.GetValue(aObject);
                return ctrl;
            }
            else if (aInfo.PropertyType.IsEnum)
            {
                //Enum properties are using combo box
                ComboBox ctrl = new ComboBox();
                ctrl.AutoSize = true;                
                ctrl.Sorted = true;                
                ctrl.DropDownStyle = ComboBoxStyle.DropDownList;
                //Data source is fine but it gives us duplicate entries for duplicated enum values
                //ctrl.DataSource = Enum.GetValues(aInfo.PropertyType);

                //Therefore we need to explicitly create our items
                Size cbSize = new Size(0,0);
                foreach (string name in aInfo.PropertyType.GetEnumNames())
                {
                    ctrl.Items.Add(name.ToString());
                    Graphics g = this.CreateGraphics();
                    //Since combobox autosize would not work we need to get measure text ourselves
                    SizeF size=g.MeasureString(name.ToString(), ctrl.Font);
                    cbSize.Width = Math.Max(cbSize.Width,(int)size.Width);
                    cbSize.Height = Math.Max(cbSize.Height, (int)size.Height);
                }

                //Make sure our combobox is large enough
                ctrl.MinimumSize = cbSize;

                // Instantiate our enum
                object enumValue = Activator.CreateInstance(aInfo.PropertyType);
                enumValue = aInfo.GetValue(aObject);
                //Set the current item
                ctrl.SelectedItem = enumValue.ToString();

                return ctrl;
            }
            else if (aInfo.PropertyType == typeof(bool))
            {
                CheckBox ctrl = new CheckBox();
                ctrl.AutoSize = true;
                ctrl.Text = aAttribute.Description;
                ctrl.Checked = (bool)aInfo.GetValue(aObject);                
                return ctrl;
            }
            else if (aInfo.PropertyType == typeof(string))
            {
                TextBox ctrl = new TextBox();
                ctrl.AutoSize = true;
                ctrl.Text = (string)aInfo.GetValue(aObject);
                return ctrl;
            }
            //TODO: add support for other control type here

            return null;
        }

        /// <summary>
        /// Don't forget to extend that one and adding types
        /// </summary>
        /// <returns></returns>
        private bool IsPropertyTypeSupported(PropertyInfo aInfo)
        {
            if (aInfo.PropertyType == typeof(int))
            {
                return true;
            }
            else if (aInfo.PropertyType.IsEnum)
            {
                return true;
            }
            else if (aInfo.PropertyType == typeof(bool))
            {
                return true;
            }
            else if (aInfo.PropertyType == typeof(string))
            {
                return true;
            }
            //TODO: add support for other type here

            return false;
        }

        /// <summary>
        /// Update our table layout.
        /// Will instantiated every field control as defined by our action.
        /// Fields must be specified by rows from the left.
        /// </summary>
        /// <param name="aLayout"></param>
        private void UpdateTableLayoutPanel(T aObject)
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


            if (aObject == null)
            {
                //Just drop it
                return;
            }
            
            //IEnumerable<PropertyInfo> properties = aObject.GetType().GetProperties().Where(
            //    prop => Attribute.IsDefined(prop, typeof(AttributeObjectProperty)));


            foreach (PropertyInfo pi in aObject.GetType().GetProperties())
            {
                AttributeObjectProperty[] attributes = ((AttributeObjectProperty[])pi.GetCustomAttributes(typeof(AttributeObjectProperty), true));
                if (attributes.Length != 1)
                {
                    continue;
                }

                AttributeObjectProperty attribute = attributes[0];

                //Before anything we need to check if that kind of property is supported by our UI
                //Create the editor
                Control ctrl = CreateControlForProperty(pi, attribute, aObject);
                if (ctrl == null)
                {
                    //Property type not supported
                    continue;
                }

                //Add a new row
                iTableLayoutPanel.RowCount++;
                iTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                //Create the label
                Label label = new Label();
                label.AutoSize = true;
                label.Dock = DockStyle.Fill;
                label.TextAlign = ContentAlignment.MiddleCenter;
                label.Text = attribute.Name;
                toolTip.SetToolTip(label, attribute.Description);
                iTableLayoutPanel.Controls.Add(label, 0, iTableLayoutPanel.RowCount-1);

                //Add our editor to our form
                iTableLayoutPanel.Controls.Add(ctrl, 1, iTableLayoutPanel.RowCount - 1);
                //Add tooltip to editor too
                toolTip.SetToolTip(ctrl, attribute.Description);

            }        

        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            FetchPropertiesValue(Object);

            //If our object has a test method with no parameters just run it then
            MethodInfo info = Object.GetType().GetMethod("Test");
            if ( info != null && info.GetParameters().Length==0)
            {
                info.Invoke(Object,null);
            }

        }
    }
}
