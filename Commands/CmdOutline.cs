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
namespace MCForge.Commands
{
    public sealed class CmdOutline : Command
    {
        public override string name { get { return "outline"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "build"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.AdvBuilder; } }
        public CmdOutline() { }

        public override void Use(Player p, string message)
        {
            int number = message.Split(' ').Length;
            if (number != 2) { Help(p); return; }

            int pos = message.IndexOf(' ');
            string t = message.Substring(0, pos).ToLower();
            string t2 = message.Substring(pos + 1).ToLower();
            byte type = Block.Byte(t);
            if (type == 255) { Player.SendMessage(p, "There is no block \"" + t + "\"."); return; }
            byte type2 = Block.Byte(t2);
            if (type2 == 255) { Player.SendMessage(p, "There is no block \"" + t2 + "\"."); return; }
            if (!Block.canPlace(p, type2)) { Player.SendMessage(p, "Cannot place that block type."); return; }

            CatchPos cpos; cpos.type2 = type2; cpos.type = type;

            cpos.x = 0; cpos.y = 0; cpos.z = 0; p.blockchangeObject = cpos;
            Player.SendMessage(p, "Place two blocks to determine the edges.");
            p.ClearBlockchange();
            p.Blockchange += new Player.BlockchangeEventHandler(Blockchange1);
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/outline [type] [type2] - Outlines [type] with [type2]");
        }
        public void Blockchange1(Player p, ushort x, ushort y, ushort z, byte type)
        {
            p.ClearBlockchange();
            byte b = p.level.GetTile(x, y, z);
            p.SendBlockchange(x, y, z, b);
            CatchPos bp = (CatchPos)p.blockchangeObject;
            bp.x = x; bp.y = y; bp.z = z; p.blockchangeObject = bp;
            p.Blockchange += new Player.BlockchangeEventHandler(Blockchange2);
        }
        public void Blockchange2(Player p, ushort x, ushort y, ushort z, byte type)
        {
            p.ClearBlockchange();
            byte b = p.level.GetTile(x, y, z);
            p.SendBlockchange(x, y, z, b);
            CatchPos cpos = (CatchPos)p.blockchangeObject;
            unchecked { if (cpos.type != (byte)-1) { type = cpos.type; } }
            List<Pos> buffer = new List<Pos>();
            Pos pos;

            bool AddMe = false;

            for (ushort xx = Math.Min(cpos.x, x); xx <= Math.Max(cpos.x, x); ++xx)
                for (ushort yy = Math.Min(cpos.y, y); yy <= Math.Max(cpos.y, y); ++yy)
                    for (ushort zz = Math.Min(cpos.z, z); zz <= Math.Max(cpos.z, z); ++zz)
                    {
                        AddMe = false;

                        if (p.level.GetTile((ushort)(xx - 1), yy, zz) == cpos.type) AddMe = true;
                        else if (p.level.GetTile((ushort)(xx + 1), yy, zz) == cpos.type) AddMe = true;
                        else if (p.level.GetTile(xx, (ushort)(yy - 1), zz) == cpos.type) AddMe = true;
                        else if (p.level.GetTile(xx, (ushort)(yy + 1), zz) == cpos.type) AddMe = true;
                        else if (p.level.GetTile(xx, yy, (ushort)(zz - 1)) == cpos.type) AddMe = true;
                        else if (p.level.GetTile(xx, yy, (ushort)(zz + 1)) == cpos.type) AddMe = true;

                        if (AddMe && p.level.GetTile(xx, yy, zz) != cpos.type) { pos.x = xx; pos.y = yy; pos.z = zz; buffer.Add(pos); }
                    }

            if (buffer.Count > p.group.maxBlocks)
            {
                Player.SendMessage(p, "You tried to outline more than " + buffer.Count + " blocks.");
                Player.SendMessage(p, "You cannot outline more than " + p.group.maxBlocks + ".");
                return;
            }

            buffer.ForEach(delegate(Pos pos1)
            {
                p.level.Blockchange(p, pos1.x, pos1.y, pos1.z, cpos.type2);
            });

            Player.SendMessage(p, "You outlined " + buffer.Count + " blocks.");

            if (p.staticCommands) p.Blockchange += new Player.BlockchangeEventHandler(Blockchange1);
        }
        void BufferAdd(List<Pos> list, ushort x, ushort y, ushort z)
        {
            Pos pos; pos.x = x; pos.y = y; pos.z = z; list.Add(pos);
        }

        struct Pos
        {
            public ushort x, y, z;
        }
        struct CatchPos
        {
            public byte type;
            public byte type2;
            public ushort x, y, z;
        }

    }
}
