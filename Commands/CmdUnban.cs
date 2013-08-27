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

namespace MCForge.Commands
{
    /// <summary>
    /// BUG: cannot unban while typing player names partially, code is not written optimal
    /// TODO: Fix this bug
    /// </summary>
    public sealed class CmdUnban : Command
    {
        public override string name { get { return "unban"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdUnban() { }

        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }
            bool totalUnban = false;
            if (message[0] == '@')
            {
                totalUnban = true;
                message = message.Remove(0, 1).Trim();
            }

            Player who = Player.Find(message);

            if (who == null)
            {
                if (Group.findPlayerGroup(message) != Group.findPerm(LevelPermission.Banned))
                {
                    foreach (Server.TempBan tban in Server.tempBans)
                    {
                        if (tban.name.ToLower() == message.ToLower())
                        {
                            if (p != null)
                            {
                                Server.tempBans.Remove(tban);
                                Player.GlobalMessage(message + " has had their temporary ban lifted by "+ p.color + p.name + Server.DefaultColor + ".");
                                Server.s.Log("UNBANNED: by " + p.name);
                                Server.IRC.Say(message + " was unbanned by " + p.name + ".");
                                return;
                            }
                            else
                            {
                                Server.tempBans.Remove(tban);
                                Player.GlobalMessage(message + " has had their temporary ban lifted by console.");
                                Server.s.Log("UNBANNED: by console");
                                Server.IRC.Say(message + " was unbanned by console.");
                                return;
                            }

                        }
                    }
                    Player.SendMessage(p, "Player is not banned.");
                    return;
                }
                if (Group.findPlayerGroup(message) == Group.findPerm(LevelPermission.Banned))
                {
                    if (p != null)
                    {
                        Player.GlobalMessage(message + " was &8(unbanned)" + Server.DefaultColor + " by " + p.color + p.name + Server.DefaultColor + ".");
                        Server.s.Log("UNBANNED: by " + p.name);
                        Server.IRC.Say(message + " was unbanned by " + p.name + ".");
                    }
                    else
                    {
                        Player.GlobalMessage(message + " was &8(unbanned)" + Server.DefaultColor + " by console.");
                        Server.s.Log("UNBANNED: by console");
                        Server.IRC.Say(message + " was unbanned by console.");
                    }
                    Group.findPerm(LevelPermission.Banned).playerList.Remove(message);
                    if (Ban.Deleteban(message))
                        Player.SendMessage(p, "deleted ban information about " + message + ".");
                    else Player.SendMessage(p, "no info found about " + message + ".");
                }
            }
            else
            {
                if (Group.findPlayerGroup(message) != Group.findPerm(LevelPermission.Banned))
                {
                    foreach (Server.TempBan tban in Server.tempBans)
                    {
                        if (tban.name == who.name)
                        {
                            if (p != null)
                            {
                                Server.tempBans.Remove(tban);
                                Player.GlobalMessage(message + " has had their temporary ban lifted by " + p.color + p.name + Server.DefaultColor + ".");
                                Server.s.Log("UNBANNED: by " + p.name);
                                Server.IRC.Say(message + " was unbanned by " + p.name + ".");
                                return;
                            }
                            else
                            {
                                Server.tempBans.Remove(tban);
                                Player.GlobalMessage(message + " has had their temporary ban lifted by console.");
                                Server.s.Log("UNBANNED: by console");
                                Server.IRC.Say(message + " was unbanned by console.");
                                return;
                            }
                        }
                    }
                    Player.SendMessage(p, "Player is not banned.");
                    return;
                }
                if (Group.findPlayerGroup(message) == Group.findPerm(LevelPermission.Banned))
                {
                    if (p != null)
                    {
                        Player.GlobalMessage(message + " was &8(unbanned)" + Server.DefaultColor + " by " + p.color + p.name + Server.DefaultColor + ".");
                        Server.s.Log("UNBANNED: by " + p.name);
                        Server.IRC.Say(message + " was unbanned by " + p.name + ".");
                    }
                    else
                    {
                        Player.GlobalMessage(message + " was &8(unbanned)" + Server.DefaultColor + " by console.");
                        Server.s.Log("UNBANNED: by console");
                        Server.IRC.Say(message + " was unbanned by console.");
                    }
                    who.group = Group.standard; who.color = who.group.color; Player.GlobalDie(who, false);
                    Player.GlobalSpawn(who, who.pos[0], who.pos[1], who.pos[2], who.rot[0], who.rot[1], false);
                    Group.findPerm(LevelPermission.Banned).playerList.Remove(message);
                }
            }

            Group.findPerm(LevelPermission.Banned).playerList.Save(); 
            if (totalUnban)
            {
                Command.all.Find("unbanip").Use(p, "@" + message);
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/unban <player> - Unbans a player.  This includes temporary bans.");
        }
    }
}