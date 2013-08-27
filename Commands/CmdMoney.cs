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
namespace MCForge.Commands
{
    public sealed class CmdMoney : Command
    {
        public override string name { get { return "money"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
        public override void Use(Player p, string message)
        {
            bool emptyMessage = message == "" || message == null || message == string.Empty;
            if (p != null && emptyMessage)
            {
                Player.SendMessage(p, "You currently have %f" + p.money + " %3" + Server.moneys);
            }
            else if (message.Split().Length == 1)
            {
                Player who = Player.Find(message);
                if (who == null)
                { //player is offline
                    Economy.EcoStats ecos = Economy.RetrieveEcoStats(message);
                    Player.SendMessage(p, ecos.playerName + "(%foffline" + Server.DefaultColor + ") currently has %f" + ecos.money + " %3" + Server.moneys);
                    return;
                }
                //you can see everyone's stats with /eco stats [player]
                /*if (who.group.Permission >= p.group.Permission) {
                    Player.SendMessage(p, "%cCannot see the money of someone of equal or greater rank.");
                    return;
                }*/
                Player.SendMessage(p, who.color + who.name + Server.DefaultColor + " currently has %f" + who.money + " %3" + Server.moneys);
            }
            else if (p == null && emptyMessage)
            {
                Player.SendMessage(p, "%Console can't have %3" + Server.moneys);
            }
            else
            {
                Player.SendMessage(p, "%cInvalid parameters!");
                Help(p);
            }

        }

        public override void Help(Player p)
        {
            Player.SendMessage(p, "%f/money <player>" + Server.DefaultColor + " - Shows how much %3" + Server.moneys + Server.DefaultColor + " <player> has");
        }
    }
}