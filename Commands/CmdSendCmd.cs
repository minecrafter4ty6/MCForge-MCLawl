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
    public sealed class CmdSendCmd : Command
    {
        public override string name { get { return "sendcmd"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Nobody; } }
        public override void Use(Player p, string message)
        {
            int length = message.Split().Length;
            Player player = null;
            if (length >= 1)
                player = Player.Find(message.Split(' ')[0]);
            else return;
            if (player == null)
            {
                Player.SendMessage(p, "Error: Player is not online.");
            }
            else
            {
                if (p == null) { }
                else { if (player.group.Permission >= p.group.Permission) { Player.SendMessage(p, "Cannot use this on someone of equal or greater rank."); return; } }
                string command;
                string cmdMsg = "";
                try
                {
                    command = message.Split(' ')[1];
                    for(int i = 2; i < length; i++)
                        cmdMsg += message.Split(' ')[i] + " ";
                    cmdMsg.Remove(cmdMsg.Length - 1); //removing the space " " at the end of the msg
                    Command.all.Find(command).Use(player, cmdMsg);
                }
                catch
                {
                    Player.SendMessage(p, "Error: No parameter found");
                    command = message.Split(' ')[1];
                    Command.all.Find(command).Use(player, "");
                }
            }
        }

        public override void Help(Player p)
        {
            Player.SendMessage(p, "/sendcmd - Make another user use a command, (/sendcmd player command parameter)");
            Player.SendMessage(p, "ex: /sendcmd bob tp bob2");
        }
    }
}



