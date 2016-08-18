using Ear = SharpLib.Ear;
using SharpLib.Ear;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SharpDisplayManager
{
    [DataContract]
    [AttributeObject(Id = "Harmony.Command", Name = "Harmony Command", Description = "Send a command to your Logitech Harmony Hub.")]
    class ActionHarmonyCommand : Ear.Action
    {
        [DataMember]
        [AttributeObjectProperty(
            Id = "Harmony.Command.DeviceId",
            Name = "Device ID",
            Description = "The ID of the device this command is associated with."
        )]
        public string DeviceId { get; set; } = "";


        [DataMember]
        [AttributeObjectProperty(
        Id = "Harmony.Command.FunctionName",
        Name = "Function Name",
        Description = "The name of the function defining this command."
        )]
        public string FunctionName { get; set; } = "";

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

    }
}
