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

using System.IO;
namespace MCForge.Commands
{
    public sealed class CmdRestore : Command
    {
        public override string name { get { return "restore"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdRestore() { }

        public override void Use(Player p, string message)
        {
            //Thread CrossThread;

            if (message != "")
            {
                Level lvl;
                string[] text = new string[2];
                text[0] = "";
                text[1] = "";
                try
                {
                    text[0] = message.Split(' ')[0].ToLower();
                    text[1] = message.Split(' ')[1].ToLower();
                }
                catch
                {
                    text[1] = p.level.name;
                }
                if (message.Split(' ').Length >= 2)
                {

                    lvl = Level.Find(text[1]);
                    if (lvl == null)
                    {
                        Player.SendMessage(p, "Level not found!");
                        return;
                    }

                }
                else
                {
                    if (p != null && p.level != null) lvl = p.level;
                    else
                    {
                        Server.s.Log("u dun derped, specify the level, silly head");
                        return;
                    }
                }
                Server.s.Log(@Server.backupLocation + "/" + lvl.name + "/" + text[0] + "/" + lvl.name + ".lvl");
                if (File.Exists(@Server.backupLocation + "/" + lvl.name + "/" + text[0] + "/" + lvl.name + ".lvl"))
                {
                    try
                    {
                        File.Copy(@Server.backupLocation + "/" + lvl.name + "/" + text[0] + "/" + lvl.name + ".lvl", "levels/" + lvl.name + ".lvl", true);
                        Level temp = Level.Load(lvl.name);
                        temp.StartPhysics();
                        if (temp != null)
                        {
                            lvl.spawnx = temp.spawnx;
                            lvl.spawny = temp.spawny;
                            lvl.spawnz = temp.spawnz;

                            lvl.height = temp.height;
                            lvl.width = temp.width;
                            lvl.depth = temp.depth;

                            lvl.blocks = temp.blocks;
                            lvl.setPhysics(0);
                            lvl.ClearPhysics();

                            Command.all.Find("reveal").Use(null, "all " + text[1]);
                        }
                        else
                        {
                            Server.s.Log("Restore nulled");
                            File.Copy("levels/" + lvl.name + ".lvl.backup", "levels/" + lvl.name + ".lvl", true);
                        }

                    }
                    catch { Server.s.Log("Restore fail"); }
                }
                else { Player.SendMessage(p, "Backup " + text[0] + " does not exist."); }
            }
            else
            {
                if (Directory.Exists(@Server.backupLocation + "/" + p.level.name))
                {
                    string[] directories = Directory.GetDirectories(@Server.backupLocation + "/" + p.level.name);
                    int backupNumber = directories.Length;
                    Player.SendMessage(p, p.level.name + " has " + backupNumber + " backups .");

                    bool foundOne = false; string foundRestores = "";
                    foreach (string s in directories)
                    {
                        string directoryName = s.Substring(s.LastIndexOf('\\') + 1);
                        try
                        {
                            int.Parse(directoryName);
                        }
                        catch
                        {
                            foundOne = true;
                            foundRestores += ", " + directoryName;
                        }
                    }

                    if (foundOne)
                    {
                        Player.SendMessage(p, "Custom-named restores:");
                        Player.SendMessage(p, "> " + foundRestores.Remove(0, 2));
                    }
                }
                else
                {
                    Player.SendMessage(p, p.level.name + " has no backups yet.");
                }
            }
        }

        public override void Help(Player p)
        {
            Player.SendMessage(p, "/restore <number> - restores a previous backup of the current map");
            Player.SendMessage(p, "/restore <number> <name> - restores a previous backup of the selected map");
        }
    }
}