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
using System.Collections.Generic;
using System.Linq;
namespace MCForge.Commands
{
    public sealed class CmdPatrol : Command
    {
        public override string name { get { return "patrol"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Builder; } }
        public CmdPatrol() { }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/patrol - Teleports you to a random " + Group.findPermInt(CommandOtherPerms.GetPerm(this)).name + " or lower");
        }

        public override void Use(Player p, string message)
        {
            if (message != "")
            {
                Help(p);
                return;
            }
            if (p == null)
            {
                Player.SendMessage(p, "Are you stupid? =S You can't use this in the console!");
                return;
            }
            List<string> getpatrol = (from pl in Player.players where (int) pl.@group.Permission <= CommandOtherPerms.GetPerm(this) select pl.name).ToList();
            if (getpatrol.Count <= 0)
            {
                Player.SendMessage(p, "There must be at least one guest online to use this command!");
                return;
            }
            Random random = new Random();
            int index = random.Next(getpatrol.Count);
            string value = getpatrol[index];
            Player who = Player.Find(value);
            Command.all.Find("tp").Use(p, who.name);
            Player.SendMessage(p, "You are now visiting " + who.color + who.name + "!");
        }
    }
}