using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Diagnostics;
using SharpDisplay;


namespace SharpDisplayClient
{
    public partial class MainForm : Form
    {
        Client iClient;
        Callback iCallback;
        ContentAlignment Alignment;
        TextField iTextFieldTop;

        public MainForm()
        {
            InitializeComponent();
            Alignment = ContentAlignment.MiddleLeft;
            iTextFieldTop = new TextField(0);
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            iCallback = new Callback(this);
            iClient = new Client(iCallback);

            //Connect using unique name
            //string name = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt");
            string name = "Client-" + (iClient.ClientCount() - 1);
            iClient.SetName(name);
            //Text = Text + ": " + name;
            Text = "[[" + name + "]]  " + iClient.SessionId;

            //
            textBoxTop.Text = iClient.Name;
            textBoxBottom.Text = iClient.SessionId;

        }



        public delegate void CloseConnectionDelegate();
        public delegate void CloseDelegate();

        /// <summary>
        ///
        /// </summary>
        public void CloseConnectionThreadSafe()
        {
            if (this.InvokeRequired)
            {
                //Not in the proper thread, invoke ourselves
                CloseConnectionDelegate d = new CloseConnectionDelegate(CloseConnectionThreadSafe);
                this.Invoke(d, new object[] { });
            }
            else
            {
                //We are in the proper thread
                if (IsClientReady())
                {
                    Trace.TraceInformation("Closing client: " + iClient.SessionId);
                    iClient.Close();
                    Trace.TraceInformation("Closed client: " + iClient.SessionId);
                }

                iClient = null;
                iCallback = null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public void CloseThreadSafe()
        {
            if (this.InvokeRequired)
            {
                //Not in the proper thread, invoke ourselves
                CloseDelegate d = new CloseDelegate(CloseThreadSafe);
                this.Invoke(d, new object[] { });
            }
            else
            {
                //We are in the proper thread
                Close();
            }
        }


        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseConnectionThreadSafe();
        }

        public bool IsClientReady()
        {
            return (iClient != null && iClient.State == CommunicationState.Opened);
        }

        private void buttonAlignLeft_Click(object sender, EventArgs e)
        {
            Alignment = ContentAlignment.MiddleLeft;
            textBoxTop.TextAlign = HorizontalAlignment.Left;
            textBoxBottom.TextAlign = HorizontalAlignment.Left;
        }

        private void buttonAlignCenter_Click(object sender, EventArgs e)
        {
            Alignment = ContentAlignment.MiddleCenter;
            textBoxTop.TextAlign = HorizontalAlignment.Center;
            textBoxBottom.TextAlign = HorizontalAlignment.Center;
        }

        private void buttonAlignRight_Click(object sender, EventArgs e)
        {
            Alignment = ContentAlignment.MiddleRight;
            textBoxTop.TextAlign = HorizontalAlignment.Right;
            textBoxBottom.TextAlign = HorizontalAlignment.Right;
        }

        private void buttonSetTopText_Click(object sender, EventArgs e)
        {
            //TextField top = new TextField(0, textBoxTop.Text, ContentAlignment.MiddleLeft);
            iTextFieldTop.Text = textBoxTop.Text;
            iTextFieldTop.Alignment = Alignment;
            iClient.SetText(iTextFieldTop);
        }

        private void buttonSetText_Click(object sender, EventArgs e)
        {
            //iClient.SetText(0,"Top");
            //iClient.SetText(1, "Bottom");
            //TextField top = new TextField(0, textBoxTop.Text, ContentAlignment.MiddleLeft);

            iClient.SetTexts(new TextField[]
            {
                new TextField(0, textBoxTop.Text, Alignment),
                new TextField(1, textBoxBottom.Text, Alignment)
            });
        }

        private void buttonLayoutUpdate_Click(object sender, EventArgs e)
        {
            //Define a 2 by 2 layout
            TableLayout layout = new TableLayout(2,2);
            //Second column only takes up 25%
            layout.Columns[1].Width = 25F;
            //Send layout to server
            iClient.SetLayout(layout);
        }

        private void buttonSetBitmap_Click(object sender, EventArgs e)
        {
            int x1 = 0;
            int y1 = 0;
            int x2 = 256;
            int y2 = 32;

            Bitmap bitmap = new Bitmap(x2,y2);
            Pen blackPen = new Pen(Color.Black, 3);

            // Draw line to screen.
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.DrawLine(blackPen, x1, y1, x2, y2);
                graphics.DrawLine(blackPen, x1, y2, x2, y1);
            }

            BitmapField field = new BitmapField(0, bitmap);
            field.ColumnSpan = 2;
            iClient.SetBitmap(field);
        }

        private void buttonBitmapLayout_Click(object sender, EventArgs e)
        {
            SetLayoutWithBitmap();
        }

        /// <summary>
        /// Define a layout with a bitmap field on the left and two lines of text on the right.
        /// </summary>
        private void SetLayoutWithBitmap()
        {
            //Define a 2 by 2 layout
            TableLayout layout = new TableLayout(2, 2);
            //First column only takes 25%
            layout.Columns[0].Width = 25F;
            //Second column takes up 75% 
            layout.Columns[1].Width = 75F;
            //Send layout to server
            iClient.SetLayout(layout);

            //Set a bitmap for our first field
            int x1 = 0;
            int y1 = 0;
            int x2 = 64;
            int y2 = 64;

            Bitmap bitmap = new Bitmap(x2, y2);
            Pen blackPen = new Pen(Color.Black, 3);

            // Draw line to screen.
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.DrawLine(blackPen, x1, y1, x2, y2);
                graphics.DrawLine(blackPen, x1, y2, x2, y1);
            }

            //Create a bitmap field from the bitmap we just created
            BitmapField field = new BitmapField(0, bitmap);
            //We want our bitmap field to span across two rows
            field.RowSpan = 2;
            //Send it to our server
            iClient.SetBitmap(field);

            //Set texts
            iClient.SetTexts(new TextField[]
            {
                new TextField(1, textBoxTop.Text, Alignment),
                new TextField(2, textBoxBottom.Text, Alignment)
            });

        }
    }
}
