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
    public sealed class CmdTimer : Command
    {
        public override string name { get { return "timer"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdTimer() { }

        public override void Use(Player p, string message)
        {
            if (p.cmdTimer == true) { Player.SendMessage(p, "Can only have one timer at a time. Use /abort to cancel your previous timer."); return; }

            System.Timers.Timer messageTimer = new System.Timers.Timer(5000);
            if (message == "") { Help(p); return; }

            int TotalTime = 0;
            try
            {
                TotalTime = int.Parse(message.Split(' ')[0]);
                message = message.Substring(message.IndexOf(' ') + 1);
            }
            catch
            {
                TotalTime = 60;
            }

            if (TotalTime > 300) { Player.SendMessage(p, "Cannot have more than 5 minutes in a timer"); return; }

            Player.GlobalChatLevel(p, Server.DefaultColor + "Timer lasting for " + TotalTime + " seconds has started.", false);
            TotalTime = (int)(TotalTime / 5);

            Player.GlobalChatLevel(p, Server.DefaultColor + message, false);

            p.cmdTimer = true;
            messageTimer.Elapsed += delegate
            {
                TotalTime--;
                if (TotalTime < 1 || p.cmdTimer == false)
                {
                    Player.SendMessage(p, "Timer ended.");
                    messageTimer.Stop();
                }
                else
                {
                    Player.GlobalChatLevel(p, Server.DefaultColor + message, false);
                    Player.GlobalChatLevel(p, "Timer has " + (TotalTime * 5) + " seconds remaining.", false);
                }
            };

            messageTimer.Start();
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/timer [time] [message] - Starts a timer which repeats [message] every 5 seconds.");
            Player.SendMessage(p, "Repeats constantly until [time] has passed");
        }
    }
}