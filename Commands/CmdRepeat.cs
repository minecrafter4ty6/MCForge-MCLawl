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
    public sealed class CmdRepeat : Command
    {
        public override string name { get { return "repeat"; } }
        public override string shortcut { get { return "m"; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }
        public CmdRepeat() { }

        public override void Use(Player p, string message)
        {
            try
            {
                if (p.lastCMD == "") { Player.SendMessage(p, "No commands used yet."); return; }
                if (p.lastCMD.Length > 5)
                    if (p.lastCMD.Substring(0, 6) == "static") { Player.SendMessage(p, "Can't repeat static"); return; }

                Player.SendMessage(p, "Using &b/" + p.lastCMD);

                if (p.lastCMD.IndexOf(' ') == -1)
                {
                    Command.all.Find(p.lastCMD).Use(p, "");
                }
                else
                {
                    Command.all.Find(p.lastCMD.Substring(0, p.lastCMD.IndexOf(' '))).Use(p, p.lastCMD.Substring(p.lastCMD.IndexOf(' ') + 1));
                }
            }
            catch { Player.SendMessage(p, "An error occured!"); }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/repeat - Repeats the last used command");
        }
    }
}