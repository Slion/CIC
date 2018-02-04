using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpLib.StreamDeck;

namespace CIC
{
    class StreamDeckEditor : SharpLib.StreamDeck.FormEditor
    {

        public StreamDeckEditor(string aFilename):base(aFilename)
        {
        }

        public override void StreamDeckKeyPressed(object sender, KeyEventArgs e)
        {
            // Trigger associated event
            SharpDisplayManager.Properties.Settings.Default.EarManager.TriggerEventsByName(KeyForIndex(e.Key).EventName);
        }

    }
}
