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

using System;
namespace MCForge.Commands
{
    public sealed class CmdRankMsg : Command
    {
        public override string name { get { return "rankmsg"; } }
        public override string shortcut { get { return "rm"; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.AdvBuilder; } }
        public CmdRankMsg() { }

        public override void Use(Player p, string message)
        {
            string[] command = message.ToLower().Split(' ');
            string msg1 = String.Empty;
            string msg2 = String.Empty;
            try
            {
                msg1 = command[0];
                msg2 = command[1];
            }
            catch
            { }
            if (msg1 == "")
            {
                Help(p);
                return;
            }
            if (msg2 == "")
            {
                Command.all.Find("rankmsg").Use(p, p.group.name + " " + msg1);
                return;
            }
            Group findgroup = Group.Find(msg1);
            if (findgroup == null)
            {
                Player.SendMessage(p, "Could not find group specified!");
                return;
            }
            foreach (Player pl in Player.players)
            {
                if (pl.group.name == findgroup.name)
                {
                    pl.SendMessage(p.color + p.name + ": " + Server.DefaultColor + (message.Replace(msg1, "").Trim()));
                }
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/rankmsg [Rank] [Message] - Sends a message to the specified rank.");
            Player.SendMessage(p, "Note: If no message is given, player's rank is taken.");
        }
    }
}
