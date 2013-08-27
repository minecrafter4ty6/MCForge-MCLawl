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
    public sealed class CmdPerbuildMax : Command
    {
        public override string name { get { return "perbuildmax"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "pbm"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdPerbuildMax() { }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/perbuildmax [Level] [Rank] - Sets the highest rank able to visit [Level].");
        }

        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }
            int number = message.Split(' ').Length;
            if (number > 2 || number < 1) { Help(p); return; }
            if (number == 1)
            {
                LevelPermission Perm = Level.PermissionFromName(message);
                if (Perm == LevelPermission.Null) { Player.SendMessage(p, "Not a valid rank"); return; }
                if (p.level.perbuildmax > p.group.Permission)
                {
                    if (p.level.perbuildmax != LevelPermission.Nobody)
                    {
                        Player.SendMessage(p, "You cannot change the perbuildmax of a level with a perbuildmax higher than your rank.");
                        return;
                    }
                }
                p.level.perbuildmax = Perm;
                Level.SaveSettings(p.level);
                Server.s.Log(p.level.name + " buildmax permission changed to " + message + ".");
                Player.GlobalMessageLevel(p.level, "buildmax permission changed to " + message + ".");
            }
            else
            {
                int pos = message.IndexOf(' ');
                string t = message.Substring(0, pos).ToLower();
                string s = message.Substring(pos + 1).ToLower();
                LevelPermission Perm = Level.PermissionFromName(s);
                if (Perm == LevelPermission.Null) { Player.SendMessage(p, "Not a valid rank"); return; }

                Level level = Level.Find(t);
                if (level.perbuildmax > p.group.Permission)
                {
                    if (level.perbuildmax != LevelPermission.Nobody)
                    {
                        Player.SendMessage(p, "You cannot change the perbuildmax of a level with a perbuildmax higher than your rank.");
                        return;
                    }
                }
                if (level != null)
                {
                    level.perbuildmax = Perm;
                    Level.SaveSettings(level);
                    Server.s.Log(level.name + " buildmax permission changed to " + s + ".");
                    Player.GlobalMessageLevel(level, "buildmax permission changed to " + s + ".");
                    if (p != null)
                        if (p.level != level) { Player.SendMessage(p, "buildmax permission changed to " + s + " on " + level.name + "."); }
                    return;
                }
                else
                    Player.SendMessage(p, "There is no level \"" + s + "\" loaded.");
            }
        }
    }
}