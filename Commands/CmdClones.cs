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

using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using MCForge.SQL;
namespace MCForge.Commands
{
	public sealed class CmdClones : Command
	{
		public override string name { get { return "clones"; } }
		public override string shortcut { get { return "alts"; } }
		public override string type { get { return "information"; } }
		public override bool museumUsable { get { return true; } }
		public override LevelPermission defaultRank { get { return LevelPermission.AdvBuilder; } }
		public CmdClones() { }

		public override void Use(Player p, string message)
		{
			if ( !Regex.IsMatch(message.ToLower(), @".*%([0-9]|[a-f]|[k-r])%([0-9]|[a-f]|[k-r])%([0-9]|[a-f]|[k-r])") ) {
				if (Regex.IsMatch(message.ToLower(), @".*%([0-9]|[a-f]|[k-r])(.+?).*")) {
					Regex rg = new Regex(@"%([0-9]|[a-f]|[k-r])(.+?)");
					MatchCollection mc = rg.Matches(message.ToLower());
					if (mc.Count > 0) {
						Match ma = mc[0];
						GroupCollection gc = ma.Groups;
						message.Replace("%" + gc[1].ToString().Substring(1), "&" + gc[1].ToString().Substring(1));
					}
				}
			}
			if (message == "") message = p.name;

			string originalName = message.ToLower();

			Player who = Player.Find(message);
			if (who == null)
			{
				Player.SendMessage(p, "Could not find player. Searching Player DB.");

                Database.AddParams("@Name", message);
				DataTable FindIP = Database.fillData("SELECT IP FROM Players WHERE Name=@Name");

				if (FindIP.Rows.Count == 0) { Player.SendMessage(p, "Could not find any player by the name entered."); FindIP.Dispose(); return; }

				message = FindIP.Rows[0]["IP"].ToString();
				FindIP.Dispose();
			}
			else
			{
				message = who.ip;
			}
            Database.AddParams("@IP", message);
			DataTable Clones = Database.fillData("SELECT Name FROM Players WHERE IP=@IP");

			if (Clones.Rows.Count == 0) { Player.SendMessage(p, "Could not find any record of the player entered."); return; }

			List<string> foundPeople = new List<string>();
			for (int i = 0; i < Clones.Rows.Count; ++i)
			{
				if (!foundPeople.Contains(Clones.Rows[i]["Name"].ToString().ToLower()))
					foundPeople.Add(Clones.Rows[i]["Name"].ToString().ToLower());
			}

			Clones.Dispose();
			if (foundPeople.Count <= 1) { Player.SendMessage(p, originalName + " has no clones."); return; }

			Player.SendMessage(p, "These people have the same IP address:");
			Player.SendMessage(p, string.Join(", ", foundPeople.ToArray()));
		}

		public override void Help(Player p)
		{
			Player.SendMessage(p, "/clones <name> - Finds everyone with the same IP as <name>"); //Fixed typo
		}
	}
}
