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
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace MCForge.GUI.Eco {
    public partial class EcoLevelWindow : Form {

        private bool edit = false;
        private struct LevelEdit {
            public string name, x, y, z, oldname;
            public int price;
            public string type;
        }
        private LevelEdit lvledit;

        EconomyWindow eco;

        public EcoLevelWindow(EconomyWindow eco, string name = "", int price = 0, string x = "", string y = "", string z = "", string type = "", bool edit = false) {
            this.edit = edit;
            this.eco = eco;
            lvledit.name = name;
            lvledit.oldname = name;
            lvledit.price = price;
            lvledit.x = x;
            lvledit.y = y;
            lvledit.z = z;
            lvledit.type = type;

            InitializeComponent();

            SystemEvents.UserPreferenceChanged += new UserPreferenceChangedEventHandler(SystemEvents_UserPreferenceChanged);
            this.Font = SystemFonts.IconTitleFont;
        }

        private void SystemEvents_UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e) {
            if (e.Category == UserPreferenceCategory.Window) {
                this.Font = SystemFonts.IconTitleFont;
            }
        }

        private void PropertyWindow_FormClosing(object sender, FormClosingEventArgs e) {
            SystemEvents.UserPreferenceChanged -= new UserPreferenceChangedEventHandler(SystemEvents_UserPreferenceChanged);
            edit = false;
        }

        private void EcoLevelWindow_Load(object sender, EventArgs e) {
            List<string> dimensionsX = new List<string>();
            string[] dimensionsY = new string[6];
            string[] dimensionsZ = new string[6];
            dimensionsX.Add("16");
            dimensionsX.Add("32");
            dimensionsX.Add("64");
            dimensionsX.Add("128");
            dimensionsX.Add("256");
            dimensionsX.Add("512");
            dimensionsX.CopyTo(dimensionsY);
            dimensionsX.CopyTo(dimensionsZ);
            comboBoxX.DataSource = dimensionsX;
            comboBoxY.DataSource = dimensionsY;
            comboBoxZ.DataSource = dimensionsZ;

            List<string> types = new List<string>();
            types.Add("Flat");
            types.Add("Pixel");
            types.Add("Island");
            types.Add("Mountains");
            types.Add("Ocean");
            types.Add("Forest");
            types.Add("Desert");
            types.Add("Space");
            types.Add("Rainbow");
            types.Add("Hell");
            comboBoxType.DataSource = types;

            if (edit) {
                textBoxName.Text = lvledit.name;
                numericUpDownPrice.Value = lvledit.price;
                comboBoxX.SelectedItem = lvledit.x;
                comboBoxY.SelectedItem = lvledit.y;
                comboBoxZ.SelectedItem = lvledit.z;
                comboBoxType.SelectedItem = lvledit.type;
            } else {
                numericUpDownPrice.Value = 1000;
                comboBoxX.SelectedItem = "64";
                comboBoxY.SelectedItem = "64";
                comboBoxZ.SelectedItem = "64";
                comboBoxType.SelectedItem = "Flat";
            }
            buttonOk.Enabled = edit;
        }

        private void buttonCancel_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void buttonOk_Click(object sender, EventArgs e) {
            if (edit) Economy.Settings.LevelsList.Remove(Economy.FindLevel(lvledit.oldname));
            Economy.Settings.Level level = new Economy.Settings.Level();
            level.name = textBoxName.Text.Split()[0];
            level.price = (int)numericUpDownPrice.Value;
            level.x = comboBoxX.SelectedItem.ToString();
            level.y = comboBoxY.SelectedItem.ToString();
            level.z = comboBoxZ.SelectedItem.ToString();
            level.type = comboBoxType.SelectedItem.ToString().ToLower();
            Economy.Settings.LevelsList.Add(level);
            eco.UpdateLevels();
            eco.CheckLevelEnables();
            this.Close();
        }

        private void textBoxName_TextChanged(object sender, EventArgs e) {
            buttonOk.Enabled = textBoxName.Text != null && textBoxName.Text != String.Empty && textBoxName.Text != "";
        }

    }
}
