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
    public sealed class CmdTnt : Command
    {
        public override string name { get { return "tnt"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Builder; } }
        public CmdTnt() { }

        public override void Use(Player p, string message)
        {
            if (message.Split(' ').Length > 1) { Help(p); return; }

            if (p.BlockAction == 13 || p.BlockAction == 14)
            {
                if (p.allowTnt == false) 
                {
                    Player.SendMessage(p, "Tnt usage is not allowed at the moment!");
                    return;
                }

                p.BlockAction = 0; Player.SendMessage(p, "TNT mode is now &cOFF" + Server.DefaultColor + ".");
            }
            else if (message.ToLower() == "small" || message == "")
            {
                if (p.allowTnt == true) 
                {
                    p.BlockAction = 13; Player.SendMessage(p, "TNT mode is now &aON" + Server.DefaultColor + ".");
                    return;
                }
                Player.SendMessage(p, "Tnt usage is not allowed at the moment!");
                return;
            }
            else if (message.ToLower() == "big")
            {
                if (p.allowTnt == false) 
                {
                    Player.SendMessage(p, "Tnt usage is not allowed at the moment!");
                    return;
                }
                if ((int)p.group.Permission >= CommandOtherPerms.GetPerm(this, 1))
                {
                    p.BlockAction = 14; Player.SendMessage(p, "TNT (Big) mode is now &aON" + Server.DefaultColor + ".");
                }
                else
                {
                    Player.SendMessage(p, "This mode is reserved for " + Group.findPermInt(CommandOtherPerms.GetPerm(this, 1)).name + "+");
                }
            }
            else if (message.ToLower() == "allow")
            {
                if ((int)p.group.Permission >= CommandOtherPerms.GetPerm(this, 2))
                {
                    p.allowTnt = true;
                    Player.SendMessage(p, "&cTnt usage has now been enabled!");
                    return;
                }
                Player.SendMessage(p, "You must be " + Group.findPermInt(CommandOtherPerms.GetPerm(this, 2)).name + "+ to use this command.");
                return;
            }
            else if (message.ToLower() == "disallow")
            {
                if ((int)p.group.Permission >= CommandOtherPerms.GetPerm(this, 2))
                {
                    p.allowTnt = false;
                    Player.SendMessage(p, "&cTnt usage has now been disabled!");
                    return;
                }
                Player.SendMessage(p, "You must be " + Group.findPermInt(CommandOtherPerms.GetPerm(this, 2)).name + "+ to use this command.");
                return;
            }
            else if (message.ToLower() == "nuke")
            {
                if (p.allowTnt == false) 
                {
                    Player.SendMessage(p, "Tnt usage is not allowed at the moment!");
                    return;
                }
                if ((int)p.group.Permission >= CommandOtherPerms.GetPerm(this, 3))
                {
                    p.BlockAction = 15; Player.SendMessage(p, "TNT (Nuke) mode is now &aON" + Server.DefaultColor + ".");
                }
                else
                {
                    Player.SendMessage(p, "This mode is reserved for " + Group.findPermInt(CommandOtherPerms.GetPerm(this, 3)).name + "+");
                }
            }
            else
            {
                Help(p);
            }

            p.painting = false;
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/tnt [small/big/nuke] - Creates exploding TNT (with Physics 3).");
            Player.SendMessage(p, "Big and Nuke TNT is reserved for " + Group.findPermInt(CommandOtherPerms.GetPerm(this, 3)).name + "+");
            if ((int)p.group.Permission >= CommandOtherPerms.GetPerm(this, 2))
            {
                Player.SendMessage(p, "/tnt allow - Allows the use of tnt server-wide.");
                Player.SendMessage(p, "/tnt disallow - Disallows the use of tnt server-wide.");
            }


        }
    }
}