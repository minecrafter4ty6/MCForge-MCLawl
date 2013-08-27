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
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Net;

namespace MCForge.Gui
{
    public partial class UpdateWindow : Form
    {
        public UpdateWindow()
        {
            InitializeComponent();
        }
        private void UpdateWindow_Load(object sender, EventArgs e)
        {
            UpdLoadProp("properties/update.properties");
			using (WebClient client = new WebClient())
			{
				//client.DownloadFile(ServerSettings.RevisionList, "text/revs.txt");
                //client.DownloadFileAsync(ServerSettings.RevisionList, "text/revs.txt");
                Uri uri = new Uri(ServerSettings.RevisionList);
                client.DownloadFileCompleted += Downloaded;
                client.DownloadFileAsync(uri, "text/revs.txt");
			}
        }


        private void Downloaded(object sender, AsyncCompletedEventArgs e)
        {
            revisions_downloading.Visible = false;
            if (File.Exists("text/revs.txt"))
            {
                listRevisions.Items.Clear();
                FileInfo file = new FileInfo("text/revs.txt");
                StreamReader stRead = file.OpenText();
                if (File.Exists("text/revs.txt"))
                    while (!stRead.EndOfStream)
                        listRevisions.Items.Add(stRead.ReadLine());
                stRead.Close();
                stRead.Dispose();
                file.Delete();
            }
            else MessageBox.Show("Error downloading revisions list");
        }
        public void UpdSave(string givenPath)
        {
			File.Create(givenPath).Dispose();
			using (StreamWriter SW = File.CreateText(givenPath))
			{
				SW.WriteLine("#This file manages the update process");
				SW.WriteLine("#Toggle AutoUpdate to true for the server to automatically update");
				SW.WriteLine("#Notify notifies players in-game of impending restart");
				SW.WriteLine("#Restart Countdown is how long in seconds the server will count before restarting and updating");
				SW.WriteLine();
				SW.WriteLine("autoupdate= " + chkAutoUpdate.Checked);
				SW.WriteLine("notify = " + chkNotify.Checked);
				SW.WriteLine("restartcountdown = " + txtCountdown.Text);
			}
            Close();
        }


        public void UpdLoadProp(string givenPath)
        {
            if (File.Exists(givenPath))
            {
                string[] lines = File.ReadAllLines(givenPath);

                foreach (string line in lines)
                {
                    if (line != "" && line[0] != '#')
                    {
                        //int index = line.IndexOf('=') + 1; // not needed if we use Split('=')
                        string key = line.Split('=')[0].Trim();
                        string value = line.Split('=')[1].Trim();

                        switch (key.ToLower())
                        {
                            case "autoupdate":
                                chkAutoUpdate.Checked = (value.ToLower() == "true") ? true : false;
                                break;
                            case "notify":
                                chkNotify.Checked = (value.ToLower() == "true") ? true : false;
                                break;
                            case "restartcountdown":
                                txtCountdown.Text = value;
                                break;
                        }
                    }
                }
                //Save(givenPath);
            }
            //else Save(givenPath);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string chkNum = txtCountdown.Text.Trim();
            double Num;
            bool isNum = double.TryParse(chkNum, out Num);
            if (!isNum || txtCountdown.Text == "")
            {
                MessageBox.Show("You must enter a number for the countdown");
            }
            else
            {
                UpdSave("properties/update.properties");
                Server.autoupdate = chkAutoUpdate.Checked;
            }
        }

        private void cmdDiscard_Click(object sender, EventArgs e)
        {
            UpdLoadProp("properties/update.properties");
            Close();
        }

        private void cmdUpdate_Click(object sender, EventArgs e)
        {
            MCForge_.Gui.Program.PerformUpdate();
      /*      if (!Program.CurrentUpdate)
                Program.UpdateCheck();
            else
            {
                Thread messageThread = new Thread(new ThreadStart(delegate
                {
                    MessageBox.Show("Already checking for updates.");
                })); messageThread.Start();
            } */
        }


        private void listRevisions_SelectedValueChanged(object sender, EventArgs e)
        {
            Server.selectedrevision = listRevisions.SelectedItem.ToString();
            
        }

    }
}
