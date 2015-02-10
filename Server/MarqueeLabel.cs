//
// Copyright (C) 2014-2015 Stéphane Lenclud.
//
// This file is part of SharpDisplayManager.
//
// SharpDisplayManager is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// SharpDisplayManager is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with SharpDisplayManager.  If not, see <http://www.gnu.org/licenses/>.
//

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
    [System.ComponentModel.DesignerCategory("Code")]
    public class MarqueeLabel : Label
    {
        private bool iOwnTimer;
        private StringFormat iStringFormat;
        private SolidBrush iBrush;
        private SizeF iTextSize;
        private SizeF iSeparatorSize;
        private SizeF iScrollSize;
        private Font iFontInUse;
		private string iSeparator;

        [Category("Appearance")]
        [Description("Separator in our scrolling loop.")]
        [DefaultValue(" | ")]
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string Separator
		{
			get
			{
				return iSeparator;
			}
			set
			{
				if (value != Separator)
				{
					iSeparator = value;
					OnTextChanged(EventArgs.Empty);
				}
			}
		}

        [Category("Behavior")]
        [Description("How fast is our text scrolling, in pixels per second.")]
        [DefaultValue(32)]
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int PixelsPerSecond { get; set; }

        [Category("Behavior")]
        [Description("Should we scale down our font to try fit our text without scrolling.")]
        [DefaultValue(false)]
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool ScaleToFit { get; set; }

        [Category("Behavior")]
        [Description("Minimum size of our font allowed when scaling is enabled.")]
        [DefaultValue(15)]
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public float MinFontSize { get; set; }

        [Category("Behavior")]
        [Description("Use an internal or an external timer.")]
        [DefaultValue(true)]
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
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
            CurrentPosition = 0;
            iBrush = new SolidBrush(ForeColor);
            MinFontSize = 15;
            ScaleToFit = true;
            //Just clone our font
            iFontInUse = new Font(Font, Font.Style);

            //Following is needed if we ever switch from Label to Control base class.
            //Without it you get some pretty nasty flicker
            //SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            //SetStyle(ControlStyles.UserPaint, true);
            //SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            //SetStyle(ControlStyles.DoubleBuffer, true);
        }

        public void UpdateAnimation(DateTime aLastTickTime, DateTime aNewTickTime)
        {
            if (!NeedToScroll())
            {
                CurrentPosition = 0;
                return;
            }

            /*
            while (CurrentPosition > (iTextSize.Width + iSeparatorSize.Width))
            {
                CurrentPosition -= ((int)(iTextSize.Width + iSeparatorSize.Width));
            }
             */

            while (CurrentPosition > iScrollSize.Width)
            {
                CurrentPosition -= (int)iScrollSize.Width;
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
            StringFormat format = new StringFormat(StringFormat.GenericTypographic);
            switch (ca)
            {
                case ContentAlignment.TopCenter:
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Near;
                    break;
                case ContentAlignment.TopLeft:
                    format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Near;
                    break;
                case ContentAlignment.TopRight:
                    format.Alignment = StringAlignment.Far;
                    format.LineAlignment = StringAlignment.Near;
                    break;
                case ContentAlignment.MiddleCenter:
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;
                    break;
                case ContentAlignment.MiddleLeft:
                    format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Center;
                    break;
                case ContentAlignment.MiddleRight:
                    format.Alignment = StringAlignment.Far;
                    format.LineAlignment = StringAlignment.Center;
                    break;
                case ContentAlignment.BottomCenter:
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Far;
                    break;
                case ContentAlignment.BottomLeft:
                    format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Far;
                    break;
                case ContentAlignment.BottomRight:
                    format.Alignment = StringAlignment.Far;
                    format.LineAlignment = StringAlignment.Far;
                    break;
            }

            format.FormatFlags |= StringFormatFlags.NoWrap;
            format.FormatFlags |= StringFormatFlags.NoClip;
            format.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
            format.Trimming = StringTrimming.None;

            return format;
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            //Color has changed recreate our brush
            iBrush = new SolidBrush(ForeColor);

            base.OnForeColorChanged(e);
        }


        private void ComputeSizes()
        {
            //For all string measurements and drawing issues refer to the following article:
            // http://stackoverflow.com/questions/1203087/why-is-graphics-measurestring-returning-a-higher-than-expected-number
            //Update text size according to text and font
            Graphics g = this.CreateGraphics();
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
            iStringFormat = GetStringFormatFromContentAllignment(TextAlign);
            iTextSize = g.MeasureString(Text, iFontInUse, Int32.MaxValue, iStringFormat);
            iSeparatorSize = g.MeasureString(Separator, iFontInUse, Int32.MaxValue, iStringFormat);
            //Scroll width is the width of our text and our separator without taking kerning into account since
            //both text and separator are drawn independently from each other.
            iScrollSize.Width = iSeparatorSize.Width + iTextSize.Width;
            iScrollSize.Height = Math.Max(iSeparatorSize.Height, iTextSize.Height); //Not relevant for now
            //We don't want scroll width to take kerning into account so we don't use the following
            //iScrollSize = g.MeasureString(Text + Separator, Font, Int32.MaxValue, iStringFormat);
        }

        private void HandleTextSizeChange()
        {
            ComputeSizes();

            if (NeedToScroll())
            {
                if (ScaleToFit && iFontInUse.SizeInPoints > MinFontSize)
                {
                    //Try scaling down
                    iFontInUse = new Font(Font.FontFamily, iFontInUse.SizeInPoints - 1, Font.Style);
                    //Recurse until we are done
                    HandleTextSizeChange();
                }
                else
                {
                    if (ScaleToFit)
                    {
                        //Our minimum font size still needs scrolling
                        //Reset our font then
                        iFontInUse = new Font(Font,Font.Style);
                        ComputeSizes();
                    }

                    //Scrolling is ok or needed
                    //Always align left when scrolling
                    iStringFormat.Alignment = StringAlignment.Near;
                }
            }

            //Reset our timer whenever our text changes
            CurrentPosition = 0;
            PixelsLeft = 0;
            LastTickTime = DateTime.Now;
        }

        protected override void OnTextChanged(EventArgs e)
        {
            //Just clone our font
            iFontInUse = new Font(Font, Font.Style);

            HandleTextSizeChange();

            base.OnTextChanged(e);
        }

        protected override void OnFontChanged(EventArgs e)
        {
            //Just clone our font
            iFontInUse = new Font(Font,Font.Style);

            HandleTextSizeChange();

            base.OnFontChanged(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            //Just clone our font
            iFontInUse = new Font(Font, Font.Style);

            HandleTextSizeChange();

            base.OnSizeChanged(e);
        }

        protected override void OnTextAlignChanged(EventArgs e)
        {
            iStringFormat = GetStringFormatFromContentAllignment(TextAlign);
            if (NeedToScroll())
            {
                //Always align left when scrolling to avoid bugs
                iStringFormat.Alignment = StringAlignment.Near;
            }

            Invalidate();
            //
            base.OnTextAlignChanged(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //Disable anti-aliasing
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
            if (NeedToScroll())
            {
                //Draw it all in a single call would take kerning into account
                //e.Graphics.TranslateTransform(-(float)CurrentPosition, 0);
                //e.Graphics.DrawString(Text + Separator + Text, Font, iBrush, ClientRectangle, StringFormat);

                //Doing separate draw operation allows us not to take kerning into account between separator and string
                //Draw the first one
                e.Graphics.TranslateTransform(-(float)CurrentPosition, 0);
                e.Graphics.DrawString(Text, iFontInUse, iBrush, ClientRectangle, iStringFormat);
                //Draw separator
                e.Graphics.TranslateTransform(iTextSize.Width, 0);
                e.Graphics.DrawString(Separator, iFontInUse, iBrush, ClientRectangle, iStringFormat);
                //Draw the last one
                e.Graphics.TranslateTransform(iSeparatorSize.Width, 0);
                e.Graphics.DrawString(Text, iFontInUse, iBrush, ClientRectangle, iStringFormat);
            }
            else
            {
                e.Graphics.DrawString(Text, iFontInUse, iBrush, ClientRectangle, iStringFormat);
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
