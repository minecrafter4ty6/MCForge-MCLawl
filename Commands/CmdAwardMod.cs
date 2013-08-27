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
    public sealed class CmdAwardMod : Command
    {
        public override string name { get { return "awardmod"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Admin; } }
        public CmdAwardMod() { }

        public override void Use(Player p, string message)
        {
            if (message == "" || message.IndexOf(' ') == -1) { Help(p); return; }

            bool add = true;
            if (message.Split(' ')[0].ToLower() == "add")
            {
                message = message.Substring(message.IndexOf(' ') + 1);
            }
            else if (message.Split(' ')[0].ToLower() == "del")
            {
                add = false;
                message = message.Substring(message.IndexOf(' ') + 1);
            }

            if (add)
            {
                if (message.IndexOf(":") == -1) { Player.SendMessage(p, "&cMissing a colon!"); Help(p); return; }
                string awardName = message.Split(':')[0].Trim();
                string description = message.Split(':')[1].Trim();

                if (!Awards.addAward(awardName, description))
                    Player.SendMessage(p, "This award already exists!");
                else
                    Player.GlobalMessage("Award added: &6" + awardName + " : " + description);
            }
            else
            {
                if (!Awards.removeAward(message))
                    Player.SendMessage(p, "This award doesn't exist!"); //corrected spelling error
                else
                    Player.GlobalMessage("Award removed: &6" + message);
            }

            Awards.Save();
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/awardmod <add/del> [award name] : [description]");
            Player.SendMessage(p, "Adds or deletes a reward with the name [award name]");
            Player.SendMessage(p, "&b/awardmod add Bomb joy : Bomb lots of people" + Server.DefaultColor + " is an example");
        }
    }
}