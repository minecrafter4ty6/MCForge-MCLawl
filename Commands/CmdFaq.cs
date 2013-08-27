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
    public sealed class CmdFaq : Command
    {
        public override string name { get { return "faq"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "information"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
        public CmdFaq() { }

        public override void Use(Player p, string message)
        {
            List<string> faq = new List<string>();
            if (!File.Exists("text/faq.txt"))
            {
                File.WriteAllText("text/faq.txt", "Example: What does this server run on? This server runs on &bMCForge");
            }
			using (StreamReader r = File.OpenText("text/faq.txt"))
			{
				while (!r.EndOfStream)
					faq.Add(r.ReadLine());

			}

            Player who = null;
            if (message != "")
            {
                if ((int)p.group.Permission < CommandOtherPerms.GetPerm(this))
                { Player.SendMessage(p, "You cant send the FAQ to another player!"); return; }
                who = Player.Find(message);
            }
            else
            {
                who = p;
            }

            if (who != null)
            {
                who.SendMessage("&cFAQ&f:");
                foreach (string s in faq)
                    who.SendMessage("&f" + s);
            }
            else
            {
                Player.SendMessage(p, "There is no player \"" + message + "\"!");
            }
        }

        public override void Help(Player p)
        {
            Player.SendMessage(p, "/faq [player]- Displays frequently asked questions");
        }
    }
}
