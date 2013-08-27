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
using System.Data;
using MCForge.SQL;
namespace MCForge.Commands
{
    public sealed class CmdWhowas : Command
    {
        public override string name { get { return "whowas"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "information"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
        public CmdWhowas() { }

        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }
            Player pl = Player.Find(message);
            if (pl != null && !pl.hidden)
            {
                Player.SendMessage(p, pl.color + pl.name + Server.DefaultColor + " is online, using /whois instead.");
                Command.all.Find("whois").Use(p, message);
                return;
            }

            if (message.IndexOf("'") != -1) { Player.SendMessage(p, "Cannot parse request."); return; }

            string FoundRank = Group.findPlayer(message.ToLower());
            Database.AddParams("@Name", message);
            DataTable playerDb = Database.fillData("SELECT * FROM Players WHERE Name=@Name");
            if (playerDb.Rows.Count == 0) { Player.SendMessage(p, Group.Find(FoundRank).color + message + Server.DefaultColor + " has the rank of " + Group.Find(FoundRank).color + FoundRank); return; }
            string title = playerDb.Rows[0]["Title"].ToString();
            string color = c.Parse(playerDb.Rows[0]["color"].ToString().Trim());
            if (color == "" || color == null || String.IsNullOrEmpty(color)) color = Group.Find(FoundRank).color;
            string tcolor = c.Parse(playerDb.Rows[0]["title_color"].ToString().Trim());
            if (title == "" || title == null || String.IsNullOrEmpty(title))
                Player.SendMessage(p, color + message + Server.DefaultColor + " has :");
            else
                Player.SendMessage(p, color + "[" + tcolor + playerDb.Rows[0]["Title"] + color + "] " + message + Server.DefaultColor + " has :");
            Player.SendMessage(p, "> > the rank of " + Group.Find(FoundRank).color + FoundRank);
            try
            {
                if (!Group.Find("Nobody").commands.Contains("pay") && !Group.Find("Nobody").commands.Contains("give") && !Group.Find("Nobody").commands.Contains("take")) Player.SendMessage(p, "> > &a" + playerDb.Rows[0]["Money"] + Server.DefaultColor + " " + Server.moneys);
            }
            catch { }
            Player.SendMessage(p, "> > &cdied &a" + playerDb.Rows[0]["TotalDeaths"] + Server.DefaultColor + " times");
            Player.SendMessage(p, "> > &bmodified &a" + playerDb.Rows[0]["totalBlocks"] + " &eblocks.");
            Player.SendMessage(p, "> > was last seen on &a" + playerDb.Rows[0]["LastLogin"]);
            Player.SendMessage(p, "> > " + TotalTime(playerDb.Rows[0]["TimeSpent"].ToString()));
            Player.SendMessage(p, "> > first logged into the server on &a" + playerDb.Rows[0]["FirstLogin"]);
            Player.SendMessage(p, "> > logged in &a" + playerDb.Rows[0]["totalLogin"] + Server.DefaultColor + " times, &c" + playerDb.Rows[0]["totalKicked"] + Server.DefaultColor + " of which ended in a kick.");
            Player.SendMessage(p, "> > " + Awards.awardAmount(message) + " awards");
            if (Ban.Isbanned(message))
            {
                string[] data = Ban.Getbandata(message);
                Player.SendMessage(p, "> > was banned by " + data[0] + " for " + data[1] + " on " + data[2]);
            }

            if (Server.Devs.Contains(message.ToLower()))
            {
                Player.SendMessage(p, Server.DefaultColor + "> > Player is a &9Developer");
                if (Server.forgeProtection == ForgeProtection.Mod || Server.forgeProtection == ForgeProtection.Dev)
                    Player.SendMessage(p, Server.DefaultColor + "> > Player is &CPROTECTED" + Server.DefaultColor + " under MCForge Staff protection");
            }
            else if (Server.Mods.Contains(message.ToLower()))
            {
                Player.SendMessage(p, Server.DefaultColor + "> > Player is a &9MCForge Moderator");
                if (Server.forgeProtection == ForgeProtection.Mod)
                    Player.SendMessage(p, Server.DefaultColor + "> > Player is &CPROTECTED" + Server.DefaultColor + " under MCForge Staff protection");
            }
            else if (Server.GCmods.Contains(message.ToLower()))
                Player.SendMessage(p, Server.DefaultColor + "> > Player is a &9Global Chat Moderator");

            if (!(p != null && (int)p.group.Permission <= CommandOtherPerms.GetPerm(this)))
            {
                if (Server.bannedIP.Contains(playerDb.Rows[0]["IP"].ToString()))
                    playerDb.Rows[0]["IP"] = "&8" + playerDb.Rows[0]["IP"] + ", which is banned";
                Player.SendMessage(p, "> > the IP of " + playerDb.Rows[0]["IP"]);
                if (Server.useWhitelist)
                {
                    if (Server.whiteList.Contains(message.ToLower()))
                    {
                        Player.SendMessage(p, "> > Player is &fWhitelisted");
                    }
                }
            }
            playerDb.Dispose();
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/whowas <name> - Displays information about someone who left.");
        }
        public string TotalTime(string time)
        {
            return "time spent on server: " + time.Split(' ')[0] + " Days, " + time.Split(' ')[1] + " Hours, " + time.Split(' ')[2] + " Minutes, " + time.Split(' ')[3] + " Seconds.";
        }
    }
}