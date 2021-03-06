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
    public sealed class CmdHigh5 : Command
    {
        public override string name { get { return "high5"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }
        public CmdHigh5() { }
        public override void Use(Player p, string message)
        {
            if (message == "")
            {
                Help(p);
				return;
            }
            Player player1 = Player.Find(message);
            if (player1 == null || player1.hidden == true)
            {
                Player.SendMessage(p, "Could not find player specified.");
				return;
            }

			string giver = (p == null) ? "The Console" : p.name;
			string color = (p == null) ? "" : p.color;
            Player.SendMessage(player1, giver + " just highfived you");
            Player.GlobalMessage(color + giver + " " + Server.DefaultColor + "just highfived " + player1.color + player1.name);
        }

        // This one controls what happens when you use /help [commandname].
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/high5 <player> - High five someone :D");
        }
    }
}