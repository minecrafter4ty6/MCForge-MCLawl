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
using System.Collections.Generic;
using System.Linq;

namespace MCForge.Commands
{
    public sealed class CmdReplaceAll : Command
    {
        public override string name { get { return "replaceall"; } }
        public override string shortcut { get { return "ra"; } }
        public override string type { get { return "build"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Admin; } }
        public CmdReplaceAll() { }
        public static byte wait;

        public override void Use(Player p, string message)
        {
            wait = 0;
            string[] args = message.Split(' ');
            if (args.Length != 2) { p.SendMessage("Invalid number of arguments!"); Help(p); wait = 1; return; }

            List<string> temp;

            if (args[0].Contains(","))
                temp = new List<string>(args[0].Split(','));
            else
                temp = new List<string>() { args[0] };

            temp = temp.Distinct().ToList(); // Remove duplicates

            List<string> invalid = new List<string>(); //Check for invalid blocks
            foreach (string name in temp)
                if (Block.Byte(name) == 255)
                    invalid.Add(name);
            if (Block.Byte(args[1]) == 255)
                invalid.Add(args[1]);
            if (invalid.Count > 0)
            {
                p.SendMessage(String.Format("Invalid block{0}: {1}", invalid.Count == 1 ? "" : "s", String.Join(", ", invalid.ToArray())));
                return;
            }
            if (temp.Contains(args[1]))
                temp.Remove(args[1]);
            if (temp.Count < 1)
            {
                p.SendMessage("Replacing a block with the same one would be pointless!");
                return;
            }

            List<byte> oldType = new List<byte>();
            foreach (string name in temp)
                oldType.Add(Block.Byte(name));
            byte newType = Block.Byte(args[1]);

            foreach (Byte type in oldType)
                if (!Block.canPlace(p, type) && !Block.BuildIn(type)) { p.SendMessage("Cannot replace that."); wait = 1; return; }
            if (!Block.canPlace(p, newType)) { Player.SendMessage(p, "Cannot place that."); wait = 1; return; }
            
            ushort x, y, z; int currentBlock = 0;
            List<Pos> stored = new List<Pos>(); Pos pos;

            foreach (byte b in p.level.blocks)
            {
                if (oldType.Contains(b))
                {
                    p.level.IntToPos(currentBlock, out x, out y, out z);
                    pos.x = x; pos.y = y; pos.z = z;
                    stored.Add(pos);
                }
                currentBlock++;
            }

            if (stored.Count > (p.group.maxBlocks * 2)) { Player.SendMessage(p, "Cannot replace more than " + (p.group.maxBlocks * 2) + " blocks."); wait = 1; return; }

            p.SendMessage(stored.Count + " blocks out of " + currentBlock + " will be replaced.");

            if (p.level.bufferblocks && !p.level.Instant)
            {
                foreach (Pos Pos in stored)
                {
                    BlockQueue.Addblock(p, Pos.x, Pos.y, Pos.z, newType);
                }
            }
            else
            {
                foreach (Pos Pos in stored)
                {
                    p.level.Blockchange(p, Pos.x, Pos.y, Pos.z, newType);
                }
            }

            Player.SendMessage(p, "&4/replaceall finished!");
            wait = 2;
        }
        public struct Pos { public ushort x, y, z; }

        public override void Help(Player p)
        {
            p.SendMessage("/replaceall [block,block2,...] [new] - Replaces all of [block] with [new] in a map");
            p.SendMessage("If more than one block is specified, they will all be replaced.");
        }
    }
}