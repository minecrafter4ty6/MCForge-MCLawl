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
    public sealed class CmdLogoutMessage : Command
    {
        public override string name { get { return "logoutmessage"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdLogoutMessage() { }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/logoutmessage [Player] [Message] - Customize your logout message.");
            if (Server.mono == true)
            {
                Player.SendMessage(p, "Please note that if the player is offline, the name is case sensitive.");
            }
        }

        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }
            int number = message.Split(' ').Length;
            if (number > 18) { Help(p); return; }
            if (number >= 2)
            {
                int pos = message.IndexOf(' ');
                string t = message.Substring(0, pos);
                string s = message.Substring(pos + 1);
                Player target = Player.Find(t);
                if (target != null)
                {
                    if (!File.Exists("text/logout/" + target.name + ".txt"))
                    {
                        Player.SendMessage(p, "The player you specified does not exist!");
                        return;
                    }
                    else
                    {
                        File.WriteAllText("text/logout/" + target.name + ".txt", s);
                    }
                    Player.SendMessage(p, "The logout message of " + target.name + " has been changed to:");
                    Player.SendMessage(p, s);
                    if (p != null)
                    {
                        Server.s.Log(p.name + " changed " + target.name + "'s logout message to:");
                    }
                    else
                    {
                        Server.s.Log("The Console changed " + target.name + "'s logout message to:");
                    }
                    Server.s.Log(s);
                }
                else
                {
                    if (!File.Exists("text/logout/" + t + ".txt"))
                    {
                        Player.SendMessage(p, "The player you specified does not exist!");
                        return;
                    }
                    else
                    {
                        File.WriteAllText("text/logout/" + t + ".txt", s);
                    }
                    Player.SendMessage(p, "The logout message of " + t + " has been changed to:");
                    Player.SendMessage(p, s);
                    if (p != null)
                    {
                        Server.s.Log(p.name + " changed " + t + "'s logout message to:");
                    }
                    else
                    {
                        Server.s.Log("The Console changed " + t + "'s logout message to:");
                    }
                    Server.s.Log(s);
                }
            }
            /*
            if (number == 1)
            {
                int pos = message.IndexOf(' ');
                string t = message.Substring(0, pos);
                string s = message.Substring(pos + 1);
                if (!File.Exists("text/logout/" + p.name + ".txt"))
                {
                    Player.SendMessage(p, "You do not exist!");
                    return;
                }
                else
                    File.WriteAllText("text/logout/" + p.name + ".txt", message);
                Player.SendMessage(p, "Your logout message has now been changed to:");
                Player.SendMessage(p, message);
                Server.s.Log(p.name + " changed their logout message to:");
                Server.s.Log(t);




            }*/
        }
    }
}