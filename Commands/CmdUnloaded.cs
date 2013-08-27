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
using System.IO;
namespace MCForge.Commands
{
    public sealed class CmdUnloaded : Command
    {
        public override string name { get { return "unloaded"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "information"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }
        public CmdUnloaded() { }

        public override void Use(Player p, string message)
        {
            try
            {
                List<string> levels = new List<string>(Server.levels.Count);

                string unloadedLevels = ""; int currentNum = 0; int maxMaps = 0;

                if (message != "")
                {
                    try {
                        int n = int.Parse(message);
                        if (n <= 0) { Help(p); return; }
                        maxMaps = n * 50;
                        currentNum = maxMaps - 50;
                    } catch { Help(p); return; }
                }

                DirectoryInfo di = new DirectoryInfo("levels/");
                FileInfo[] fi = di.GetFiles("*.lvl");
                foreach (Level l in Server.levels) { levels.Add(l.name.ToLower()); }

                if (maxMaps == 0)
                {
                    foreach (FileInfo file in fi)
                    {
                        if (!levels.Contains(file.Name.Replace(".lvl", "").ToLower()))
                        {
                            string level = file.Name.Replace(".lvl", "");
                            string visit = GetLoadOnGoto(level) && p.group.Permission >= GetPerVisitPermission(level) ? "%aYes" : "%cNo";
                            unloadedLevels += ", " + Group.findPerm(GetPerBuildPermission(level)).color + level + " &b[" + visit + "&b]";
                        }
                    }
                    if (unloadedLevels != "")
                    {
                        Player.SendMessage(p, "Unloaded levels [Accessible]: ");
                        Player.SendMessage(p, unloadedLevels.Remove(0, 2));
                        if (fi.Length > 50) { Player.SendMessage(p, "For a more structured list, use /unloaded <1/2/3/..>"); }
                    }
                    else Player.SendMessage(p, "No maps are unloaded");
                }
                else
                {
                    if (maxMaps > fi.Length) maxMaps = fi.Length;
                    if (currentNum > fi.Length) { Player.SendMessage(p, "No maps beyond number " + fi.Length); return; }

                    Player.SendMessage(p, "Unloaded levels [Accessible] (" + currentNum + " to " + maxMaps + "):");
                    for (int i = currentNum; i < maxMaps; i++)
                    {
                        if (!levels.Contains(fi[i].Name.Replace(".lvl", "").ToLower()))
                        {
                            string level = fi[i].Name.Replace(".lvl", "");
                            string visit = GetLoadOnGoto(level) && p.group.Permission >= GetPerVisitPermission(level) ? "%aYes" : "%cNo";
                            unloadedLevels += ", " + Group.findPerm(GetPerBuildPermission(level)).color + level + " &b[" + visit + "&b]";
                        }
                    }

                    if (unloadedLevels != "")
                    {
                        Player.SendMessage(p, unloadedLevels.Remove(0, 2));
                    }
                    else Player.SendMessage(p, "No maps are unloaded");
                }
            }
            catch (Exception e) { Server.ErrorLog(e); Player.SendMessage(p, "An error occured"); }
            //Exception catching since it needs to be tested on Ocean Flatgrass
        }

        private LevelPermission GetPerVisitPermission(string level) {
            string location = "levels/level properties/" + level + ".properties";
            LevelPermission lvlperm = LevelPermission.Guest;
            try {
                using (StreamReader reader = new StreamReader(location)) {
                    string line;
                    while ((line = reader.ReadLine()) != null) {
                        if (line.Split()[0].ToLower() == "pervisit") {
                            lvlperm = Group.Find(line.Split()[2]).Permission;
                            break;
                        }
                    }
                }
            } catch { return LevelPermission.Guest; }

            return lvlperm;
        }

        private LevelPermission GetPerBuildPermission(string level) {
            string location = "levels/level properties/" + level + ".properties";
            LevelPermission lvlperm = LevelPermission.Guest;
            try {
                using (StreamReader reader = new StreamReader(location)) {
                    string line;
                    while ((line = reader.ReadLine()) != null) {
                        if (line.Split()[0].ToLower() == "perbuild") {
                            lvlperm = Group.Find(line.Split()[2]).Permission;
                            break;
                        }
                    }
                }
            } catch { return LevelPermission.Guest; }

            return lvlperm;
        }

        private bool GetLoadOnGoto(string level) {
            string location = "levels/level properties/" + level + ".properties";
            bool loadOnGoto = false;
            try {
                using (StreamReader reader = new StreamReader(location)) {
                    string line;
                    while ((line = reader.ReadLine()) != null) {
                        if (line.Split()[0].ToLower() == "loadongoto") {
                            loadOnGoto = bool.Parse(line.Split()[2]);
                            break;
                        }
                    }
                }
            } catch { return false; }

            return loadOnGoto;
        }

        public override void Help(Player p)
        {
            Player.SendMessage(p, "%f/unloaded " + Server.DefaultColor + "- Lists all unloaded levels and their accessible state.");
            Player.SendMessage(p, "%f/unloaded <1/2/3/..> " + Server.DefaultColor + "- Shows a compact list.");
        }
    }
}