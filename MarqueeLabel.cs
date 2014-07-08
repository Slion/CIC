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
        private TextFormatFlags iTextFormatFlags;
        private StringFormat iStringFormat;
        private SolidBrush iBrush;

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
            while (CurrentPosition > Width)
            {
                CurrentPosition = -Width;
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
            iBrush = new SolidBrush(ForeColor);

            base.OnForeColorChanged(e);
        }

        protected override void OnTextAlignChanged(EventArgs e)
        {
            iTextFormatFlags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter;

            switch (TextAlign)
            {
                case ContentAlignment.BottomCenter:
                    iTextFormatFlags = TextFormatFlags.HorizontalCenter | TextFormatFlags.Bottom;
                    break;

                case ContentAlignment.BottomLeft:
                    iTextFormatFlags = TextFormatFlags.Left | TextFormatFlags.Bottom;
                    break;

                case ContentAlignment.BottomRight:
                    iTextFormatFlags = TextFormatFlags.Right | TextFormatFlags.Bottom;
                    break;

                case ContentAlignment.MiddleCenter:
                    iTextFormatFlags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;
                    break;

                case ContentAlignment.MiddleLeft:
                    iTextFormatFlags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter;
                    break;

                case ContentAlignment.MiddleRight:
                    iTextFormatFlags = TextFormatFlags.Right | TextFormatFlags.VerticalCenter;
                    break;

                case ContentAlignment.TopCenter:
                    iTextFormatFlags = TextFormatFlags.HorizontalCenter | TextFormatFlags.Top;
                    break;

                case ContentAlignment.TopLeft:
                    iTextFormatFlags = TextFormatFlags.Left | TextFormatFlags.Top;
                    break;

                case ContentAlignment.TopRight:
                    iTextFormatFlags = TextFormatFlags.Right | TextFormatFlags.Top;
                    break;
            }


            iTextFormatFlags |= TextFormatFlags.PreserveGraphicsTranslateTransform;
            //format |= TextFormatFlags.PreserveGraphicsClipping;
            iTextFormatFlags |= TextFormatFlags.NoClipping;

            iStringFormat = GetStringFormatFromContentAllignment(TextAlign);


            base.OnTextAlignChanged(e);

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //Disable anti-aliasing
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
            e.Graphics.TranslateTransform((float)CurrentPosition, 0);
            e.Graphics.DrawString(Text, Font, iBrush, ClientRectangle, iStringFormat);

            //DrawText is not working without anti-aliasing. See: stackoverflow.com/questions/8283631/graphics-drawstring-vs-textrenderer-drawtextwhich-can-deliver-better-quality
            //TextRenderer.DrawText(e.Graphics, Text, Font, ClientRectangle, ForeColor, BackColor, iTextFormatFlags);

            //base.OnPaint(e);
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
