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
namespace MCForge
{
    public sealed class CmdCTF : Command
    {
        public override string name { get { return "ctf"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "game"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdCTF() { }

        public override void Use(Player p, string message)
        {
            if (p == null)
            {
                Server.s.Log("You must be in-game");
                return;
            }
            if ((message != null && String.IsNullOrEmpty(message))) { Help(p); return; }
            if (message == "start")
            {
                //Prevent null errors :/
                if (Server.ctf == null)
                {
                    Player.SendMessage(p, "Starting CTF..");
                    Server.ctf = new Auto_CTF();
                    Player.GlobalMessage("A CTF GAME IS STARTING AT CTF! TYPE /goto CTF to join!");
                }
                else if (Server.ctf.started)
                {
                    Player.SendMessage(p, "A ctf is already in session!");
                    return;
                }
                else if (!Server.ctf.started)
                {
                    Server.ctf.Start();
                    Player.GlobalMessage("A CTF GAME IS STARTING AT CTF! TYPE /goto CTF to join!");
                }
            }
            if (message == "stop")
            {
                //Prevent null error :/
                if (Server.ctf == null)
                {
                    Player.SendMessage(p, "A ctf game isnt active!");
                    return;
                }
                else if (!Server.ctf.started)
                {
                    Player.SendMessage(p, "A ctf game isnt active!");
                    return;
                }
                else
                    Server.ctf.Stop();
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/ctf <start/stop> - Start / Stop the ctf game!");
        }
    }
}
