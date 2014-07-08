using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CodeProject.Dialog;

namespace SharpDisplayManager
{
    public partial class MainForm : Form
    {
        DateTime LastTickTime;
        Display iDisplay;

        public MainForm()
        {
            LastTickTime = DateTime.Now;
            iDisplay = new Display();

            InitializeComponent();
            UpdateStatus();
        }

        private void buttonFont_Click(object sender, EventArgs e)
        {
            //fontDialog.ShowColor = true;
            //fontDialog.ShowApply = true;
            fontDialog.ShowEffects = true;
            //fontDialog.ShowHelp = true;

            //fontDlg.MaxSize = 40;
            //fontDlg.MinSize = 22;

            //fontDialog.Parent = this;
            //fontDialog.StartPosition = FormStartPosition.CenterParent;

            //DlgBox.ShowDialog(fontDialog);

            //if (fontDialog.ShowDialog(this) != DialogResult.Cancel)
            if (DlgBox.ShowDialog(fontDialog) != DialogResult.Cancel)
            {

                //MsgBox.Show("MessageBox MsgBox", "MsgBox caption");

                //MessageBox.Show("Ok");
                marqueeLabelTop.Font = fontDialog.Font;
                marqueeLabelBottom.Font = fontDialog.Font;
                //label1.Font = fontDlg.Font;
                //textBox1.BackColor = fontDlg.Color;
                //label1.ForeColor = fontDlg.Color;
            }
        }

        private void buttonCapture_Click(object sender, EventArgs e)
        {
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(tableLayoutPanel.Width, tableLayoutPanel.Height);
            tableLayoutPanel.DrawToBitmap(bmp, tableLayoutPanel.ClientRectangle);
            bmp.Save("c:\\capture.png");
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            //Update our animations
            DateTime NewTickTime = DateTime.Now;

            marqueeLabelTop.UpdateAnimation(LastTickTime, NewTickTime);
            marqueeLabelBottom.UpdateAnimation(LastTickTime, NewTickTime);

            LastTickTime = NewTickTime;

            //Update our display
            if (iDisplay.IsOpen())
            {
                //Draw to bitmap
                System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(tableLayoutPanel.Width, tableLayoutPanel.Height);
                tableLayoutPanel.DrawToBitmap(bmp, tableLayoutPanel.ClientRectangle);
                //Send it to our display
                for (int i = 0; i < bmp.Width; i++)
                {
                    for (int j = 0; j < bmp.Height; j++)
                    {
                        unchecked
                        {
                        uint color=(uint)bmp.GetPixel(i, j).ToArgb();
                        iDisplay.SetPixel(i, j, Convert.ToInt32(color!=0xFFFFFFFF));
                        }
                    }
                }

                iDisplay.SwapBuffers();

            }
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            if (iDisplay.Open())
            {
                UpdateStatus();
            }
            else
            {
                UpdateStatus();
                toolStripStatusLabelConnect.Text = "Connection error";
            }

        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            iDisplay.Close();
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            iDisplay.Clear();
            iDisplay.SwapBuffers();
        }

        private void buttonFill_Click(object sender, EventArgs e)
        {
            iDisplay.Fill();
            iDisplay.SwapBuffers();
        }

        private void trackBarBrightness_Scroll(object sender, EventArgs e)
        {
            iDisplay.SetBrightness(trackBarBrightness.Value);
        }

        private void UpdateStatus()
        {
            if (iDisplay.IsOpen())
            {
                buttonFill.Enabled = true;
                buttonClear.Enabled = true;
                buttonOpen.Enabled = false;
                buttonClose.Enabled = true;
                trackBarBrightness.Enabled = true;
                trackBarBrightness.Minimum = iDisplay.MinBrightness();
                trackBarBrightness.Maximum = iDisplay.MaxBrightness();
                toolStripStatusLabelConnect.Text = "Connected";
            }
            else
            {
                buttonFill.Enabled = false;
                buttonClear.Enabled = false;
                buttonOpen.Enabled = true;
                buttonClose.Enabled = false;
                trackBarBrightness.Enabled = false;
                toolStripStatusLabelConnect.Text = "Not connected";
            }
        }

    }
}
