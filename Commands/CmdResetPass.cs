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
using System.IO;


namespace MCForge.Commands
{
    public class CmdResetPass : Command
    {
        public override string name { get { return "resetpass"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Admin; } }
        public CmdResetPass() { }

        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }
            Player who = Player.Find(message);
            if (Server.server_owner == "Notch" || Server.server_owner == "")
            {
                Player.SendMessage(p, "Please tell the server owner to change the 'Server Owner' property.");
                return;
            }
            if (p != null && Server.server_owner != p.name)
            {
                Player.SendMessage(p, "You're not the server owner!");
                return;
            }
            if (p != null && p.adminpen == true)
            {
                Player.SendMessage(p, "You cannot reset a password while in the admin pen!");
                return;
            }
            if (who == null)
            {
                Player.SendMessage(p, "The specified player does not exist.");
                return;
            }
            if (!File.Exists("extra/passwords/" + who.name + ".xml"))
            {
                Player.SendMessage(p, "The player you specified does not have a password!");
                return;
            }
            try
            {
                File.Delete("extra/passwords/" + who.name + ".xml");
                Player.SendMessage(p, "The admin password has sucessfully been removed for " + who.color + who.name + "!");
            }
            catch (Exception e)
            {
                Player.SendMessage(p, "Password Deletion Failed. Please manually delete the file. It is in extra/passwords.");
                Server.ErrorLog(e);
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/resetpass [Player] - Resets the password for the specified player.");
            Player.SendMessage(p, "Note: May only be used by the server owner!");
        }
    }
}