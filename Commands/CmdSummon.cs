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
using System.Threading;
namespace MCForge.Commands
{
    public sealed class CmdSummon : Command
    {
        public override string name { get { return "summon"; } }
        public override string shortcut { get { return "s"; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.AdvBuilder; } }
        public CmdSummon() { }

        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }
            if (p == null) { Player.SendMessage(p, "You cannot use this command from the console"); return; }
            if (message.ToLower() == "all")
            {
                try
                {
                    foreach (Player pl in Player.players)
                    {
                        if (pl.level == p.level && pl != p && p.group.Permission > pl.group.Permission)
                        {
                            unchecked { pl.SendPos((byte)-1, p.pos[0], p.pos[1], p.pos[2], p.rot[0], 0); }
                            pl.SendMessage("You were summoned by " + p.color + p.name + Server.DefaultColor + ".");
                        }
                    }
                }
                catch (Exception e)
                {
                    Server.ErrorLog(e);
                }
                Player.GlobalMessage(p.color + p.name + Server.DefaultColor + " summoned everyone!");
                return;
            }

            Player who = Player.Find(message);
            if (who == null || who.hidden) { Player.SendMessage(p, "There is no player \"" + message + "\"!"); return; }
            if (p.group.Permission < who.group.Permission)
            {
                Player.SendMessage(p, "You cannot summon someone ranked higher than you!");
                return;
            }
            if (p.level != who.level)
            {
                Player.SendMessage(p, who.name + " is in a different Level. Forcefetching has started!");
                Level where = p.level;
                Command.all.Find("goto").Use(who, where.name);
                Thread.Sleep(1000);
                // Sleep for a bit while they load
                while (who.Loading) { Thread.Sleep(250); }
            }

            unchecked { who.SendPos((byte)-1, p.pos[0], p.pos[1], p.pos[2], p.rot[0], 0); }
            who.SendMessage("You were summoned by " + p.color + p.name + Server.DefaultColor + ".");
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/summon <player> - Summons a player to your position.");
            Player.SendMessage(p, "/summon all - Summons all players in the map");
        }
    }
}