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
using System.IO;
using System.IO.Compression;
namespace MCForge.Commands
{
    public sealed class CmdRestoreSelection : Command
    {
        public override string name { get { return "rs"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }

        public override void Use(Player p, string message)
        {
            if (message != "")
                if (File.Exists(@Server.backupLocation + "/" + p.level.name + "/" + message + "/" + p.level.name + ".lvl"))
                {
                    try
                    {
                        p.blockchangeObject = new CatchPos() { backup = int.Parse(message) };
                        p.ClearBlockchange();
                        p.Blockchange += Blockchange1;
                        p.SendMessage("Select two corners for restore.");
                    }
                    catch { Server.s.Log("Restore fail"); }
                }
                else Player.SendMessage(p, "Backup " + message + " does not exist."); 
            else Help(p);
        }

        public void Blockchange1(Player p, ushort x, ushort y, ushort z, byte type)
        {
            p.ClearBlockchange();
            p.SendBlockchange(x, y, z, p.level.GetTile(x, y, z));
            CatchPos bp = (CatchPos)p.blockchangeObject;
            bp.x = x; bp.y = y; bp.z = z; p.blockchangeObject = bp;
            p.Blockchange += Blockchange2;
        }

        public void Blockchange2(Player p, ushort x, ushort y, ushort z, byte type)
        {
            p.ClearBlockchange();
            p.SendBlockchange(x, y, z, p.level.GetTile(x, y, z));
            CatchPos cpos = (CatchPos)p.blockchangeObject;
            FileStream fs = File.OpenRead(@Server.backupLocation + "/" + p.level.name + "/" + cpos.backup + "/" + p.level.name + ".lvl");
            GZipStream gs = new GZipStream(fs, CompressionMode.Decompress);
            byte[] ver = new byte[2];
            gs.Read(ver, 0, ver.Length);
            ushort version = BitConverter.ToUInt16(ver, 0);
            ushort[] vars = new ushort[6];
            try
            {
                if (version == 1874)
                {
                    byte[] header = new byte[16]; gs.Read(header, 0, header.Length);

                    vars[0] = BitConverter.ToUInt16(header, 0);
                    vars[1] = BitConverter.ToUInt16(header, 2);
                    vars[2] = BitConverter.ToUInt16(header, 4);
                }
                else
                {
                    byte[] header = new byte[12]; gs.Read(header, 0, header.Length);

                    vars[0] = version;
                    vars[1] = BitConverter.ToUInt16(header, 0);
                    vars[2] = BitConverter.ToUInt16(header, 2);
                }
                byte[] blocks = new byte[vars[0] * vars[2] * vars[1]];
                gs.Read(blocks, 0, blocks.Length);
                gs.Dispose();
                fs.Dispose();

                if (blocks.Length != p.level.blocks.Length) { p.SendMessage("Cant restore selection of different size maps."); blocks = null; return; }

                if (p.level.bufferblocks && !p.level.Instant)
                {
                    for (ushort xx = Math.Min(cpos.x, x); xx <= Math.Max(cpos.x, x); ++xx)
                        for (ushort yy = Math.Min(cpos.y, y); yy <= Math.Max(cpos.y, y); ++yy)
                            for (ushort zz = Math.Min(cpos.z, z); zz <= Math.Max(cpos.z, z); ++zz)
                                BlockQueue.Addblock(p, xx, yy, zz, blocks[xx + (zz * vars[0]) + (yy * vars[0] * vars[1])]);
                }
                else
                {
                    for (ushort xx = Math.Min(cpos.x, x); xx <= Math.Max(cpos.x, x); ++xx)
                        for (ushort yy = Math.Min(cpos.y, y); yy <= Math.Max(cpos.y, y); ++yy)
                            for (ushort zz = Math.Min(cpos.z, z); zz <= Math.Max(cpos.z, z); ++zz)
                                p.level.Blockchange(p, xx, yy, zz, blocks[xx + (zz * vars[0]) + (yy * vars[0] * vars[1])]);
                }

                blocks = null;
                if (p.staticCommands) p.Blockchange += Blockchange1;
            }
            catch { Server.s.Log("Restore selection failed"); }
        }

        struct CatchPos
        {
            public int backup;
            public ushort x, y, z;
        }

        public override void Help(Player p)
        {
            Player.SendMessage(p, "/restoreselection <number> - restores a previous backup of the current selection");
        }
    }
}
