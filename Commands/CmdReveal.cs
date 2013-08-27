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
	public sealed class CmdReveal : Command
	{
		public override string name { get { return "reveal"; } }
		public override string shortcut { get { return ""; } }
		public override string type { get { return "mod"; } }
		public override bool museumUsable { get { return true; } }
		public override LevelPermission defaultRank { get { return LevelPermission.AdvBuilder; } }
		public CmdReveal() { }

		public override void Use(Player p, string message)
		{
			if (message == "") message = p.name;
			Level lvl;
			string[] text = new string[2];
			text[0] = "";
			text[1] = "";
			try
			{
				text[0] = message.Split(' ')[0].ToLower();
				text[1] = message.Split(' ')[1].ToLower();
			}
			catch { }
			{
				if (p != null && p.level != null) lvl = p.level;
				else
				{
					lvl = Level.Find(text[1]);
					if (lvl == null)
					{
						Player.SendMessage(p, "Level not found!");
						return;
					}
				}
			}
			if (text[0].ToLower() == "all")
			{
				if (p != null && (int)p.group.Permission < CommandOtherPerms.GetPerm(this)) { Player.SendMessage(p, "Reserved for " + Group.findPermInt(CommandOtherPerms.GetPerm(this)).name + "+"); return; }

				foreach (Player who in Player.players.ToArray())
				{
					if (who.level == lvl)
					{

						who.Loading = true;
						foreach (Player pl in Player.players.ToArray()) if (who.level == pl.level && who != pl) who.SendDie(pl.id);
						foreach (PlayerBot b in PlayerBot.playerbots.ToArray()) if (who.level == b.level) who.SendDie(b.id);

						Player.GlobalDie(who, true);
						who.SendUserMOTD(); who.SendMap();

						ushort x = (ushort)((0.5 + who.level.spawnx) * 32);
						ushort y = (ushort)((1 + who.level.spawny) * 32);
						ushort z = (ushort)((0.5 + who.level.spawnz) * 32);

						if (!who.hidden) Player.GlobalSpawn(who, x, y, z, who.level.rotx, who.level.roty, true);
						else unchecked { who.SendPos((byte)-1, x, y, z, who.level.rotx, who.level.roty); }

						foreach (Player pl in Player.players.ToArray())
							if (pl.level == who.level && who != pl && !pl.hidden)
								who.SendSpawn(pl.id, pl.color + pl.name, pl.pos[0], pl.pos[1], pl.pos[2], pl.rot[0], pl.rot[1]);

						foreach (PlayerBot b in PlayerBot.playerbots.ToArray())
							if (b.level == who.level)
								who.SendSpawn(b.id, b.color + b.name, b.pos[0], b.pos[1], b.pos[2], b.rot[0], b.rot[1]);

						who.Loading = false;

						if (p != null && !p.hidden) { who.SendMessage("&bMap reloaded by " + p.name); }
						if (p != null && p.hidden) { who.SendMessage("&bMap reloaded"); }
						Player.SendMessage(p, "&4Finished reloading for " + who.name);
						/*
						foreach (Player pl in Player.players) if (who.level == pl.level && who != pl) who.SendDie(pl.id);
						foreach (PlayerBot b in PlayerBot.playerbots) if (who.level == b.level) who.SendDie(b.id);
						Player.GlobalDie(who, true);

						who.SendMap();

						ushort x = (ushort)((0.5 + who.level.spawnx) * 32);
						ushort y = (ushort)((1 + who.level.spawny) * 32);
						ushort z = (ushort)((0.5 + who.level.spawnz) * 32);

						Player.GlobalSpawn(who, x, y, z, who.level.rotx, who.level.roty, true);

						foreach (Player pl in Player.players)
							if (pl.level == who.level && who != pl && !pl.hidden)
								who.SendSpawn(pl.id, pl.color + pl.name, pl.pos[0], pl.pos[1], pl.pos[2], pl.rot[0], pl.rot[1]);

						foreach (PlayerBot b in PlayerBot.playerbots)
							if (b.level == who.level)
								who.SendSpawn(b.id, b.color + b.name, b.pos[0], b.pos[1], b.pos[2], b.rot[0], b.rot[1]);

						who.SendMessage("Map reloaded.");
						*/
					}
				}

				GC.Collect();
				GC.WaitForPendingFinalizers();
			}
			else
			{
				Player who = Player.Find(text[0]);
				if (who == null) { Player.SendMessage(p, "Could not find player."); return; }
				else if (who.group.Permission > p.group.Permission && p != who) { Player.SendMessage(p, "Cannot reload the map of someone higher than you."); return; }

				who.Loading = true;
				foreach (Player pl in Player.players.ToArray()) if (who.level == pl.level && who != pl) who.SendDie(pl.id);
				foreach (PlayerBot b in PlayerBot.playerbots.ToArray()) if (who.level == b.level) who.SendDie(b.id);

				Player.GlobalDie(who, true);
				who.SendUserMOTD(); who.SendMap();

				ushort x = (ushort)((0.5 + who.level.spawnx) * 32);
				ushort y = (ushort)((1 + who.level.spawny) * 32);
				ushort z = (ushort)((0.5 + who.level.spawnz) * 32);

				if (!who.hidden) Player.GlobalSpawn(who, x, y, z, who.level.rotx, who.level.roty, true);
				else unchecked { who.SendPos((byte)-1, x, y, z, who.level.rotx, who.level.roty); }

				foreach (Player pl in Player.players.ToArray())
					if (pl.level == who.level && who != pl && !pl.hidden)
						who.SendSpawn(pl.id, pl.color + pl.name, pl.pos[0], pl.pos[1], pl.pos[2], pl.rot[0], pl.rot[1]);

				foreach (PlayerBot b in PlayerBot.playerbots.ToArray())
					if (b.level == who.level)
						who.SendSpawn(b.id, b.color + b.name, b.pos[0], b.pos[1], b.pos[2], b.rot[0], b.rot[1]);

				who.Loading = false;
				GC.Collect();
				GC.WaitForPendingFinalizers();

				who.SendMessage("&bMap reloaded by " + p.name);
				Player.SendMessage(p, "&4Finished reloading for " + who.name);

				/*
				foreach (Player pl in Player.players) if (who.level == pl.level && who != pl) who.SendDie(pl.id);
				foreach (PlayerBot b in PlayerBot.playerbots) if (who.level == b.level) who.SendDie(b.id);
				Player.GlobalDie(who, true);

				who.SendMap();

				ushort x = (ushort)((0.5 + who.level.spawnx) * 32);
				ushort y = (ushort)((1 + who.level.spawny) * 32);
				ushort z = (ushort)((0.5 + who.level.spawnz) * 32);

				Player.GlobalSpawn(who, x, y, z, who.level.rotx, who.level.roty, true);

				foreach (Player pl in Player.players)
					if (pl.level == who.level && who != pl && !pl.hidden)
						who.SendSpawn(pl.id, pl.color + pl.name, pl.pos[0], pl.pos[1], pl.pos[2], pl.rot[0], pl.rot[1]);

				foreach (PlayerBot b in PlayerBot.playerbots)
					if (b.level == who.level)
						who.SendSpawn(b.id, b.color + b.name, b.pos[0], b.pos[1], b.pos[2], b.rot[0], b.rot[1]);

				who.SendMessage("Map reloaded.");
				 */
			}
		}
		public override void Help(Player p)
		{
			Player.SendMessage(p, "/reveal <name> - Reveals the map for <name>.");
			Player.SendMessage(p, "/reveal all - Reveals for all in the map");
			Player.SendMessage(p, "/reveal all <map> - Reveals for all in <map>");
			Player.SendMessage(p, "Will reload the map for anyone. (incl. banned)");
		}
	}
}