using System;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace SharpDisplayManager
{
    public class RichTextBoxTextWriter : TextWriter
    {
        public delegate void WriteDelegate(char aChar);

        RichTextBox iRichTextBox = null;
        string iAccumulator = "";

        public RichTextBoxTextWriter(RichTextBox aRichTextBox)
        {
            iRichTextBox = aRichTextBox;
        }

        public override void Write(char aChar)
        {
            base.Write(aChar);
            if (iRichTextBox.InvokeRequired)
            {
                lock (iAccumulator)
                {
                    iAccumulator += aChar;
                }
                
                //WriteDelegate d = new WriteDelegate(Write);
                //iRichTextBox.Invoke(d, new object[] { aChar });
            }
            else
            {
                Flush();
                iRichTextBox.AppendText(aChar.ToString()); // When character data is written, append it to the text box.
            }            
        }

        public override Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }

        public void FlushAccumulator()
        {
            lock (iAccumulator)
            {
                if (!string.IsNullOrEmpty(iAccumulator))
                {
                    iRichTextBox.AppendText(iAccumulator); // When character data is written, append it to the text box.
                    iAccumulator = "";
                }

            }

        }
    }
}