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
using System.Threading;
namespace MCForge.Commands
{
    public sealed class CmdWarp : Command
    {
        public override string name { get { return "warp"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }
        public override void Use(Player p, string message)
        {
            if (p == null) { Player.SendMessage(p, "This command can only be used in-game"); return; }
            string[] command = message.ToLower().Split(' ');
            string par0 = String.Empty;
            string par1 = String.Empty;
            string par2 = String.Empty;
            try
            {
                par0 = command[0];
                par1 = command[1];
                par2 = command[2];
            }
            catch { }
            if (par0 == "list" || par0 == "view" || par0 == "l" || par0 == "v")
            {
                Player.SendMessage(p, "Warps:");
                foreach (Warp.Wrp wr in Warp.Warps)
                {
                    if (Level.Find(wr.lvlname) != null)
                    {
                        Player.SendMessage(p, wr.name + " : " + wr.lvlname);
                        Thread.Sleep(300); // I feel this is needed so that if there are a lot of warps, they do not immediatly go off the screen!
                    }
                }
                return;
            }

            if (par0 == "create" || par0 == "add" || par0 == "c" || par0 == "a")
            {
                if ((int)p.group.Permission >= CommandOtherPerms.GetPerm(this, 1))
                {
                    if (par1 == null) { Player.SendMessage(p, "You didn't specify a name for the warp!"); return; }
                    if (Warp.WarpExists(par1)) { Player.SendMessage(p, "Warp has already been created!!"); return; }
                    {
                        if (par2 == null) { Warp.AddWarp(par1, p); }
                        else { Warp.AddWarp(par1, Player.Find(par2)); }
                    }
                    {
                        if (Warp.WarpExists(par1))
                        {
                            Player.SendMessage(p, "Warp created!");
                            return;
                        }
                        else
                        {
                            Player.SendMessage(p, "Warp creation failed!!");
                            return;
                        }
                    }
                }
                else { Player.SendMessage(p, "You can't use that because you aren't a" + Group.findPermInt(CommandOtherPerms.GetPerm(this, 1)).name + "+"); return; }
            }

            if (par0 == "delete" || par0 == "remove" || par0 == "d" || par0 == "r")
            {
                if ((int)p.group.Permission >= CommandOtherPerms.GetPerm(this, 2))
                {
                    if (par1 == null) { Player.SendMessage(p, "You didn't specify a warp to delete!"); return; }
                    if (!Warp.WarpExists(par1)) { Player.SendMessage(p, "Warp doesn't exist!!"); return; }
                    {
                        Warp.DeleteWarp(par1);
                    }
                    {
                        if (!Warp.WarpExists(par1))
                        {
                            Player.SendMessage(p, "Warp deleted!");
                            return;
                        }
                        else
                        {
                            Player.SendMessage(p, "Warp deletion failed!!");
                            return;
                        }
                    }
                }
                else { Player.SendMessage(p, "You can't use that because you aren't a" + Group.findPermInt(CommandOtherPerms.GetPerm(this, 2)).name + "+"); return; }
            }

            if (par0 == "move" || par0 == "change" || par0 == "edit" || par0 == "m" || par0 == "e")
            {
                if ((int)p.group.Permission >= CommandOtherPerms.GetPerm(this, 3))
                {
                    if (par1 == null) { Player.SendMessage(p, "You didn't specify a warp to be moved!"); return; }
                    if (!Warp.WarpExists(par1)) { Player.SendMessage(p, "Warp doesn't exist!!"); return; }
                    {
                        if (par2 == null) { Warp.MoveWarp(par1, p); }
                        else { Warp.MoveWarp(par1, Player.Find(par2)); }
                    }
                    {
                        if (Warp.WarpExists(par1))
                        {
                            Player.SendMessage(p, "Warp moved!");
                            return;
                        }
                        else
                        {
                            Player.SendMessage(p, "Warp moving failed!!");
                            return;
                        }
                    }
                }
                else { Player.SendMessage(p, "You can't use that because you aren't a " + Group.findPermInt(CommandOtherPerms.GetPerm(this, 3)).name + "+"); return; }
            }

            else
            {
                if (Warp.WarpExists(par0) == true)
                {
                    Warp.Wrp w = new Warp.Wrp();
                    w = Warp.GetWarp(par0);
                    Level lvl = Level.Find(w.lvlname);
                    if (lvl != null)
                    {
                        if (p.level != lvl)
                        {
                            if (lvl.permissionvisit > p.group.Permission) { Player.SendMessage(p, "Sorry, you aren't a high enough rank to visit the map that that warp is on."); return; }
                            Command.all.Find("goto").Use(p, lvl.name);
                            while (p.Loading) { Thread.Sleep(250); }
                        }
                        unchecked { p.SendPos((byte)-1, w.x, w.y, w.z, w.rotx, w.roty); }
                        return;
                    }
                    else
                    {
                        Player.SendMessage(p, "The level that that warp is on (" + w.lvlname + ") either no longer exists or is currently unloaded");
                        return;
                    }
                }
                else
                {
                    Player.SendMessage(p, "That is not a command addition or a warp");
                    return;
                }
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/warp [name] - warp to that warp");
            Player.SendMessage(p, "/warp list - list all the warps");
            if ((int)p.group.Permission >= CommandOtherPerms.GetPerm(this, 1))
            {
                Player.SendMessage(p, "/warp create [name] <player> - create a warp, if a <player> is given, it will be created where they are");
            }
            if ((int)p.group.Permission >= CommandOtherPerms.GetPerm(this, 2))
            {
                Player.SendMessage(p, "/warp delete [name] - delete a warp");
            }
            if ((int)p.group.Permission >= CommandOtherPerms.GetPerm(this, 3))
            {
                Player.SendMessage(p, "/warp move [name] <player> - move a warp, if a <player> is given, it will be created where they are");
            }
        }
    }
}
