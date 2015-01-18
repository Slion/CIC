using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Principal;
using System.Diagnostics;
using System.Reflection;

namespace SharpDisplayManager
{
    static class Program
    {
        //WARNING: This is assuming we have a single instance of our program.
        //That is what we want but we should enforce it somehow.
        public static MainForm iMainForm;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

			/*
			if (!IsRunAsAdministrator())
			{
				var processInfo = new ProcessStartInfo(Assembly.GetExecutingAssembly().CodeBase);

				// The following properties run the new process as administrator
				processInfo.UseShellExecute = true;
				processInfo.Verb = "runas";

				// Start the new process
				try
				{
					Process.Start(processInfo);
				}
				catch (Exception)
				{
					// The user did not allow the application to run as administrator
					MessageBox.Show("Sorry, this application must be run as Administrator.");
				}

				// Shut down the current process
				Application.Exit();
				return;
				//Application.Current.Shutdown();
			}*/


			Application.ApplicationExit += new EventHandler(OnApplicationExit);
			//
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            iMainForm = new MainForm();
            Application.Run(iMainForm);
        }



		static private bool IsRunAsAdministrator()
		{
		  var wi = WindowsIdentity.GetCurrent();
		  var wp = new WindowsPrincipal(wi);

		  return wp.IsInRole(WindowsBuiltInRole.Administrator);
		}

		/// <summary>
		/// Occurs after form closing.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		static private void OnApplicationExit(object sender, EventArgs e)
		{

		}
    }
}
