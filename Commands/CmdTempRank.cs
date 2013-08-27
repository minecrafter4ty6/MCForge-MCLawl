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
    public sealed class CmdTempRank : Command
    {
        public override string name { get { return "temprank"; } }
        public override string shortcut { get { return "tr"; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdTempRank() { }

        public override void Use(Player p, string message)
        {
            string player = "", rank = "", period = "";
            try
            {
                player = message.Split(' ')[0];
                rank = message.Split(' ')[1];
                period = message.Split(' ')[2];
            }
            catch
            {
                Help(p);
            }
           
            Player who = Player.Find(player);
            
            
            if (player == "") { Player.SendMessage(p, "&cYou have to enter a player!"); return; }          
            if (who == null) { Player.SendMessage(p, "&cPlayer &a" + player + "&c not found!"); return; }
            if (rank == "") { Player.SendMessage(p, "&cYou have to enter a rank!"); return; }
            else
            {
                Group groupNew = Group.Find(rank);
                if (groupNew == null)
                {
                    Player.SendMessage(p, "&cRank &a" + rank + "&c does not exist");
                    return;
                }
            }
            if (period == "") { Player.SendMessage(p, "&cYou have to enter a time period!"); return; }
            Boolean isnumber = true;
            try
            {
                Convert.ToInt32(period);
            }
            catch
            {
                isnumber = false;
            }
            if (!isnumber)
            {
                Player.SendMessage(p, "&cThe period needs to be a number!");
                return;
            }

            string alltext = File.ReadAllText("text/tempranks.txt");
            if (alltext.Contains(player))
            {
                Player.SendMessage(p, "&cThe player already has a temporary rank assigned!");
                return;
            }
            bool byconsole;
            if (p == null)
            {
                byconsole = true;
                goto skipper;
            }
            else
            {
                byconsole = false;
            }
            if (player == p.name)
            {
                Player.SendMessage(p, "&cYou cannot assign yourself a temporary rank!");
                return;
            }
            Player who3 = Player.Find(player);
            if (who3.group.Permission >= p.group.Permission)
            {
                Player.SendMessage(p, "Cannot change the temporary rank of someone equal or higher to yourself.");
                return;
            }
            Group newRank2 = Group.Find(rank);
            if (newRank2.Permission >= p.group.Permission)
            {
                Player.SendMessage(p, "Cannot change the temporary rank to a higher rank than yourself");
                return;
            }
        skipper:
            string year = DateTime.Now.Year.ToString();
            string month = DateTime.Now.Month.ToString();
            string day = DateTime.Now.Day.ToString();
            string hour = DateTime.Now.Hour.ToString();
            string minute = DateTime.Now.Minute.ToString();
            string oldrank = who.group.name;
            string assigner;
            if (byconsole)
            {
                assigner = "Console";
            }
            else {
                assigner = p.name;
            }
        
            Boolean tryer = true;
            try
            {
                StreamWriter sw;
                sw = File.AppendText("text/tempranks.txt");
                sw.WriteLine(who.name + " " + rank + " " + oldrank + " " + period + " " + minute + " " + hour + " " + day + " " + month + " " + year + " " + assigner);
                sw.Close();
            }
            catch
            {
                tryer = false;
            }

            
                if (!tryer)
                {
                    Player.SendMessage(p, "&cAn error occurred!");
                }
                else
                {
                    Group newgroup = Group.Find(rank);
                    Command.all.Find("setrank").Use(null, who.name + " " + newgroup.name);
                    Player.SendMessage(p, "Temporary rank (" + rank + ") is assigned succesfully to " + player + " for " + period + " hours");
                    Player who2 = Player.Find(player);
                    Player.SendMessage(who2, "Your Temporary rank (" + rank + ") is assigned succesfully for " + period + " hours");
                }
            
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/temprank <player> <rank> <period(hours)> - Sets a temporary rank for the specified player.");
        }
            
    }
}
