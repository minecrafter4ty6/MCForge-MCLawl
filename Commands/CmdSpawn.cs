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
    public sealed class CmdSpawn : Command
    {
        public override string name { get { return "spawn"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
        public CmdSpawn() { }

        public override void Use(Player p, string message)
        {
            if (message != "") { Help(p); return; }
            ushort x = (ushort)((0.5 + p.level.spawnx) * 32);
            ushort y = (ushort)((1 + p.level.spawny) * 32);
            ushort z = (ushort)((0.5 + p.level.spawnz) * 32);
            if (!p.referee)
            {
                if (!p.infected && Server.zombie.GameInProgess())
                {
                    Server.zombie.InfectPlayer(p);
                }
            }
            if (p.PlayingTntWars)
            {
                TntWarsGame it = TntWarsGame.GetTntWarsGame(p);
                if (it.GameMode == TntWarsGame.TntWarsGameMode.TDM && it.GameStatus != TntWarsGame.TntWarsGameStatus.WaitingForPlayers && it.GameStatus != TntWarsGame.TntWarsGameStatus.Finished && it.RedSpawn != null && it.BlueSpawn != null)
                unchecked
                {
                    p.SendPos((byte)-1,
                                (ushort)((0.5 + (it.FindPlayer(p).Blue ? it.BlueSpawn[0] : it.RedSpawn[0]) * 32)),
                                (ushort)((1 + (it.FindPlayer(p).Blue ? it.BlueSpawn[1] : it.RedSpawn[1]) * 32)),
                                (ushort)((0.5 + (it.FindPlayer(p).Blue ? it.BlueSpawn[2] : it.RedSpawn[2]) * 32)),
                                (byte)(it.FindPlayer(p).Blue ? it.BlueSpawn[3] : it.RedSpawn[3]),
                                (byte)(it.FindPlayer(p).Blue ? it.BlueSpawn[4] : it.RedSpawn[4]));
                    return;
                }
            }
            unchecked
            {
                p.SendPos((byte)-1, x, y, z,
                            p.level.rotx,
                            p.level.roty);
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/spawn - Teleports yourself to the spawn location.");
        }
    }
}
