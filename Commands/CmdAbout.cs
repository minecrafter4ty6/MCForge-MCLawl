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
using System.Data;
using MCForge.SQL;

namespace MCForge.Commands
{
    public sealed class CmdAbout : Command
    {
        public override string name { get { return "about"; } }
        public override string shortcut { get { return "b"; } }
        public override string type { get { return "information"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }
        public CmdAbout() { }

        public override void Use(Player p, string message)
        {
            if (p != null)
            {
                Player.SendMessage(p, "Break/build a block to display information.");
                p.ClearBlockchange();
                p.Blockchange += new Player.BlockchangeEventHandler(AboutBlockchange);
                return;
            }
            Player.SendMessage(p, "This command can only be used in-game");
            
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/about - Displays information about a block.");
        }

        public void AboutBlockchange(Player p, ushort x, ushort y, ushort z, byte type)
        {
            if (!p.staticCommands) p.ClearBlockchange();
            byte b = p.level.GetTile(x, y, z);
            if (b == Block.Zero) { Player.SendMessage(p, "Invalid Block(" + x + "," + y + "," + z + ")!"); return; }
            p.SendBlockchange(x, y, z, b);

            string message = "Block (" + x + "," + y + "," + z + "): ";
            message += "&f" + b + " = " + Block.Name(b);
            Player.SendMessage(p, message + Server.DefaultColor + ".");
            message = p.level.foundInfo(x, y, z);
            if (message != "") Player.SendMessage(p, "Physics information: &a" + message);

            //safe against SQL injections because no user input is given here
            DataTable Blocks = Database.fillData("SELECT * FROM Block" + p.level.name + " WHERE X=" + (int)x + " AND Y=" + (int)y + " AND Z=" + (int)z);

            string Username, TimePerformed, BlockUsed;
            bool Deleted, foundOne = false;
            for (int i = 0; i < Blocks.Rows.Count; i++)
            {
                foundOne = true;
                Username = Blocks.Rows[i]["Username"].ToString();
                TimePerformed = DateTime.Parse(Blocks.Rows[i]["TimePerformed"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                //Server.s.Log(Blocks.Rows[i]["Type"].ToString());
                BlockUsed = Block.Name(Convert.ToByte(Blocks.Rows[i]["Type"])).ToString();
                Deleted = Convert.ToBoolean(Blocks.Rows[i]["Deleted"]);

                if (!Deleted)
                    Player.SendMessage(p, "&3Created by " + Server.FindColor(Username.Trim()) + Username.Trim() + Server.DefaultColor + ", using &3" + BlockUsed);
                else
                    Player.SendMessage(p, "&4Destroyed by " + Server.FindColor(Username.Trim()) + Username.Trim() + Server.DefaultColor + ", using &3" + BlockUsed);
                Player.SendMessage(p, "Date and time modified: &2" + TimePerformed);
            }

            List<Level.BlockPos> inCache = p.level.blockCache.FindAll(bP => bP.x == x && bP.y == y && bP.z == z);

            for (int i = 0; i < inCache.Count; i++)
            {
                foundOne = true;
                Deleted = inCache[i].deleted;
                Username = inCache[i].name;
                TimePerformed = inCache[i].TimePerformed.ToString("yyyy-MM-dd HH:mm:ss");
                BlockUsed = Block.Name(inCache[i].type);

                if (!Deleted)
                    Player.SendMessage(p, "&3Created by " + Server.FindColor(Username.Trim()) + Username.Trim() + Server.DefaultColor + ", using &3" + BlockUsed);
                else
                    Player.SendMessage(p, "&4Destroyed by " + Server.FindColor(Username.Trim()) + Username.Trim() + Server.DefaultColor + ", using &3" + BlockUsed);
                Player.SendMessage(p, "Date and time modified: &2" + TimePerformed);
            }

            if (!foundOne)
                Player.SendMessage(p, "This block has not been modified since the map was cleared.");
 
            Blocks.Dispose();

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}