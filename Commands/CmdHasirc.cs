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
    public sealed class CmdHasirc : Command
    {
        public override string name { get { return "hasirc"; } }
        public override string shortcut { get { return "irc"; } }
        public override string type { get { return "information"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
        public CmdHasirc() { }

        public override void Use(Player p, string message)
        {
            if (message != "")
            {
                Help(p);
                return;
            }
            else
            {
                string hasirc;
                string ircdetails = "";
                if (Server.irc)
                {
                    hasirc = "&aEnabled" + Server.DefaultColor + ".";
                    ircdetails = Server.ircServer + " > " + Server.ircChannel;
                }
                else
                {
                    hasirc = "&cDisabled" + Server.DefaultColor + ".";
                }
                Player.SendMessage(p, "IRC is " + hasirc);
                if (ircdetails != "")
                {
                    Player.SendMessage(p, "Location: " + ircdetails);
                }
            }
        }

        public override void Help(Player p)
        {
            Player.SendMessage(p, "/hasirc - Denotes whether or not the server has IRC active.");
            Player.SendMessage(p, "If IRC is active, server and channel are also displayed.");
        }
    }
}
