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

using System.Collections.Generic;
using System.Data;
using MCForge.SQL;
namespace MCForge.Commands
{
    public sealed class CmdPortal : Command
    {
        public override string name { get { return "portal"; } }
        public override string shortcut { get { return "o"; } }
        public override string type { get { return "build"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.AdvBuilder; } }
        public CmdPortal() { }

        public override void Use(Player p, string message)
        {
            portalPos portalPos;

            portalPos.Multi = false;

            if (message.IndexOf(' ') != -1)
            {
                if (message.Split(' ')[1].ToLower() == "multi")
                {
                    portalPos.Multi = true;
                    message = message.Split(' ')[0];
                }
                else
                {
                    Player.SendMessage(p, "Invalid parameters");
                    return;
                }
            }

            if (message.ToLower() == "blue" || message == "") { portalPos.type = Block.blue_portal; }
            else if (message.ToLower() == "orange") { portalPos.type = Block.orange_portal; }
            else if (message.ToLower() == "air") { portalPos.type = Block.air_portal; }
            else if (message.ToLower() == "water") { portalPos.type = Block.water_portal; }
            else if (message.ToLower() == "lava") { portalPos.type = Block.lava_portal; }
            else if (message.ToLower() == "show") { showPortals(p); return; }
            else { Help(p); return; }

            p.ClearBlockchange();

            portPos port;

            port.x = 0; port.y = 0; port.z = 0; port.portMapName = "";
            portalPos.port = new List<portPos>();

            p.blockchangeObject = portalPos;
            Player.SendMessage(p, "Place an &aEntry block" + Server.DefaultColor + " for the portal"); p.ClearBlockchange();
            p.Blockchange += new Player.BlockchangeEventHandler(EntryChange);
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/portal [orange/blue/air/water/lava] [multi] - Activates Portal mode.");
            Player.SendMessage(p, "/portal [type] multi - Place Entry blocks until exit is wanted.");
            Player.SendMessage(p, "/portal show - Shows portals, green = in, red = out.");
        }

        public void EntryChange(Player p, ushort x, ushort y, ushort z, byte type)
        {
            p.ClearBlockchange();
            portalPos bp = (portalPos)p.blockchangeObject;

            if (bp.Multi && type == Block.red && bp.port.Count > 0) { ExitChange(p, x, y, z, type); return; }

            byte b = p.level.GetTile(x, y, z);
            p.level.Blockchange(p, x, y, z, bp.type);
            p.SendBlockchange(x, y, z, Block.green);
            portPos Port;

            Port.portMapName = p.level.name;
            Port.x = x; Port.y = y; Port.z = z;

            bp.port.Add(Port);

            p.blockchangeObject = bp;

            if (!bp.Multi)
            {
                p.Blockchange += ExitChange;
                Player.SendMessage(p, "&aEntry block placed");
            }
            else
            {
                p.Blockchange += EntryChange;
                Player.SendMessage(p, "&aEntry block placed. &cRed block for exit");
            }
        }
        public void ExitChange(Player p, ushort x, ushort y, ushort z, byte type)
        {
            p.ClearBlockchange();
            byte b = p.level.GetTile(x, y, z);
            p.SendBlockchange(x, y, z, b);
            portalPos bp = (portalPos)p.blockchangeObject;

            foreach (portPos pos in bp.port)
            {
                //safe against SQL injections because no user input is given here
                DataTable Portals = Database.fillData("SELECT * FROM `Portals" + pos.portMapName + "` WHERE EntryX=" + (int)pos.x + " AND EntryY=" + (int)pos.y + " AND EntryZ=" + (int)pos.z);
                Portals.Dispose();

                if (Portals.Rows.Count == 0)
                {//safe against SQL injections because no user input is given here
                    Database.executeQuery("INSERT INTO `Portals" + pos.portMapName + "` (EntryX, EntryY, EntryZ, ExitMap, ExitX, ExitY, ExitZ) VALUES (" + (int)pos.x + ", " + (int)pos.y + ", " + (int)pos.z + ", '" + p.level.name + "', " + (int)x + ", " + (int)y + ", " + (int)z + ")");
                }
                else
                {//safe against SQL injections because no user input is given here
                    Database.executeQuery("UPDATE `Portals" + pos.portMapName + "` SET ExitMap='" + p.level.name + "', ExitX=" + (int)x + ", ExitY=" + (int)y + ", ExitZ=" + (int)z + " WHERE EntryX=" + (int)pos.x + " AND EntryY=" + (int)pos.y + " AND EntryZ=" + (int)pos.z);
                }
                //DB

                if (pos.portMapName == p.level.name) p.SendBlockchange(pos.x, pos.y, pos.z, bp.type);
            }

            Player.SendMessage(p, "&3Exit" + Server.DefaultColor + " block placed");

            if (!p.staticCommands)
                return;
            bp.port.Clear(); p.blockchangeObject = bp; p.Blockchange += EntryChange;
        }

        public struct portalPos { public List<portPos> port; public byte type; public bool Multi; }
        public struct portPos { public ushort x, y, z; public string portMapName; }

        public void showPortals(Player p)
        {
            p.showPortals = !p.showPortals;
            //safe against SQL injections because no user input is given here
            DataTable Portals = Database.fillData("SELECT * FROM `Portals" + p.level.name + "`");

            int i;

            if (p.showPortals)
            {
                for (i = 0; i < Portals.Rows.Count; i++)
                {
                    if (Portals.Rows[i]["ExitMap"].ToString() == p.level.name)
                        p.SendBlockchange((ushort)Portals.Rows[i]["ExitX"], (ushort)Portals.Rows[i]["ExitY"], (ushort)Portals.Rows[i]["ExitZ"], Block.orange_portal);
                    p.SendBlockchange((ushort)Portals.Rows[i]["EntryX"], (ushort)Portals.Rows[i]["EntryY"], (ushort)Portals.Rows[i]["EntryZ"], Block.blue_portal);
                }

                Player.SendMessage(p, "Now showing &a" + i.ToString() + Server.DefaultColor + " portals.");
            }
            else
            {
                for (i = 0; i < Portals.Rows.Count; i++)
                {
                    if (Portals.Rows[i]["ExitMap"].ToString() == p.level.name)
                        p.SendBlockchange((ushort)Portals.Rows[i]["ExitX"], (ushort)Portals.Rows[i]["ExitY"], (ushort)Portals.Rows[i]["ExitZ"], Block.air);

                    p.SendBlockchange((ushort)Portals.Rows[i]["EntryX"], (ushort)Portals.Rows[i]["EntryY"], (ushort)Portals.Rows[i]["EntryZ"], p.level.GetTile((ushort)Portals.Rows[i]["EntryX"], (ushort)Portals.Rows[i]["EntryY"], (ushort)Portals.Rows[i]["EntryZ"]));
                }

                Player.SendMessage(p, "Now hiding portals.");
            }

            Portals.Dispose();
        }
    }
}