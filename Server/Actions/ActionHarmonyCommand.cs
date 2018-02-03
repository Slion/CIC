using Ear = SharpLib.Ear;
using SharpLib.Ear;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace SharpDisplayManager
{
    [DataContract]
    [AttributeObject(Id = "Harmony.Command", Name = "Harmony Command", Description = "Send a command to your Logitech Harmony Hub.")]
    class ActionHarmonyCommand : Ear.Action
    {
        [DataMember]
        public string DeviceId { get; set; } = "";

        [DataMember]
        public string Command { get; set; } = "";

        [DataMember]
        [AttributeObjectProperty(
        Id = "Harmony.Command.SelectCommand",
        Name = "Select command",
        Description = "Click to select a command."
        )]
        public PropertyButton SelectCommand { get; set; } = new PropertyButton { Text = "None" };

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string BriefBase()
        {
            string brief="Harmony: ";

            if (Program.HarmonyConfig != null)
            {
                //What if the device ID is not there anymore?
                brief += Program.HarmonyConfig.DeviceNameFromId(DeviceId);
            }
            else
            {
                //No config found just show the device ID then.
                brief += DeviceId;
            }

            // TODO: Fetch function label from command
            brief += " do " + Command;

            return brief;
        }


        protected override void DoConstruct()
        {
            base.DoConstruct();

            if (SelectCommand == null)
            {
                SelectCommand = new PropertyButton { Text = "None"};
            }
            SelectCommand.ClickEventHandler = ClickEventHandler;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override async Task DoExecute(Context aContext)
        {
            if (Program.HarmonyClient!=null)
            {
                // Send our command and wait for it async
                await Program.HarmonyClient.TrySendKeyPressAsync(DeviceId, Command);               
            }
            else
            {
                Trace.WriteLine("WARNING: No Harmony client connection.");
            }            
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
            if (Program.HarmonyConfig != null)
            {
                foreach (HarmonyHub.Device d in Program.HarmonyConfig.Devices)
                {
                    if (d.Id.Equals(DeviceId))
                    {
                        foreach (HarmonyHub.ControlGroup cg in d.ControlGroups)
                        {
                            foreach (HarmonyHub.Function f in cg.Functions)
                            {
                                if (f.Action.Command.Equals(Command))
                                {
                                    //We found our device and our function
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ClickEventHandler(object sender, EventArgs e)
        {
            if (Program.HarmonyConfig == null)
            {
                SharpLib.Forms.ErrBox.Show("No Harmony Hub configuration!");
                return;
            }

            FormSelectHarmonyCommand dlg = new FormSelectHarmonyCommand();
            DialogResult res = SharpLib.Forms.DlgBox.ShowDialog(dlg);
            if (res == DialogResult.OK)
            {
                DeviceId = dlg.DeviceId;
                Command = dlg.Command;
                SelectCommand.Text = Brief();
                //Tell observer the object itself changed
                OnPropertyChanged("Brief");
            }

        }

    }
}
