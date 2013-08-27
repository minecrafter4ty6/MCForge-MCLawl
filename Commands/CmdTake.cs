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
using System.Globalization;
namespace MCForge.Commands
{
    public sealed class CmdTake : Command
    {
        public override string name { get { return "take"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Admin; } }
        public CmdTake() { }

        public override void Use(Player p, string message)
        {
            if (message.IndexOf(' ') == -1) { Help(p); return; }
            if (message.Split(' ').Length != 2) { Help(p); return; }

            string user1 = "";
            string user2 = "";
            if (p == null) { user1 = "%f[ " + Server.DefaultColor + "Console%f]"; user2 = String.Format("{0}Console [&a{1}{0}]", Server.DefaultColor, Server.ZallState); } else { user1 = p.color + p.name; user2 = p.prefix + p.name; }

            int amountTaken = 0;
            bool all = false;
            try { amountTaken = int.Parse(message.Split(' ')[1]); }
            catch
            {
                if (message.Split()[1].ToLower() != "all")
                {
                    Player.SendMessage(p, "%cInvalid amount");
                    return;
                }
                all = true;
            }
            if (amountTaken < 0) { Player.SendMessage(p, "%cYou can't take negative %3" + Server.moneys); return; }


            Player who = Player.Find(message.Split()[0]);
            Economy.EcoStats ecos;
            if (who == null)
            { //player is offline
                Player.OfflinePlayer off = Player.FindOffline(message.Split()[0]);
                if (off.name == "") { Player.SendMessage(p, "%cThe player %f" + message.Split()[0] + Server.DefaultColor + "(offline)%c does not exist or has never logged on to this server"); return; }
                ecos = Economy.RetrieveEcoStats(message.Split()[0]);
                if (all || ecos.money - amountTaken < 0)
                {
                    amountTaken = ecos.money;
                    ecos.money = 0;
                }
                else
                    ecos.money -= amountTaken;
                ecos.fine = "%f" + amountTaken + " %3" + Server.moneys + " by " + user1 + "%3 on %f" + DateTime.Now.ToString(CultureInfo.InvariantCulture);
                Economy.UpdateEcoStats(ecos);
                Player.GlobalMessage(user2 + Server.DefaultColor + " took %f" + amountTaken + " %3" + Server.moneys + Server.DefaultColor + " from " + off.color + off.name + "%f(offline)");
                return;
            }
            ecos = Economy.RetrieveEcoStats(who.name);
            if (who == p)
            {
                Player.SendMessage(p, "%cYou can't take %3" + Server.moneys + "%c from yourself");
                return;
            }

            if (all || ecos.money - amountTaken < 0)
            {
                amountTaken = who.money;
                who.money = 0;
                ecos.money = 0;
            }
            else
            {
                who.money -= amountTaken;
                ecos.money = who.money;
            }
            ecos.fine = "%f" + amountTaken + " %3" + Server.moneys + " by " + user1 + "%3 on %f" + DateTime.Now.ToString(CultureInfo.InvariantCulture);
            Economy.UpdateEcoStats(ecos);
            Player.GlobalMessage(user2 + Server.DefaultColor + " took %f" + amountTaken + " %3" + Server.moneys + Server.DefaultColor + " from " + who.prefix + who.name);
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "&f/take [player] <amount> " + Server.DefaultColor + "- Takes <amount> of " + Server.moneys + " from [player]");
            Player.SendMessage(p, "&f/take [player] all " + Server.DefaultColor + "- Takes all the " + Server.moneys + " from [player]");
        }
    }
}