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
using System.Text.RegularExpressions;
/*
	Copyright 2011 MCForge
	
	Dual-licensed under the	Educational Community License, Version 2.0 and
	the GNU General Public License, Version 3 (the "Licenses"); you may
	not use this file except in compliance with the Licenses. You may
	obtain a copy of the Licenses at
	
	http://www.opensource.org/licenses/ecl2.php
	http://www.gnu.org/licenses/gpl-3.0.html
	
	Unless required by applicable law or agreed to in writing,
	software distributed under the Licenses are distributed on an "AS IS"
	BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
	or implied. See the Licenses for the specific language governing
	permissions and limitations under the Licenses.
*/
using MCForge.SQL;
namespace MCForge.Commands {
	public sealed class CmdClearBlockChanges : Command {
		public override string name { get { return "clearblockchanges"; } }
		public override string shortcut { get { return "cbc"; } }
		public override string type { get { return "mod"; } }
		public override bool museumUsable { get { return false; } }
		public override LevelPermission defaultRank { get { return LevelPermission.Admin; } }
		public CmdClearBlockChanges() { }

		public override void Use(Player p, string message) {
			if ( p == null ) {
				Player.SendMessage(p, "This command can only be used in-game");
				return;
			}
			Level l = Level.Find(message);
			if ( l == null && message != "" ) { Player.SendMessage(p, "Could not find level."); return; }
			if ( l == null ) l = p.level;

			if ( !Regex.IsMatch(l.name.ToLower(), @"^[a-z0-9]*?$") ) {
				Player.SendMessage(p, "Level name is not accepted");
				return;
			}
            //safe against SQL injections because no user input is given here
			if ( Server.useMySQL ) MySQL.executeQuery("TRUNCATE TABLE `Block" + l.name + "`"); else SQLite.executeQuery("DELETE FROM `Block" + l.name + "`");
			Player.SendMessage(p, "Cleared &cALL" + Server.DefaultColor + " recorded block changes in: &d" + l.name);
		}
		public override void Help(Player p) {
			Player.SendMessage(p, "/clearblockchanges <map> - Clears the block changes stored in /about for <map>.");
			Player.SendMessage(p, "&cUSE WITH CAUTION");
		}
	}
}