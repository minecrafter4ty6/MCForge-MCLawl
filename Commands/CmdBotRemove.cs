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

using System;
namespace MCForge.Commands
{
    public sealed class CmdBotRemove : Command
    {
        public override string name { get { return "botremove"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Admin; } }
        public string[,] botlist;
        public CmdBotRemove() { }

        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }
            if (p == null)
            {
                Player.SendMessage(p, "This command can only be used in-game!");
                return;
            }
                try
                {
                    if (message.ToLower() == "all")
                    {
                        for (int i = 0; i < PlayerBot.playerbots.Count; i++)
                        {
                            if (PlayerBot.playerbots[i].level == p.level)
                            {
                                //   PlayerBot.playerbots.Remove(PlayerBot.playerbots[i]);
                                PlayerBot Pb = PlayerBot.playerbots[i];
                                Pb.removeBot();
                                i--;
                            }
                        }
                    }
                    else
                    {
                        PlayerBot who = PlayerBot.Find(message);
                        if (who == null) { Player.SendMessage(p, "There is no bot " + who + "!"); return; }
                        if (p.level != who.level) { Player.SendMessage(p, who.name + " is in a different level."); return; }
                        who.removeBot();
                        Player.SendMessage(p, "Removed bot.");
                    }
                }
                catch (Exception e) { Server.ErrorLog(e); Player.SendMessage(p, "Error caught"); }
            }
        
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/botremove <name> - Remove a bot on the same level as you");
            //   Player.SendMessage(p, "If All is used, all bots on the current level are removed");
        }
    }
}