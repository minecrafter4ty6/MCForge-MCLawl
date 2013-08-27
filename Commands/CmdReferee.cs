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
    /// <summary>
    /// This is the command /referee
    /// use /help referee in-game for more info
    /// </summary>
    public sealed class CmdReferee : Command
    {
        public override string name { get { return "ref"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdReferee() { }
        public override void Use(Player p, string message)
        {
            if (p == null) { Player.SendMessage(p, "This command can only be used in-game!"); return; }
            if (p.referee)
            {
                p.referee = false;
                LevelPermission perm = Group.findPlayerGroup(name).Permission;
                Player.GlobalDie(p, false);
                Player.GlobalChat(p, p.color + p.name + Server.DefaultColor + " is no longer a referee", false);
                if (Server.zombie.GameInProgess())
                {
                    Server.zombie.InfectPlayer(p);
                }
                else
                {
                    Player.GlobalDie(p, false);
                    Player.GlobalSpawn(p, p.pos[0], p.pos[1], p.pos[2], p.rot[0], p.rot[1], false);
                    ZombieGame.infectd.Remove(p);
                    ZombieGame.alive.Add(p);
                    p.color = p.group.color;
                }
            }
            else
            {
                p.referee = true;
                Player.GlobalChat(p, p.color + p.name + Server.DefaultColor + " is now a referee", false);
                Player.GlobalDie(p, false);
                if (Server.zombie.GameInProgess())
                {
                    p.color = p.group.color;
                    try
                    {
                        ZombieGame.infectd.Remove(p);
                        ZombieGame.alive.Remove(p);
                    }
                    catch { }
                    Server.zombie.InfectedPlayerDC();
                }
                else
                {
                    ZombieGame.infectd.Remove(p);
                    ZombieGame.alive.Remove(p);
                    p.color = p.group.color;
                }
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/referee - Turns referee mode on/off.");
        }
    }
}