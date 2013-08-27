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
    public sealed class CmdWhois : Command
    {
        public override string name { get { return "whois"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
        public CmdWhois() { }

        public override void Use(Player p, string message)
        {
            Player who = null;
            if (message == "") { who = p; message = p.name; } else { who = Player.Find(message); }
            if (who != null && !who.hidden)
            {
                Player.SendMessage(p, who.color + who.name + Server.DefaultColor + " is on &b" + who.level.name);
                Player.SendMessage(p, who.color + who.prefix + who.name + Server.DefaultColor + " has :");
                Player.SendMessage(p, "> > the rank of " + who.group.color + who.group.name);
                try
                {
                    if (!Group.Find("Nobody").commands.Contains("pay") && !Group.Find("Nobody").commands.Contains("give") && !Group.Find("Nobody").commands.Contains("take")) Player.SendMessage(p, "> > &a" + who.money + Server.DefaultColor + " " + Server.moneys);
                }
                catch { }
                Player.SendMessage(p, "> > &cdied &a" + who.overallDeath + Server.DefaultColor + " times");
                Player.SendMessage(p, "> > &bmodified &a" + who.overallBlocks + " &eblocks &eand &a" + who.loginBlocks + " &ewere changed &9since logging in&e.");
                string storedTime = Convert.ToDateTime(DateTime.Now.Subtract(who.timeLogged).ToString()).ToString("HH:mm:ss");
				Player.SendMessage(p, "> > time spent on server: " + who.time.Split(' ')[0] + " Days, " + who.time.Split(' ')[1] + " Hours, " + who.time.Split(' ')[2] + " Minutes, " + who.time.Split(' ')[3] + " Seconds.");
                Player.SendMessage(p, "> > been logged in for &a" + storedTime);
                Player.SendMessage(p, "> > first logged into the server on &a" + who.firstLogin.ToString("yyyy-MM-dd") + " at " + who.firstLogin.ToString("HH:mm:ss"));
                Player.SendMessage(p, "> > logged in &a" + who.totalLogins + Server.DefaultColor + " times, &c" + who.totalKicked + Server.DefaultColor + " of which ended in a kick.");
                Player.SendMessage(p, "> > " + Awards.awardAmount(who.name) + " awards");
                if (Ban.Isbanned(who.name)) {
                	string[] data = Ban.Getbandata(who.name);
                	Player.SendMessage(p, "> > is banned for " + data[1] + " by " + data[0]);
                }

                if (who.isDev) {
                    Player.SendMessage(p, Server.DefaultColor + "> > Player is a &9Developer");
                    if (Server.forgeProtection == ForgeProtection.Mod || Server.forgeProtection == ForgeProtection.Dev)
                        Player.SendMessage(p, Server.DefaultColor + "> > Player is &CPROTECTED" + Server.DefaultColor + " under MCForge Staff protection");
                }else if (who.isMod) {
                    Player.SendMessage(p, Server.DefaultColor + "> > Player is a &9MCForge Moderator");
                    if(Server.forgeProtection == ForgeProtection.Mod)
                        Player.SendMessage(p, Server.DefaultColor + "> > Player is &CPROTECTED" + Server.DefaultColor + " under MCForge Staff protection");
                }else if (who.isGCMod)
                    Player.SendMessage(p, Server.DefaultColor + "> > Player is a &9Global Chat Moderator");

                if (!(p != null && (int)p.group.Permission <= CommandOtherPerms.GetPerm(this)))
                    {
                        string givenIP;
                        if (Server.bannedIP.Contains(who.ip)) givenIP = "&8" + who.ip + ", which is banned"; 
                        else givenIP = who.ip;
                        Player.SendMessage(p, "> > the IP of " + givenIP);
                        if (Server.useWhitelist)
                        {
                            if (Server.whiteList.Contains(who.name))
                            {
                                Player.SendMessage(p, "> > Player is &fWhitelisted");
                            }
                        }
                    }
            }
            else { Player.SendMessage(p, "\"" + message + "\" is offline! Using /whowas instead."); Command.all.Find("whowas").Use(p, message); }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/whois [player] - Displays information about someone.");
        }
    }
}