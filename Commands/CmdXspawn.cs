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
	public class CmdXspawn : Command
	{
    	public override string name { get { return "xspawn"; } }
		public override string shortcut { get { return ""; } }
		public override string type { get { return "other"; } }
		public override bool museumUsable { get { return false; } }
		public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public override void Use(Player p, string message)
        {
            Player player = Player.Find(message.Split(' ')[0]);
            if (player == null)
            {
                Player.SendMessage(p, "Error: " + player.color + player.name + Server.DefaultColor + " was not found");
                return;
            }
            if (player == p)
            {
                Player.SendMessage(p, "Error: Seriously? Just use /spawn!");
                return;
            }
            if (player.group.Permission > p.group.Permission)
            {
                Player.SendMessage(p, "Cannot move someone of greater rank");
                return;
            }
            Command.all.Find("spawn").Use(player, "");
            Player.SendMessage(p, "Succesfully spawned " + player.color + player.name + Server.DefaultColor + ".");
            Player.GlobalMessage(player.color + player.name + Server.DefaultColor + " was respawned by " + p.color + p.name + Server.DefaultColor + ".");
        }
		public override void Help(Player p)
		{
			Player.SendMessage(p, "/xspawn - Spawn another player.");
            Player.SendMessage(p, "WARNING: It says who used it!");
		}
	}
}