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
    public sealed class CmdJoker : Command
    {
        public override string name { get { return "joker"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public static string keywords { get { return ""; } }
        public CmdJoker() { }

        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }
            bool stealth = false;
            if (message[0] == '#')
            {
                message = message.Remove(0, 1).Trim();
                stealth = true;
                Server.s.Log("Stealth joker attempted");
            }

            Player who = Player.Find(message);
            if (who == null)
            {
                Player.SendMessage(p, "Could not find player.");
                return;
            }
            if (p != null && who.group.Permission > p.group.Permission) { Player.SendMessage(p, "Cannot joker someone of equal or greater rank."); return; }

            if (!who.joker)
            {
                who.joker = true;
                if (stealth) { Player.GlobalMessageOps(who.color + who.name + Server.DefaultColor + " is now STEALTH joker'd. "); return; }
                Player.GlobalChat(who, who.color + who.name + Server.DefaultColor + " is now a &aJ&bo&ck&5e&9r" + Server.DefaultColor + ".", false);
            }
            else
            {
                who.joker = false;
                if (stealth) { Player.GlobalMessageOps(who.color + who.name + Server.DefaultColor + " is now STEALTH Unjoker'd. "); return; }
                Player.GlobalChat(who, who.color + who.name + Server.DefaultColor + " is no longer a &aJ&bo&ck&5e&9r" + Server.DefaultColor + ".", false);
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/joker <name> - Causes a player to become a joker!");
            Player.SendMessage(p, "/joker # <name> - Makes the player a joker silently");
            return;
        }
    }
}
