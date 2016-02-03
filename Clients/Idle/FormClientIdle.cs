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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using SharpLib.Display;


namespace SharpDisplayIdleClient
{

    /// <summary>
    /// Sharp Display Client designed to act as an idle client.
    /// It should take care of screen saving and other such concerns.
    /// </summary>
    public partial class FormClientIdle : Form
    {
        public StartParams Params { get; set; }

        Client iClient;
        ContentAlignment iAlignment;
        TextField iTextField;

        public delegate void CloseDelegate();
        public delegate void CloseConnectionDelegate();


        public FormClientIdle()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormClientIdle_Load(object sender, EventArgs e)
        {
            //Display client
            iClient = new Client();
            iClient.CloseOrderEvent += OnCloseOrder;
            iClient.Open();
            iClient.SetName("Idle");
            iClient.SetPriority(SharpLib.Display.Priorities.Background);
            SetupDisplayClient();

            //Timer
            iTimer.Interval = IntervalToNextMinute();
            iTimer.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int IntervalToNextMinute()
        {
            DateTime now = DateTime.Now;
            return 60000 - now.Millisecond;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsClientReady()
        {
            return (iClient != null && iClient.IsReady());
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetupDisplayClient()
        {
            //Setup our layout

            //Set one column one line layout
            TableLayout layout = new TableLayout(1, 1);
            iClient.SetLayout(layout);

            //Setup our fields
            iAlignment = ContentAlignment.MiddleCenter;
            iTextField = new TextField(DateTime.Now.ToString("t"), iAlignment, 0, 0);

            //Set our fields
            iClient.CreateFields(new DataField[]
            {
                iTextField
            });
        }

        public void OnCloseOrder()
        {
            CloseThreadSafe();
        }

        /// <summary>
        ///
        /// </summary>
        public void CloseThreadSafe()
        {
            if (this.InvokeRequired)
            {
                //Not in the proper thread, invoke ourselves
                CloseDelegate d = new CloseDelegate(CloseThreadSafe);
                this.Invoke(d, new object[] { });
            }
            else
            {
                //We are in the proper thread
                Close();
            }
        }

        /// <summary>
        ///
        /// </summary>
        public void CloseConnectionThreadSafe()
        {
            if (this.InvokeRequired)
            {
                //Not in the proper thread, invoke ourselves
                CloseConnectionDelegate d = new CloseConnectionDelegate(CloseConnectionThreadSafe);
                this.Invoke(d, new object[] { });
            }
            else
            {
                //We are in the proper thread
                if (IsClientReady())
                {
                    string sessionId = iClient.SessionId;
                    Trace.TraceInformation("Closing client: " + sessionId);
                    iClient.Close();
                    Trace.TraceInformation("Closed client: " + sessionId);
                }

                iClient = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormClientIdle_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseConnectionThreadSafe();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iTimer_Tick(object sender, EventArgs e)
        {
            //Timer
            iTimer.Interval = IntervalToNextMinute();
            iTimer.Start();

            //
            if (String.IsNullOrEmpty(iTextField.Text))
            {
                //Time to show our time
                iTextField.Text = DateTime.Now.ToString("t");
            }
            else
            {
                //Do some screen saving
                iTextField.Text = "";
            }

            // Update our field
            iClient.SetField(iTextField);
        }

        private void FormClientIdle_Shown(object sender, EventArgs e)
        {
            //Visible = false;
        }
    }

}
