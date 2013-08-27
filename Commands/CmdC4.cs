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
    public sealed class CmdC4 : Command
    {
        public override string name { get { return "c4"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.AdvBuilder; } }
        public CmdC4() { }

        public override void Use(Player p, string message)
        {
            if (p != null)
            {
                if (p.level.physics >= 1 && p.level.physics < 5)
                {
                    sbyte numb = Level.C4.NextCircuit(p.level);
                    Level.C4.C4s c4 = new Level.C4.C4s(numb);
                    p.level.C4list.Add(c4);
                    p.c4circuitNumber = numb;
                    Player.SendMessage(p, "Place any block for c4 and place a " + c.red + "red" + Server.DefaultColor + " block for the detonator!");
                    p.Blockchange += new Player.BlockchangeEventHandler(Blockchange1);
                    return;
                }
                else
                {
                    Player.SendMessage(p, "To use c4, the physics level must be 1, 2, 3 or 4");
                    return;
                }
            }
            Player.SendMessage(p, "This command can only be used in-game!");
            return;
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/c4 - Place c4!");
        }
        public void Blockchange1(Player p, ushort x, ushort y, ushort z, byte type)
        {
            p.ClearBlockchange();

            if (type == Block.red) { Blockchange2(p, x, y, z, type); return; }
            if (type != Block.air)
            {
                p.level.Blockchange(p, x, y, z, Block.c4);
            }
            p.Blockchange += new Player.BlockchangeEventHandler(Blockchange1);
        }

        public void Blockchange2(Player p, ushort x, ushort y, ushort z, byte type)
        {
            p.ClearBlockchange();
            byte b = p.level.GetTile(x, y, z);
            p.level.Blockchange(p, x, y, z, Block.c4det);
            Player.SendMessage(p, "Placed detonator block!");
        }
    }
}