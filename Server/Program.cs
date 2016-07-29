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
        public static FormMain iFormMain;
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
            iFormMain = new FormMain();
            Application.Run(iFormMain);
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
