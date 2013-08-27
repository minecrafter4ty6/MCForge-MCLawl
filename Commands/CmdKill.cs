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
    public sealed class CmdKill : Command
    {
        public override string name { get { return "kill"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdKill() { }

        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }

            Player who; string killMsg; int killMethod = 0;
            if (message.IndexOf(' ') == -1)
            {
                who = Player.Find(message);
                if (p != null)
                {
                    killMsg = " was killed by " + p.color + p.name;
                }
                else
                {
                    killMsg = " was killed by " + "the Console.";
                }
            }
            else
            {
                who = Player.Find(message.Split(' ')[0]);
                message = message.Substring(message.IndexOf(' ') + 1);

                if (message.IndexOf(' ') == -1)
                {
                    if (message.ToLower() == "explode")
                    {
                        if (p != null)
                        {
                            killMsg = " was exploded by " + p.color + p.name;
                        }
                        else
                        {
                            killMsg = " was exploded by the Console.";
                        }
                        killMethod = 1;
                    }
                    else
                    {
                        killMsg = " " + message;
                    }
                }
                else
                {
                    if (message.Split(' ')[0].ToLower() == "explode")
                    {
                        killMethod = 1;
                        message = message.Substring(message.IndexOf(' ') + 1);
                    }

                    killMsg = " " + message;
                }
            }

            if (who == null)
            {
                if (p != null)
                {
                    p.HandleDeath(Block.rock, " killed itself in its confusion");
                }
                Player.SendMessage(p, "Could not find player");
                return;
            }

            if (p != null)
            {
                if (who.group.Permission > p.group.Permission)
                {
                    p.HandleDeath(Block.rock, " was killed by " + who.color + who.name);
                    Player.SendMessage(p, "Cannot kill someone of higher rank");
                    return;
                }
            }

            if (killMethod == 1)
                who.HandleDeath(Block.rock, killMsg, true);
            else
                who.HandleDeath(Block.rock, killMsg);
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/kill <name> [explode] <message>");
            Player.SendMessage(p, "Kills <name> with <message>. Causes explosion if [explode] is written");
        }
    }
}