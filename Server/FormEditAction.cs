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
                Type t = ManagerEventAction.Current.ActionTypes[key];
                comboBoxActionType.Items.Add(t);                
            }

            comboBoxActionType.SelectedIndex = 0;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            Action = (SharpLib.Ear.Action)Activator.CreateInstance((Type)comboBoxActionType.SelectedItem);
        }

        private void FormEditAction_Validating(object sender, CancelEventArgs e)
        {

        }
    }
}
