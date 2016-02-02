using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpDisplayIdleClient
{
    public partial class FormIdleClient : Form
    {
        public StartParams Params { get; set; }

        public FormIdleClient()
        {
            InitializeComponent();
        }
    }
}
