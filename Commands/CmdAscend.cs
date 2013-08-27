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
    public class CmdAscend : Command
    {
        public override string name { get { return "ascend"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Builder; } }
        public CmdAscend() { }

        public override void Use(Player p, string message)
        {
            ushort max = p.level.height;
            ushort posy = (ushort)(p.pos[1] / 32);
            bool found = false;
            ushort xpos = (ushort)(p.pos[0] / 32);
            ushort zpos = (ushort)(p.pos[2] / 32);
            while (!found && posy < max)
            {
                posy++;
                byte block = p.level.GetTile(xpos, posy, zpos);
                if (block == Block.air || block == Block.air_door || block == Block.air_switch || block == Block.Zero)
                {
                    ushort blockabove = (ushort) (posy + 1);
                    ushort blockunder = (ushort) (posy - 1);
                    if (p.level.GetTile(xpos, blockabove, zpos) == Block.air || p.level.GetTile(xpos, blockabove, zpos) == Block.air_door || p.level.GetTile(xpos, blockabove, zpos) == Block.air_switch || p.level.GetTile(xpos, blockabove, zpos) == Block.Zero)
                    {
                        if (p.level.GetTile(xpos, blockunder, zpos) != Block.air && p.level.GetTile(xpos, blockunder, zpos) != Block.air_switch && p.level.GetTile(xpos, blockunder, zpos) != Block.air_door && p.level.GetTile(xpos, blockunder, zpos) != Block.air_flood && p.level.GetTile(xpos, blockunder, zpos) != Block.air_flood_down && p.level.GetTile(xpos, blockunder, zpos) != Block.air_flood_layer && p.level.GetTile(xpos, blockunder, zpos) != Block.air_flood_up && p.level.GetTile(xpos, blockunder, zpos) != Block.air_portal && p.level.GetTile(xpos, blockunder, zpos) != Block.redflower && p.level.GetTile(xpos, blockunder, zpos) != Block.yellowflower && p.level.GetTile(xpos, blockunder, zpos) != Block.finiteWater && p.level.GetTile(xpos, blockunder, zpos) != Block.finiteLava && p.level.GetTile(xpos, blockunder, zpos) != Block.fire && p.level.GetTile(xpos, blockunder, zpos) != Block.water && p.level.GetTile(xpos, blockunder, zpos) != Block.water_door && p.level.GetTile(xpos, blockunder, zpos) != Block.water_portal && p.level.GetTile(xpos, blockunder, zpos) != Block.WaterDown && p.level.GetTile(xpos, blockunder, zpos) != Block.WaterFaucet && p.level.GetTile(xpos, blockunder, zpos) != Block.lava && p.level.GetTile(xpos, blockunder, zpos) != Block.lava_door && p.level.GetTile(xpos, blockunder, zpos) != Block.lava_fast && p.level.GetTile(xpos, blockunder, zpos) != Block.lava_portal && p.level.GetTile(xpos, blockunder, zpos) != Block.LavaDown && p.level.GetTile(xpos, blockunder, zpos) != Block.lavastill && p.level.GetTile(xpos, blockunder, zpos) != Block.Zero)
                        {
                            Player.SendMessage(p, "Teleported you up!");
                            unchecked { p.SendPos((byte)-1, p.pos[0], (ushort)((posy + 1) * 32), p.pos[2], p.rot[0], p.rot[1]); }
                            found = true;
                        }
                    }
                }
            }
            if (!found)
            {
                Player.SendMessage(p, "No free spaces found above you");
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/ascend - Teleports you to the first free space above you.");
        }
    }
}