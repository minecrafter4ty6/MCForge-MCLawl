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
    public sealed class CmdGlobal : Command
    {
        public override string name { get { return "global"; } }
        public override string shortcut { get { return "gc"; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }
        public CmdGlobal() { }
        //bla
        public override void Use(Player p, string message)
        {
            if (p != null && (p.isGCMod || p.isMod || p.isDev) && !p.verifiedName) { Player.SendMessage(p, "You can't use GC, because the server hasn't verify-names on"); return; }

            if (String.IsNullOrEmpty(message)) { 
                p.InGlobalChat = !p.InGlobalChat;
                Player.SendMessage(p, p.InGlobalChat ? "%aGlobal Chat enabled" : "%cGlobal Chat Disabled");
                return;
             }

            if (!Server.UseGlobalChat) { Player.SendMessage(p, "Global Chat is disabled."); return; }
            if (p != null && p.muted) { Player.SendMessage(p, "You are muted."); return; }
            if (p != null && p.muteGlobal) { Player.SendMessage(p, "You cannot use Global Chat while you have it muted."); return; }
            if (p != null && Server.chatmod && !p.voice) { Player.SendMessage(p, "You cannot use Global Chat while in Chat Moderation!"); return; }
            if (p != null && !Server.gcaccepted.Contains(p.name.ToLower())) { RulesMethod(p); return; }
            if (p != null)
            {
                string reason;
                if (Server.gcnamebans.TryGetValue(p.name.ToLower(), out reason)) {
                    Player.SendMessage(p, "You are %cBANNED" + Server.DefaultColor + " from" + Server.GlobalChatColor + " Global Chat" + Server.DefaultColor + " by " + reason);
                    Player.SendMessage(p, "You can apply a 'Ban Appeal' at %9www.mcforge.net");
                    return;
                }
                if (Server.gcipbans.TryGetValue(p.exIP, out reason)) {
                    Player.SendMessage(p, "Your IP is %cBANNED" + Server.DefaultColor + " from" + Server.GlobalChatColor + " Global Chat" + Server.DefaultColor + " by " + reason);
                    Player.SendMessage(p, "You can apply a 'Ban Appeal' at %9www.mcforge.net");
                    return;
                }
            }            //Server.GlobalChat.Say((p != null ? p.name + ": " : "Console: ") + message, p);
            Server.GlobalChat.Say(p == null ? "Console: " + message : p.name + ": " + message, p);
            Player.GlobalMessage(Server.GlobalChatColor + "<[Global] " + (p != null ? p.name + ": " : "Console: ") + "&f" + (Server.profanityFilter ? ProfanityFilter.Parse(message) : message), true);

        }
        public void RulesMethod(Player p)
        {
            Player.SendMessage(p, "&cBy using the Global Chat you agree to the following rules:");
            Player.SendMessage(p, "1. No Spamming");
            Player.SendMessage(p, "2. No Advertising (Trying to get people to your server)");
            Player.SendMessage(p, "3. No links");
            Player.SendMessage(p, "4. No Excessive Cursing (You are allowed to curse, but not pointed at anybody)");
            Player.SendMessage(p, "5. No use of $ Variables.");
            Player.SendMessage(p, "6. English only. No exceptions.");
            Player.SendMessage(p, "7. Be respectful");
            Player.SendMessage(p, "8. Do not ask for ranks");
            Player.SendMessage(p, "9. Do not ask for a server name");
            Player.SendMessage(p, "10. Use common sense.");
            Player.SendMessage(p, "11. Don't say any server name");
            Player.SendMessage(p, "&3Type /gcaccept to accept these rules");
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/global [message] - Send a message to Global Chat.");
        }
    }
}
