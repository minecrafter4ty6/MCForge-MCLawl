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
    public sealed class CmdTempRankInfo : Command
    {
        public override string name { get { return "temprankinfo"; } }
        public override string shortcut { get { return "tri"; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdTempRankInfo() { }

        public override void Use(Player p, string message)
        {
            string alltext = File.ReadAllText("text/tempranks.txt");
            if (alltext.Contains(message) == false)
            {
                Player.SendMessage(p, "&cPlayer &a" + message + "&c Has not been assigned a temporary rank. Cannot unnasign.");
                return;
            }
            if (message == "")
            {
                Help(p);
                Player.SendMessage(p, "&cYou need to enter a player!");
                return;
            }

            foreach (string line3 in File.ReadAllLines("text/tempranks.txt"))
            {
                if (line3.Contains(message))
                {
                    string temprank = line3.Split(' ')[1];
                    string oldrank = line3.Split(' ')[2];
                    string tempranker = line3.Split(' ')[9];
                    int period = Convert.ToInt32(line3.Split(' ')[3]);
                    int minutes = Convert.ToInt32(line3.Split(' ')[4]);
                    int hours = Convert.ToInt32(line3.Split(' ')[5]);
                    int days = Convert.ToInt32(line3.Split(' ')[6]);
                    int months = Convert.ToInt32(line3.Split(' ')[7]);
                    int years = Convert.ToInt32(line3.Split(' ')[8]);
                    DateTime ExpireDate = new DateTime(years, months, days, hours, minutes, 0);
                    DateTime tocheck = ExpireDate.AddHours(Convert.ToDouble(period));
                    Player.SendMessage(p, "&1Temporary Rank Information of " + message);
                    Player.SendMessage(p, "&aTemporary Rank: " + temprank);
                    Player.SendMessage(p, "&aOld Rank: " + oldrank);
                    Player.SendMessage(p, "&aDate of assignment: " + ExpireDate.ToString());
                    Player.SendMessage(p, "&aDate of expiry: " + tocheck.ToString());
                    Player.SendMessage(p, "&aTempranked by: " + tempranker);
                }
            }
        }

        public override void Help(Player p)
        {
            Player.SendMessage(p, "/TempRankInfo <player> - Lists the info of the Temporary rank of the given player");
        }
    }
    
}
