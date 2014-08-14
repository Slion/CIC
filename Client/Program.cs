using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpDisplayClient
{
    static public class Program
    {
        public static MainForm iMainForm;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static public void Main()
        {
            //Set high priority to our process to avoid lags when rendering to our screen
            System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.AboveNormal;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            iMainForm = new MainForm();
            Application.Run(iMainForm);
        }
    }
}
