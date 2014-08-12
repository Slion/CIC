using System;
using System.Windows.Forms;

namespace SharpDisplayManager
{
    /// <summary>
    /// Implement our display service.
    /// This class is instantiated anew whenever a client send a request.
    /// </summary>
    class DisplayServer : IDisplayService
    {
        //From IDisplayService
        public void SetText(int aLineIndex, string aText)
        {
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
