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
using System.Data;
using MCForge.SQL;
namespace MCForge.Commands
{
    public sealed class CmdUnbanip : Command
    {
        public override string name { get { return "unbanip"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdUnbanip() { }

        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }
            if (message[0] == '@')
            {
                message = message.Remove(0, 1).Trim();
                Player who = Player.Find(message);
                if (who == null)
                {
                    DataTable ip;
                    int tryCounter = 0;
             rerun: try
                    {
                        Database.AddParams("@Name", message);
                        ip = Database.fillData("SELECT IP FROM Players WHERE Name = @Name");
                    }
                    catch (Exception e)
                    {
                        tryCounter++;
                        if (tryCounter < 10)
                        {
                            goto rerun;
                        }
                        else
                        {
                            Server.ErrorLog(e);
                            Player.SendMessage(p, "There was a database error fetching the IP address.  It has been logged.");
                            return;
                        }
                    }
                    if (ip.Rows.Count > 0)
                    {
                        message = ip.Rows[0]["IP"].ToString();
                    }
                    else
                    {
                        Player.SendMessage(p, "Unable to find an IP address for that user.");
                        return;
                    }
                    ip.Dispose();
                }
                else
                {
                    message = who.ip;
                }
            }

            if (message.IndexOf('.') == -1) { Player.SendMessage(p, "Not a valid ip!"); return; }
            if (p != null) if (p.ip == message) { Player.SendMessage(p, "You shouldn't be able to use this command..."); return; }
            if (!Server.bannedIP.Contains(message)) { Player.SendMessage(p, message + " doesn't seem to be banned..."); return; }
            Server.bannedIP.Remove(message); Server.bannedIP.Save("banned-ip.txt", false);

            if (p != null)
            {
                Server.IRC.Say(message.ToLower() + " was un-ip-banned by " + p.name + ".");
                Server.s.Log("IP-UNBANNED: " + message.ToLower() + " by " + p.name + ".");
                Player.GlobalMessage(message + " was &8un-ip-banned" + Server.DefaultColor + " by " + p.color + p.name + Server.DefaultColor + ".");
            }
            else
            {
                Server.IRC.Say(message.ToLower() + " was un-ip-banned by console.");
                Server.s.Log("IP-UNBANNED: " + message.ToLower() + " by console.");
                Player.GlobalMessage(message + " was &8un-ip-banned" + Server.DefaultColor + " by console.");
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/unbanip <ip/player> - Un-bans an ip.  Also accepts a player name when you use @ before the name.");
        }
    }
}