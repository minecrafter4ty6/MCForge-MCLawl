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

using System.IO;
namespace MCForge.Commands
{
    public sealed class CmdXJail : Command
    {
        public override string name { get { return "xjail"; } }
        public override string shortcut { get { return "xj"; } }
        public override string type { get { return "other"; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public override bool museumUsable { get { return true; } }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/xjail <player> - Mutes <player>, freezes <player> and sends <player> to the XJail map (shortcut = /xj)");
            Player.SendMessage(p, "If <player> is already jailed, <player> will be spawned, unfrozen and unmuted");
            Player.SendMessage(p, "/xjail set - Sets the map to be used for xjail to your current map and sets jail to current location");
        }
        public override void Use(Player p, string message)
        {
            string dir = "extra/jail/";
            string jailMapFile = dir + "xjail.map.xjail";
            if (!Directory.Exists(dir)) { Directory.CreateDirectory(dir); }
            if (!File.Exists(jailMapFile))
            {
                using (StreamWriter SW = new StreamWriter(jailMapFile))
                {
                    SW.WriteLine(Server.mainLevel.name);
                }
            }
            if (message == "") { Help(p); return; }
            else
            {
                using (StreamReader SR = new StreamReader(jailMapFile))
                {
                    string xjailMap = SR.ReadLine();
                    SR.Close();
                    Command jail = Command.all.Find("jail");
                    if (message == "set")
                    {
                        if (!p.level.name.Contains("cMuseum"))
                        {
                            jail.Use(p, "create");
                            using (StreamWriter SW = new StreamWriter(jailMapFile))
                            {
                                SW.WriteLine(p.level.name);
                            }
                            Player.SendMessage(p, "The xjail map was set from '" + xjailMap + "' to '" + p.level.name + "'");
                            return;
                        }
                        else { Player.SendMessage(p, "You are in a museum!"); return; }
                    }
                    else
                    {
                        Player player = Player.Find(message);

                        if (player != null)
                        {
                            Command move = Command.all.Find("move");
                            Command spawn = Command.all.Find("spawn");
                            Command freeze = Command.all.Find("freeze");
                            Command mute = Command.all.Find("mute");
                            string playerFile = dir + player.name + "_temp.xjail";
                            if (!File.Exists(playerFile))
                            {
                                using (StreamWriter writeFile = new StreamWriter(playerFile))
                                {
                                    writeFile.WriteLine(player.level.name);
                                }
                                if (!player.muted) { mute.Use(p, message); }
                                if (!player.frozen) { freeze.Use(p, message); }
                                move.Use(p, message + " " + xjailMap);
                                while (player.Loading)
                                {
                                }
                                if (!player.jailed) { jail.Use(p, message); }
                                Player.GlobalMessage(player.color + player.name + Server.DefaultColor + " was XJailed!");
                                return;
                            }
                            else
                            {
                                using (StreamReader readFile = new StreamReader(playerFile))
                                {
                                    string playerMap = readFile.ReadLine();
                                    readFile.Close();
                                    File.Delete(playerFile);
                                    move.Use(p, message + " " + playerMap);
                                    while (player.Loading)
                                    {
                                    }
                                    mute.Use(p, message);
                                    jail.Use(p, message);
                                    freeze.Use(p, message);
                                    spawn.Use(player, "");
                                    Player.GlobalMessage(player.color + player.name + Server.DefaultColor + " was released from XJail!");
                                }
                                return;
                            }
                        }
                        else { Player.SendMessage(p, "Player not found"); return; }
                    }
                }
            }
        }
    }
}