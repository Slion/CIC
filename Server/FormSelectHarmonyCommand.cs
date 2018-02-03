using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpDisplayManager
{
    public partial class FormSelectHarmonyCommand : Form
    {
        public FormSelectHarmonyCommand()
        {
            InitializeComponent();
        }

        public string DeviceId;
        public string Command;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormSelectHarmonyCommand_Load(object sender, EventArgs e)
        {
            if (Program.HarmonyConfig != null)
            {
                PopulateTreeViewHarmony(Program.HarmonyConfig);
            }
            else
            {
                SharpLib.Forms.ErrBox.Show("No Harmony Hub configuration!");
                Close();
            }           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aConfig"></param>
        private void PopulateTreeViewHarmony(HarmonyHub.Config aConfig)
        {
            iTreeViewHarmony.Nodes.Clear();
            //Add our devices
            foreach (HarmonyHub.Device device in aConfig.Devices)
            {
                TreeNode deviceNode = iTreeViewHarmony.Nodes.Add(device.Id, $"{device.Label} ({device.DeviceTypeDisplayName}/{device.Model})");
                deviceNode.Tag = device;

                foreach (HarmonyHub.ControlGroup cg in device.ControlGroups)
                {
                    TreeNode cgNode = deviceNode.Nodes.Add(cg.Name);
                    cgNode.Tag = cg;

                    foreach (HarmonyHub.Function f in cg.Functions)
                    {
                        TreeNode fNode = cgNode.Nodes.Add(f.Label);
                        fNode.Tag = f;
                    }
                }
            }

            //treeViewConfig.ExpandAll();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void iTreeViewHarmony_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //Upon function node double click we execute it, so that user can test
            var tag = e.Node.Tag as HarmonyHub.Function;
            if (tag != null && e.Node.Parent.Parent.Tag is HarmonyHub.Device)
            {
                HarmonyHub.Function f = tag;
                HarmonyHub.Device d = (HarmonyHub.Device)e.Node.Parent.Parent.Tag;

                Trace.WriteLine($"Harmony: Sending {f.Label} to {d.Label}...");

                await Program.HarmonyClient.TrySendKeyPressAsync(d.Id, f.Action.Command);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iTreeViewHarmony_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //Enable ok button if a function is selected
            iButtonOk.Enabled = e.Node.Tag is HarmonyHub.Function;

            // Get selected device ID and function name
            HarmonyHub.Function f = e.Node.Tag as HarmonyHub.Function;
            if (f != null && e.Node.Parent.Parent.Tag is HarmonyHub.Device)
            {
                HarmonyHub.Device d = (HarmonyHub.Device)e.Node.Parent.Parent.Tag;

                DeviceId = d.Id;
                Command = f.Action.Command;
            }
            else
            {
                DeviceId = "";
                Command = "";
            }

        }
    }
}
