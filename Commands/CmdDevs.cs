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

namespace MCForge.Commands {
	public sealed class CmdDevs : Command {
		public override string name { get { return "devs"; } }
		public override string shortcut { get { return "dev"; } }
		public override string type { get { return "information"; } }
		public override bool museumUsable { get { return true; } }
		public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
		public CmdDevs() { }

		public override void Use(Player p, string message) {
			if ( message != "" ) { Help(p); return; }
			string devlist = "";
			string temp;
			foreach ( string dev in Server.Devs ) {
				temp = dev.Substring(0, 1);
				temp = temp.ToUpper() + dev.Remove(0, 1);
				devlist += temp + ", ";
			}
			devlist = devlist.Remove(devlist.Length - 2);
			Player.SendMessage(p, "&9MCForge Development Team: " + Server.DefaultColor + devlist + "&e.");
		}

		public override void Help(Player p) {
			Player.SendMessage(p, "/devs - Displays the list of MCForge developers.");
		}
	}
}