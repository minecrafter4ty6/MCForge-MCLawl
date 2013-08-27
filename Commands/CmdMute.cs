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
    /// <summary>
    /// Bug:
    /// Doesn't save mute.txt
    /// Players with the defaultRank can unmute themself
    /// </summary>
    public sealed class CmdMute : Command
    {
        public override string name { get { return "mute"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdMute() { }

        public override void Use(Player p, string message)
        {
            if (message == "" || message.Split(' ').Length > 2) { Help(p); return; }
            Player who = Player.Find(message);
            if (Server.forgeProtection == ForgeProtection.Mod && who != null && who.isGCMod)
            {
                Player.SendMessage(p, "%cGlobal Chat Moderators can't be muted");
                return;
            }
            if (who == null)
            {
                if (Server.muted.Contains(message))
                {
                    Server.muted.Remove(message);
                    Player.GlobalMessage(message + Server.DefaultColor + " is not online but is now &bun-muted");
                    Extensions.DeleteLineWord("ranks/muted.txt", who.name.ToLower());
                    return;
                }
            }

            if (who == p)
            {
                if (p.muted)
                {
                    p.muted = false;
                    Player.SendMessage(p, "You &bun-muted" + Server.DefaultColor + " yourself!");
                    Extensions.DeleteLineWord("ranks/muted.txt", p.name.ToLower());
                    return;
                }
                else
                {
                    Player.SendMessage(p, "You cannot mute yourself!");
                }
                return;
            }

            if (who.muted)
            {
                who.muted = false;
                Player.GlobalChat(who, who.color + who.name + Server.DefaultColor + " has been &bun-muted", false);
                Extensions.DeleteLineWord("ranks/muted.txt", who.name.ToLower());
            }
            else
            {
                if (p != null)
                {
                    if (who != p) if (who.group.Permission >= p.group.Permission) { Player.SendMessage(p, "Cannot mute someone of a higher or equal rank."); return; }
                }
                who.muted = true;
                Player.GlobalChat(who, who.color + who.name + Server.DefaultColor + " has been &8muted", false);
                using (StreamWriter writer = new StreamWriter("ranks/muted.txt", true))
                {
                    writer.WriteLine(who.name.ToLower());
                }
                Server.s.Log("SAVED: ranks/muted.txt");
            }
        }

        public override void Help(Player p)
        {
            Player.SendMessage(p, "/mute <player> - Mutes or unmutes the player.");
        }
    }
}