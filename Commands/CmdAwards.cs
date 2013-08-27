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

using System.Collections.Generic;
namespace MCForge.Commands
{
    public sealed class CmdAwards : Command
    {
        public override string name { get { return "awards"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }

        public override void Use(Player p, string message)
        {
            if (message.Split(' ').Length > 2) { Help(p); return; }
            // /awards
            // /awards 1
            // /awards bob
            // /awards bob 1

            int totalCount = 0;
            string foundPlayer = "";

            if (message != "")
            {
                if (message.Split(' ').Length == 2)
                {
                    foundPlayer = message.Split(' ')[0];
                    Player who = Player.Find(foundPlayer);
                    if (who != null) foundPlayer = who.name;
                    try
                    {
                        totalCount = int.Parse(message.Split(' ')[1]);
                    }
                    catch
                    {
                        Help(p);
                        return;
                    }
                }
                else
                {
                    if (message.Length <= 3)
                    {
                        try
                        {
                            totalCount = int.Parse(message);
                        }
                        catch
                        {
                            foundPlayer = message;
                            Player who = Player.Find(foundPlayer);
                            if (who != null) foundPlayer = who.name;
                        }
                    }
                    else
                    {
                        foundPlayer = message;
                        Player who = Player.Find(foundPlayer);
                        if (who != null) foundPlayer = who.name;
                    }
                }
            }

            if (totalCount < 0)
            {
                Player.SendMessage(p, "Cannot display pages less than 0");
                return;
            }

            List<Awards.awardData> awardList = new List<Awards.awardData>();
            if (foundPlayer == "")
            {
                awardList = Awards.allAwards;
            }
            else
            {
                foreach (string s in Awards.getPlayersAwards(foundPlayer))
                {
                    Awards.awardData aD = new Awards.awardData();
                    aD.awardName = s;
                    aD.description = Awards.getDescription(s);
                    awardList.Add(aD);
                }
            }

            if (awardList.Count == 0)
            {
                if (foundPlayer != "")
                    Player.SendMessage(p, "The player has no awards!");
                else
                    Player.SendMessage(p, "There are no awards in this server yet");

                return;
            }

            int max = totalCount * 5;
            int start = (totalCount - 1) * 5;
            if (start > awardList.Count)
            {
                Player.SendMessage(p, "There aren't that many awards. Enter a smaller number");
                return;
            }
            if (max > awardList.Count) 
                max = awardList.Count;

            if (foundPlayer != "")
                Player.SendMessage(p, Server.FindColor(foundPlayer) + foundPlayer + Server.DefaultColor + " has the following awards:");
            else
                Player.SendMessage(p, "Awards available: ");

            if (totalCount == 0)
            {
                foreach (Awards.awardData aD in awardList)
                    Player.SendMessage(p, "&6" + aD.awardName + ": &7" + aD.description);

                if (awardList.Count > 8) Player.SendMessage(p, "&5Use &b/awards " + message + " 1/2/3/... &5for a more ordered list");
            }
            else
            {
                for (int i = start; i < max; i++)
                {
                    Awards.awardData aD = awardList[i];
                    Player.SendMessage(p, "&6" + aD.awardName + ": &7" + aD.description);
                }
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/awards [player] - Gives a full list of awards");
            Player.SendMessage(p, "If [player] is specified, shows awards for that player");
            Player.SendMessage(p, "Use 1/2/3/... to get an ordered list");
        }
    }
}