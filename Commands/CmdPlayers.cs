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
using System.Collections.Generic;
namespace MCForge.Commands
{
    public sealed class CmdPlayers : Command
    {
        public override string name { get { return "players"; } }
        public override string shortcut { get { return "who"; } }
        public override string type { get { return "information"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }
        public CmdPlayers() { }

        struct groups { public Group group; public List<string> players; }
        public override void Use(Player p, string message)
        {
            try
            {
                List<groups> playerList = new List<groups>();

                foreach (Group grp in Group.GroupList)
                {
                    if (grp.name != "nobody")
                    {
                        if (String.IsNullOrEmpty(message) || !Group.Exists(message))
                        {
                            groups groups;
                            groups.group = grp;
                            groups.players = new List<string>();
                            playerList.Add(groups);
                        }
                        else
                        {
                            Group grp2 = Group.Find(message);
                            if (grp2 != null && grp == grp2)
                            {
                                groups groups;
                                groups.group = grp;
                                groups.players = new List<string>();
                                playerList.Add(groups);
                            }
                        }
                    }
                }

                string devs = "";
                string mods = "";
                string gcmods = "";
                int totalPlayers = 0;
                foreach (Player pl in Player.players)
                {
                    if (!pl.hidden || p == null || p.group.Permission > LevelPermission.Operator)
                    {
                        if (String.IsNullOrEmpty(message) || !Group.Exists(message) || Group.Find(message) == pl.group)
                        {
                            totalPlayers++;
                            string foundName = pl.name;

                            if (Server.afkset.Contains(pl.name))
                            {
                                foundName = pl.name + "-afk";
                            }

                            if (pl.muted) foundName += "[muted]";


                            if (pl.isDev)
                            {
                                if (pl.voice)
                                    devs += " " + "&f+" + Server.DefaultColor + foundName + " (" + pl.level.name + "),";
                                else
                                    devs += " " + foundName + " (" + pl.level.name + "),";
                            }
                            if (pl.isMod) {
                                if (pl.voice)
                                    mods += " " + "&f+" + Server.DefaultColor + foundName + " (" + pl.level.name + "),";
                                else
                                    mods += " " + foundName + " (" + pl.level.name + "),";
                            }
                            if (pl.isGCMod) {
                                if (pl.voice)
                                    gcmods += " " + "&f+" + Server.DefaultColor + foundName + " (" + pl.level.name + "),";
                                else
                                    gcmods += " " + foundName + " (" + pl.level.name + "),";
                            }

                            if (pl.voice)
                                playerList.Find(grp => grp.group == pl.group).players.Add("&f+" + Server.DefaultColor + foundName + " (" + pl.level.name + ")");
                            else
                                playerList.Find(grp => grp.group == pl.group).players.Add(foundName + " (" + pl.level.name + ")");
                        }
                        else
                        {
                            Player.SendMessage(p, "There are no players of that rank online.");
                            return;
                        }
                    }
                }

                Player.SendMessage(p, "There are %a" + totalPlayers + Server.DefaultColor + " players online.");
                bool staff = devs.Length > 0 || mods.Length > 0 || gcmods.Length > 0;
                if (staff) Player.SendMessage(p, "%cMCForge Staff Online:");
                if (devs.Length > 0)
                {
                    Player.SendMessage(p, "#%9MCForge Devs:" + Server.DefaultColor + devs.Trim(','));
                }
                if (mods.Length > 0) {
                    Player.SendMessage(p, "#%2MCForge Mods:" + Server.DefaultColor + mods.Trim(','));
                }
                if (gcmods.Length > 0) {
                    Player.SendMessage(p, "#%6MCForge GCMods:" + Server.DefaultColor + gcmods.Trim(','));
                }
                if (staff) Player.SendMessage(p, "%aNormal Players Online:");
                for (int i = playerList.Count - 1; i >= 0; i--)
                {
                    groups groups = playerList[i];
                    if (groups.players.Count > 0 || Server.showEmptyRanks)
                    {
                        string appendString = "";
                        foreach (string player in groups.players)
                            appendString += ", " + player;

                        if (appendString != "")
                            appendString = appendString.Remove(0, 2);
                        appendString = ":" + groups.group.color + getPlural(groups.group.trueName) + ": " + appendString;

                        Player.SendMessage(p, appendString);
                    }
                }
            }
            catch (Exception e) { Server.ErrorLog(e); }
        }

        public string getPlural(string groupName)
        {
            try
            {
                string last2 = groupName.Substring(groupName.Length - 2).ToLower();
                if ((last2 != "ed" || groupName.Length <= 3) && last2[1] != 's')
                {
                    return groupName + "s";
                }
                return groupName;
            }
            catch
            {
                return groupName;
            }
        }

        public override void Help(Player p)
        {
            Player.SendMessage(p, "/players [rank] - Shows name and general rank of all players");
        }
    }
}
