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
namespace MCForge.Commands {
    public sealed class CmdBan : Command {
        public override string name { get { return "ban"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdBan() { }

        public override void Use(Player p, string message) {
            try {
                if (message == "") { Help(p); return; }
                bool stealth = false; bool totalBan = false;
                if (message[0] == '#') {
                    message = message.Remove(0, 1).Trim();
                    stealth = true;
                    Server.s.Log("Stealth Ban Attempted by " + p == null ? "Console" : p.name);
                } else if (message[0] == '@') {
                    if (p == null) {
                        message = message.Remove(0, 1).Trim();
                        stealth = true;
                        Server.s.Log("Total Ban Attempted by Console");
                    } else {
                        totalBan = true;
                        message = message.Remove(0, 1).Trim();
                        Server.s.Log("Total Ban Attempted by " + p.name);
                    }
                }
                string reason = "-";
                if (message.Split(' ').Length > 1) {
                    reason = message;
                    string newreason = reason.Remove(0, reason.Split(' ')[0].Length + 1);
                    int removetrim = newreason.Length + 1;
                    string newmessage = message.Remove(message.Length - removetrim, removetrim);
                    reason = newreason;
                    message = newmessage;
                }
                if (reason == "-") {
                    reason = "&c-";
                }
                reason = reason.Replace(" ", "%20");


                Player who = Player.Find(message);

                if (who == null) {
                    if (!Player.ValidName(message)) {
                        Player.SendMessage(p, "Invalid name \"" + message + "\".");
                        return;
                    }

                    Group foundGroup = Group.findPlayerGroup(message);

                    if ((int)foundGroup.Permission >= CommandOtherPerms.GetPerm(this)) {
                        Player.SendMessage(p, "You can't ban players ranked " + CommandOtherPerms.GetPerm(this) + " or higher!");
                        return;
                    }
                    if (foundGroup.Permission == LevelPermission.Banned) {
                        Player.SendMessage(p, message + " is already banned.");
                        return;
                    }
                    if (p != null && foundGroup.Permission >= p.group.Permission) {
                        Player.SendMessage(p, "You cannot ban a person ranked equal or higher than you.");
                        return;
                    }
                    string oldgroup = foundGroup.name.ToString();
                    foundGroup.playerList.Remove(message);
                    foundGroup.playerList.Save();
                    if (p != null) {
                        Player.GlobalMessage(message + " &f(offline)" + Server.DefaultColor + " was &8banned" + Server.DefaultColor + " by " + p.color + p.name + Server.DefaultColor + ".");
                    } else {
                        Player.GlobalMessage(message + " &f(offline)" + Server.DefaultColor + " was &8banned" + Server.DefaultColor + " by console.");
                    }
                    Group.findPerm(LevelPermission.Banned).playerList.Add(message);
                    Ban.Banplayer(p, message.ToLower(), reason, stealth, oldgroup);
                } else {
                    if (!Player.ValidName(who.name)) {
                        Player.SendMessage(p, "Invalid name \"" + who.name + "\".");
                        return;
                    }
                    if ((int)who.group.Permission >= CommandOtherPerms.GetPerm(this)) {
                        Player.SendMessage(p, "You can't ban players ranked " + Group.findPermInt(CommandOtherPerms.GetPerm(this)).name + " or higher!");
                        return;
                    }
                    if (who.group.Permission == LevelPermission.Banned) {
                        Player.SendMessage(p, message + " is already banned.");
                        return;
                    }
                    if (p != null && who.group.Permission >= p.group.Permission) {
                        Player.SendMessage(p, "You cannot ban a person ranked equal or higher than you.");
                        return;
                    }
                    string oldgroup = who.group.name.ToString();
                    who.group.playerList.Remove(message);
                    who.group.playerList.Save();

                    if (p != null) {
                        if (stealth) Player.GlobalMessageOps(who.color + who.name + Server.DefaultColor + " was STEALTH &8banned" + Server.DefaultColor + " by " + p.color + p.name + Server.DefaultColor + "!");
                        else Player.GlobalMessage(who.color + who.name + Server.DefaultColor + " was &8banned" + Server.DefaultColor + " by " + p.color + p.name + Server.DefaultColor + "!");
                    } else {
                        if (stealth) Player.GlobalMessageOps(who.color + who.name + Server.DefaultColor + " was STEALTH &8banned" + Server.DefaultColor + " by console.");
                        else Player.GlobalMessage(who.color + who.name + Server.DefaultColor + " was &8banned" + Server.DefaultColor + " by console.");
                    }
                    who.group = Group.findPerm(LevelPermission.Banned);
                    who.color = who.group.color;
                    Player.GlobalDie(who, false);
                    Player.GlobalSpawn(who, who.pos[0], who.pos[1], who.pos[2], who.rot[0], who.rot[1], false);
                    Group.findPerm(LevelPermission.Banned).playerList.Add(who.name);
                    Ban.Banplayer(p, who.name.ToLower(), reason, stealth, oldgroup);
                }
                Group.findPerm(LevelPermission.Banned).playerList.Save();

                if (p != null) {
                    Server.IRC.Say(message + " was banned by " + p.name + ".");
                    Server.s.Log("BANNED: " + message.ToLower() + " by " + p.name);
                } else {
                    Server.IRC.Say(message + " was banned by console.");
                    Server.s.Log("BANNED: " + message.ToLower() + " by console.");
                }

                if (totalBan == true) {
                    Command.all.Find("undo").Use(p, message + " 0");
                    Command.all.Find("banip").Use(p, "@ " + message);
                }
            } catch (Exception e) { Server.ErrorLog(e); }
        }
        public override void Help(Player p) {
            Player.SendMessage(p, "/ban <player> [reason] - Bans a player without kicking him.");
            Player.SendMessage(p, "Add # before name to stealth ban.");
            Player.SendMessage(p, "Add @ before name to total ban.");
        }
    }
}