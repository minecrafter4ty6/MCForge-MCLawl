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
    public sealed class CmdNews : Command
    {
        public override string name { get { return "news"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "information"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
        public override void Use(Player p, string message)
        {
            string newsFile = "text/news.txt";
            if (!File.Exists(newsFile) || (File.Exists(newsFile) && File.ReadAllLines(newsFile).Length == -1))
            {
                using (var SW = new StreamWriter(newsFile))
                {
                    SW.WriteLine("News have not been created. Put News in '" + newsFile + "'.");
                }
                return;
            }
            string[] strArray = File.ReadAllLines(newsFile);
            if (message == "")
            {
                foreach (string t in strArray)
                {
                    Player.SendMessage(p, t);
                }
            }
            else
            {
                string[] split = message.Split(' ');
                if (split[0] == "all") { if ((int)p.group.Permission < CommandOtherPerms.GetPerm(this)) { Player.SendMessage(p, "You must be at least " + Group.findPermInt(CommandOtherPerms.GetPerm(this)).name + " to send this to all players."); return; } for (int k = 0; k < strArray.Length; k++) { Player.GlobalMessage(strArray[k]); } return; }
                Player player = Player.Find(split[0]);
                if (player == null) { Player.SendMessage(p, "Could not find player \"" + split[0] + "\"!"); return; }
                foreach (string t in strArray)
                {
                    Player.SendMessage(player, t);
                }
                Player.SendMessage(p, "The News were successfully sent to " + player.name + ".");
               
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/news - Shows server news.");
            Player.SendMessage(p, "/news <player> - Sends the News to <player>.");
            Player.SendMessage(p, "/news all - Sends the News to everyone.");
        }
    }
}