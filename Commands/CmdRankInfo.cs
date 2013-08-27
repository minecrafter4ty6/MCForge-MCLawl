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
    public sealed class CmdRankInfo : Command
    {
        public override string name { get { return "rankinfo"; } }
        public override string shortcut { get { return "ri"; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.AdvBuilder; } }
        public CmdRankInfo() { }

        public override void Use(Player p, string message)
        {
            string alltext = File.ReadAllText("text/rankinfo.txt");
            if (message == "")
            {
                Help(p);
                Player.SendMessage(p, "&cYou need to enter a player!");
                return;
            }
            Player who2 = Player.Find(message);
            if (who2 == null)
            {
                Player.SendMessage(p, "&cPlayer &e" + message + " &cHas not been found!");
                return;
            }
            if (!alltext.Contains(message))
            {
                Player.SendMessage(p, "&cPlayer &a" + message + "&c has not been ranked yet!");
                return;
            }
            


            foreach (string line3 in File.ReadAllLines("text/rankinfo.txt"))
            {
                if (!line3.Contains(message))
                    continue;
                string newrank = line3.Split(' ')[7];
                string oldrank = line3.Split(' ')[8];
                string assigner = line3.Split(' ')[1];
                Group newrankcolor = Group.Find(newrank);
                Group oldrankcolor = Group.Find(oldrank);
                int minutes = Convert.ToInt32(line3.Split(' ')[2]);
                int hours = Convert.ToInt32(line3.Split(' ')[3]);
                int days = Convert.ToInt32(line3.Split(' ')[4]);
                int months = Convert.ToInt32(line3.Split(' ')[5]);
                int years = Convert.ToInt32(line3.Split(' ')[6]);
                var ExpireDate = new DateTime(years, months, days, hours, minutes, 0);
                Player.SendMessage(p, "&1Rank Information of " + message);
                Player.SendMessage(p, "&aNew rank: " + newrankcolor.color + newrank);
                Player.SendMessage(p, "&aOld Rank: " + oldrankcolor.color + oldrank);
                Player.SendMessage(p, "&aDate of assignment: " + ExpireDate.ToString());
                Player.SendMessage(p, "&aRanked by: " + assigner);
            }


        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/rankinfo - Returns the information available about someones ranking");
        }
    }
}
