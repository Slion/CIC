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
        private char iLastChar='\n';

        public RichTextBoxTextWriter(RichTextBox aRichTextBox)
        {
            iRichTextBox = aRichTextBox;            
        }

        public override void Write(char aChar)
        {
            if (aChar == '\r')
            {
                //Skip
                return;
            }

            //Put our time stamp if starting a new line
            char previousChar = iLastChar;
            iLastChar = aChar;
            if (previousChar == '\n')
            {
                //Write(DateTime.Now.ToString("yyyy/MM/dd - HH:mm:ss.fff: "));
                Write(DateTime.Now.ToString("MM/dd HH:mm:ss.fff: "));
            }

            
            base.Write(aChar);
            if (iRichTextBox.InvokeRequired)
            {
                lock (iAccumulator)
                {
                    iAccumulator += aChar;
                }
                
                //Invoke was not working from here
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

        /// <summary>
        /// 
        /// </summary>
        public override void Flush()
        {
            base.Flush();

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