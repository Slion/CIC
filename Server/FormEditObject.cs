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
using System.IO;

namespace SharpDisplayManager
{
    /// <summary>
    /// Object edit dialog form.
    /// </summary>
    public partial class FormEditObject<T> : Form where T: SharpLib.Ear.Object
    {
        public T Object = null;

        public FormEditObject()
        {
            InitializeComponent();
#if DEBUG
            // Show border in debug mode
            iTableLayoutPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormEditObject_Load(object sender, EventArgs e)
        {
            // Disable trigger until we are done editing
            Properties.Settings.Default.EarManager.CurrentState = SharpLib.Ear.Object.State.Edit;

            // Populate registered object types
            IEnumerable < Type > types = Reflection.GetConcreteClassesDerivedFrom<T>();
            foreach (Type type in types)
            {
                ItemObjectType item = new ItemObjectType(type);
                iComboBoxObjectType.Items.Add(item);
            }

            if (Object == null)
            {
                // Creating new issue, select our first item
                iComboBoxObjectType.SelectedIndex = 0;
            }
            else
            {
                // Editing existing object
                // Look up our item in our object type combobox
                foreach (ItemObjectType item in iComboBoxObjectType.Items)
                {
                    if (item.Type == Object.GetType())
                    {
                        iComboBoxObjectType.SelectedItem = item;
                    }
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOk_Click(object sender, EventArgs e)
        {
            FetchPropertiesValue(Object);
            if (!Object.IsValid())
            {
                // Tell closing event to cancel
                DialogResult = DialogResult.None;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            //FetchPropertiesValue(Object);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormEditObject_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Check if we need to cancel the closing of our form.
            e.Cancel = DialogResult == DialogResult.None;

            if (!e.Cancel)
            {
                //Exit edit mode                
                Object.CurrentState = SharpLib.Ear.Object.State.Rest;
                Object.PropertyChanged -= PropertyChangedEventHandlerThreadSafe;
                // Make sure we enable event triggers again
                Properties.Settings.Default.EarManager.CurrentState = SharpLib.Ear.Object.State.Rest;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iComboBoxObjectType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Instantiate an action corresponding to our type
            Type objectType = ((ItemObjectType) iComboBoxObjectType.SelectedItem).Type;
            //Create another type of action only if needed
            if (Object == null || Object.GetType() != objectType)
            {
                string name = "";
                if (Object != null)
                {
                    // Make sure we exit edit mode and unhook from events
                    Object.CurrentState = SharpLib.Ear.Object.State.Rest;
                    Object.PropertyChanged -= PropertyChangedEventHandlerThreadSafe;
                    name = Object.Name;
                    Object = null;
                }
                Object = (T)Activator.CreateInstance(objectType);
                //Keep the name when changing the type
                Object.Name = name;
            }

            //Create input fields
            UpdateControls();
        }


        /// <summary>
        /// Get properties values from our generated input fields
        /// </summary>
        private void FetchPropertiesValue(T aObject)
        {
            int ctrlIndex = 0;
            //For each of our properties
            foreach (PropertyInfo pi in aObject.GetType().GetProperties())
            {
                //Get our property attribute
                AttributeObjectProperty[] attributes = ((AttributeObjectProperty[]) pi.GetCustomAttributes(typeof(AttributeObjectProperty), true));
                if (attributes.Length != 1)
                {
                    //No attribute, skip this property then.
                    continue;
                }
                AttributeObjectProperty attribute = attributes[0];

                //Check that we support this type of property
                if (!IsPropertyTypeSupported(pi))
                {
                    continue;
                }

                //Now fetch our property value
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
            else if (aInfo.PropertyType == typeof(float))
            {
                NumericUpDown ctrl = (NumericUpDown)aControl;
                aInfo.SetValue(aObject, (float)ctrl.Value);
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
            else if (aInfo.PropertyType == typeof(PropertyFile))
            {
                Button ctrl = (Button)aControl;
                PropertyFile value = new PropertyFile {FullPath=ctrl.Text};
                aInfo.SetValue(aObject, value);
            }
            else if (aInfo.PropertyType == typeof(PropertyComboBox))
            {
                ComboBox ctrl = (ComboBox)aControl;
                if (ctrl.SelectedItem != null)
                {
                    string currentItem = ctrl.SelectedItem.ToString();
                    PropertyComboBox value = (PropertyComboBox)aInfo.GetValue(aObject);
                    value.CurrentItem = currentItem;
                    //Not strictly needed but makes sure the set method is called
                    aInfo.SetValue(aObject, value);
                    //
                    aObject.OnPropertyChanged(aInfo.Name);
                }
            }
            else if (aInfo.PropertyType == typeof(PropertyButton))
            {
                Button ctrl = (Button)aControl;
                PropertyButton value = new PropertyButton { Text = ctrl.Text };
                aInfo.SetValue(aObject, value);
            }
            else if (aInfo.PropertyType == typeof(PropertyCheckedListBox))
            {
                CheckedListBox ctrl = (CheckedListBox)aControl;
                PropertyCheckedListBox value = (PropertyCheckedListBox)aInfo.GetValue(aObject);
                List<string> checkedItems = new List<string>();                
                foreach (string item in ctrl.CheckedItems)
                {
                    checkedItems.Add(item);
                }
                value.CheckedItems = checkedItems;

                //value.CurrentItem = currentItem;
                //Not strictly needed but makes sure the set method is called
                aInfo.SetValue(aObject, value);
                //
                //aObject.OnPropertyChanged(aInfo.Name);
            }

            //TODO: add support for other types here
        }


        /// <summary>
        /// Create a control for the given property.
        /// TODO: Handle cases where a property value is null. That can be the case when extending an existing class and loading it from an older save.
        /// Though I guess that should really be taken care of in Ear.Object.Construct which is called once the object was internalized.
        /// </summary>
        /// <param name="aInfo"></param>
        /// <param name="aAttribute"></param>
        /// <param name="aObject"></param>
        /// <returns></returns>
        private Control CreateControlForProperty(PropertyInfo aInfo, AttributeObjectProperty aAttribute, T aObject)
        {
            if (aInfo.PropertyType == typeof(int) || aInfo.PropertyType == typeof(float))
            {
                //Integer properties are using numeric editor
                NumericUpDown ctrl = new NumericUpDown();
                ctrl.AutoSize = true;
                //ctrl.Dock = DockStyle.Fill; // Fill the whole table cell
                ctrl.Minimum = (decimal)aAttribute.Minimum;
                ctrl.Maximum = (decimal)aAttribute.Maximum;
                ctrl.Increment = (decimal)aAttribute.Increment;
                ctrl.DecimalPlaces = aAttribute.DecimalPlaces;
                ctrl.Value = decimal.Parse(aInfo.GetValue(aObject).ToString());                
                // Hook-in change notification after setting the value 
                ctrl.ValueChanged += ControlValueChanged;
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
                Size cbSize = new Size(0, 0);
                foreach (string name in aInfo.PropertyType.GetEnumNames())
                {
                    ctrl.Items.Add(name.ToString());
                    Graphics g = this.CreateGraphics();
                    //Since combobox autosize would not work we need to measure text ourselves
                    SizeF size = g.MeasureString(name.ToString(), ctrl.Font);
                    cbSize.Width = Math.Max(cbSize.Width, (int)size.Width);
                    cbSize.Height = Math.Max(cbSize.Height, (int)size.Height);
                }

                //Make sure our combobox is large enough
                ctrl.MinimumSize = cbSize;

                // Instantiate our enum
                object enumValue = Activator.CreateInstance(aInfo.PropertyType);
                enumValue = aInfo.GetValue(aObject);
                //Set the current item
                ctrl.SelectedItem = enumValue.ToString();
                // Hook-in change notification after setting the value 
                ctrl.SelectedIndexChanged += ControlValueChanged;

                return ctrl;
            }
            else if (aInfo.PropertyType == typeof(bool))
            {
                CheckBox ctrl = new CheckBox();
                ctrl.AutoSize = true;
                ctrl.Text = aAttribute.Description;
                ctrl.Checked = (bool)aInfo.GetValue(aObject);
                // Hook-in change notification after setting the value 
                ctrl.CheckedChanged += ControlValueChanged;
                return ctrl;
            }
            else if (aInfo.PropertyType == typeof(string))
            {
                TextBox ctrl = new TextBox();
                ctrl.AutoSize = true;
                ctrl.Dock = DockStyle.Fill; // Fill the whole table cell
                ctrl.Text = (string)aInfo.GetValue(aObject);

                // Multiline setup
                ctrl.Multiline = aAttribute.Multiline;
                if (ctrl.Multiline)
                {
                    ctrl.AcceptsReturn = true;
                    ctrl.ScrollBars = ScrollBars.Vertical;
                    // Adjust our height as needed
                    Size size = ctrl.Size;
                    size.Height += ctrl.Font.Height * (aAttribute.HeightInLines - 1);
                    ctrl.MinimumSize = size;
                }

                // Hook-in change notification after setting the value 
                ctrl.TextChanged += ControlValueChanged;
                return ctrl;
            }
            else if (aInfo.PropertyType == typeof(PropertyFile))
            {
                // We have a file property
                // Create a button that will trigger the open file dialog to select our file.
                Button ctrl = new Button();
                ctrl.AutoSize = true;
                ctrl.Text = ((PropertyFile)aInfo.GetValue(aObject)).FullPath;
                // Add lambda expression to Click event
                ctrl.Click += (sender, e) =>
                {
                    // Create open file dialog
                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.RestoreDirectory = true;
                    // Use file filter specified by our property
                    ofd.Filter = aAttribute.Filter;
                    // Show our dialog
                    if (SharpLib.Forms.DlgBox.ShowDialog(ofd) == DialogResult.OK)
                    {
                        // Fetch selected file name
                        ctrl.Text = ofd.FileName;
                    }
                };

                // Hook-in change notification after setting the value 
                ctrl.TextChanged += ControlValueChanged;
                return ctrl;
            }
            else if (aInfo.PropertyType == typeof(PropertyComboBox))
            {
                //ComboBox property
                PropertyComboBox property = ((PropertyComboBox)aInfo.GetValue(aObject));

                ComboBox ctrl = new ComboBox();
                ctrl.AutoSize = true;
                ctrl.Sorted = property.Sorted;
                ctrl.DropDownStyle = ComboBoxStyle.DropDownList;
                //Data source is such a pain to set the current item
                //ctrl.DataSource = ((PropertyComboBox)aInfo.GetValue(aObject)).Items;                

                Size cbSize = new Size(0, 0);
                foreach (string item in property.Items)
                {
                    ctrl.Items.Add(item);
                    Graphics g = this.CreateGraphics();
                    //Since combobox autosize would not work we need to measure text ourselves
                    SizeF size = g.MeasureString(item, ctrl.Font);
                    cbSize.Width = Math.Max(ctrl.Width, (int)size.Width);
                    cbSize.Height = Math.Max(ctrl.Height, (int)size.Height);
                }

                //Make sure our combobox is large enough
                // TODO: Check why that still won't be large enough for our HID devices
                ctrl.MinimumSize = cbSize;

                ctrl.SelectedItem = property.CurrentItem;

                // Hook-in change notification after setting the value 
                // Make sure form content is updated after property change
                ctrl.SelectedIndexChanged += ControlValueChanged;
                return ctrl;
            }
            else if (aInfo.PropertyType == typeof(PropertyButton))
            {
                // We have a button property
                // Create a button that will trigger the custom action.
                Button ctrl = new Button();
                ctrl.AutoSize = true;
                ctrl.Text = ((PropertyButton)aInfo.GetValue(aObject)).Text;
                // Hook in click event
                ctrl.Click += ((PropertyButton)aInfo.GetValue(aObject)).ClickEventHandler;
                // Hook-in change notification after setting the value 
                ctrl.TextChanged += ControlValueChanged;
                return ctrl;
            }
            else if (aInfo.PropertyType == typeof(PropertyCheckedListBox))
            {
                //CheckedListBox property
                PropertyCheckedListBox property = ((PropertyCheckedListBox)aInfo.GetValue(aObject));
                
                CheckedListBox ctrl = new CheckedListBox();
                ctrl.AutoSize = true;
                ctrl.Sorted = property.Sorted;
                ctrl.CheckOnClick = true;          

                // Populate our box with list items
                foreach (string item in property.Items)
                {
                    int index = ctrl.Items.Add(item);
                    // Check items if needed
                    if (property.CheckedItems.Contains(item))
                    {
                        ctrl.SetItemChecked(index,true);
                    }
                }
                //
                // Hook-in change notification after setting the value 
                // That's essentially making sure title/brief gets updated as items are checked or unchecked
                // This looks convoluted as we are working around the fact that ItemCheck event is triggered before the model is updated.
                // See: https://stackoverflow.com/a/48645552/3969362
                ctrl.ItemCheck += (s, e) => BeginInvoke((MethodInvoker)(() => ControlValueChanged(s, e)));
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
            else if (aInfo.PropertyType == typeof(float))
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
            else if (aInfo.PropertyType == typeof(PropertyFile))
            {
                return true;
            }
            else if (aInfo.PropertyType == typeof(PropertyComboBox))
            {
                return true;
            }
            else if (aInfo.PropertyType == typeof(PropertyButton))
            {
                return true;
            }
            else if (aInfo.PropertyType == typeof(PropertyCheckedListBox))
            {
                return true;
            }
            
            //TODO: add support for other type here

            return false;
        }

        /// <summary>
        /// Update our table layout.
        /// Will instantiated every field control as defined by our object.
        /// </summary>
        /// <param name="aLayout"></param>
        private void UpdateControls()
        {
            iTableLayoutPanel.SuspendLayout(); // Avoid flicker   

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


            if (Object == null)
            {                
                iTableLayoutPanel.ResumeLayout(true);
                //Just drop it
                return;
            }

            // Tell our object to prepare for edit
            Object.CurrentState = SharpLib.Ear.Object.State.PrepareEdit;
            Object.PropertyChanged -= PropertyChangedEventHandlerThreadSafe; // Most important to avoid accumulating handlers
            //
            UpdateStaticControls();

            //IEnumerable<PropertyInfo> properties = aObject.GetType().GetProperties().Where(
            //    prop => Attribute.IsDefined(prop, typeof(AttributeObjectProperty)));

            //TODO: Do this whenever a field changes
            iLabelBrief.Text = Object.Brief();


            foreach (PropertyInfo pi in Object.GetType().GetProperties())
            {
                AttributeObjectProperty[] attributes = ((AttributeObjectProperty[])pi.GetCustomAttributes(typeof(AttributeObjectProperty), true));
                if (attributes.Length != 1)
                {
                    continue;
                }

                AttributeObjectProperty attribute = attributes[0];

                //Before anything we need to check if that kind of property is supported by our UI
                //Create the editor
                Control ctrl = CreateControlForProperty(pi, attribute, Object);
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

            //Enter object edit mode
            Object.CurrentState = SharpLib.Ear.Object.State.Edit;
            Object.PropertyChanged += PropertyChangedEventHandlerThreadSafe;

            iTableLayoutPanel.ResumeLayout(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PropertyChangedEventHandlerThreadSafe(object sender, PropertyChangedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                //Not in the proper thread, invoke ourselves
                PropertyChangedEventHandler d = new PropertyChangedEventHandler(PropertyChangedEventHandlerThreadSafe);
                this.Invoke(d, new object[] { sender, e });
            }
            else
            {               
                // We could test the name of the property that has changed as follow
                // It's currently not needed though
                //if (e.PropertyName == "Brief")

                // Our object has changed behind our back.
                // That's currently only the case for HID events that are listening for inputs.
                //if (Object is EventHid)
                //{
                    //HID can't do full control updates for some reason
                    //We are getting spammed with HID events after a few clicks
                    //We need to investigate, HID bug?
                    //TODO: Bug mentioned above possibly fixed
                    // I guess it was due to accumulation of Object.PropertyChanged handler
                    // Considerer removing all HID specific stuff from this class
                    UpdateStaticControls();
                //}
                //else
                //{
                      // That would only be needed if changing a property would change our object editor layout
                      // I've retired that for now as I don't think that's ever needed. I could be wrong though.
                      // If ever needed again I guess we should mark our property with a special attribute flag.
                      // Try avoiding it altogether as it's rather slow and causes flicker.
                //    UpdateControls();
                //}
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ControlValueChanged(object sender, EventArgs e)
        {
            UpdateObject();
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateObject()
        {
            // Update our object with the content of our controls
            FetchPropertiesValue(Object);

            UpdateStaticControls();
            //
            //PerformLayout();
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateStaticControls()
        {
            // Update OK and test button status
            iButtonOk.Enabled = Object.IsValid();
            iButtonTest.Enabled = iButtonOk.Enabled;

            // Update brief title
            iLabelBrief.Text = Object.Brief();

            // Update object description
            iLabelDescription.Text = Object.AttributeDescription;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iComboBoxObjectType_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Special case for HID events
            if (Object is EventHid)
            {
                //Disable handling of key input as we are using key input for changing our event
                e.Handled = true;
            }
        }

        private void iComboBoxObjectType_Enter(object sender, EventArgs e)
        {
            //Only edit HID event when our type combo box has the focus
            // TODO: That's an ugly workaround, fix that somehow. Maybe by only doing HID scan when a button property has the focus
            if (Object is EventHid)
            {
                Object.CurrentState = SharpLib.Ear.Object.State.Edit;
            }
        }

        private void iComboBoxObjectType_Leave(object sender, EventArgs e)
        {
            //Only edit HID event when our type combo box has the focus
            // TODO: That's an ugly workaround, fix that somehow. Maybe by only doing HID scan when a button property has the focus
            if (Object is EventHid)
            {
                Object.CurrentState = SharpLib.Ear.Object.State.Rest;
            }
        }

    }
}
