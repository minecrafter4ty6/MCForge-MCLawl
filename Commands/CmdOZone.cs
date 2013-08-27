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
    public sealed class CmdOZone : Command
    {
        public override string name { get { return "ozone"; } }
        public override string shortcut { get { return "oz"; } }
        public override string type { get { return "mod"; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public override bool museumUsable { get { return false; } }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/ozone <rank/player> - Zones the entire map to <rank/player>");
            Player.SendMessage(p, "To delete a zone, just use /zone del anywhere on the map");
        }
        public override void Use(Player p, string message)
        {
            if (message == "") { this.Help(p); }
            else
            {
                int x2 = p.level.width - 1;
                int y2 = p.level.depth - 1;
                int z2 = p.level.height - 1;
                Command zone = Command.all.Find("zone");
                Command click = Command.all.Find("click");
                zone.Use(p, "add " + message);
                click.Use(p, 0 + " " + 0 + " " + 0);
                click.Use(p, x2 + " " + y2 + " " + z2);
            }
        }
    }
}