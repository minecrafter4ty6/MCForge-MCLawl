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
    public sealed class CmdMe : Command
    {
        public override string name { get { return "me"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
        public CmdMe() { }

        public override void Use(Player p, string message)
        {
            if (message == "") { Player.SendMessage(p, "You"); return; }
            if (p == null) { Player.SendMessage(p, "This command can only be used in-game!"); return; }

            if (p.muted) { Player.SendMessage(p, "You are currently muted and cannot use this command."); return; }
            if (Server.chatmod && !p.voice) { Player.SendMessage(p, "Chat moderation is on, you cannot emote."); return; }

            if (Server.worldChat)
            {
                Player.GlobalChat(p, p.color + "*" + p.name + " " + message, false);
            }
            else
            {
                Player.GlobalChatLevel(p, p.color + "*" + p.name + " " + message, false);
            }
            //IRCBot.Say("*" + p.name + " " + message);
            Server.IRC.Say("*" + p.name + " " + message);


        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "What do you need help with, m'boy?! Are you stuck down a well?!");
        }
    }
}