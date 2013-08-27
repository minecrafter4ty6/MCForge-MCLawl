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
    public sealed class CmdSearch : Command
    {
        public override string name { get { return "search"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "information"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Builder; } }
        public CmdSearch() { }

        public override void Use(Player p, string message)
        {
            if (message.Split(' ').Length < 2)
            {
                Help(p);
                return;
            }
            string type = message.Split(' ')[0];
            string keyword = message.Remove(0, (type.Length + 1));
            //
            if (type.ToLower().Contains("command") || type.ToLower().Contains("cmd"))
            {
                bool mode = true;
                string[] keywords = keyword.Split(' ');
                string[] found = null;
                if (keywords.Length == 1) { found = Commands.CommandKeywords.Find(keywords[0]); }
                else { found = Commands.CommandKeywords.Find(keywords); }
                if (found == null) { Player.SendMessage(p, "No commands found matching keyword(s): '" + message.Remove(0, (type.Length + 1)) + "'"); }
                else
                {
                    Player.SendMessage(p, "&bfound: ");
                    foreach (string s in found) { if (mode) { Player.SendMessage(p, "&2/" + s); } else { Player.SendMessage(p, "&9/" + s); } mode = (mode) ? false : true; }
                }
            }
            if (type.ToLower().Contains("block"))
            {
                string blocks = "";
                bool mode = true;
                for (byte i = 0; i < 255; i++)
                {
                    if (Block.Name(i).ToLower() != "unknown")
                    {
                        if (Block.Name(i).Contains(keyword))
                        {
                            if (mode) { blocks += Server.DefaultColor + ", &9" + Block.Name(i); }
                            else { blocks += Server.DefaultColor + ", &2" + Block.Name(i); }
                            mode = (mode) ? false : true;
                        }
                    }
                }
                if (blocks == "") { Player.SendMessage(p, "No blocks found containing &b" + keyword); }
                Player.SendMessage(p, blocks.Remove(0, 2));
            }
            if (type.ToLower().Contains("rank"))
            {
                string ranks = "";
                foreach (Group g in Group.GroupList)
                {
                    if (g.name.Contains(keyword)) 
                    {
                        ranks += g.color + g.name + "'";
                    }
                }
                if (ranks == "") { Player.SendMessage(p, "No ranks found containing &b" + keyword); }
                else { foreach (string r in ranks.Split('\'')) { Player.SendMessage(p, r); } }
            }
            if (type.ToLower().Contains("player"))
            {
                string players = "";
                foreach (Player who in Player.players)
                {
                    if (who.name.ToLower().Contains(keyword.ToLower()))
                    {
                        players += ", " + who.color + who.name;
                    }
                }
                if (players == "") { Player.SendMessage(p, "No usernames found containing &b" + keyword); }
                else { Player.SendMessage(p, players.Remove(0, 2)); }
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "&b/search &2commands &a<keywords[more]> &e- finds commands with those keywords");
            Player.SendMessage(p, "&b/search &2blocks &a<keyword> &e- finds blocks with that keyword");
            Player.SendMessage(p, "&b/search &2ranks &a<keyword> &e- finds blocks with that keyword");
            Player.SendMessage(p, "&b/search &2players &a<keyword> &e- find players with that keyword");
        }
    }
}