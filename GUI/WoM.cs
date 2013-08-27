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

namespace MCForge.GUI
{
    public partial class WoM : Form
    {
        public WoM()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.Text.ToCharArray().Length > 15)
            {
                MessageBox.Show("Only 15 characters allowed!", "Warning");
                return;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (WOMBeat.SetSettings(Server.IP, "" + Server.port, textBox1.Text, textBox2.Text, textBox3.Text))
            {
                MessageBox.Show("Your settings have been saved!", "Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Server.Server_ALT = textBox1.Text;
                Server.Server_Disc = textBox2.Text;
                Server.Server_Flag = textBox3.Text;
                this.Close();
            }
            else
                MessageBox.Show("There was an error, check the error log for more details!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void WoM_Load(object sender, EventArgs e)
        {
            textBox1.Text = Server.Server_ALT;
            textBox2.Text = Server.Server_Disc;
            textBox3.Text = Server.Server_Flag;
        }
    }
}
