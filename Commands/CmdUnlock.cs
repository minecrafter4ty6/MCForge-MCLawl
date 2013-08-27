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
namespace MCForge.Commands
{
    public sealed class CmdUnlock : Command
    {
        // The command's name, in all lowercase.  What you'll be putting behind the slash when using it.
        public override string name { get { return "unlock"; } }

        // Command's shortcut (please take care not to use an existing one, or you may have issues.
        public override string shortcut { get { return "ul"; } }

        // Determines which submenu the command displays in under /help.
        public override string type { get { return "other"; } }

        // Determines whether or not this command can be used in a museum.  Block/map altering commands should be made false to avoid errors.
        public override bool museumUsable { get { return false; } }

        // Determines the command's default rank.  Valid values are:
        // LevelPermission.Nobody, LevelPermission.Banned, LevelPermission.Guest
        // LevelPermission.Builder, LevelPermission.AdvBuilder, LevelPermission.Operator, LevelPermission.Admin
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }

        // This is where the magic happens, naturally.
        // p is the player object for the player executing the command.  message is everything after the command invocation itself.
        public override void Use(Player p, string message)
        {
            if (!Directory.Exists("text/lockdown"))
            {
                p.SendMessage("Could not locate the folder creating one now.");
                Directory.CreateDirectory("text/lockdown");
                Directory.CreateDirectory("text/lockdown/map");
                p.SendMessage("Added the settings for the command.");
            }
            string[] param = message.Split(' ');
            if (param.Length == 2 && (param[0] == "map" || param[0] == "player"))
            {
                if (param.Length == 2)
                {
                    if (param[0] == "map")
                    {
                        if (File.Exists("text/lockdown/map/" + param[1] + ""))
                        {
                            File.Delete("text/lockdown/map/" + param[1] + "");
                            Player.SendMessage(p, "The map " + param[1] + " has been unlocked.");
                        }
                        else
                            Player.SendMessage(p, "The map " + param[1] + " is not locked or does not exist.");
                    }
                }
                if (param.Length == 2) // shift number to the next lockdown command
                {
                    if (param[0] == "player")
                    {
                        Player who = Player.Find(param[1]);
                        {
                            if (who != null)
                            {
                                if (who.jailed)
                                {
                                    if (p != null) if (who.group.Permission >= p.group.Permission) { Player.SendMessage(p, "Cannot lock down someone of equal or greater rank."); return; }
                                    if (who.level != p.level) Command.all.Find("goto").Use(who, p.level.name);
                                    who.jailed = false;
                                    Player.GlobalMessage(who.color + who.name + Server.DefaultColor + " has been unlocked!", true);
                                    return;
                                }
                                else Player.SendMessage(p, "The player " + param[1] + " is not locked down."); return;
                            }
                            else Player.SendMessage(p, "There is no such player online!"); return;
                        }
                    }

                }
            } else {
                Help(p);
                return;
            }
        }

        public override void Help(Player p)
        {
            Player.SendMessage(p, "Use /unlock map <mapname> to unlock it (open it).");
            Player.SendMessage(p, "Use /unlock player <playername> to unlock player."); return;
        }
    }
}