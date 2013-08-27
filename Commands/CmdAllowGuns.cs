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
namespace MCForge.Commands
{
    public sealed class CmdAllowGuns : Command
    {
        public override string name { get { return "allowguns"; } }
        public override string shortcut { get { return "ag"; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdAllowGuns() { }

        public override void Use(Player p, string message)
        {
            if (p == null) { Player.SendMessage(p, "This command can only be used in-game!"); return; }
            if (String.IsNullOrEmpty(message))
            {
                if (p.level.guns)
                {
                    p.level.guns = false;
                    Player.GlobalMessage("&9Gun usage has been disabled on &c" + p.level.name + "&9!");
                    Level.SaveSettings(p.level);

                    foreach (Player pl in Player.players)
                        if (pl.level == p.level)
                            pl.aiming = false;
                }
                else
                {
                    p.level.guns = true;
                    Player.GlobalMessage("&9Gun usage has been enabled on &c" + p.level.name + "&9!");
                    Level.SaveSettings(p.level);
                }
                return;
            }







            if (p != null)
            {
                Level foundLevel;
                if (message == "")
                {
                    if (p.level.guns == true)
                    {
                        p.level.guns = false;
                        Player.GlobalMessage("&9Gun usage has been disabled on &c" + p.level.name + "&9!");
                        Level.SaveSettings(p.level);
                        foreach (Player pl in Player.players)
                        {
                            if (pl.level.name.ToLower() == p.level.name.ToLower())
                            {
                                pl.aiming = false;
                                p.aiming = false;
                                return;
                            }
                            return;
                        }
                        return;
                    }
                    if (p.level.guns == false)
                    {
                        p.level.guns = true;
                        Player.GlobalMessage("&9Gun usage has been enabled on &c" + p.level.name + "&9!");
                        Level.SaveSettings(p.level);
                        return;
                    }
                }

                if (message != "")
                {
                    foundLevel = Level.Find(message);
                    if (!File.Exists("levels/" + message + ".lvl"))
                    {
                        Player.SendMessage(p, "&9The level, &c" + message + " &9does not exist!"); return;
                    }
                    if (foundLevel.guns == true)
                    {
                        foundLevel.guns = false;
                        Player.GlobalMessage("&9Gun usage has been disabled on &c" + message + "&9!");
                        Level.SaveSettings(foundLevel);
                        foreach (Player pl in Player.players)
                        {
                            if (pl.level.name.ToLower() == message.ToLower())
                            {
                                pl.aiming = false;
                            }
                            if (p.level.name.ToLower() == message.ToLower())
                            {
                                p.aiming = false;

                            }
                        }
                        return;
                    }
                    else
                    {
                        foundLevel.guns = true;
                        Player.GlobalMessage("&9Gun usage has been enabled on &c" + message + "&9!");
                        Level.SaveSettings(foundLevel);
                        return;
                    }
                }
            }
            if (p == null)
            {
                if (message == null)
                {
                    Player.SendMessage(p, "You must specify a level!");
                    return;
                }
                Level foundLevel;
                foundLevel = Level.Find(message);
                if (!File.Exists("levels/" + message + ".lvl"))
                {
                    Player.SendMessage(p, "The level, " + message + " does not exist!"); return;
                }
                if (foundLevel.guns == true)
                {
                    foundLevel.guns = false;
                    Player.GlobalMessage("&9Gun usage has been disabled on &c" + message + "&9!");
                    Level.SaveSettings(foundLevel);
                    Player.SendMessage(p, "Gun usage has been disabled on " + message + "!");
                    foreach (Player pl in Player.players)
                    {
                        if (pl.level.name.ToLower() == message.ToLower())
                        {
                            pl.aiming = false;
                            return;
                        }
                    }
                }
                foundLevel.guns = true;
                Player.GlobalMessage("&9Gun usage has been enabled on &c" + message + "&9!");
                Level.SaveSettings(foundLevel);
                Player.SendMessage(p, "Gun usage has been enabled on " + message + "!");
                return;
            }
        }



        public override void Help(Player p)
        {
            Player.SendMessage(p, "/allowguns - Allow/disallow guns and missiles on the specified level. If no message is given, the current level is taken.");
            Player.SendMessage(p, "Note: If guns are allowed on a map, and /allowguns is used, all guns and missiles will be disabled.");
        }
    }
}