//
// Copyright (C) 2014-2016 Stéphane Lenclud.
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
using System.Drawing;

namespace SharpDisplayClientMessage
{
    static public class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            //Set high priority to our process to avoid lags when rendering to our screen
            //System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.AboveNormal;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormClientMessage());
        }

        [STAThread]
        public static void MainWithParams(object aParams)
        {
            //Set high priority to our process to avoid lags when rendering to our screen
            //System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.AboveNormal;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            FormClientMessage form = new FormClientMessage();
            form.Params = (StartParams)aParams;
            form.WindowState = FormWindowState.Minimized;
            form.ShowInTaskbar = false;
            Application.Run(form);
        }

    }

    public class StartParams
    {
        public StartParams(string aPrimaryText, string aSecondaryText ="", int aDurationInMs = 5000, uint aClientPriority = SharpLib.Display.Priorities.Foreground)
        {
            PrimaryText = aPrimaryText;
            SecondaryText = aSecondaryText;
            Priority = aClientPriority;
            DurationInMs = aDurationInMs;
        }

        public string PrimaryText { get; set; }
        public string SecondaryText { get; set; }
        public uint Priority { get; set; }
        public int DurationInMs { get; set; }
    }
}
