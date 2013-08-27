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
using System.Threading;
namespace MCForge.Commands
{
    public sealed class CmdFly : Command
    {
        public override string name { get { return "fly"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }
        public CmdFly() { }

        public override void Use(Player p, string message)
        {
            Command CmdFly = Command.all.Find("fly");
            if (p.level.ctfmode)
            {
                Player.SendMessage(p, "You can not fly while playing CTF, that is cheating!");
                if (p.isFlying) CmdFly.Use(p, String.Empty);
                return;
            }
            p.isFlying = !p.isFlying;
            if (!p.isFlying) return;

            Player.SendMessage(p, "You are now flying. &cJump!");

            Thread flyThread = new Thread(new ThreadStart(delegate
            {
                Pos pos;
                ushort[] oldpos = new ushort[3];
                List<Pos> buffer = new List<Pos>();
                while (p.isFlying)
                {
                    Thread.Sleep(20);
                    if (p.pos == oldpos) continue;
                    try
                    {
                        List<Pos> tempBuffer = new List<Pos>();
                        List<Pos> toRemove = new List<Pos>();
                        ushort x = (ushort)((p.pos[0]) / 32);
                        ushort y = (ushort)((p.pos[1] - 60) / 32);
                        ushort z = (ushort)((p.pos[2]) / 32);

                        try
                        {
                            for (ushort xx = (ushort)(x - 1); xx <= x + 1; xx++)
                            {
                                for (ushort yy = (ushort)(y - 1); yy <= y; yy++)
                                {
                                    for (ushort zz = (ushort)(z - 1); zz <= z + 1; zz++)
                                    {
                                        if (p.level.GetTile(xx, yy, zz) == Block.air)
                                        {
                                            pos.x = xx; pos.y = yy; pos.z = zz;
                                            tempBuffer.Add(pos);
                                        }
                                    }
                                }
                            }
                            foreach (Pos cP in tempBuffer)
                            {
                                if (!buffer.Contains(cP))
                                {
                                    buffer.Add(cP);
                                    p.SendBlockchange(cP.x, cP.y, cP.z, Block.glass);
                                }
                            }
                            foreach (Pos cP in buffer)
                            {
                                if (!tempBuffer.Contains(cP))
                                {
                                    p.SendBlockchange(cP.x, cP.y, cP.z, Block.air);
                                    toRemove.Add(cP);
                                }
                            }
                            foreach (Pos cP in toRemove)
                            {
                                buffer.Remove(cP);
                            }
                            tempBuffer.Clear();
                            toRemove.Clear();
                        }
                        catch { }
                    }
                    catch { }
                    p.pos.CopyTo(oldpos, 0);
                }

                foreach (Pos cP in buffer)
                {
                    p.SendBlockchange(cP.x, cP.y, cP.z, Block.air);
                }

                Player.SendMessage(p, "Stopped flying");
            }));
            flyThread.Start();
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/fly - Allows you to fly, it is a bit glitchy, you can't fly while playing CTF.");
        }

        struct Pos { public ushort x, y, z; }
    }
}