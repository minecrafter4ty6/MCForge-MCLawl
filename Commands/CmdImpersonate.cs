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
    public sealed class CmdImpersonate : Command
	{
		public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
		public override void Help(Player p) { Player.SendMessage(p, "/impersonate <player> <message> - Sends a message as if it came from <player>"); }
		public override bool museumUsable { get { return true; } }
		public override string name { get { return "impersonate"; } }
		public override string shortcut { get { return "imp"; } }
		public override string type { get { return "other"; } }
		public void SendIt(Player p, string message, Player player)
		{
			if (message.Split(' ').Length > 1)
			{
				if (player != null)
				{
					message = message.Substring(message.IndexOf(' ') + 1);
					//Player.GlobalMessage(player.color + player.voicestring + player.color + player.prefix + player.name + ": &f" + message);
					Player.GlobalChat(player, message);
				}
				else
				{
					string playerName = message.Split(' ')[0];
					message = message.Substring(message.IndexOf(' ') + 1);
					Player.GlobalMessage(playerName + ": &f" + message);
				}
			}
			else { Player.SendMessage(p, "No message was given."); }
		}
		public override void Use(Player p, string message)
		{
			if ((message == "")) { this.Help(p); }
			else
			{
				Player player = Player.Find(message.Split(' ')[0]);
				if (player != null)
				{
					if (p == null) { this.SendIt(p, message, player); }
					else
					{
						if (player == p) { this.SendIt(p, message, player); }
						else
						{
							if (p.group.Permission > player.group.Permission) { this.SendIt(p, message, player); }
							else { Player.SendMessage(p, "You cannot impersonate a player of equal or greater rank."); }
						}
					}
				}
				else
				{
					if (p != null)
					{
						if (p.group.Permission >= LevelPermission.Admin)
						{
							if (Group.findPlayerGroup(message.Split(' ')[0]).Permission < p.group.Permission) { this.SendIt(p, message, null); }
							else { Player.SendMessage(p, "You cannot impersonate a player of equal or greater rank."); }
						}
						else { Player.SendMessage(p, "You are not allowed to impersonate offline players"); }
					}
					else { this.SendIt(p, message, null); }
				}
			}
		}
	}
}