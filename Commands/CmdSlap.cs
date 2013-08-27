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
    public sealed class CmdSlap : Command
    {
        public override string name { get { return "slap"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.AdvBuilder; } }
        public CmdSlap() { }

        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }

            Player who = Player.Find(message);

            if (who == null)
            {
                Level which = Level.Find(message);

                if (which == null)
                {
                    Player.SendMessage(p, "Could not find player or map specified");
                    return;
                }
                else
                {
                    foreach (Player pl in Player.players)
                    {
                        if (pl.level == which && pl.group.Permission < p.group.Permission)
                        {
                               Command.all.Find("slap").Use(p, pl.name);
                        }
                    }
                    return;
                }
            }
            if (p != null)
            {
                if (who.group.Permission > p.group.Permission)
                {
                    Player.SendMessage(p, "You cannot slap someone ranked higher than you!");
                    return;
                }
            }

            ushort currentX = (ushort)(who.pos[0] / 32);
            ushort currentY = (ushort)(who.pos[1] / 32);
            ushort currentZ = (ushort)(who.pos[2] / 32);
            ushort foundHeight = 0;

            for (ushort yy = currentY; yy <= 1000; yy++)
            {
                if (p != null)
                {
                    if (!Block.Walkthrough(p.level.GetTile(currentX, yy, currentZ)) && p.level.GetTile(currentX, yy, currentZ) != Block.Zero)
                    {
                        foundHeight = (ushort)(yy - 1);
                        who.level.ChatLevel(who.color + who.name + Server.DefaultColor + " was slapped into the roof by " + p.color + p.name);
                        break;
                    }
                }
                else
                {
                    if (!Block.Walkthrough(who.level.GetTile(currentX, yy, currentZ)) && who.level.GetTile(currentX, yy, currentZ) != Block.Zero)
                    {
                        foundHeight = (ushort)(yy - 1);
                        who.level.ChatLevel(who.color + who.name + Server.DefaultColor + " was slapped into the roof by " + "the Console.");
                        break;
                    }
                }
            }
            if (foundHeight == 0)
            {
                if (p != null)
                {
                    who.level.ChatLevel(who.color + who.name + Server.DefaultColor + " was slapped sky high by " + p.color + p.name);
                }
                else
                {
                    who.level.ChatLevel(who.color + who.name + Server.DefaultColor + " was slapped sky high by " + "the Console.");
                }
                foundHeight = 1000;
            }
            
            unchecked { who.SendPos((byte)-1, who.pos[0], (ushort)(foundHeight * 32), who.pos[2], who.rot[0], who.rot[1]); }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/slap <name> - Slaps <name>, knocking them into the air");
            Player.SendMessage(p, "/slap <level> - Slaps all players on <level> that are a lower rank than you, knocking them into the air");
        }
    }
}