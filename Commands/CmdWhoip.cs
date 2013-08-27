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

using System.Data;
using MCForge.SQL;
namespace MCForge.Commands
{
    public sealed class CmdWhoip : Command
    {
        public override string name { get { return "whoip"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "information"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdWhoip() { }

        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }
            if (message.IndexOf("'") != -1) { Player.SendMessage(p, "Cannot parse request."); return; }

            Database.AddParams("@IP", message);
            DataTable playerDb = Database.fillData("SELECT Name FROM Players WHERE IP=@IP");

            if (playerDb.Rows.Count == 0) { Player.SendMessage(p, "Could not find anyone with this IP"); return; }

            string playerNames = "Players with this IP: ";

            for (int i = 0; i < playerDb.Rows.Count; i++)
            {
                playerNames += playerDb.Rows[i]["Name"] + ", ";
            }
            playerNames = playerNames.Remove(playerNames.Length - 2);

            Player.SendMessage(p, playerNames);
            playerDb.Dispose();
        }
        public override void Help(Player p)
        {
            p.SendMessage("/whoip <ip address> - Displays players associated with a given IP address.");
        }
    }
}