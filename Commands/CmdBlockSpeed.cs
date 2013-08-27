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
    public sealed class CmdBlockSpeed : Command
    {
        public override string name { get { return "blockspeed"; } }
        public override string shortcut { get { return "bs"; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdBlockSpeed() { }

        public override void Use(Player p, string text)
        {
            if (text == "")
            {
                SendEstimation(p);
                return;
            }
            if (text == "clear")
            {
                Server.levels.ForEach((l) => { l.blockqueue.Clear(); });
                return;
            }
            if (text.StartsWith("bs"))
            {
                try { BlockQueue.blockupdates = int.Parse(text.Split(' ')[1]); }
                catch { Player.SendMessage(p, "Invalid number specified."); return; }
                Player.SendMessage(p, String.Format("Blocks per interval is now {0}.", BlockQueue.blockupdates));
                return;
            }
            if (text.StartsWith("ts"))
            {
                try { BlockQueue.time = int.Parse(text.Split(' ')[1]); }
                catch { Player.SendMessage(p, "Invalid number specified."); return; }
                Player.SendMessage(p, String.Format("Block interval is now {0}.", BlockQueue.time));
                return;
            }
            if (text.StartsWith("buf"))
            {
                if (p.level.bufferblocks)
                {
                    p.level.bufferblocks = false;
                    Player.SendMessage(p, String.Format("Block buffering on {0} disabled.", p.level.name));
                }
                else
                {
                    p.level.bufferblocks = true;
                    Player.SendMessage(p, String.Format("Block buffering on {0} enabled.", p.level.name));
                }
                return;
            }
            if (text.StartsWith("net"))
            {
                switch (int.Parse(text.Split(' ')[1]))
                {
                    case 2:
                        BlockQueue.blockupdates = 25;
                        BlockQueue.time = 100;
                        break;
                    case 4:
                        BlockQueue.blockupdates = 50;
                        BlockQueue.time = 100;
                        break;
                    case 8:
                        BlockQueue.blockupdates = 100;
                        BlockQueue.time = 100;
                        break;
                    case 12:
                        BlockQueue.blockupdates = 200;
                        BlockQueue.time = 100;
                        break;
                    case 16:
                        BlockQueue.blockupdates = 200;
                        BlockQueue.time = 100;
                        break;
                    case 161:
                        BlockQueue.blockupdates = 100;
                        BlockQueue.time = 50;
                        break;
                    case 20:
                        BlockQueue.blockupdates = 125;
                        BlockQueue.time = 50;
                        break;
                    case 24:
                        BlockQueue.blockupdates = 150;
                        BlockQueue.time = 50;
                        break;
                    default:
                        BlockQueue.blockupdates = 200;
                        BlockQueue.time = 100;
                        break;
                }
                SendEstimation(p);
                return;
            }
        }
        private static void SendEstimation(Player p)
        {
            Player.SendMessage(p, String.Format("{0} blocks every {1} milliseconds = {2} blocks per second.", BlockQueue.blockupdates, BlockQueue.time, BlockQueue.blockupdates * (1000 / BlockQueue.time)));
            Player.SendMessage(p, String.Format("Using ~{0}KB/s times {1} player(s) = ~{2}KB/s", (BlockQueue.blockupdates * (1000 / BlockQueue.time) * 8) / 1000, Player.players.Count, Player.players.Count * ((BlockQueue.blockupdates * (1000 / BlockQueue.time) * 8) / 1000)));
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/bs [option] [option value] - Options for block speeds.");
            Player.SendMessage(p, "Options are: bs (blocks per interval), ts (interval in milliseconds), buf (toggles buffering), clear, net.");
            Player.SendMessage(p, "/bs net [2,4,8,12,16,20,24] - Presets, divide by 8 and times by 1000 to get blocks per second.");
        }
    }
}