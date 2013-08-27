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
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using MCForge.SQL;
namespace MCForge.Commands {
    public sealed class CmdBanip : Command {
        public override string name { get { return "banip"; } }
        public override string shortcut { get { return "bi"; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdBanip() { }

        public override void Use(Player p, string message) {
            if (String.IsNullOrEmpty(message.Trim())) { Help(p); return; }
            if (!Regex.IsMatch(message.ToLower(), @".*%([0-9]|[a-f]|[k-r])%([0-9]|[a-f]|[k-r])%([0-9]|[a-f]|[k-r])")) {
                if (Regex.IsMatch(message.ToLower(), @".*%([0-9]|[a-f]|[k-r])(.+?).*")) {
                    Regex rg = new Regex(@"%([0-9]|[a-f]|[k-r])(.+?)");
                    MatchCollection mc = rg.Matches(message.ToLower());
                    if (mc.Count > 0) {
                        Match ma = mc[0];
                        GroupCollection gc = ma.Groups;
                        message.Replace("%" + gc[1].ToString().Substring(1), "&" + gc[1].ToString().Substring(1));
                    }
                }
            }
            string name = "";
            if (message[0] == '@') {
                message = message.Remove(0, 1).Trim();
                Player who = Player.Find(message);

                if (who == null) {
                    DataTable ip;
                    int tryCounter = 0;
                rerun: try {
                        Database.AddParams("@Name", message);
                        ip = Database.fillData("SELECT IP FROM Players WHERE Name = @Name");
                    } catch (Exception e) {
                        tryCounter++;
                        if (tryCounter < 10)
                            goto rerun;
                        else {
                            Server.ErrorLog(e);
                            return;
                        }
                    }
                    if (ip.Rows.Count > 0) {
                        name = message.ToLower();
                        message = ip.Rows[0]["IP"].ToString();
                    } else {
                        Player.SendMessage(p, "Unable to find an IP address for that user.");
                        return;
                    }
                    ip.Dispose();
                } else {
                    name = who.name.ToLower();
                    message = who.ip;
                }
            } else {
                Player who = Player.Find(message);
                if (who != null) {
                    name = who.name.ToLower();
                    message = who.ip;
                }
            }

            if (Server.Devs.Contains(name) && (Server.forgeProtection == ForgeProtection.Mod || Server.forgeProtection == ForgeProtection.Dev))
                return;
            if (Server.Mods.Contains(name) && Server.forgeProtection == ForgeProtection.Mod)
                return;

            if (message.Equals("127.0.0.1")) { Player.SendMessage(p, "You can't ip-ban the server!"); return; }
            if (message.IndexOf('.') == -1) { Player.SendMessage(p, "Invalid IP!"); return; }
            if (message.Split('.').Length != 4) { Player.SendMessage(p, "Invalid IP!"); return; }
            if (p != null && p.ip == message) { Player.SendMessage(p, "You can't ip-ban yourself.!"); return; }
            if (Server.bannedIP.Contains(message)) { Player.SendMessage(p, message + " is already ip-banned."); return; }

            // Check if IP belongs to an op+
            // First get names of active ops+ with that ip
            List<string> opNamesWithThatIP = (from pl in Player.players where (pl.ip == message && pl.@group.Permission >= LevelPermission.Operator) select pl.name).ToList();
            // Next, add names from the database
            Database.AddParams("@IP", message);
            DataTable dbnames = Database.fillData("SELECT Name FROM Players WHERE IP = @IP");

            foreach (DataRow row in dbnames.Rows) {
                opNamesWithThatIP.Add(row[0].ToString());
            }
            dbnames.Dispose();

            if (opNamesWithThatIP != null && opNamesWithThatIP.Count > 0) {
                // We have at least one op+ with a matching IP
                // Check permissions of everybody who matched that IP
                foreach (string opname in opNamesWithThatIP) {
                    // Console can ban anybody else, so skip this section
                    if (p != null) {
                        // If one of these guys matches a player with a higher rank don't allow the ipban to proceed! 
                        Group grp = Group.findPlayerGroup(opname);
                        if (grp != null) {
                            if (grp.Permission >= p.group.Permission) {
                                Player.SendMessage(p, "You can only ipban IPs used by players with a lower rank.");
                                Player.SendMessage(p, Server.DefaultColor + opname + "(" + grp.color + grp.name + Server.DefaultColor + ") uses that IP.");
                                Server.s.Log(p.name + "failed to ipban " + message + " - IP is also used by: " + opname + "(" + grp.name + ")");
                                return;
                            }
                        }
                    }
                }
            }



            if (p != null) {
                Server.IRC.Say(message.ToLower() + " was ip-banned by " + p.name + ".");
                Server.s.Log("IP-BANNED: " + message.ToLower() + " by " + p.name + ".");
                Player.GlobalMessage(message + " was &8ip-banned" + Server.DefaultColor + " by " + p.color + p.name + Server.DefaultColor + ".");
            } else {
                Server.IRC.Say(message.ToLower() + " was ip-banned by console.");
                Server.s.Log("IP-BANNED: " + message.ToLower() + " by console.");
                Player.GlobalMessage(message + " was &8ip-banned" + Server.DefaultColor + " by console.");
            }
            Server.bannedIP.Add(message);
            Server.bannedIP.Save("banned-ip.txt", false);

            /*
            foreach (Player pl in Player.players) {
                if (message == pl.ip) { pl.Kick("Kicked by ipban"); }
            }*/
        }
        public override void Help(Player p) {
            Player.SendMessage(p, "/banip <ip/name> - Bans an ip. Also accepts a player name when you use @ before the name.");
        }
    }
}