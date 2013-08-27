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
using System.IO;
namespace MCForge.Commands
{
    public sealed class CmdSetRank : Command
    {
        public override string name { get { return "setrank"; } }
        public override string shortcut { get { return "rank"; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdSetRank() { }

        public override void Use(Player p, string message)
        {
            var split = message.Split(' ');
            if (split.Length < 2) { Help(p); return; }
            Player who = Player.Find(split[0]);
            Group newRank = Group.Find(split[1]);
            string msgGave = "";
            string oldcolor = "";
            string oldgroupstr = "";

            if (who != null)
            {
                oldgroupstr = who.group.name;
                oldcolor = who.group.color;
            }
            else
            {
                Group hey = Group.findPlayerGroup(split[0]);
                oldgroupstr = hey.name;
            }
            if (message.Split(' ').Length > 2) msgGave = message.Substring(message.IndexOf(' ', message.IndexOf(' ') + 1)); else msgGave = "Congratulations!";

            if (newRank == null) { Player.SendMessage(p, "Could not find specified rank."); return; }

            Group bannedGroup = Group.findPerm(LevelPermission.Banned);
            if (who == null)
            {
                string foundName = split[0];
                if (Group.findPlayerGroup(foundName) == bannedGroup || newRank == bannedGroup)
                {
                    Player.SendMessage(p, "Cannot change the rank to or from \"" + bannedGroup.name + "\".");
                    return;
                }

                if (p != null)
                {
                    if (Group.findPlayerGroup(foundName).Permission >= p.group.Permission || newRank.Permission >= p.group.Permission)
                    {
                        Player.SendMessage(p, "Cannot change the rank of someone equal or higher than you"); return;
                    }
                }

                Group oldGroup = Group.findPlayerGroup(foundName);
                oldGroup.playerList.Remove(foundName);
                oldGroup.playerList.Save();

                newRank.playerList.Add(foundName);
                newRank.playerList.Save();

                Player.GlobalMessage(foundName + " &f(offline)" + Server.DefaultColor + "'s rank was set to " + newRank.color + newRank.name);
            }
            else if (who == p)
            {
                Player.SendMessage(p, "Cannot change your own rank."); return;
            }
            else
            {
                if (p != null)
                {
                    if (who.group == bannedGroup || newRank == bannedGroup)
                    {
                        Player.SendMessage(p, "Cannot change the rank to or from \"" + bannedGroup.name + "\".");
                        return;
                    }

                    if (who.group.Permission >= p.group.Permission || newRank.Permission >= p.group.Permission)
                    {
                        Player.SendMessage(p, "Cannot change the rank of someone equal or higher to yourself."); return;
                    }
                }
                Group.because(who, newRank);
                if (Group.cancelrank)
                {
                    Group.cancelrank = false;
                    return;
                }
                who.group.playerList.Remove(who.name);
                who.group.playerList.Save();

                newRank.playerList.Add(who.name);
                newRank.playerList.Save();

                Player.GlobalChat(who, who.color + who.name + Server.DefaultColor + "'s rank was set to " + newRank.color + newRank.name, false);
                Player.SendMessage(who, "&6" + msgGave, false);

                who.group = newRank;
                if(who.color == "" || who.color == oldcolor )
                    who.color = who.group.color;
                who.SetPrefix();

                Player.GlobalDie(who, false);

                who.SendMessage("You are now ranked " + newRank.color + newRank.name + Server.DefaultColor + ", type /help for your new set of commands.");
                who.SendUserType(Block.canPlace(who.group.Permission, Block.blackrock));

                string year = DateTime.Now.Year.ToString();
                string month = DateTime.Now.Month.ToString();
                string day = DateTime.Now.Day.ToString();
                string hour = DateTime.Now.Hour.ToString();
                string minute = DateTime.Now.Minute.ToString();
                string assigner;
                if (p == null)
                {
                    assigner = "Console";
                }
                else
                {
                    assigner = p.name;
                }
                string allrankinfos = "";
                foreach (string line in File.ReadAllLines("text/rankinfo.txt"))
                {
                    if (!line.Contains(split[0]))
                    {
                        allrankinfos = allrankinfos + line + "\r\n";
                    }
                }
                File.WriteAllText("text/rankinfo.txt", allrankinfos);
                try
                {
                    StreamWriter sw;
                    sw = File.AppendText("text/rankinfo.txt");
                    sw.WriteLine(who.name + " " + assigner + " " + minute + " " + hour + " " + day + " " + month + " " + year + " " + split[1] + " " + oldgroupstr);
                    sw.Close();
                }
                catch
                {
                    Player.SendMessage(p, "&cAn error occurred!");
                }

                Player.GlobalSpawn(who, who.pos[0], who.pos[1], who.pos[2], who.rot[0], who.rot[1], false);
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/setrank <player> <rank> <yay> - Sets or returns a players rank.");
            Player.SendMessage(p, "You may use /rank as a shortcut");
            Player.SendMessage(p, "Valid Ranks are: " + Group.concatList(true, true));
            Player.SendMessage(p, "<yay> is a celebratory message");
        }
    }
}
