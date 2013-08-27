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
    public sealed class CmdBaninfo : Command
    {
        public override string name { get { return "baninfo"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdBaninfo() { }

        public override void Use(Player p, string message)
        {
            string[] data;
            if (message == "") { Help(p); return; }
            if (message.Length <= 3) { Help(p); }
            else
            {
                if (Ban.Isbanned(message))
                {
                    data = Ban.Getbandata(message);
                    // string[] end = { bannedby, reason, timedate, oldrank, stealth };
                    // usefull to know :-)
                    string reason = data[1].Replace("%20", " ");
                    string datetime = data[2].Replace("%20", " ");
                    Player.SendMessage(p, "&9User: &e" + message);
                    Player.SendMessage(p, "&9Banned by: &e" + data[0]);
                    Player.SendMessage(p, "&9Reason: &e" + reason);
                    Player.SendMessage(p, "&9Date and time: &e" + datetime);
                    Player.SendMessage(p, "&9Old rank: &e" + data[3]);
                    string stealth; if (data[4] == "true") stealth = "&aYes"; else stealth = "&cNo";
                    Player.SendMessage(p, "&9Stealth banned: " + stealth);
                }
                else if (!Group.findPerm(LevelPermission.Banned).playerList.Contains(message)) Player.SendMessage(p, "That player isn't banned");
                else if (!Ban.Isbanned(message)) Player.SendMessage(p, "Couldn't find ban info about " + message + ".");
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/baninfo <player> - returns info about banned player.");
        }
    }
}