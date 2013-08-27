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
    public sealed class CmdWarn : Command
    {
        public override string name { get { return "warn"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Builder; } }
        string reason;

        public override void Use(Player p, string message)
        {
            string warnedby;

            if (message == "") { Help(p); return; }

            Player who = Player.Find(message.Split(' ')[0]);

            // Make sure we have a valid player
            if (who == null)
            {
                Player.SendMessage(p, "Player not found!");
                return;
            }

            // Don't warn yourself... derp
            if (who == p)
            {
                Player.SendMessage(p, "you can't warn yourself");
                return;
            }

            // Check the caller's rank
            if (p != null && p.group.Permission <= who.group.Permission)
            {
                Player.SendMessage(p, "you can't warn a player equal or higher rank.");
                return;
            }
            
            // We need a reason
            if (message.Split(' ').Length == 1)
            {
                // No reason was given
                reason = "you know why.";
            }
            else
            {
                reason = message.Substring(message.IndexOf(' ') + 1).Trim();
            }

            warnedby = (p == null) ? "<CONSOLE>" : p.color + p.name;
            Player.GlobalMessage(warnedby + " %ewarned " + who.color + who.name + " %ebecause:");
            Player.GlobalMessage("&c" + reason);

            //Player.SendMessage(who, "Do it again ");
            if (who.warn == 0)
            {
                Player.SendMessage(who, "Do it again twice and you will get kicked!");
                who.warn = 1;
                return;
            }
            if (who.warn == 1)
            {
                Player.SendMessage(who, "Do it one more time and you will get kicked!");
                who.warn = 2;
                return;
            }
            if (who.warn == 2)
            {
                Player.GlobalMessage(who.color + who.name + " " + Server.DefaultColor + "was warn-kicked by " + warnedby);
                who.warn = 0;
                who.Kick("KICKED BECAUSE " + reason + "");
                return;
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/warn <player> - Warns a player.");
            Player.SendMessage(p, "Player will get kicked after 3 warnings.");
        }
    }
}