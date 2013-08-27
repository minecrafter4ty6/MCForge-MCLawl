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
    public sealed class CmdPay : Command
    {
        public override string name { get { return "pay"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
        public CmdPay() { }

        public override void Use(Player p, string message)
        {
            if (message.IndexOf(' ') == -1) { Help(p); return; }
            if (message.Split(' ').Length != 2) { Help(p); return; }

            Player who = Player.Find(message.Split(' ')[0]);
            Economy.EcoStats payer;
            Economy.EcoStats receiver;

            int amountPaid;
            try { amountPaid = int.Parse(message.Split(' ')[1]); }
            catch { Player.SendMessage(p, "%cInvalid amount"); return; }
            if (amountPaid < 0) { Player.SendMessage(p, "%cCannot pay negative %3" + Server.moneys); return; }

            if (who == null)
            { //player is offline
                Player.OfflinePlayer off = Player.FindOffline(message.Split()[0]);
                if (off.name == "") { Player.SendMessage(p, "%cThe player %f" + message.Split()[0] + Server.DefaultColor + "(offline)%c does not exist or has never logged on to this server"); return; }

                payer = Economy.RetrieveEcoStats(p.name);
                receiver = Economy.RetrieveEcoStats(message.Split()[0]);
                if (!IsLegalPayment(p, payer.money, receiver.money, amountPaid)) return;

                p.money -= amountPaid;

                payer.money = p.money;
                receiver.money += amountPaid;

                payer.payment = "%f" + amountPaid + " %3" + Server.moneys + " to " + off.color + off.name + "%3 on %f" + DateTime.Now.ToString(CultureInfo.InvariantCulture);
                receiver.salary = "%f" + amountPaid + " %3" + Server.moneys + " by " + p.color + p.name + "%3 on %f" + DateTime.Now.ToString(CultureInfo.InvariantCulture);

                Economy.UpdateEcoStats(payer);
                Economy.UpdateEcoStats(receiver);

                Player.GlobalMessage(p.prefix + p.name + Server.DefaultColor + " paid %f" + off.color + off.name + Server.DefaultColor + "(offline) %f" + amountPaid + " %3" + Server.moneys);
                return;
            }
            if (who == p) { Player.SendMessage(p, "%cYou can't pay yourself %3" + Server.moneys); return; }

            payer = Economy.RetrieveEcoStats(p.name);
            receiver = Economy.RetrieveEcoStats(who.name);
            if (!IsLegalPayment(p, payer.money, receiver.money, amountPaid)) return;

            p.money -= amountPaid;
            who.money += amountPaid;

            payer.money = p.money;
            receiver.money = who.money;

            payer.payment = "%f" + amountPaid + " %3" + Server.moneys + " to " + who.color + who.name + "%3 on %f" + DateTime.Now.ToString(CultureInfo.InvariantCulture);
            receiver.salary = "%f" + amountPaid + " %3" + Server.moneys + " by " + p.color + p.name + "%3 on %f" + DateTime.Now.ToString(CultureInfo.InvariantCulture);

            Economy.UpdateEcoStats(payer);
            Economy.UpdateEcoStats(receiver);
            Player.GlobalMessage(p.prefix + p.name + Server.DefaultColor + " paid " + who.prefix + who.name + " %f" + amountPaid + " %3" + Server.moneys);
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "%f/pay [player] <amount> " + Server.DefaultColor + "- Pays <amount> " + Server.moneys + " to [player]");
        }

        private bool IsLegalPayment(Player p, int payer, int receiver, int amount)
        {
            if (receiver + amount > 16777215) { Player.SendMessage(p, "%cPlayers cannot have over %f16777215 %3" + Server.moneys); return false; }
            if (payer - amount < 0) { Player.SendMessage(p, "%cYou don't have enough %3" + Server.moneys); return false; }
            return true;
        }
    }
}