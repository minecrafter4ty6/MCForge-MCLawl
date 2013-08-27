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
    public sealed class CmdDelTempRank : Command
    {
        public override string name { get { return "deltemprank"; } }
        public override string shortcut { get { return "dtr"; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdDelTempRank() { }
        public override void Use(Player p, string message)
        {
            string alltext = File.ReadAllText("text/tempranks.txt");
            if (!alltext.Contains(message))
            {
                Player.SendMessage(p, "&cPlayer &a" + message + "&c Has not been assigned a temporary rank. Cannot unnasign.");
                return;
            }
            string alltempranks = "";
           Player who = Player.Find(message);
           foreach (string line in File.ReadAllLines("text/tempranks.txt"))
           {
               if (line.Contains(message))
               {
                   string group = line.Split(' ')[2];
                   Group newgroup = Group.Find(group);
                   Command.all.Find("setrank").Use(null, who.name + " " + newgroup.name);
                   Player.SendMessage(p, "&eTemporary rank of &a" + message + "&e has been unassigned");
                   Player.SendMessage(who, "&eYour temporary rank has been unassigned");
               }
               if (!line.Contains(message))
               {
                   alltempranks = alltempranks + line + "\r\n";
               }
           }
           File.WriteAllText("text/tempranks.txt", alltempranks);
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/deltemprank - Deletes someones temporary rank");
        }
    }
}
