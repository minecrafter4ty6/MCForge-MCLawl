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
    public sealed class CmdAgree : Command
    {
        public override string name { get { return "agree"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }
        public CmdAgree() { }

        public override void Use(Player p, string message)
        {
            if (Server.agreetorulesonentry == false)
            {
                Player.SendMessage(p, "This command can only be used if agree-to-rules-on-entry is enabled!");
                return;
            }
            if (p == null)
            {
                Player.SendMessage(p, "This command can only be used in-game");
                return;
            }
            //If someone is ranked before agreeing to the rules they are locked and cannot use any commands unless demoted back to guest
            /*if (p.group.Permission > LevelPermission.Guest)
            {
                Player.SendMessage(p, "Your rank is higher than guest and you have already agreed to the rules!");
                return;
            }*/
            var agreed = File.ReadAllText("ranks/agreed.txt");
            /*
            if (File.Exists("logs/" + DateTime.Now.ToString("yyyy") + "-" + DateTime.Now.ToString("MM") + "-" + DateTime.Now.ToString("dd") + ".txt"))
            {
                var checklogs = File.ReadAllText("logs/" + DateTime.Now.ToString("yyyy") + "-" + DateTime.Now.ToString("MM") + "-" + DateTime.Now.ToString("dd") + ".txt");
                if (!checklogs.Contains(p.name.ToLower() + " used /rules"))
                {
                    Player.SendMessage(p, "&9You must read /rules before agreeing!");
                    return;
                }
            }*/
            if (p.hasreadrules == false)
            {
                Player.SendMessage(p, "&9You must read /rules before agreeing!");
                return;
            }
            if ((agreed+" ").Contains(" " + p.name.ToLower() + " ")) //Edited to prevent inner names from working.
            {
                Player.SendMessage(p, "You have already agreed to the rules!");
                return;
            }
            p.agreed = true;
            Player.SendMessage(p, "Thank you for agreeing to follow the rules. You may now build and use commands!");
            string playerspath = "ranks/agreed.txt";
            if (File.Exists(playerspath))
            { 
                //We don't want player "test" to have already agreed if "nate" and "stew" agrred.
                // the preveious one, though, would put "natesteve" which also allows test
                //There is a better way, namely regular expressions, but I'll worry about that later.
                File.AppendAllText(playerspath, " " + p.name.ToLower());  //Ensures every name is seperated by a space.
            }
        
        }

        public override void Help(Player p)
        {
            Player.SendMessage(p, "/agree - Agree to the rules when entering the server");
        }
    }
}