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
	public sealed class CmdSeen : Command
	{
		public override string name { get { return "seen"; } }
		public override string shortcut { get { return ""; } }
		public override bool museumUsable { get { return true; } }
		public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
		public override string type { get { return "mod"; } }
		public CmdSeen() { }

		public override void Use(Player p, string message)
		{
            if(message == "") { Help(p); return; }

			Player pl = Player.Find(message);
			if (pl != null && !pl.hidden)
			{
				Player.SendMessage(p, pl.color + pl.name + Server.DefaultColor + " is currently online.");
				return;
			}

            Database.AddParams("@Name", message);
			using (DataTable playerDb = Database.fillData("SELECT * FROM Players WHERE Name=@Name"))
			{
				if (playerDb.Rows != null && playerDb.Rows.Count > 0)
					Player.SendMessage(p, message + " was last seen: " + playerDb.Rows[0]["LastLogin"]);
				else
					Player.SendMessage(p, "Unable to find player");
			}
		}
		public override void Help(Player p)
		{
			Player.SendMessage(p, "/seen [player] - says when a player was last seen on the server");
		}
	}
}