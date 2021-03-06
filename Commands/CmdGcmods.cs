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
    public sealed class CmdGcmods : Command
    {
        public override string name { get { return "gcmods"; } }
        public override string shortcut { get { return "gcmod"; } }
        public override string type { get { return "information"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
        public CmdGcmods() { }

        public override void Use(Player p, string message)
        {
            if (message != "") { Help(p); return; }
            string gcmodlist = "";
            string tempgcmods;
            foreach (string gcmod in Server.GCmods)
            {
                tempgcmods = gcmod.Substring(0, 1);
                tempgcmods = tempgcmods.ToUpper() + gcmod.Remove(0, 1);
                gcmodlist += tempgcmods + ", ";
            }
            gcmodlist = gcmodlist.Remove(gcmodlist.Length - 2);
            Player.SendMessage(p, "&9MCForge Global Chat Moderation Team: " + Server.DefaultColor + gcmodlist + "&e.");
        }

        public override void Help(Player p)
        {
            Player.SendMessage(p, "/gcmods - Displays the list of MCForge global chat moderators.");
        }
    }
}