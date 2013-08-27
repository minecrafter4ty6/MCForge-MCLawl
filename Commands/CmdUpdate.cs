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
    public sealed class CmdUpdate : Command
    {
        public override string name { get { return "update"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "information"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.AdvBuilder; } }
        public CmdUpdate() { }

        public override void Use(Player p, string message)
        {
            if (message.ToLower() != "force" && message.ToLower() != "help")
            {
                if (p == null || p.group.Permission > defaultRank) MCForge_.Gui.Program.UpdateCheck(false, p);
                else Player.SendMessage(p, "Ask an " + Group.findPerm(defaultRank).name + "+ to do it!");
            }
            else if (message.ToLower() == "help")
            {
                Help(p);
                return;
            }
            else
            {
                if (p == null || p.group.Permission > defaultRank) MCForge_.Gui.Program.PerformUpdate();
                else Player.SendMessage(p, "Ask an " + Group.findPerm(defaultRank).name + "+ to do it!");
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/update - Updates the server if it's out of date");
            Player.SendMessage(p, "/update force - Forces the server to update");
        }
    }
}