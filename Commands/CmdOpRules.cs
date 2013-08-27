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

using System.Collections.Generic;
using System.IO;
namespace MCForge.Commands
{
    public sealed class CmdOpRules : Command
    {
        public override string name { get { return "oprules"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "information"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
        public CmdOpRules() { }

        public override void Use(Player p, string message)
        {
            //Do you really need a list for this?
            List<string> oprules = new List<string>();
            if (!File.Exists("text/oprules.txt"))
            {
                File.WriteAllText("text/oprules.txt", "No oprules entered yet!");
            }

			using (StreamReader r = File.OpenText("text/oprules.txt"))
			{
				while (!r.EndOfStream)
					oprules.Add(r.ReadLine());
			}

            Player who = null;
            if (message != "")
            {
                who = Player.Find(message);
                 if (p.group.Permission < who.group.Permission) { Player.SendMessage(p, "You cant send /oprules to another player!"); return; }
            }
            else
            {
                who = p;
            }

            if (who != null)
            {
                //if (who.level == Server.mainLevel && Server.mainLevel.permissionbuild == LevelPermission.Guest) { who.SendMessage("You are currently on the guest map where anyone can build"); }
                who.SendMessage("Server OPRules:");
                foreach (string s in oprules)
                    who.SendMessage(s);
            }
            else
            {
                Player.SendMessage(p, "There is no player \"" + message + "\"!");
            }
        }

        public override void Help(Player p)
        {
            Player.SendMessage(p, "/oprules [player]- Displays server oprules to a player");
        }
    }
}
