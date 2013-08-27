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
    public sealed class CmdAward : Command
    {
        public override string name { get { return "award"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdAward() { }

        public override void Use(Player p, string message)
        {
            if (message == "" || message.IndexOf(' ') == -1) { Help(p); return; }

            bool give = true;
            if (message.Split(' ')[0].ToLower() == "give")
            {
                give = true;
                message = message.Substring(message.IndexOf(' ') + 1);
            }
            else if (message.Split(' ')[0].ToLower() == "take")
            {
                give = false;
                message = message.Substring(message.IndexOf(' ') + 1);
            }
            
            string foundPlayer = message.Split(' ')[0];
            Player who = Player.Find(message);
            if (who != null) foundPlayer = who.name;
            string awardName = message.Substring(message.IndexOf(' ') + 1);
            if (!Awards.awardExists(awardName))
            {
                Player.SendMessage(p, "The award you entered doesn't exist");
                Player.SendMessage(p, "Use /awards for a list of awards");
                return;
            }

            if (give)
            {
                if (Awards.giveAward(foundPlayer, awardName))
                {
                    Player.GlobalMessage(Server.FindColor(foundPlayer) + foundPlayer + Server.DefaultColor + " was awarded: &b" + Awards.camelCase(awardName));
                }
                else
                {
                    Player.SendMessage(p, "The player already has that award!");
                }
            }
            else
            {
                if (Awards.takeAward(foundPlayer, awardName))
                {
                    Player.GlobalMessage(Server.FindColor(foundPlayer) + foundPlayer + Server.DefaultColor + " had their &b" + Awards.camelCase(awardName) + Server.DefaultColor + " award removed");
                }
                else
                {
                    Player.SendMessage(p, "The player didn't have the award you tried to take");
                }
            }

            Awards.Save();
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/award <give/take> [player] [award] - Gives [player] the [award]");
            Player.SendMessage(p, "If no Give or Take is given, Give is used");
            Player.SendMessage(p, "[award] needs to be the full award's name. Not partial");
        }
    }
}