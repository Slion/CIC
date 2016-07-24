using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpLib.Ear;

namespace SharpDisplayManager
{
    /// <summary>
    /// Used to populate our action type combobox with friendly names
    /// </summary>
    class ItemActionType
    {
        public Type Type;

        public ItemActionType(Type type)
        {
            this.Type = type;
        }

        public override string ToString()
        {
            //Get friendly action name from action attribute.
            //That we then show up in our combobox
            return SharpLib.Utils.Reflection.GetAttribute<AttributeAction>(Type).Name;
        }
    }



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
            Action = (SharpLib.Ear.Action)Activator.CreateInstance(((ItemActionType)comboBoxActionType.SelectedItem).Type);
        }

        private void FormEditAction_Validating(object sender, CancelEventArgs e)
        {

        }
    }
}
