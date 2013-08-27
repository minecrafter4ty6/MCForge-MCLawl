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
    /// <summary>
    /// This is the command /afk
    /// use /help afk in-game for more info
    /// </summary>
    public sealed class CmdAfk : Command
    {
        public override string name { get { return "afk"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "information"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }
        public static string keywords { get { return ""; } }
        public CmdAfk() { }

        public override void Use(Player p, string message)
        {
            if (p != null)
            {

                if (Server.chatmod) {
                    Player.SendMessage(p, "You cannot use /afk while chat moderation is enabled");
                    return;
                }

                if (message != "list")
                {
                    if (p.joker)
                    {
                        message = "";
                    }
                    if (!Server.afkset.Contains(p.name))
                    {
                        Server.afkset.Add(p.name);
                        if (p.muted)
                        {
                            message = "";
                        }
                        Player.GlobalMessage("-" + p.color + p.name + Server.DefaultColor + "- is AFK " + message);
                        //IRCBot.Say(p.name + " is AFK " + message);
                        Server.IRC.Say(p.name + " is AFK " + message);
                        p.afkStart = DateTime.Now;
                        return;

                    }
                    else
                    {
                        Server.afkset.Remove(p.name);
                        Player.GlobalMessage("-" + p.color + p.name + Server.DefaultColor + "- is no longer AFK");
                        //IRCBot.Say(p.name + " is no longer AFK");
                        Server.IRC.Say(p.name + " is no longer AFK");
                        return;
                    }
                }
                else
                {
                    foreach (string s in Server.afkset) Player.SendMessage(p, s);
                    return;
                }
            }
            Player.SendMessage(p, "This command can only be used in-game");
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/afk <reason> - mark yourself as AFK. Use again to mark yourself as back");
        }
    }
}