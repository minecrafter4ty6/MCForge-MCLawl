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
using System.Text.RegularExpressions;
using MCForge.SQL;
namespace MCForge.Commands
{
    public sealed class CmdSend : Command
    {
        public override string name { get { return "send"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Builder; } }
        public CmdSend() { }

        public override void Use(Player p, string message)
        {
            if (message == "" || message.IndexOf(' ') == -1) { Help(p); return; }

            Player who = Player.Find(message.Split(' ')[0]);

            string whoTo, fromname;
            if (who != null) whoTo = who.name;
            else whoTo = message.Split(' ')[0];
            if (p != null) fromname = p.name;
            else fromname = "Console";

            if (!Player.ValidName(whoTo))
            {
                Player.SendMessage(p, "%cIllegal name!");
                return;
            }

            message = message.Substring(message.IndexOf(' ') + 1);

            if (!Regex.IsMatch(message.ToLower(), @".*%([0-9]|[a-f]|[k-r])%([0-9]|[a-f]|[k-r])%([0-9]|[a-f]|[k-r])"))
            {
                if (Regex.IsMatch(message.ToLower(), @".*%([0-9]|[a-f]|[k-r])(.+?).*"))
                {
                    Regex rg = new Regex(@"%([0-9]|[a-f]|[k-r])(.+?)");
                    MatchCollection mc = rg.Matches(message.ToLower());
                    if (mc.Count > 0)
                    {
                        Match ma = mc[0];
                        GroupCollection gc = ma.Groups;
                        message.Replace("%" + gc[1].ToString().Substring(1), "&" + gc[1].ToString().Substring(1));
                    }
                }
            }

            //DB
            if (message.Length > 255 && Server.useMySQL) { Player.SendMessage(p, "Message was too long. The text below has been trimmed."); Player.SendMessage(p, message.Substring(256)); message = message.Remove(256); }
            //safe against SQL injections because whoTo is checked for illegal characters
            Database.executeQuery("CREATE TABLE if not exists `Inbox" + whoTo + "` (PlayerFrom CHAR(20), TimeSent DATETIME, Contents VARCHAR(255));");
            if (!Server.useMySQL)
                Server.s.Log(message.Replace("'", "\\'"));
            Database.AddParams("@From", fromname);
            Database.AddParams("@Time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            Database.AddParams("@Content", message);
            Database.executeQuery("INSERT INTO `Inbox" + whoTo + "` (PlayerFrom, TimeSent, Contents) VALUES (@From, @Time, @Content)");
            //DB

            Player.SendMessage(p, "Message sent to &5" + whoTo + ".");
            if (who != null) who.SendMessage("Message recieved from &5" + fromname + Server.DefaultColor + ".");
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/send [name] <message> - Sends <message> to [name].");
        }
    }
}