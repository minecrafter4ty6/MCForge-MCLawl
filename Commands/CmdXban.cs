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
    public sealed class CmdXban : Command
    {
        public override string name { get { return "xban"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdXban() { }
        public override void Use(Player p, string message)
        {

            if (message == "") { Help(p); return; }

            Player who = Player.Find(message.Split(' ')[0]);
            string msg = message.Split(' ')[0];
            if (p != null)
            {
                p.ignorePermission = true;
            }
            try
            {
                if (who != null)
                {
                    Command.all.Find("xundo").Use(p, msg);
                    Command.all.Find("ban").Use(p, msg);
                    Command.all.Find("banip").Use(p, "@" + msg);
                    Command.all.Find("kick").Use(p, message);
                    Command.all.Find("xundo").Use(p, msg);

                }
                else
                {
                    Command.all.Find("ban").Use(p, msg);
                    Command.all.Find("banip").Use(p, "@" + msg);
                    Command.all.Find("xundo").Use(p, msg);
                }

            }
            finally
            {
                if (p != null) p.ignorePermission = false;
            }



        }


        public override void Help(Player p)
        {
            Player.SendMessage(p, "/xban [name] [message]- Bans, banips, undoes, and kicks [name] with [message], if specified.");
        }
    }
}