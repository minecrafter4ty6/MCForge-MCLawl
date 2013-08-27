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
    public sealed class CmdZombieGame : Command
    {
        public override string name { get { return "zombiegame"; } }
        public override string shortcut { get { return "zg"; } }
        public override string type { get { return "game"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdZombieGame() { }
        public override void Use(Player p, string message)
        {
            if (String.IsNullOrEmpty(message)) { Help(p); return; }
            string[] s = message.ToLower().Split(' ');
            if (s[0] == "status")
            {
                switch (Server.zombie.ZombieStatus())
                {
                    case 0:
                        Player.GlobalMessage("There is no Zombie Survival game currently in progress.");
                        return;
                    case 1:
                        Player.SendMessage(p, "There is a Zombie Survival game currently in progress with infinite rounds.");
                        return;
                    case 2:
                        Player.SendMessage(p, "There is a one-time Zombie Survival game currently in progress.");
                        return;
                    case 3:
                        Player.SendMessage(p, "There is a Zombie Survival game currently in progress with a " + Server.zombie.limitRounds + " amount of rounds.");
                        return;
                    case 4:
                        Player.SendMessage(p, "There is a Zombie Survival game currently in progress, scheduled to stop after this round.");
                        return;
                    default:
                        Player.SendMessage(p, "An unknown error occurred.");
                        return;
                }
            }
            else if (s[0] == "start")
            {
                if (Server.zombie.ZombieStatus() != 0) { Player.SendMessage(p, "There is already a Zombie Survival game currently in progress."); return; }
                if (s.Length == 2)
                {
                    int i = 1;
                    bool result = int.TryParse(s[1], out i);
                    if (result == false) { Player.SendMessage(p, "You need to specify a valid option!"); return; }
                    if (s[1] == "0")
                    {
                        Server.zombie.StartGame(1, 0);
                    }
                    else
                    {
                        Server.zombie.StartGame(3, i);
                    }
                }
                else
                    Server.zombie.StartGame(2, 0);
            }
            else if (s[0] == "stop")
            {
                if (Server.zombie.ZombieStatus() == 0) { Player.SendMessage(p, "There is no Zombie Survival game currently in progress."); return; }
                Player.GlobalMessage("The current game of Zombie Survival will end this round!");
                Server.gameStatus = 4;
            }
            else if (s[0] == "force")
            {
                if (Server.zombie.ZombieStatus() == 0) { Player.SendMessage(p, "There is no Zombie Survival game currently in progress."); return; }
                Server.s.Log("Zombie Survival ended forcefully by " + p.name);
                Server.zombie.aliveCount = 0;
                Server.gameStatus = 0; Server.gameStatus = 0; Server.zombie.limitRounds = 0; Server.zombie.initialChangeLevel = false; Server.ZombieModeOn = false; Server.zombieRound = false;
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/zombiegame - Shows this help menu.");
            Player.SendMessage(p, "/zombiegame start - Starts a Zombie Survival game for one round.");
            Player.SendMessage(p, "/zombiegame start 0 - Starts a Zombie Survival game for an unlimited amount of rounds.");
            Player.SendMessage(p, "/zombiegame start [x] - Starts a Zombie Survival game for [x] amount of rounds.");
            Player.SendMessage(p, "/zombiegame stop - Stops the Zombie Survival game after the round has finished.");
            Player.SendMessage(p, "/zombiegame force - Force stops the Zombie Survival game immediately.");
        }
    }
}