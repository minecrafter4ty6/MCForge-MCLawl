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
    public sealed class CmdTree : Command
    {
        public override string name { get { return "tree"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "build"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Builder; } }
        public CmdTree() { }

        public override void Use(Player p, string message)
        {
            p.ClearBlockchange();
            switch (message.ToLower())
            {
                case "2":
                case "cactus": p.Blockchange += new Player.BlockchangeEventHandler(AddCactus); break;
                case "3":
                case "notch": p.Blockchange += new Player.BlockchangeEventHandler(AddNotchTree); break;
                case "4":
                case "swamp": p.Blockchange += new Player.BlockchangeEventHandler(AddNotchSwampTree); break;
                /*case "5":
                case "big": p.Blockchange += new Player.BlockchangeEventHandler(AddNotchBigTree); break;
                case "6":
                case "pine": p.Blockchange += new Player.BlockchangeEventHandler(AddNotchPineTree); break;*/
                default: p.Blockchange += new Player.BlockchangeEventHandler(AddTree); break;
            }
            Player.SendMessage(p, "Select where you wish your tree to grow");
            p.painting = false;
        }

        void AddTree(Player p, ushort x, ushort y, ushort z, byte type)
        {
            Server.MapGen.AddTree(p.level, x, y, z, p.random, true, true, p);
            if (!p.staticCommands) p.ClearBlockchange();
        }
        void AddNotchTree(Player p, ushort x, ushort y, ushort z, byte type)
        {
            Server.MapGen.AddNotchTree(p.level, x, y, z, p.random, true, true, p);
            if (!p.staticCommands) p.ClearBlockchange();
        }
        void AddNotchBigTree(Player p, ushort x, ushort y, ushort z, byte type)
        {
            Server.MapGen.AddNotchBigTree(p.level, x, y, z, p.random, true, true, p);
            if (!p.staticCommands) p.ClearBlockchange();
        }
        void AddNotchPineTree(Player p, ushort x, ushort y, ushort z, byte type)
        {
            Server.MapGen.AddNotchPineTree(p.level, x, y, z, p.random, true, true, p);
            if (!p.staticCommands) p.ClearBlockchange();
        }
        void AddNotchSwampTree(Player p, ushort x, ushort y, ushort z, byte type)
        {
            Server.MapGen.AddNotchSwampTree(p.level, x, y, z, p.random, true, true, p);
            if (!p.staticCommands) p.ClearBlockchange();
        }
        void AddCactus(Player p, ushort x, ushort y, ushort z, byte type)
        {
            Server.MapGen.AddCactus(p.level, x, y, z, p.random, true, true, p);
            if (!p.staticCommands) p.ClearBlockchange();
        }

        public override void Help(Player p)
        {
            Player.SendMessage(p, "/tree [type] - Turns tree mode on or off.");
            Player.SendMessage(p, "Types - (Fern | 1), (Cactus | 2), (Notch | 3), (Swamp | 4)");
        }
    }
}