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

using System.IO;
namespace MCForge.Commands
{
    public sealed class CmdBanlist : Command
    {
        public override string name { get { return "banlist"; } }
        public override string shortcut { get { return "bl"; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
        public CmdBanlist() { }

        public override void Use(Player p, string message)
        {
            string endresult = "";
            foreach (string line in File.ReadAllLines("ranks/banned.txt"))
            {
                if (Ban.Isbanned(line))
                {
                    endresult = endresult + "&a" + line + Server.DefaultColor + ", ";
                }
                else
                {
                    endresult = endresult + "&c" + line + Server.DefaultColor + ", ";
                }
            }
            if (endresult == "")
            {
                Player.SendMessage(p, "There are no players banned");
            }
            else
            {
                endresult = endresult.Remove(endresult.Length - 2, 2);
                endresult = "&9Banned players: " + Server.DefaultColor + endresult + Server.DefaultColor + ".";
                Player.SendMessage(p, endresult);
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/banlist - shows who's banned on server");
        }
    }
}