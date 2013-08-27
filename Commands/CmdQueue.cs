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
    public sealed class CmdQueue : Command
    {
        public override string name { get { return "queue"; } }
        public override string shortcut { get { return "qz"; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdQueue() { }

        public override void Use(Player p, string message)
        {
            int number = message.Split(' ').Length;
            if (number > 2) { Help(p); return; }
            if (number == 2)
            {
                Server.s.Log(message);
                string t = message.Split(' ')[0];
                string s = message.Split(' ')[1];
                if (t == "zombie")
                {
                    bool asdfasdf = false;
                    Player.players.ForEach(delegate(Player player)
                    {
                        if (player.name == s)
                        {
                            p.SendMessage(s + " was queued.");
                            Server.queZombie = true;
                            Server.nextZombie = s;
                            asdfasdf = true;
                            return;
                        }
                    });
                    if (!asdfasdf)
                    {
                        p.SendMessage(s + " is not online.");
                        return;
                    }
                }
                else if (t == "level")
                {
                    bool yes = false;
                    DirectoryInfo di = new DirectoryInfo("levels/");
                    FileInfo[] fi = di.GetFiles("*.lvl");
                    foreach (FileInfo file in fi)
                    {
                        if (file.Name.Replace(".lvl", "").ToLower().Equals(s.ToLower()))
                        {
                            yes = true;
                        }
                    }
                    if (yes)
                    {
                        p.SendMessage(s + " was queued.");
                        Server.queLevel = true;
                        Server.nextLevel = s.ToLower();
                        return;
                    }
                    else
                    {
                        p.SendMessage("Level does not exist.");
                        return;
                    }
                }
                else
                {
                    p.SendMessage("You did not enter a valid option.");
                }
            }
        }

        public override void Help(Player p)
        {
            Player.SendMessage(p, "/queue zombie [name] - Next round [name] will be infected");
            Player.SendMessage(p, "/queue level [name] - Next round [name] will be the round loaded");
        }
    }
}