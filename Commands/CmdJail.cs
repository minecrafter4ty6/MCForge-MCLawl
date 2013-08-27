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

using System.IO;

namespace MCForge
{
    public sealed class CmdJail : Command
    {
        public override string name { get { return "jail"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdJail() { }

        public override void Use(Player p, string message)
        {
            if ((message.ToLower() == "set") && p != null)
            {
                p.level.jailx = p.pos[0]; p.level.jaily = p.pos[1]; p.level.jailz = p.pos[2];
                p.level.jailrotx = p.rot[0]; p.level.jailroty = p.rot[1];
                Player.SendMessage(p, "Set Jail point.");
            }
            else
            {
                Player who = Player.Find(message);
                if (who != null)
                {
                    if (!who.jailed)
                    {
                        if (p != null)
                        {
                            if (who.group.Permission >= p.group.Permission) { Player.SendMessage(p, "Cannot jail someone of equal or greater rank."); return; }
                            Player.SendMessage(p, "You jailed " + who.name);
                        }
                        Player.GlobalDie(who, false);
                        who.jailed = true;
                        Player.GlobalSpawn(who, who.level.jailx, who.level.jaily, who.level.jailz, who.level.jailrotx, who.level.jailroty, true);
                        if (!File.Exists("ranks/jailed.txt")) File.Create("ranks/jailed.txt").Close();
                        Extensions.DeleteLineWord("ranks/jailed.txt", who.name);
                        using (StreamWriter writer = new StreamWriter("ranks/jailed.txt", true))
                        {
                            writer.WriteLine(who.name.ToLower() + " " + who.level.name);
                        }
                        Player.GlobalChat(who, who.color + who.name + Server.DefaultColor + " was &8jailed", false);
                    }
                    else
                    {
                        if (!File.Exists("ranks/jailed.txt")) File.Create("ranks/jailed.txt").Close();
                        Extensions.DeleteLineWord("ranks/jailed.txt", who.name.ToLower());
                        who.jailed = false;
                        Command.all.Find("spawn").Use(who, "");
                        Player.SendMessage(p, "You freed " + who.name + " from jail");
                        Player.GlobalChat(who, who.color + who.name + Server.DefaultColor + " was &afreed" + Server.DefaultColor + " from jail", false);
                    }
                }
                else
                {
                    Player.SendMessage(p, "Could not find specified player.");
                }
            }
        }

        public override void Help(Player p)
        {
            Player.SendMessage(p, "/jail [user] - Places [user] in jail unable to use commands.");
            Player.SendMessage(p, "/jail [set] - Creates the jail point for the map.");
            Player.SendMessage(p, "This command has been deprecated in favor of /xjail.");
        }
    }
}