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


namespace SharpDisplayClientMessage
{

    /// <summary>
    /// Sharp Display Client designed to act as an idle client.
    /// It should take care of screen saving and other such concerns.
    /// </summary>
    public partial class FormClientMessage : Form
    {
        public StartParams Params { get; set; }

        Client iClient;
        ContentAlignment iAlignment;
        TextField iPrimaryTextField;
        TextField iSecondaryTextField;


        public delegate void CloseDelegate();
        public delegate void CloseConnectionDelegate();


        public FormClientMessage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormClientMessage_Load(object sender, EventArgs e)
        {
            //Prevents showing in the Open Task view (Windows Key + Tab)
            Visible = false;

            //Display client
            iClient = new Client();
            iClient.CloseOrderEvent += OnCloseOrder;
            iClient.Open();
            iClient.SetName("Message");
            iClient.SetPriority(Params.Priority);
            SetupDisplayClient();

            //Timer
            iTimer.Interval = Params.DurationInMs;
            iTimer.Start();
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

            //Setup our fields
            iAlignment = ContentAlignment.MiddleCenter;
            iPrimaryTextField = new TextField(Params.PrimaryText, iAlignment, 0, 0);
            iSecondaryTextField = new TextField(Params.SecondaryText, iAlignment, 0, 1);

            //Set our fields
            if (string.IsNullOrEmpty(Params.SecondaryText))
            {
                //One field layout
                TableLayout layout = new TableLayout(1, 1);
                iClient.SetLayout(layout);

                iClient.CreateFields(new DataField[]
                {
                iPrimaryTextField
                });
            }
            else
            {
                //Two fields layout
                TableLayout layout = new TableLayout(1, 2);
                iClient.SetLayout(layout);

                iClient.CreateFields(new DataField[]
                {
                iPrimaryTextField,
                iSecondaryTextField
                });
            }
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
        private void FormClientMessage_FormClosing(object sender, FormClosingEventArgs e)
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
            Close();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormClientMessage_Shown(object sender, EventArgs e)
        {
            //Visible = false;
        }

    }

}
