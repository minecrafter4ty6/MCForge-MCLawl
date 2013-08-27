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

using MCForge.SQL;
namespace MCForge.Commands
{
    public sealed class CmdTColor : Command
    {
        public override string name { get { return "tcolor"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Admin; } }
        public CmdTColor() { }

        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }
            string[] args = message.Split(' ');
            Player who = Player.Find(args[0]);
            if (who == null)
            {
                Player.SendMessage(p, "Could not find player.");
                return;
            }
            if (p != null && who.group.Permission > p.group.Permission) {
				Player.SendMessage(p, "You cannot change the title color of someone ranked higher than you");
				return;
			}
            if (args.Length == 1)
            {
                who.titlecolor = "";
                Player.GlobalChat(who, who.color + who.name + Server.DefaultColor + " had their title color removed.", false);
                Database.AddParams("@Name", who.name);
                Database.executeQuery("UPDATE Players SET title_color = '' WHERE Name = @Name");
                who.SetPrefix();
                return;
            }
            else
            {
                string color = c.Parse(args[1]);
                if (color == "") { Player.SendMessage(p, "There is no color \"" + args[1] + "\"."); return; }
                else if (color == who.titlecolor) { Player.SendMessage(p, who.name + " already has that title color."); return; }
                else
                {
                    Database.AddParams("@Color", c.Name(color));
                    Database.AddParams("@Name", who.name);
                    Database.executeQuery("UPDATE Players SET title_color = @Color WHERE Name = @Name");
                    Player.GlobalChat(who, who.color + who.name + Server.DefaultColor + " had their title color changed to " + color + c.Name(color) + Server.DefaultColor + ".", false);
                    who.titlecolor = color;
                    who.SetPrefix();
                }
            }
        }

        public override void Help(Player p)
        {
            Player.SendMessage(p, "/tcolor <player> [color] - Gives <player> the title color of [color].");
            Player.SendMessage(p, "If no [color] is specified, title color is removed.");
            Player.SendMessage(p, "&0black &1navy &2green &3teal &4maroon &5purple &6gold &7silver");
            Player.SendMessage(p, "&8gray &9blue &alime &baqua &cred &dpink &eyellow &fwhite");
        }
    }
}
