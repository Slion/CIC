using Ear = SharpLib.Ear;
using SharpLib.Ear;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharpDisplayManager
{


    [DataContract]
    [AttributeObject(Id = "Display.Message", Name = "Display Message", Description = "Shows a message on your internal display.")]
    class ActionDisplayMessage : Ear.Action
    {
        [DataMember]
        [AttributeObjectProperty(
            Id = "Display.Message.Duration",
            Name = "Duration (ms)",
            Description = "Specifies the number of milliseconds this message should be displayed.",
            Minimum = "1", //Otherwise time throws an exception
            Maximum = "30000",
            Increment = "1000"
            )]
        public int DurationInMilliseconds { get; set; } = 5000;


        [DataMember]
        [AttributeObjectProperty(
            Id = "Display.Message.PrimaryText",
            Name = "Primary Text",
            Description = "The primary text of this message."
            )]
        public string PrimaryText { get; set; } = "Your message";

        [DataMember]
        [AttributeObjectProperty(
            Id = "Display.Message.SecondaryText",
            Name = "Secondary Text",
            Description = "The secondary text of this message."
            )]
        public string SecondaryText { get; set; } = "";


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string Brief()
        {
            string brief = Name + ": " + PrimaryText;
            if (!string.IsNullOrEmpty(SecondaryText))
            {
                brief += " - " + SecondaryText;
            }

            brief += " ( " + DurationInMilliseconds.ToString() + " ms )";

            return brief;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override async Task DoExecute()
        {
            StartMessageClient();
        }

        /// <summary>
        /// Just launch our idle client.
        /// </summary>
        private void StartMessageClient()
        {
            Thread clientThread = new Thread(SharpDisplayClientMessage.Program.MainWithParams);
            SharpDisplayClientMessage.StartParams myParams =
                new SharpDisplayClientMessage.StartParams(PrimaryText, SecondaryText, DurationInMilliseconds);
            clientThread.Start(myParams);
        }


    }

}
