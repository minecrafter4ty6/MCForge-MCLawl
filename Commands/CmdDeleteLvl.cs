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
using System.IO;
using MCForge.SQL;
namespace MCForge.Commands {
    public sealed class CmdDeleteLvl : Command {
        public override string name { get { return "deletelvl"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Admin; } }
        public CmdDeleteLvl() { }

        public override void Use(Player p, string message) {
            if (message == "" || message.Split().Length > 1) { Help(p); return; }
            Level foundLevel = Level.Find(message);
            if (foundLevel != null) {
                if (foundLevel.permissionbuild > p.group.Permission) {
                    Player.SendMessage(p, "%cYou can't delete levels with a perbuild rank higher than yours!");
                    return;
                }
                foundLevel.Unload();
            }

            if (foundLevel == Server.mainLevel) { Player.SendMessage(p, "Cannot delete the main level."); return; }

            try {
                if (!Directory.Exists("levels/deleted")) Directory.CreateDirectory("levels/deleted");

                if (File.Exists("levels/" + message + ".lvl")) {

                    using (StreamReader reader = new StreamReader("levels/level properties/" + message + ".properties")) {
                        string line;
                        while ((line = reader.ReadLine()) != null) {
                            if (line[0] == '#') continue;
                            if (line.Split()[0].ToLower() == "perbuild") {
                                if (Level.PermissionFromName(line.Split()[2].ToLower()) > p.group.Permission) {
                                    Player.SendMessage(p, "%cYou can't delete levels with a perbuild rank higher than yours!");
                                    return;
                                }
                                break;
                            }
                        }
                    }

                    if (File.Exists("levels/deleted/" + message + ".lvl")) {
                        int currentNum = 0;
                        while (File.Exists("levels/deleted/" + message + currentNum + ".lvl")) currentNum++;

                        File.Move("levels/" + message + ".lvl", "levels/deleted/" + message + currentNum + ".lvl");
                    } else {
                        File.Move("levels/" + message + ".lvl", "levels/deleted/" + message + ".lvl");
                    }
                    Player.SendMessage(p, "Created backup.");

                    try { File.Delete("levels/level properties/" + message + ".properties"); } catch { }
                    try { File.Delete("levels/level properties/" + message); } catch { }

                    //safe against SQL injections because the levelname (message) is first being checked if it exists
                    Database.executeQuery("DROP TABLE Block" + message + ", Portals" + message + ", Messages" + message + " Zone" + message + "");

                    Player.GlobalMessage("Level " + message + " was deleted.");
                } else {
                    Player.SendMessage(p, "Could not find specified level.");
                }
            } catch (Exception e) { Player.SendMessage(p, "Error when deleting."); Server.ErrorLog(e); }
        }
        public override void Help(Player p) {
            Player.SendMessage(p, "/deletelvl [map] - Completely deletes [map] (portals, MBs, everything");
            Player.SendMessage(p, "A backup of the map will be placed in the levels/deleted folder");
        }
    }
}