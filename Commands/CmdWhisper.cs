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
    public sealed class CmdWhisper : Command
    {
        public override string name { get { return "whisper"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }
        public CmdWhisper() { }

        public override void Use(Player p, string message)
        {
            if (message == "")
            {
                p.whisper = !p.whisper; p.whisperTo = "";
                if (p.whisper) Player.SendMessage(p, "All messages sent will now auto-whisper");
                else Player.SendMessage(p, "Whisper chat turned off");
            }
            else
            {
                Player who = Player.Find(message);
                if (who == null) { p.whisperTo = ""; p.whisper = false; Player.SendMessage(p, "Could not find player."); return; }
                if (who.hidden)
                {
                    if (p.hidden == false || who.group.Permission > p.group.Permission)
                    {
                        Player.SendMessage(p, "Could not find player.");
                        return;
                    }
                }

                p.whisper = true;
                p.whisperTo = who.name;
                Player.SendMessage(p, "Auto-whisper enabled.  All messages will now be sent to " + who.name + ".");
            }

        }

        public override void Help(Player p)
        {
            Player.SendMessage(p, "/whisper <name> - Makes all messages act like whispers");
        }
    }
}