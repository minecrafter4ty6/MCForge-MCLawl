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
    public sealed class CmdAbort : Command
    {
        public override string name { get { return "abort"; } }
        public override string shortcut { get { return "a"; } }
        public override string type { get { return "build"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Builder; } }
        public CmdAbort() { }

        public override void Use(Player p, string message)
        {
            if (p != null)
            {
                p.ClearBlockchange();
                p.painting = false;
                p.BlockAction = 0;
                p.megaBoid = false;
                p.cmdTimer = false;
                p.staticCommands = false;
                p.deleteMode = false;
                p.ZoneCheck = false;
                p.modeType = 0;
                p.aiming = false;
                p.onTrain = false;
                p.isFlying = false;
                try
                {
                    p.level.blockqueue.RemoveAll((BlockQueue.block b) => { if (b.p == p) return true; return false; });
                }
                finally { BlockQueue.resume(); }
                Player.SendMessage(p, "Every toggle or action was aborted.");
                return;
            }
            Player.SendMessage(p, "This command can only be used in-game!");
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/abort - Cancels an action.");
        }
    }
}