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
    public sealed class CmdSpheroid : Command
    {
        public override string name { get { return "spheroid"; } }
        public override string shortcut { get { return "e"; } }
        public override string type { get { return "build"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Builder; } }
        public CmdSpheroid() { }
        public static byte wait = 0;

        public override void Use(Player p, string message)
        {
            wait = 0;
            CatchPos cpos;

            cpos.x = 0; cpos.y = 0; cpos.z = 0;

            if (message == "")
            {
                cpos.type = Block.Zero;
                cpos.vertical = false;
            }
            else if (message.IndexOf(' ') == -1)
            {
                cpos.type = Block.Byte(message);
                cpos.vertical = false;
                if (message.ToLower() != "vertical" && !Block.canPlace(p, cpos.type)) { Player.SendMessage(p, "Cannot place that."); wait = 1; return; }
                if (cpos.type == Block.Zero)
                {
                    if (message.ToLower() == "vertical")
                    {
                        cpos.vertical = true;
                    }
                    else
                    {
                        Help(p); wait = 1; return;
                    }
                }
            }
            else
            {
                cpos.type = Block.Byte(message.Split(' ')[0]);
                if (!Block.canPlace(p, cpos.type)) { Player.SendMessage(p, "Cannot place that."); wait = 1; return; }
                if (cpos.type == Block.Zero || message.Split(' ')[1].ToLower() != "vertical")
                {
                    Help(p); wait = 1; return;
                }
                cpos.vertical = true;
            }

            if (!Block.canPlace(p, cpos.type) && cpos.type != Block.Zero) { Player.SendMessage(p, "Cannot place this block type!"); wait = 1; return; }

            p.blockchangeObject = cpos;

            Player.SendMessage(p, "Place two blocks to determine the edges.");
            p.ClearBlockchange();
            p.Blockchange += new Player.BlockchangeEventHandler(Blockchange1);
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/spheroid [type] <vertical> - Create a spheroid of blocks.");
            Player.SendMessage(p, "If <vertical> is added, it will be a vertical tube");
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
            if (cpos.type != Block.Zero) { type = cpos.type; }
            List<Pos> buffer = new List<Pos>();

            if (!cpos.vertical)
            {
                /* Courtesy of fCraft's awesome Open-Source'ness :D */

                // find start/end coordinates
                int sx = Math.Min(cpos.x, x);
                int ex = Math.Max(cpos.x, x);
                int sy = Math.Min(cpos.y, y);
                int ey = Math.Max(cpos.y, y);
                int sz = Math.Min(cpos.z, z);
                int ez = Math.Max(cpos.z, z);

                // find center points
                double cx = (ex + sx) / 2 + (((ex + sx) % 2 == 1) ? 0.5 : 0);
                double cy = (ey + sy) / 2 + (((ey + sy) % 2 == 1) ? 0.5 : 0);
                double cz = (ez + sz) / 2 + (((ez + sz) % 2 == 1) ? 0.5 : 0);

                // find axis lengths
                double rx = Convert.ToDouble(ex) - cx + 0.25;
                double ry = Convert.ToDouble(ey) - cy + 0.25;
                double rz = Convert.ToDouble(ez) - cz + 0.25;

                double rx2 = 1 / (rx * rx);
                double ry2 = 1 / (ry * ry);
                double rz2 = 1 / (rz * rz);

                int totalBlocks = (int)(Math.PI * 0.75 * rx * ry * rz);

                if (totalBlocks > p.group.maxBlocks)
                {
                    Player.SendMessage(p, "You tried to spheroid " + totalBlocks + " blocks.");
                    Player.SendMessage(p, "You cannot spheroid more than " + p.group.maxBlocks + ".");
                    wait = 1;
                    return;
                }

                Player.SendMessage(p, totalBlocks + " blocks.");

                for (int xx = sx; xx <= ex; xx += 8)
                    for (int yy = sy; yy <= ey; yy += 8)
                        for (int zz = sz; zz <= ez; zz += 8)
                            for (int z3 = 0; z3 < 8 && zz + z3 <= ez; z3++)
                                for (int y3 = 0; y3 < 8 && yy + y3 <= ey; y3++)
                                    for (int x3 = 0; x3 < 8 && xx + x3 <= ex; x3++)
                                    {
                                        // get relative coordinates
                                        double dx = (xx + x3 - cx);
                                        double dy = (yy + y3 - cy);
                                        double dz = (zz + z3 - cz);

                                        // test if it's inside ellipse
                                        if ((dx * dx) * rx2 + (dy * dy) * ry2 + (dz * dz) * rz2 <= 1)
                                        {
                                            p.level.Blockchange(p, (ushort)(x3 + xx), (ushort)(yy + y3), (ushort)(zz + z3), type);
                                        }
                                    }
            }
            else
            {
                // find start/end coordinates
                int sx = Math.Min(cpos.x, x);
                int ex = Math.Max(cpos.x, x);
                int sy = Math.Min(cpos.y, y);
                int ey = Math.Max(cpos.y, y);
                int sz = Math.Min(cpos.z, z);
                int ez = Math.Max(cpos.z, z);

                // find center points
                double cx = (ex + sx) / 2 + (((ex + sx) % 2 == 1) ? 0.5 : 0);
                double cz = (ez + sz) / 2 + (((ez + sz) % 2 == 1) ? 0.5 : 0);

                // find axis lengths
                double rx = Convert.ToDouble(ex) - cx + 0.25;
                double rz = Convert.ToDouble(ez) - cz + 0.25;

                double rx2 = 1 / (rx * rx);
                double rz2 = 1 / (rz * rz);
                double smallrx2 = 1 / ((rx - 1) * (rx - 1));
                double smallrz2 = 1 / ((rz - 1) * (rz - 1));

                Pos pos = new Pos();

                for (int xx = sx; xx <= ex; xx += 8)
                    for (int zz = sz; zz <= ez; zz += 8)
                        for (int z3 = 0; z3 < 8 && zz + z3 <= ez; z3++)
                            for (int x3 = 0; x3 < 8 && xx + x3 <= ex; x3++)
                            {
                                // get relative coordinates
                                double dx = (xx + x3 - cx);
                                double dz = (zz + z3 - cz);

                                // test if it's inside ellipse
                                if ((dx * dx) * rx2 + (dz * dz) * rz2 <= 1 && (dx * dx) * smallrx2 + (dz * dz) * smallrz2 > 1)
                                {
                                    pos.x = (ushort)(x3 + xx);
                                    pos.y = (ushort)(sy);
                                    pos.z = (ushort)(zz + z3);
                                    buffer.Add(pos);
                                }
                            }

                int ydiff = Math.Abs(y - cpos.y) + 1;

                if (buffer.Count * ydiff > p.group.maxBlocks)
                {
                    Player.SendMessage(p, "You tried to spheroid " + buffer.Count * ydiff + " blocks.");
                    Player.SendMessage(p, "You cannot spheroid more than " + p.group.maxBlocks + ".");
                    wait = 1;
                    return;
                }
                Player.SendMessage(p, buffer.Count * ydiff + " blocks.");
               

                foreach (Pos Pos in buffer)
                {
                    for (ushort yy = (ushort)sy; yy <= (ushort)ey; yy++)
                    {
                        p.level.Blockchange(p, Pos.x, yy, Pos.z, type);
                    }
                }
            }
            wait = 2;
            if (p.staticCommands) p.Blockchange += new Player.BlockchangeEventHandler(Blockchange1);
        }
        void BufferAdd(List<Pos> list, ushort x, ushort y, ushort z)
        {
            Pos pos; pos.x = x; pos.y = y; pos.z = z; list.Add(pos);
        }
        struct Pos { public ushort x, y, z; }
        struct CatchPos
        {
            public byte type;
            public ushort x, y, z;
            public bool vertical;
        }
    }
}