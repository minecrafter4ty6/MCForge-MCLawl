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
    /// <summary>
    /// This is the command /infected
    /// use /help infected in-game for more info
    /// </summary>
    public sealed class CmdInfected : Command
    {
        public override string name { get { return "infected"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "game"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
        public CmdInfected() { }
        public override void Use(Player p, string message)
        {
            Player who = null;
            if (message == "") { who = p; message = p.name; } else { who = Player.Find(message); }
            if (ZombieGame.infectd.Count == 0)
            {
                Player.SendMessage(p, "No one is infected");
            }
            else
            {
                Player.SendMessage(p, "Players who are " + c.red + "infected " + Server.DefaultColor + "are:");
                string playerstring = "";
                ZombieGame.infectd.ForEach(delegate(Player player)
                {
                    playerstring = playerstring + c.red + player.name + Server.DefaultColor + ", ";
                });
                Player.SendMessage(p, playerstring);
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/infected - shows who is infected");
        }
    }
}