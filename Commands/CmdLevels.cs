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
    public sealed class CmdLevels : Command
    {
        public override string name { get { return "levels"; } }
        public override string shortcut { get { return "maps"; } }
        public override string type { get { return "information"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }
        public CmdLevels() { }

        public override void Use(Player p, string message)
        {
            try
            {

                if (!String.IsNullOrEmpty(message.Trim()))
                {
                    Help(p);
                    return;
                }

                message = "";
                string message2 = "";
                bool Once = false;
                Server.levels.ForEach((level) =>
                {
                    if (p != null && level.permissionvisit <= p.group.Permission)
                    {
                        if (Group.findPerm(level.permissionbuild) != null)
                            message += ", " + Group.findPerm(level.permissionbuild).color + level.name + " &b[&f" + level.physics + "&b]";
                        else
                            message += ", " + level.name + " &b[" + level.physics + "]";
                    }
                    else
                    {
                        if (!Once)
                        {
                            Once = true;
                            if (Group.findPerm(level.permissionvisit) != null)
                                message2 += Group.findPerm(level.permissionvisit).color + level.name + " &b[&f" + level.physics + "&b]";
                            else
                                message2 += level.name + " &b[" + level.physics + "]";
                        }
                        else
                        {
                            if (Group.findPerm(level.permissionvisit) != null)
                                message2 += ", " + Group.findPerm(level.permissionvisit).color + level.name + " &b[&f" + level.physics + "&b]";
                            else
                                message2 += ", " + level.name + " &b[&f" + level.physics + "&b]";
                        }
                    }
                });

                Player.SendMessage(p, "Loaded levels [physics_level]: " + message.Remove(0, 2));
                if (message2 != "")
                    Player.SendMessage(p, "Can't Goto: " + message2);
                Player.SendMessage(p, "Use &f/unloaded" + Server.DefaultColor + " for unloaded levels.");
            }
            catch (Exception e)
            {
                Server.ErrorLog(e);
            }

        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "%f/levels " + Server.DefaultColor + "- Lists all loaded levels and their physics levels.");
        }
    }
}