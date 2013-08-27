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
    public sealed class CmdSave : Command
    {
        public override string name { get { return "save"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdSave() { }

        public override void Use(Player p, string message) {
            if (message.ToLower() == "all") {
                foreach (Level l in Server.levels) {
                    try {
                        if (!Server.lava.active || !Server.lava.HasMap(name)) l.Save();
                        else { Server.s.Log("The level \"" + l.name + "\" is a Lava Survival level, only saving block change history."); l.saveChanges(); }
                    } catch { }
                }
                Player.GlobalMessage("All levels have been saved.");
            } else {
                if (message == "") { // for empty string/no parameters.
                    if (p == null) {
                        Use(p, "all");
                    } else {
                        p.level.Save(true);
                        Player.SendMessage(p, "Level \"" + p.level.name + "\" saved.");

                        int backupNumber = p.level.Backup(true);
                        if (backupNumber != -1) {
                            // Notify console and the player who called /save
                            Player.SendMessage(null, "Backup " + backupNumber + " saved for " + p.level.name);
                            if (p != null)
                                p.level.ChatLevel("Backup " + backupNumber + " saved.");
                        }
                    }
                } else if (message.Split(' ').Length == 1) { //Just save level given 
                    Level foundLevel = Level.Find(message);
                    if (foundLevel != null) {
                        foundLevel.Save(true);
                        Player.SendMessage(p, "Level \"" + foundLevel.name + "\" saved.");
                        int backupNumber = foundLevel.Backup(true);
                        if (backupNumber != -1) {
                            // Notify console and the player who called /save
                            Player.SendMessage(null, "Backup " + backupNumber + " saved for " + foundLevel.name);
                            if (p != null)
                                p.level.ChatLevel("Backup " + backupNumber + " saved.");
                        }
                    } else {
                        Player.SendMessage(p, "Could not find level specified");
                    }
                } else if (message.Split(' ').Length == 2) {
                    Level foundLevel = Level.Find(message.Split(' ')[0]);
                    string restoreName = message.Split(' ')[1].ToLower();
                    if (foundLevel != null) {
                        foundLevel.Save(true);
                        int backupNumber = p.level.Backup(true, restoreName);
                        Player.GlobalMessage(foundLevel.name + " had a backup created named &b" + restoreName);
                        Player.SendMessage(null, foundLevel.name + " had a backup created named &b" + restoreName);
                    } else {
                        Player.SendMessage(p, "Could not find level specified");
                    }
                } else { // Invalid number of arguments
                    Help(p);
                }
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/save - Saves the level you are currently in");
            Player.SendMessage(p, "/save all - Saves all loaded levels.");
            Player.SendMessage(p, "/save <map> - Saves the specified map.");
            Player.SendMessage(p, "/save <map> <name> - Backups the map with a given restore name");
        }
    }
}