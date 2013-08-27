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
    public sealed class CmdBanEdit : Command
    {
        public override string name { get { return "banedit"; } }
        public override string shortcut { get { return "be"; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdBanEdit() { }

        public override void Use(Player p, string message)
        {
            if (message.Split(' ').Length < 2) { Help(p); return; }
            if (message.Split(' ')[1].Length == 1)
            {
                Help(p); return;
            }
            string username = message.Split(' ')[0];
            string creason = message;
            string reason = creason.Remove(0, username.Length + 1).Replace(" ", "%20");
            string errormessage = Ban.Editreason(username, reason);
            if (errormessage != "")
            {
                Player.SendMessage(p, errormessage);
            }
            else
            {
                Player.SendMessage(p, "Succesfully edited baninfo about &0" + username + Server.DefaultColor + " to: &2" + reason.Replace("%20", " "));
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/banedit <username> <reason> - Edits reason of ban for the user.");
        }
    }
}