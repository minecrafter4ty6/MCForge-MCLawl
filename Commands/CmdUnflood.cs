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
    public sealed class CmdUnflood : Command
    {
        public override string name { get { return "unflood"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public override void Help(Player p) { Player.SendMessage(p, "/unflood [liquid] - Unfloods the map you are on of [liquid]"); }
        public override void Use(Player p, string message)
        {
            if (p == null) { Player.SendMessage(p, "This command can only be used in-game!"); return; }
            if (String.IsNullOrEmpty(message)) { Help(p); return; }
            if (message.ToLower() != "all" && Block.Byte(message) == Block.Zero) { Player.SendMessage(p, "There is no block \"" + message + "\"."); return; }
            int phys = p.level.physics;
            Command.all.Find("physics").Use(p, "0");
            if (!p.level.Instant)
                Command.all.Find("map").Use(p, "instant");

            if (message.ToLower() == "all")
            {
                Command.all.Find("replaceall").Use(p, "lavafall air");
                Command.all.Find("replaceall").Use(p, "waterfall air");
                Command.all.Find("replaceall").Use(p, "lava_fast air");
                Command.all.Find("replaceall").Use(p, "active_lava air");
                Command.all.Find("replaceall").Use(p, "active_water air");
                Command.all.Find("replaceall").Use(p, "active_hot_lava air");
                Command.all.Find("replaceall").Use(p, "active_cold_water air");
                Command.all.Find("replaceall").Use(p, "fast_hot_lava air");
                Command.all.Find("replaceall").Use(p, "magma air");
            }
            else
            {
                Command.all.Find("replaceall").Use(p, message + " air");
            }

            if (p.level.Instant)
                Command.all.Find("map").Use(p, "instant");
            Command.all.Find("reveal").Use(p, "all");
            Command.all.Find("physics").Use(p, phys.ToString());
            Player.GlobalMessage("Unflooded!");
        }
    }
}