using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Timers;
using System.Windows.Forms;
using System.Drawing;

namespace SharpDisplayManager
{
    class MarqueeLabel : Label
    {
        private bool iOwnTimer;
        private StringFormat iStringFormat;
        private SolidBrush iBrush;
        private SizeF iTextSize;
        private SizeF iSeparatorSize;

        [Category("Appearance")]
        [Description("Separator in our scrolling loop.")]
        [DefaultValue(" | ")]
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public string Separator { get; set; }

        [Category("Behavior")]
        [Description("How fast is our text scrolling, in pixels per second.")]
        [DefaultValue(32)]
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public int PixelsPerSecond { get; set; }

        [Category("Behavior")]
        [Description("Use an internal or an external timer.")]
        [DefaultValue(true)]
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public bool OwnTimer
        {
            get
            {
                return iOwnTimer;
            }
            set
            {
                iOwnTimer = value;

                if (iOwnTimer)
                {
                    Timer = new Timer();
                    Timer.Interval = 10;
                    Timer.Tick += new EventHandler(Timer_Tick);
                    Timer.Start();
                }
                else
                {
                    if (Timer != null)
                        Timer.Dispose();
                    Timer = null;
                }

            }
        }

        private int CurrentPosition { get; set; }
        private Timer Timer { get; set; }
        private DateTime LastTickTime { get; set; }
        private double PixelsLeft { get; set; }
        //DateTime a = new DateTime(2010, 05, 12, 13, 15, 00);
        //DateTime b = new DateTime(2010, 05, 12, 13, 45, 00);
        //Console.WriteLine(b.Subtract(a).TotalMinutes);

        public MarqueeLabel()
        {
            UseCompatibleTextRendering = true;
            //PixelsPerSecond = 32;
            LastTickTime = DateTime.Now;
            PixelsLeft = 0;
            iBrush = new SolidBrush(ForeColor);
        }

        public void UpdateAnimation(DateTime aLastTickTime, DateTime aNewTickTime)
        {
            if (!NeedToScroll())
            {
                CurrentPosition = 0;
                return;
            }

            while (CurrentPosition > (iTextSize.Width + iSeparatorSize.Width))
            {
                CurrentPosition -= ((int)(iTextSize.Width + iSeparatorSize.Width));
            }

            PixelsLeft += aNewTickTime.Subtract(aLastTickTime).TotalSeconds * PixelsPerSecond;

            //Keep track of our pixels left over
            //PixelsLeft = offset - Math.Truncate(offset);
            double offset = Math.Truncate(PixelsLeft);
            PixelsLeft -= offset;

            CurrentPosition += Convert.ToInt32(offset);

            /*
            if (offset > 1.0)
            {
                BackColor = Color.Red;
            }
            else if (offset==1.0)
            {
                if (BackColor != Color.White)
                {
                    BackColor = Color.White;
                }

            }
            else
            {
                //Too slow
                //BackColor = Color.Green;
            }*/

            //Only redraw if something has changed
            if (offset != 0)
            {
                Invalidate();
            }
        }

        void Timer_Tick(object sender, EventArgs e)
        {
            DateTime NewTickTime = DateTime.Now;
            //
            UpdateAnimation(LastTickTime, NewTickTime);
            //
            LastTickTime = NewTickTime;
        }

        private StringFormat GetStringFormatFromContentAllignment(ContentAlignment ca)
        {
            StringFormat format = new StringFormat();
            switch (ca)
            {
                case ContentAlignment.TopCenter:
                    format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Center;
                    break;
                case ContentAlignment.TopLeft:
                    format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Near;
                    break;
                case ContentAlignment.TopRight:
                    format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Far;
                    break;
                case ContentAlignment.MiddleCenter:
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;
                    break;
                case ContentAlignment.MiddleLeft:
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Near;
                    break;
                case ContentAlignment.MiddleRight:
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Far;
                    break;
                case ContentAlignment.BottomCenter:
                    format.Alignment = StringAlignment.Far;
                    format.LineAlignment = StringAlignment.Center;
                    break;
                case ContentAlignment.BottomLeft:
                    format.Alignment = StringAlignment.Far;
                    format.LineAlignment = StringAlignment.Near;
                    break;
                case ContentAlignment.BottomRight:
                    format.Alignment = StringAlignment.Far;
                    format.LineAlignment = StringAlignment.Far;
                    break;
            }

            format.FormatFlags |= StringFormatFlags.NoWrap;
            format.FormatFlags |= StringFormatFlags.NoClip;
            format.Trimming = StringTrimming.None;

            return format;
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            //Color has changed recreate our brush
            iBrush = new SolidBrush(ForeColor);

            base.OnForeColorChanged(e);
        }


        private void HandleTextSizeChange()
        {
            //Update text size according to text and font
            Graphics g = this.CreateGraphics();
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
            iTextSize = g.MeasureString(Text, Font);
            iSeparatorSize = g.MeasureString(Separator, Font);
            iStringFormat = GetStringFormatFromContentAllignment(TextAlign);

            if (NeedToScroll())
            {
                //Always align left when scrolling
                iStringFormat.Alignment = StringAlignment.Near;
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            HandleTextSizeChange();

            base.OnTextChanged(e);
        }

        protected override void OnFontChanged(EventArgs e)
        {
            HandleTextSizeChange();

            base.OnFontChanged(e);
        }

        protected override void OnTextAlignChanged(EventArgs e)
        {
            iStringFormat = GetStringFormatFromContentAllignment(TextAlign);

            base.OnTextAlignChanged(e);

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //Disable anti-aliasing
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
            if (NeedToScroll())
            {
                //Draw the first one
                e.Graphics.TranslateTransform(-(float)CurrentPosition, 0);
                e.Graphics.DrawString(Text, Font, iBrush, ClientRectangle, iStringFormat);
                //Draw separator
                e.Graphics.TranslateTransform(iTextSize.Width, 0);
                e.Graphics.DrawString(Separator, Font, iBrush, ClientRectangle, iStringFormat);
                //Draw the last one
                e.Graphics.TranslateTransform(iSeparatorSize.Width, 0);
                e.Graphics.DrawString(Text, Font, iBrush, ClientRectangle, iStringFormat);
            }
            else
            {
                e.Graphics.DrawString(Text, Font, iBrush, ClientRectangle, iStringFormat);
            }



            //DrawText is not working without anti-aliasing. See: stackoverflow.com/questions/8283631/graphics-drawstring-vs-textrenderer-drawtextwhich-can-deliver-better-quality
            //TextRenderer.DrawText(e.Graphics, Text, Font, ClientRectangle, ForeColor, BackColor, iTextFormatFlags);

            //base.OnPaint(e);
        }

        public bool NeedToScroll()
        {
            //if (Width < e.Graphics.MeasureString(Text, Font).Width)
            if (Width < iTextSize.Width)
            {
                return true;
            }
            return false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Timer != null)
                    Timer.Dispose();
            }
            Timer = null;
        }
    }
}
