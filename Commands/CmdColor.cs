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
namespace MCForge.Commands {
    public sealed class CmdColor : Command {
        public override string name { get { return "color"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdColor() { }

        public override void Use(Player p, string message) {
            if ( message == "" || message.Split(' ').Length > 2 ) { Help(p); return; }
            int pos = message.IndexOf(' ');
            if ( pos != -1 ) {
                Player who = Player.Find(message.Substring(0, pos));

                if ( p != null && who.group.Permission > p.group.Permission ) { Player.SendMessage(p, "You cannot change the color of someone ranked higher than you!"); return; }

                if ( who == null ) { Player.SendMessage(p, "There is no player \"" + message.Substring(0, pos) + "\"!"); return; }

                if ( message.Substring(pos + 1) == "del" ) {
                    Database.AddParams("@Name", who.name);
                    Database.executeQuery("UPDATE Players SET color = '' WHERE name = @Name");
                    Player.GlobalChat(who, who.color + "*" + Name(who.name) + " color reverted to " + who.group.color + "their group's default" + Server.DefaultColor + ".", false);
                    who.color = who.group.color;

                    Player.GlobalDie(who, false);
                    Player.GlobalSpawn(who, who.pos[0], who.pos[1], who.pos[2], who.rot[0], who.rot[1], false);
                    who.SetPrefix();
                    return;
                }
                string color = c.Parse(message.Substring(pos + 1));
                if ( color == "" ) { Player.SendMessage(p, "There is no color \"" + message + "\"."); }
                else if ( color == who.color ) { Player.SendMessage(p, who.name + " already has that color."); }
                else {
                    //Player.GlobalChat(who, p.color + "*" + p.name + "&e changed " + who.color + Name(who.name) +
                    //                  " color to " + color +
                    //                  c.Name(color) + "&e.", false);
                    Database.AddParams("@Color", c.Name(color));
                    Database.AddParams("@Name", who.name);
                    Database.executeQuery("UPDATE Players SET color = @Color WHERE name = @Name");

                    Player.GlobalChat(who, who.color + "*" + Name(who.name) + " color changed to " + color + c.Name(color) + Server.DefaultColor + ".", false);
                    if ( p == null ) {
                        Player.SendMessage(p, "*" + Name(who.name) + " color was changed to " + c.Name(color) + ".");
                    }
                    who.color = color;

                    Player.GlobalDie(who, false);
                    Player.GlobalSpawn(who, who.pos[0], who.pos[1], who.pos[2], who.rot[0], who.rot[1], false);
                    who.SetPrefix();
                }
            }
            else {
                if ( p != null ) {
                    if ( message == "del" ) {
                        Database.AddParams("@Name", p.name);
                        Database.executeQuery("UPDATE Players SET color = '' WHERE name = @Name");

                        Player.GlobalChat(p, p.color + "*" + Name(p.name) + " color reverted to " + p.group.color + "their group's default" + Server.DefaultColor + ".", false);
                        p.color = p.group.color;

                        Player.GlobalDie(p, false);
                        Player.GlobalSpawn(p, p.pos[0], p.pos[1], p.pos[2], p.rot[0], p.rot[1], false);
                        p.SetPrefix();
                        return;
                    }
                    string color = c.Parse(message);
                    if ( color == "" ) { Player.SendMessage(p, "There is no color \"" + message + "\"."); }
                    else if ( color == p.color ) { Player.SendMessage(p, "You already have that color."); }
                    else {
                        Database.AddParams("@Color", c.Name(color));
                        Database.AddParams("@Name", p.name);
                        Database.executeQuery("UPDATE Players SET color = @Color WHERE name = @Name");

                        Player.GlobalChat(p, p.color + "*" + Name(p.name) + " color changed to " + color + c.Name(color) + Server.DefaultColor + ".", false);
                        p.color = color;

                        Player.GlobalDie(p, false);
                        Player.GlobalSpawn(p, p.pos[0], p.pos[1], p.pos[2], p.rot[0], p.rot[1], false);
                        p.SetPrefix();
                    }
                }
            }
        }
        public override void Help(Player p) {
            Player.SendMessage(p, "/color [player] <color/del>- Changes the nick color.  Using 'del' removes color.");
            Player.SendMessage(p, "&0black &1navy &2green &3teal &4maroon &5purple &6gold &7silver");
            Player.SendMessage(p, "&8gray &9blue &alime &baqua &cred &dpink &eyellow &fwhite");
        }
        static string Name(string name) {
            string ch = name[name.Length - 1].ToString().ToLower();
            if ( ch == "s" || ch == "x" ) { return name + Server.DefaultColor + "'"; }
            else { return name + Server.DefaultColor + "'s"; }
        }
    }
}