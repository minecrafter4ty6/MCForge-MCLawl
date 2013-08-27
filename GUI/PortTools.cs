/*
Copyright (C) 2010-2013 David Mitchell

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using MCForge.Core;

namespace MCForge.Gui.Popups {
    public partial class PortTools : Form {

        private readonly BackgroundWorker mWorkerChecker;
        private readonly BackgroundWorker mWorkerForwarder;

        public PortTools() {
            InitializeComponent();
            mWorkerChecker = new BackgroundWorker { WorkerSupportsCancellation = true };
            mWorkerChecker.DoWork += mWorker_DoWork;
            mWorkerChecker.RunWorkerCompleted += mWorker_RunWorkerCompleted;

            mWorkerForwarder = new BackgroundWorker { WorkerSupportsCancellation = true };
            mWorkerForwarder.DoWork += mWorkerForwarder_DoWork;
            mWorkerForwarder.RunWorkerCompleted += mWorkerForwarder_RunWorkerCompleted;
        }

        private void linkManually_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            try { Process.Start("http://www.canyouseeme.org/"); }
            catch { }
        }

        private void linkHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            try { Process.Start( "http://www.mcforge.net/community/forum/46-help-support/" ); }
            catch { }
        }

        private void btnCheck_Click(object sender, EventArgs e) {
            int port = 25565;
            if (String.IsNullOrEmpty(txtPort.Text.Trim()))
                txtPort.Text = "25565";

            try {
                port = int.Parse(txtPort.Text);
            }
            catch {
                txtPort.Text = "25565";
            }

            btnCheck.Enabled = false;
            txtPort.Enabled = false;
            lblStatus.Text = "Checking...";
            mWorkerChecker.RunWorkerAsync(port);
        }

        void mWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            if (e.Cancelled)
                return;

            btnCheck.Enabled = true;
            txtPort.Enabled = true;

            int result = (int)e.Result;
            switch (result) {
                case 0:
                    lblStatus.Text = "Problems Occurred";
                    lblStatus.ForeColor = Color.Red;
                    return;
                case 1:
                    lblStatus.Text = "Open";
                    lblStatus.ForeColor = Color.Green;
                    return;
                case 2:
                    lblStatus.Text = "Closed";
                    lblStatus.ForeColor = Color.Red;
                    return;
                case 3:
                    lblStatus.Text = "Web site error";
                    lblStatus.ForeColor = Color.Yellow;
                    return;
            }
        }

        void mWorker_DoWork(object sender, DoWorkEventArgs e) {
            try {
                using (var webClient = new WebClient()) {
                    string response = webClient.DownloadString("http://www.mcforge.net/ports.php?port=" + e.Argument);
                    switch (response.ToLower()) {
                        case "open":
                            e.Result = 1;
                            return;
                        case "closed":
                            e.Result = 2;
                            return;
                        default:
                            e.Result = 3;
                            return;
                    }
                }
            }
            catch {
                e.Result = 0;
            }
        }

        private void PortChecker_FormClosing(object sender, FormClosingEventArgs e) {
            mWorkerChecker.CancelAsync();
            mWorkerForwarder.CancelAsync();
        }

        private void linkHelpForward_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            try { Process.Start("https://github.com/MCForge/MCForge-Vanilla/wiki/Setup%20MCForge%205.5.0.2"); }
            catch { }
        }

        private void btnForward_Click(object sender, EventArgs e) {
            int port = 25565;
            if (String.IsNullOrEmpty(txtPortForward.Text.Trim()))
                txtPortForward.Text = "25565";

            try {
                port = int.Parse(txtPortForward.Text);
            }
            catch {
                txtPortForward.Text = "25565";
            }
            btnDelete.Enabled = false;
            btnForward.Enabled = false;
            txtPortForward.Enabled = false;
            mWorkerForwarder.RunWorkerAsync(new object[] { port, true });
        }

        private void btnDelete_Click(object sender, EventArgs e) {
            int port = 25565;
            if (String.IsNullOrEmpty(txtPortForward.Text.Trim()))
                txtPortForward.Text = "25565";

            try {
                port = int.Parse(txtPortForward.Text);
            }
            catch {
                txtPortForward.Text = "25565";
            }

            btnDelete.Enabled = false;
            btnForward.Enabled = false;
            txtPortForward.Enabled = false;
            mWorkerForwarder.RunWorkerAsync(new object[] { port, false });

        }

        void mWorkerForwarder_DoWork(object sender, DoWorkEventArgs e) {
            int tries = 0;
            int port = (int)((object[])e.Argument)[0];
            bool adding = (bool)((object[])e.Argument)[1];
            retry:
            try {
                if (!UPnP.CanUseUpnp) {
                    e.Result = 0;
                }
                else {

                    if (adding) {
                        tries++;
                        UPnP.ForwardPort(port, ProtocolType.Tcp, "MCForgeServer");
                        e.Result = 1;
                    }
                    else {
                        UPnP.DeleteForwardingRule(port, ProtocolType.Tcp);
                        e.Result = 3;
                    }
                }
            }
            catch {
                if (tries < 2) goto retry;

                e.Result = 2;
            }
        }

        void mWorkerForwarder_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            if (e.Cancelled)
                return;

            btnDelete.Enabled = true;
            btnForward.Enabled = true;
            txtPortForward.Enabled = true;

            int result = (int)e.Result;

            switch (result) {
                case 0:
                    lblForward.Text = "Error contacting router.";
                    lblForward.ForeColor = Color.Red;
                    return;
                case 1:
                    lblForward.Text = "Port forwarded automatically using UPnP";
                    lblForward.ForeColor = Color.Green;
                    return;
                case 2:
                    lblForward.Text = "Something Weird just happened, try again.";
                    lblForward.ForeColor = Color.Black;
                    return;
                case 3:
                    lblForward.Text = "Deleted Port Forward Rule.";
                    lblForward.ForeColor = Color.Green;
                    return;
            }
        }

    }
}
