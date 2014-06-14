﻿using System;
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
        private int CurrentPosition { get; set; }
        private Timer Timer { get; set; }


        [Category("Behavior")]
        [Description("How fast is our text scrolling, in pixels per second.")]
        [DefaultValue(32)]
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public int PixelsPerSecond { get; set; }

        private DateTime LastTickTime { get; set; }
        private double PixelsLeft { get; set; }
        //DateTime a = new DateTime(2010, 05, 12, 13, 15, 00);
//DateTime b = new DateTime(2010, 05, 12, 13, 45, 00);
//Console.WriteLine(b.Subtract(a).TotalMinutes);

        public MarqueeLabel()
        {
            UseCompatibleTextRendering = true;
            Timer = new Timer();
            Timer.Interval = 10;
            Timer.Tick += new EventHandler(Timer_Tick);
            Timer.Start();
            //PixelsPerSecond = 32;
            LastTickTime = DateTime.Now;
            PixelsLeft = 0;
        }

        void Timer_Tick(object sender, EventArgs e)
        {
            while (CurrentPosition > Width)
            {
                CurrentPosition = -Width;
            }

            DateTime NewTickTime=DateTime.Now;
            PixelsLeft += NewTickTime.Subtract(LastTickTime).TotalSeconds * PixelsPerSecond;
            LastTickTime = NewTickTime;
            //offset += PixelsLeft;

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

        protected override void OnPaint(PaintEventArgs e)
        {
            //Disable anti-aliasing
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
            e.Graphics.TranslateTransform((float)CurrentPosition, 0);
            base.OnPaint(e);
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
