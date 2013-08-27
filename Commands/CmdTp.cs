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
    public sealed class CmdTp : Command
    {
        public override string name { get { return "tp"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Builder; } }
        public CmdTp() { }

        public override void Use(Player p, string message)
        {
            if (message == "")
            {
                Command.all.Find("spawn");
                return;
            }
            int number = message.Split(' ').Length;
            if (number > 2) { Help(p); return; }
            if (number == 2)
            {
                if (!p.group.CanExecute(Command.all.Find("P2P")))
                { 
                   Player.SendMessage(p, "You cannot teleport others!"); 
                   return; 
                }
                Command.all.Find("P2P").Use(p, message);
            }
            if (number == 1)
            {
                Player who = Player.Find(message);
                if (who == null || (who.hidden && p.group.Permission < LevelPermission.Admin)) { Player.SendMessage(p, "There is no player \"" + message + "\"!"); return; }
                if (p.level != who.level)
                {
                    if (who.level.name.Contains("cMuseum"))
                    {
                        Player.SendMessage(p, "Player \"" + message + "\" is in a museum!");
                        return;
                    }
                    else
                    {
                        if (Server.higherranktp == false)
                        {
                            if (p.group.Permission < who.group.Permission)
                            {
                                Player.SendMessage(p, "You cannot teleport to a player of higher rank!");
                                return;
                            }
                        }
                        Command.all.Find("goto").Use(p, who.level.name);
                    }
                }
                if (p.level == who.level)
                {
                    if (Server.higherranktp == false)
                    {
                        if (p.group.Permission < who.group.Permission)
                        {
                            Player.SendMessage(p, "You cannot teleport to a player of higher rank!");
                            return;
                        }
                    }

                    if (who.Loading)
                    {
                        Player.SendMessage(p, "Waiting for " + who.color + who.name + Server.DefaultColor + " to spawn...");
                        while (who.Loading) { }
                    }
                    while (p.Loading) { }  //Wait for player to spawn in new map
                    unchecked { p.SendPos((byte)-1, who.pos[0], who.pos[1], who.pos[2], who.rot[0], 0); }
                }
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/tp <player1> [player2] - Teleports yourself to a player.");
            Player.SendMessage(p, "[player2] is optional but if present will act like /p2p.");
            Player.SendMessage(p, "If <player1> is blank, /spawn is used.");
        }
    }
}