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
using System.Threading;
namespace MCForge.Commands
{
    public sealed class CmdLockdown : Command
    {
        public override string name { get { return "lockdown"; } }
        public override string shortcut { get { return "ld"; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }

        public override void Use(Player p, string message)
        {
            if (!Directory.Exists("text/lockdown"))
            {
                Player.SendMessage(p, "Could not locate the folder creating one now.");
                Directory.CreateDirectory("text/lockdown");
                Directory.CreateDirectory("text/lockdown/map");
                Player.SendMessage(p, "Added the settings for the command");
            }

            string[] param = message.Split(' ');


            if (param.Length == 2 && (param[0] == "map" || param[0] == "player"))
            {
                if (param[0] == "map")
                {
                    if (!Directory.Exists("text/lockdown/map"))
                    {
                        p.SendMessage("Could not locate the map folder, creating one now.");
                        Directory.CreateDirectory("text/lockdown/map");
                        p.SendMessage("Added the map settings Directory within 'text/lockdown'!");
                    }

                    string filepath = "text/lockdown/map/" + param[1] + "";
                    bool mapIsLockedDown = File.Exists(filepath);

                    if (!mapIsLockedDown)
                    {
                        File.Create(filepath).Dispose();
                        Player.GlobalMessage("The map " + param[1] + " has been locked");
                        Player.GlobalMessageOps("Locked by: " + ((p == null) ? "Console" : p.name));
                    }
                    else
                    {
                        File.Delete(filepath);
                        Player.GlobalMessage("The map " + param[1] + " has been unlocked");
                        Player.GlobalMessageOps("Unlocked by: " + ((p == null) ? "Console" : p.name));
                    }
                }

                if (param[0] == "player")
                {
                    Player who = Player.Find(param[1]);

                    if (who == null)
                    {
                        Player.SendMessage(p, "There is no player with such name online");
                        return;
                    }

                    if (!who.jailed)
                    {
                        if (p != null)
                        {
                            if (who.group.Permission >= p.group.Permission)
                            {
                                Player.SendMessage(p, "Cannot lock down someone of equal or greater rank.");
                                return;
                            }
                        }
                        if (p != null && who.level != p.level)
                        {
                            Player.SendMessage(p, "Moving player to your map...");
                            Command.all.Find("goto").Use(who, p.level.name);
                            int waits = 0;
                            while (who.Loading)
                            {
                                Thread.Sleep(500);
                                // If they don't load in 10 seconds, eff it.
                                if (waits++ == 20)
                                    break;
                            }
                        }
                        who.jailed = true;
                        Player.GlobalMessage(who.color + who.name + Server.DefaultColor + " has been locked down!");
                        Player.GlobalMessageOps("Locked by: " + ((p == null) ? "Console" : p.name));
                        return;
                    }
                    else
                    {
                        who.jailed = false;
                        Player.GlobalMessage(who.color + who.name + Server.DefaultColor + " has been unlocked.");
                        Player.GlobalMessageOps("Unlocked by: " + ((p == null) ? "Console" : p.name));
                        return;
                    }
                }
            }
            else
            {
                Help(p);
                return;
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, " use /lockdown map <mapname> to lock it down.");
            Player.SendMessage(p, " use /lockdown player <playername> to lock down player."); return;
        }
    }
}