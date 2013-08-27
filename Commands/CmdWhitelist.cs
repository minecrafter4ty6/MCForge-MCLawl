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
    public sealed class CmdWhitelist : Command
    {
        public override string name { get { return "whitelist"; } }
        public override string shortcut { get { return "w"; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdWhitelist() { }

        public override void Use(Player p, string message)
        {
            if (!Server.useWhitelist) { Player.SendMessage(p, "Whitelist is not enabled."); return; }
            if (message == "") { Help(p); return; }
            int pos = message.IndexOf(' ');
            if (pos != -1)
            {
                string action = message.Substring(0, pos);
                string player = message.Substring(pos + 1);

                switch (action)
                {
                    case "add":
                        if (Server.whiteList.Contains(player))
                        {
                            Player.SendMessage(p, "&f" + player + Server.DefaultColor + " is already on the whitelist!");
                            break;
                        }
                        Server.whiteList.Add(player);
                        Player.GlobalMessageOps(p.color + p.prefix + p.name + Server.DefaultColor + " added &f" + player + Server.DefaultColor + " to the whitelist.");
                        Server.whiteList.Save("whitelist.txt");
                        Server.s.Log("WHITELIST: Added " + player);
                        break;
                    case "del":
                        if (!Server.whiteList.Contains(player))
                        {
                            Player.SendMessage(p, "&f" + player + Server.DefaultColor + " is not on the whitelist!");
                            break;
                        }
                        Server.whiteList.Remove(player);
                        Player.GlobalMessageOps(p.color + p.prefix + p.name + Server.DefaultColor + " removed &f" + player + Server.DefaultColor + " from the whitelist.");
                        Server.whiteList.Save("whitelist.txt");
                        Server.s.Log("WHITELIST: Removed " + player);
                        break;
                    case "list":
                        string output = "Whitelist:&f";
                        foreach (string wlName in Server.whiteList.All())
                        {
                            output += " " + wlName + ",";
                        }
                        output = output.Substring(0, output.Length - 1);
                        Player.SendMessage(p, output);
                        break;
                    default:
                        Help(p);
                        return;
                }
            }
            else
            {
                if (message == "list")
                {
                    string output = "Whitelist:&f";
                    foreach (string wlName in Server.whiteList.All())
                    {
                        output += " " + wlName + ",";
                    }
                    output = output.Substring(0, output.Length - 1);
                    Player.SendMessage(p, output);
                }
                else
                {
                    Help(p);
                }
            }
        }

        public override void Help(Player p)
        {
            Player.SendMessage(p, "/whitelist <add/del/list> [player] - Handles whitelist entry for [player], or lists all entries.");
        }
    }
}
