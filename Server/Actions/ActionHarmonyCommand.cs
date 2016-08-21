using Ear = SharpLib.Ear;
using SharpLib.Ear;
using System;
using System.Collections.Generic;
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
        public string FunctionName { get; set; } = "";

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
        public override string Brief()
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

            brief += " do " + FunctionName;

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
        protected override void DoExecute()
        {
            //Fire and forget our command
            //TODO: check if the harmony client connection is opened
            if (Program.HarmonyClient!=null)
            {
                Program.HarmonyClient.SendCommandAsync(DeviceId, FunctionName);
            }
            else
            {
                Console.WriteLine("WARNING: No Harmony client connection.");
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
                                if (f.Name.Equals(FunctionName))
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


        void ClickEventHandler(object sender, EventArgs e)
        {
            FormSelectHarmonyCommand dlg = new FormSelectHarmonyCommand();
            DialogResult res = CodeProject.Dialog.DlgBox.ShowDialog(dlg);
            if (res == DialogResult.OK)
            {
                DeviceId = dlg.DeviceId;
                FunctionName = dlg.FunctionName;
                SelectCommand.Text = Brief();
                //Tell observer the object itself changed
                OnPropertyChanged("Brief");
            }

        }

    }
}
