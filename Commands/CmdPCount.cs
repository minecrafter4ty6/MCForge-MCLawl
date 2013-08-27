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

using System.Data;
using MCForge.SQL;
namespace MCForge.Commands
{
    public sealed class CmdPCount : Command
    {
        public override string name { get { return "pcount"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "information"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
        public CmdPCount() { }

        public override void Use(Player p, string message)
        {
            int bancount = Group.findPerm(LevelPermission.Banned).playerList.All().Count;

            DataTable count = Database.fillData("SELECT COUNT(id) FROM Players");
            Player.SendMessage(p, "A total of " + count.Rows[0]["COUNT(id)"] + " unique players have visited this server.");
            Player.SendMessage(p, "Of these players, " + bancount + " have been banned.");
            count.Dispose();

            int playerCount = 0;
            int hiddenCount = 0;
           
            foreach (Player pl in Player.players)
            {
                if (!pl.hidden || p == null || p.group.Permission > LevelPermission.AdvBuilder)
                {
                    playerCount++;
                    if (pl.hidden && pl.group.Permission <= p.group.Permission && (p == null || p.group.Permission > LevelPermission.AdvBuilder))
                    {
                        hiddenCount++;
                    }
                }
            }
            if (playerCount == 1)
            {
                if (hiddenCount == 0)
                {
                    Player.SendMessage(p, "There is 1 player currently online.");
                }
                else
                {
                    Player.SendMessage(p, "There is 1 player currently online (" + hiddenCount + " hidden).");
                }
            }
            else
            {
                if (hiddenCount == 0)
                {
                    Player.SendMessage(p, "There are " + playerCount + " players online.");
                }
                else
                {
                    Player.SendMessage(p, "There are " + playerCount + " players online (" + hiddenCount + " hidden).");
                }
            }
        }

        public override void Help(Player p)
        {
            Player.SendMessage(p, "/pcount - Displays the number of players online and total.");
        }
    }
}
