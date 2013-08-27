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
    public sealed class CmdXundo : Command
    {
        public override string name { get { return "xundo"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }

        public CmdXundo() { }
        public override void Use(Player p, string message)
        {

            if (message == "") { Help(p); return; }
            int number = message.Split(' ').Length;
            if (number != 1) { Help(p); return; }

            Player who = Player.Find(message);

            string error = "You are not allowed to undo this player";

            if (who == null || p == null || !(who.group.Permission >= LevelPermission.Operator && p.group.Permission < LevelPermission.Operator))
            {
                //This executes if who doesn't exist, if who is lower than Operator, or if the user is an op+.
                //It also executes on any combination of the three
                Command.all.Find("undo").Use(p, ((who == null) ? message : who.name) + " all"); //Who null check
                return;
            }
            Player.SendMessage(p, error);
        }


        public override void Help(Player p)
        {
            Player.SendMessage(p, "/xundo [name]  -  works as 'undo [name] all' but now anyone can use it (up to their undo limit)");
        }
    }
}