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
    public sealed class CmdCompLoad : Command
    {
        public override string name { get { return "compload"; } }
        public override string shortcut { get { return "cml"; } }
        public override string type { get { return "other"; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Admin; } }
        public override bool museumUsable { get { return true; } }
        public override void Help(Player p) { Player.SendMessage(p, "/compload <command> - Compiles AND loads <command> for use (shortcut = /cml)"); }
        public override void Use(Player p, string message)
        {
            string[] param = message.Split(' ');
            if (message == "") { Help(p); return; }

            if (param.Length == 1)
            {
                Command.all.Find("compile").Use(p, message);
                Command.all.Find("cmdload").Use(p, message);
                Command.all.Find("help").Use(p, message);
                return;
            }
            else if (param[1] == "vb")
            {
                Command.all.Find("compile").Use(p, message + "vb");
                Command.all.Find("cmdload").Use(p, message + "vb");
                Command.all.Find("help").Use(p, message);
                return;
            }
            else { Help(p); return; }
        }
    }
}