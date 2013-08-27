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
using System.Threading;
namespace MCForge.Commands
{
    public sealed class CmdXmute : Command
    {
        public override string name { get { return "xmute"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public override void Use(Player p, string message)
        {
            if (message == "")
            {
                Help(p);
                return;
            }

            if (p == null)
            {
                Player.SendMessage(p, "This command can only be used in-game. Use /mute [Player] instead.");
                return;
            }

            var split = message.Split(' ');
            Player muter = Player.Find(split[0]);
            if (muter == null)
            {
                Player.SendMessage(p, "Player not found.");
                return;
            }

            if (p != null && muter.group.Permission > p.group.Permission)
            {
                Player.SendMessage(p, "You cannot xmute someone ranked higher than you!");
                return;
            }
            if (p == muter)
            {
                Player.SendMessage(p, "You cannot use xmute on yourself!");
                return;
            }
            Command.all.Find("mute").Use(p, muter.name);

            int time = 120;
            try
            {
                time = Convert.ToInt32(message.Split(' ')[1]);
            }
            catch/* (Exception ex)*/
            {
                Player.SendMessage(p, "Invalid time given.");
                Help(p);
                return;
            }

            Player.GlobalMessage(muter.color + muter.name + " has been muted for " + time + " seconds");
            Player.SendMessage(muter, "You have been muted for " + time + " seconds");

            Thread.Sleep(time * 1000);

            Command.all.Find("mute").Use(p, muter.name);
        }

        // This one controls what happens when you use /help [commandname].
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/xmute <player> <seconds> - Mutes <player> for <seconds> seconds");
        }
    }
}


