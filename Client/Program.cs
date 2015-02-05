using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace SharpDisplayClient
{
    static public class Program
    {
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
			Application.Run(new MainForm());
        }

		[STAThread]
		static public void MainWithParams(object aParams)
		{
			//Set high priority to our process to avoid lags when rendering to our screen
			System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.AboveNormal;

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			MainForm mainForm = new MainForm();
			mainForm.Params = (StartParams)aParams;
			Application.Run(mainForm);
		}

    }

	public class StartParams
	{
		public StartParams(Point aLocation, string aTopText="", string aBottomText="")
		{
			TopText = aTopText;
			BottomText = aBottomText;
			Location = aLocation;
		}

		public string TopText { get; set; }
		public string BottomText { get; set; }
		public Point Location { get; set; }
	}
}
