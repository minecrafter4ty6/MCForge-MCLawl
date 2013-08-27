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
namespace MCForge.Commands
{
    public sealed class CmdCmdBind : Command
    {
        public override string name { get { return "cmdbind"; } }
        public override string shortcut { get { return "cb"; } }
        public override string type { get { return "build"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Builder; } }
        public CmdCmdBind() { }

        public override void Use(Player p, string message)
        {
            if (p == null)
            {
                Player.SendMessage(p, "This command can only be used in-game");
                return;
            }
            string foundcmd, foundmessage = ""; int foundnum = 0;

            if (message.IndexOf(' ') == -1)
            {
                bool OneFound = false;
                for (int i = 0; i < 10; i++)
                {
                    if (p.cmdBind[i] != null)
                    {
                        Player.SendMessage(p, "&c/" + i + Server.DefaultColor + " bound to &b" + p.cmdBind[i] + " " + p.messageBind[i]);
                        OneFound = true;
                    }
                }
                if (!OneFound) Player.SendMessage(p, "You have no commands binded");
                return;
            }

            if (message.Split(' ').Length == 1)
            {
                try
                {
                    foundnum = Convert.ToInt16(message);
                    if (p.cmdBind[foundnum] == null) { Player.SendMessage(p, "No command stored here yet."); return; }
                    foundcmd = "/" + p.cmdBind[foundnum] + " " + p.messageBind[foundnum];
                    Player.SendMessage(p, "Stored command: &b" + foundcmd);
                }
                catch { Help(p); }
            }
            else if (message.Split(' ').Length > 1)
            {
                try
                {
                    foundnum = Convert.ToInt16(message.Split(' ')[message.Split(' ').Length - 1]);
                    foundcmd = message.Split(' ')[0];
                    if (message.Split(' ').Length > 2)
                    {
                        foundmessage = message.Substring(message.IndexOf(' ') + 1);
                        foundmessage = foundmessage.Remove(foundmessage.LastIndexOf(' '));
                    }

                    p.cmdBind[foundnum] = foundcmd;
                    p.messageBind[foundnum] = foundmessage;

                    Player.SendMessage(p, "Binded &b/" + foundcmd + " " + foundmessage + " to &c/" + foundnum);
                }
                catch { Help(p); }
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/cmdbind [command] [num] - Binds [command] to [num]");
            Player.SendMessage(p, "[num] must be between 0 and 9");
            Player.SendMessage(p, "Use with \"/[num]\" &b(example: /2)");
            Player.SendMessage(p, "Use /cmdbind [num] to see stored commands.");
        }
    }
}