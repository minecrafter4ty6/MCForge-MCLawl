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
using System.IO;
using MCForge.Util;
namespace MCForge.Commands
{
    public sealed class CmdSetPass : Command
    {
        public override string name { get { return "setpass"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
        public CmdSetPass() { }

        public override void Use(Player p, string message)
        {
            if (!Directory.Exists("extra/passwords"))
                Directory.CreateDirectory("extra/passwords");
            if (p.group.Permission < Server.verifyadminsrank)
            {
                Player.SendMessage(p, "You do not have the &crequired rank " + Server.DefaultColor + "to use this command!");
                return;
            }
            if (!Server.verifyadmins)
            {
                Player.SendMessage(p, "Verification of admins is &cdisabled!");
                return;
            }
            if (p.adminpen)
            {
                if (File.Exists("extra/passwords/" + p.name + ".dat"))
                {
                    Player.SendMessage(p, "&cYou already have a password set. " + Server.DefaultColor + "You &ccannot change " + Server.DefaultColor + "it unless &cyou verify it with &a/pass [Password]. " + Server.DefaultColor + "If you have &cforgotten " + Server.DefaultColor + "your password, contact &c" + Server.server_owner + Server.DefaultColor + " and they can &creset it!");
                    return;
                }
            }
            if (String.IsNullOrEmpty(message.Trim()))
            {
                Help(p);
                return;

            }
            int number = message.Split(' ').Length;
            if (number > 1)
            {
                Player.SendMessage(p, "Your password must be one word!");
                return;
            }
            PasswordHasher.StoreHash(p.name, message);
            Player.SendMessage(p, "Your password has &asuccessfully &abeen set to:");
            Player.SendMessage(p, "&c" + message);
        }

        public override void Help(Player p)
        {
            Player.SendMessage(p, "/setpass [Password] - Sets your admin password to [password].");
            Player.SendMessage(p, "Note: Do NOT set this as your Minecraft password!");
        }
    }
}
