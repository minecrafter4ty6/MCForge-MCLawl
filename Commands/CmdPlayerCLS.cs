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
    public sealed class CmdPlayerCLS : Command
    {
        public override string name { get { return "playercls"; } }
        public override string shortcut { get { return "cls"; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }

        public override void Use(Player p, string message)
        {
           int i = 0;
           for (i = 0; i < 20; i++)
           {
               BlankMessage(p);
           }
           Player.SendMessage(p, "%4Chat cleared.");
        }
        //Yes this does work
        //Trust me...I'm a doctor
        public void BlankMessage(Player p)
        {
            byte[] buffer = new byte[65];
            Player.StringFormat(" ", 64).CopyTo(buffer, 1);
            p.SendRaw(13, buffer);
            buffer = null;
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/playercls - Clears your chat.");
        }
    }
}