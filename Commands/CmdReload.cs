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
    public sealed class CmdReload : Command
    {
        public override string name { get { return "reload"; } }
        public override string shortcut { get { return "rd"; } }
        public override string type { get { return "build"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdReload() { }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/reload <map> - Reloads the specified map. Uses the current map if no message is given.");
        }

        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }
            {
                if (!File.Exists("levels/" + message + ".lvl"))
                {
                    Player.SendMessage(p, "The specified level does not exist!");
                    return;
                }
                if (Server.mainLevel.name == message)
                {
                    Player.SendMessage(p, "You cannot reload the main level!");
                    return;
                }
                if (p == null)
                {
                    {
                        foreach (Player pl in Player.players)
                        {
                            if (pl.level.name.ToLower() == message.ToLower())
                            {
                                Command.all.Find("unload").Use(p, message);
                                Command.all.Find("load").Use(p, message);
                                Command.all.Find("goto").Use(pl, message);
                            }
                        }
                        Player.GlobalMessage("&cThe map, " + message + " has been reloaded!");
                        //IRCBot.Say("The map, " + message + " has been reloaded.");
                        Server.IRC.Say("The map, " + message + " has been reloaded.");
                        Server.s.Log("The map, " + message + " was reloaded by the console");
                        return;
                    }
                }
                if (p != null)
                {
                    {
                        foreach (Player pl in Player.players)
                        {
                            if (pl.level.name.ToLower() == message.ToLower())
                            {
                                p.ignorePermission = true;
                                Command.all.Find("unload").Use(p, message);
                                Command.all.Find("load").Use(p, message);
                                Command.all.Find("goto").Use(pl, message);
                            }
                        }
                        Player.GlobalMessage("&cThe map, " + message + " has been reloaded!");
						//IRCBot.Say("The map, " + message + " has been reloaded.");
                        Server.IRC.Say("The map, " + message + " has been reloaded.");
						Server.s.Log("The map, " + message + " was reloaded by " + p.name);
						p.ignorePermission = false;
						return;
					}
				}
			}
		}
	}
}
