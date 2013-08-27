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
	public sealed class CmdZz : Command
	{
        public override string name { get { return "zz"; } }
		public override string shortcut { get { return ""; } }
        public override string type { get { return "mod"; } }
		public override bool museumUsable { get { return false; } }
		public override LevelPermission defaultRank { get { return LevelPermission.Builder; } }
		public override void Use(Player p, string message)
		{
            if ((p.group.CanExecute(Command.all.Find("cuboid"))) && (p.group.CanExecute(Command.all.Find("static"))))
            {
                if ((!p.staticCommands == true) && (!p.megaBoid == true))
                {
                    Command.all.Find("static").Use(p, "");
                    Command.all.Find("cuboid").Use(p, message);
                    Player.SendMessage(p, p.color + p.name + Server.DefaultColor + " to stop this, use /zz again");
                }
                else 
                {
                    p.ClearBlockchange();
                    p.staticCommands = false;
                    Player.SendMessage(p, "/zz has ended.");
                }
            }
            else { Player.SendMessage(p, "Sorry, your rank cannot use one of the commands this uses!"); }
               
		}
		public override void Help(Player p)
		{
			Player.SendMessage(p, "/zz - Like cuboid but in static mode automatically.");
		}
	}
}