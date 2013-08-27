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
    public sealed class CmdP2P : Command
    {
        public override string name { get { return "p2p"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdP2P() { }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/p2p [Player1] [Player2] - Teleports player 1 to player 2.");
        }

        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }
            int number = message.Split(' ').Length;
            if (number > 2) { Help(p); return; }
            if (number == 2)
            {
                int pos = message.IndexOf(' ');
                string t = message.Substring(0, pos).ToLower();
                string s = message.Substring(pos + 1).ToLower();
                Player who = Player.Find(t);
                Player who2 = Player.Find(s);
                if (who == null)
                {
                    if (who2 == null)
                    {
                        Player.SendMessage(p, "Neither of the players you specified, can be found or exist!");
                        return;
                    }
                    Player.SendMessage(p, "Player 1 is not online or does not exist!");
                    return;
                }
                if (who2 == null)
                {
                    Player.SendMessage(p, "Player 2 is not online or does not exist!");
                    return;
                }
                if (who == p)
                {
                    if (who2 == p)
                    {
                        Player.SendMessage(p, "Why are you trying to teleport yourself to yourself? =S");
                        return;
                    }
                    Player.SendMessage(p, "Why not, just use /tp " + who2.name + "!");
                }
                if (who2 == p)
                {
                    Player.SendMessage(p, "Why not, just use /summon " + who.name + "!");
                }
                if (p.group.Permission < who.group.Permission)
                {
                    Player.SendMessage(p, "You cannot force a player of higher rank to tp to another player!");
                    return;
                }
                if (s == "")
                {
                    Player.SendMessage(p, "You did not specify player 2!");
                    return;
                }
                Command.all.Find("tp").Use(who, who2.name);
                Player.SendMessage(p, who.name + " has been successfully teleported to " + who2.name + "!");
            }

            if (number == 1)
            {
                Player.SendMessage(p, "You did not specify player 2!");
                return;
            }
        }
    }
}