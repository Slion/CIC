using System;
using System.Windows.Forms;
using System.Collections;

namespace SharpDisplayManager
{
    /// <summary>
    /// Implement our display service.
    /// This class is instantiated anew whenever a client send a request.
    /// </summary>
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

    }

}
