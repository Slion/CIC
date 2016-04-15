using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpDisplayManager
{
    public partial class FxControl : UserControl
    {
        public FxControl()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // Call the OnPaint method of the base class.
            base.OnPaint(e);

            SolidBrush brush = new SolidBrush(Color.Blue);
            // Declare and instantiate a new pen.
            Pen myPen = new Pen(brush);

            // Draw an aqua rectangle in the rectangle represented by the control.
            e.Graphics.DrawRectangle(myPen, new Rectangle(new Point(0,0), this.Size - new Size(1,1)));            
        }
    }
}
