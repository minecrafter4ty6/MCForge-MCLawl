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
    public sealed class CmdBlockSet : Command
    {
        public override string name { get { return "blockset"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdBlockSet() { }

        public override void Use(Player p, string message)
        {
            if (message == "" || message.IndexOf(' ') == -1) { Help(p); return; }

            byte foundBlock = Block.Byte(message.Split(' ')[0]);
            if (foundBlock == Block.Zero) { Player.SendMessage(p, "Could not find block entered"); return; }
            LevelPermission newPerm = Level.PermissionFromName(message.Split(' ')[1]);
            if (newPerm == LevelPermission.Null) { Player.SendMessage(p, "Could not find rank specified"); return; }
            if (p != null && newPerm > p.group.Permission) { Player.SendMessage(p, "Cannot set to a rank higher than yourself."); return; }

            if (p != null && !Block.canPlace(p, foundBlock)) { Player.SendMessage(p, "Cannot modify a block set for a higher rank"); return; }

            Block.Blocks newBlock = Block.BlockList.Find(bs => bs.type == foundBlock);
            newBlock.lowestRank = newPerm;

            Block.BlockList[Block.BlockList.FindIndex(bL => bL.type == foundBlock)] = newBlock;

            Block.SaveBlocks(Block.BlockList);

            Player.GlobalMessage("&d" + Block.Name(foundBlock) + Server.DefaultColor + "'s permission was changed to " + Level.PermissionToName(newPerm));
            if (p == null)
            {
                Player.SendMessage(p, Block.Name(foundBlock) + "'s permission was changed to " + Level.PermissionToName(newPerm));
                return;
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/blockset [block] [rank] - Changes [block] rank to [rank]");
            Player.SendMessage(p, "Only blocks you can use can be modified");
            Player.SendMessage(p, "Available ranks: " + Group.concatList());
        }
    }
}