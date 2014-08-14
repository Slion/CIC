using System;
using System.Windows.Forms;
using System.Collections;
using System.ServiceModel;
using System.Collections.Generic;
using System.Linq;
using SharpDisplayInterface;

namespace SharpDisplayManager
{
    /// <summary>
    /// Implement our display service.
    /// This class is instantiated anew whenever a client send a request.
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    class DisplayServer : IDisplayService
    {
        //From IDisplayService
        public void SetTexts(System.Collections.Generic.IList<string> aTexts)
        {
            //Only support two lines for now
            for (int i=0; i<aTexts.Count; i++)
            {
                if (i == 0)
                {
                    Program.iMainForm.marqueeLabelTop.Text = aTexts[i];
                }
                else if (i == 1)
                {
                    Program.iMainForm.marqueeLabelBottom.Text = aTexts[i];
                }
            }
        }

        //
        public void SetText(int aLineIndex, string aText)
        {
            //Only support two lines for now
                if (aLineIndex == 0)
                {
                    Program.iMainForm.marqueeLabelTop.Text = aText;
                }
                else if (aLineIndex == 1)
                {
                    Program.iMainForm.marqueeLabelBottom.Text = aText;
                }
        }

        //
        public void Connect(string aClientName)
        {
            Disconnect(aClientName);

            //Register our client and its callback interface
            IDisplayServiceCallback callback = OperationContext.Current.GetCallbackChannel<IDisplayServiceCallback>();
            Program.iMainForm.iClients.Add(aClientName, callback);

            //For some reason MP still hangs on that one
            //callback.OnConnected();
        }

        ///
        public void Disconnect(string aClientName)
        {
            //remove the old client if any
            if (Program.iMainForm.iClients.Keys.Contains(aClientName))
            {
                Program.iMainForm.iClients.Remove(aClientName);
            }

        }


    }

}
