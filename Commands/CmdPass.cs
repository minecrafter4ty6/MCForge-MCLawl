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
    public sealed class CmdPass : Command
    {
        public override string name { get { return "pass"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }

        public CmdPass() { }

        public override void Use(Player p, string message)
        {

            if (p.group.Permission < Server.verifyadminsrank)
            {
                Player.SendMessage(p, "You do not have the &crequired rank to use this command!");
                return;
            }

            if (!Server.verifyadmins)
            {
                Player.SendMessage(p, "Verification of admins is &cdisabled!");
                return;
            }

            if (!p.adminpen)
            {
                Player.SendMessage(p, "You have &calready verified.");
                return;
            }

            if (p.passtries >= 3)
            {
                p.Kick("Did you really think you could keep on guessing?");
                return;
            }

            if (String.IsNullOrEmpty(message.Trim()))
            {
                Help(p);
                return;
            }

            int number = message.Split(' ').Length;

            if (number > 1)
            {
                Player.SendMessage(p, "Your password must be &cone " + Server.DefaultColor + "word!");
                return;
            }

            if (!Directory.Exists("extra/passwords"))
            {
                Player.SendMessage(p, "You have not &cset a password, " + Server.DefaultColor + "use &a/setpass [Password] &cto set one!");
                return;
            }

            DirectoryInfo di = new DirectoryInfo("extra/passwords/");
            FileInfo[] fi = di.GetFiles("*.dat");
            if (!File.Exists("extra/passwords/" + p.name + ".dat"))
            {
                Player.SendMessage(p, "You have not &cset a password, " + Server.DefaultColor + "use &a/setpass [Password] &cto set one!");
                return;
            }

            if (PasswordHasher.MatchesPass(p.name, message))
            {
                Player.SendMessage(p, "Thank you, " + p.color + p.name + Server.DefaultColor + "! You have now &averified " + Server.DefaultColor + "and have &aaccess to admin commands and features!");
                if (p.adminpen)
                {
                    p.adminpen = false;
                }
                return;
            }

            p.passtries++;
            Player.SendMessage(p, "&cWrong Password. " + Server.DefaultColor + "Remember your password is &ccase sensitive!");
            Player.SendMessage(p, "Forgot your password? " + Server.DefaultColor + "Contact the owner so they can reset it!");
        }

        public override void Help(Player p)
        {
            Player.SendMessage(p, "/pass [Password] - If you are an admin, use this command to verify");
            Player.SendMessage(p, "your login. You will need to use this to be given access to commands");
            Player.SendMessage(p, "Note: If you do not have a password, use /setpass [Password]");
        }
    }
}