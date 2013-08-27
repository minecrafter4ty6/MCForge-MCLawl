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
    public sealed class CmdIgnore : Command
    {
        public override string name { get { return "ignore"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }
        public CmdIgnore() { }
        public static byte lastused = 0;
        public static string ignore = "";
        public static string ignore2 = "";

        public override void Use(Player p, string message)
        {
            if (p == null) { Player.SendMessage(p, "This command can only be used in-game!"); return; }
            if (message.Split(' ')[0] == "all")
            {
                p.ignoreglobal = !p.ignoreglobal;
                p.muteGlobal = p.ignoreglobal;
                if (p.ignoreglobal) Player.globalignores.Add(p.name);
                else Player.globalignores.Remove(p.name);
                Player.SendMessage(p, p.ignoreglobal ? "&cAll chat is now ignored!" : "&aAll chat is no longer ignored!");
                return;
            }
            if (message.Split(' ')[0] == "global")
            {
                p.muteGlobal = !p.muteGlobal;
                Player.SendMessage(p, p.muteGlobal ? "&cGlobal Chat is now ignored!" : "&aGlobal Chat is no longer ignored!");
                return;
            }
            if (message.Split(' ')[0] == "list")
            {
                Player.SendMessage(p, "&cCurrently ignoring the following players:");
                foreach (string ignoring in p.listignored)
                {
                    Player.SendMessage(p, "- " + ignoring);
                }
                return;
            }
            Player who = Player.Find(message.Split(' ')[0]);
            if (who == null)
            {
                Player.SendMessage(p, "Could not find player specified!");
                return;
            }

            if (who.name == p.name)
            {
                Player.SendMessage(p, "You cannot ignore yourself!!");
                return;
            }

            if (who.group.Permission >= Server.opchatperm)
            {
                if (p.group.Permission <= who.group.Permission)
                {
                    Player.SendMessage(p, "You cannot ignore an operator of higher rank!");
                    return;
                }
            }
            
     
            if (!Directory.Exists("ranks/ignore"))
            {
                Directory.CreateDirectory("ranks/ignore");
            }
            if (!File.Exists("ranks/ignore/" + p.name + ".txt"))
            {
                File.Create("ranks/ignore/" + p.name + ".txt").Dispose();
            }
            string chosenpath = "ranks/ignore/" + p.name + ".txt";
            if (!File.Exists(chosenpath) || !p.listignored.Contains(who.name))
            {
                p.listignored.Add(who.name);
                Player.SendMessage(p, "Player now ignored: &c" + who.name + "!");
                return;
            }
            if (p.listignored.Contains(who.name))
            {
                p.listignored.Remove(who.name);
                Player.SendMessage(p, "Player is no longer ignored: &a" + who.name + "!");
                return;
            }
            Player.SendMessage(p, "Something is stuffed.... Tell a MCForge Developer!");
        }

        public override void Help(Player p)
        {
            Player.SendMessage(p, "/ignore [Player] - Ignores the specified player.");
            Player.SendMessage(p, "/ignore global - Ignores the global chat.");
            Player.SendMessage(p, "Note: You cannot ignore operators of higher rank!");
        }
    }
}