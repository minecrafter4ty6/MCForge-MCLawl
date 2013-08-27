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
    public sealed class CmdFixGrass : Command
    {
        public override string name { get { return "fixgrass"; } }
        public override string shortcut { get { return "fg"; } }
        public override string type { get { return "moderation"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Admin; } }
        public CmdFixGrass() { }

        public override void Use(Player p, string message)
        {
            int totalFixed = 0;

            switch (message.ToLower())
            {
                case "":
                    for (int i = 0; i < p.level.blocks.Length; i++)
                    {
                        try
                        {
                            ushort x, y, z;
                            p.level.IntToPos(i, out x, out y, out z);

                            if (p.level.blocks[i] == Block.dirt)
                            {
                                if (Block.LightPass(p.level.blocks[p.level.IntOffset(i, 0, 1, 0)]))
                                {
                                    p.level.Blockchange(p, x, y, z, Block.grass);
                                    totalFixed++;
                                }
                            }
                            else if (p.level.blocks[i] == Block.grass)
                            {
                                if (!Block.LightPass(p.level.blocks[p.level.IntOffset(i, 0, 1, 0)]))
                                {
                                    p.level.Blockchange(p, x, y, z, Block.dirt);
                                    totalFixed++;
                                }
                            }
                        }
                        catch { }
                    } break;
                case "light":
                    for (int i = 0; i < p.level.blocks.Length; i++)
                    {
                        try
                        {
                            ushort x, y, z; bool skipMe = false;
                            p.level.IntToPos(i, out x, out y, out z);

                            if (p.level.blocks[i] == Block.dirt)
                            {
                                for (int iL = 1; iL < (p.level.depth - y); iL++)
                                {
                                    if (!Block.LightPass(p.level.blocks[p.level.IntOffset(i, 0, iL, 0)]))
                                    {
                                        skipMe = true; break;
                                    }
                                }
                                if (!skipMe)
                                {
                                    p.level.Blockchange(p, x, y, z, Block.grass);
                                    totalFixed++;
                                }
                            }
                            else if (p.level.blocks[i] == Block.grass)
                            {
                                for (int iL = 1; iL < (p.level.depth - y); iL++)
                                {
                                    // Used to change grass to dirt only if all the upper blocks weren't Lightpass.
                                    if (!Block.LightPass(p.level.blocks[p.level.IntOffset(i, 0, iL, 0)]))
                                    {
                                        skipMe = true; break;
                                    }
                                }
                                if (skipMe)
                                {
                                    p.level.Blockchange(p, x, y, z, Block.dirt);
                                    totalFixed++;
                                }
                            }
                        }
                        catch { }
                    } break;
                case "grass":
                    for (int i = 0; i < p.level.blocks.Length; i++)
                    {
                        try
                        {
                            ushort x, y, z;
                            p.level.IntToPos(i, out x, out y, out z);

                            if (p.level.blocks[i] == Block.grass)
                                if (!Block.LightPass(p.level.blocks[p.level.IntOffset(i, 0, 1, 0)]))
                                {
                                    p.level.Blockchange(p, x, y, z, Block.dirt);
                                    totalFixed++;
                                }
                        }
                        catch { }
                    } break;
                case "dirt":
                    for (int i = 0; i < p.level.blocks.Length; i++)
                    {
                        try
                        {
                            ushort x, y, z;
                            p.level.IntToPos(i, out x, out y, out z);

                            if (p.level.blocks[i] == Block.dirt)
                                if (Block.LightPass(p.level.blocks[p.level.IntOffset(i, 0, 1, 0)]))
                                {
                                    p.level.Blockchange(p, x, y, z, Block.grass);
                                    totalFixed++;
                                }
                        }
                        catch { }
                    } break;
                default:
                    Help(p);
                    return;
            }

            Player.SendMessage(p, "Fixed " + totalFixed + " blocks.");
        }

        public override void Help(Player p)
        {
            Player.SendMessage(p, "/fixgrass <type> - Fixes grass based on type");
            Player.SendMessage(p, "<type> as \"\": Any grass with something on top is made into dirt, dirt with nothing on top is made grass");
            Player.SendMessage(p, "<type> as \"light\": Only dirt/grass in sunlight becomes grass");
            Player.SendMessage(p, "<type> as \"grass\": Only turns grass to dirt when under stuff");
            Player.SendMessage(p, "<type> as \"dirt\": Only turns dirt with nothing on top to grass");
        }
    }
}