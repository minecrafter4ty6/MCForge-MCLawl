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
    public sealed class CmdQuick : Command
    {
        public override string name { get { return "quick"; } }
        public override string shortcut { get { return "q"; } }
        public override string type { get { return "build"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdQuick() { }

        public override void Use(Player p, string message)
        {
            string msg = String.Empty;
            string type = String.Empty;
            if (message == "")
            {
                type = "cuboid";
            }
            else
            {
                type = message.Split(' ')[0];
            }
            try
            {
                msg = message.Replace(type + " ", "");
            }
            catch { }
            if (!p.group.CanExecute(Command.all.Find(type)))
            {
                Player.SendMessage(p, "You cannot execute the actual command, therefore you cannot use the quick version of it!");
                return;
            }
            if (p.level.Instant == false)
            {
                p.level.Instant = true;
            }
            if (type == "cuboid")
            {
                CmdCuboid.wait = 0;
                Command.all.Find(type).Use(p, msg);
                while (CmdCuboid.wait == 0)
                {
                }
                if (CmdCuboid.wait != 1)
                {
                    if (CmdCuboid.wait == 2)
                    {
                        Command.all.Find("reveal").Use(p, "all");
                        if (p.level.Instant == true)
                        {
                            p.level.Instant = false;
                        }
                    }
                }
                if (p.level.Instant == true)
                {
                    p.level.Instant = false;
                }
                return;
            }
            if (type == "replace")
            {
                CmdReplace.wait = 0;
                Command.all.Find(type).Use(p, msg);
                while (CmdReplace.wait == 0)
                {
                }
                if (CmdReplace.wait != 1)
                {
                    if (CmdReplace.wait == 2)
                    {
                        Command.all.Find("reveal").Use(p, "all");
                        if (p.level.Instant == true)
                        {
                            p.level.Instant = false;
                        }
                    }
                }
                if (p.level.Instant == true)
                {
                    p.level.Instant = false;
                }
                return;
            }
            if (type == "replaceall")
            {
                CmdReplaceAll.wait = 0;
                Command.all.Find(type).Use(p, msg);
                while (CmdReplaceAll.wait == 0)
                {
                }
                if (CmdReplaceAll.wait != 1)
                {
                    if (CmdReplaceAll.wait == 2)
                    {
                        Command.all.Find("reveal").Use(p, "all");
                        if (p.level.Instant == true)
                        {
                            p.level.Instant = false;
                        }
                    }
                }
                if (p.level.Instant == true)
                {
                    p.level.Instant = false;
                }
                return;
            }
            if (type == "replacenot")
            {
                CmdReplaceNot.wait = 0;
                Command.all.Find(type).Use(p, msg);
                while (CmdReplaceNot.wait == 0)
                {
                }
                if (CmdReplaceNot.wait != 1)
                {
                    if (CmdReplaceNot.wait == 2)
                    {
                        Command.all.Find("reveal").Use(p, "all");
                        if (p.level.Instant == true)
                        {
                            p.level.Instant = false;
                        }
                    }
                }
                if (p.level.Instant == true)
                {
                    p.level.Instant = false;
                }
                return;
            }
            if (type == "spheroid")
            {
                CmdSpheroid.wait = 0;
                Command.all.Find(type).Use(p, msg);
                while (CmdSpheroid.wait == 0)
                {
                }
                if (CmdSpheroid.wait != 1)
                {
                    if (CmdSpheroid.wait == 2)
                    {
                        Command.all.Find("reveal").Use(p, "all");
                        if (p.level.Instant == true)
                        {
                            p.level.Instant = false;
                        }
                    }
                }
                if (p.level.Instant == true)
                {
                    p.level.Instant = false;
                }
                return;
            }
            if (type == "pyramid")
            {
                CmdPyramid.wait = 0;
                Command.all.Find(type).Use(p, msg);
                while (CmdPyramid.wait == 0)
                {
                }
                if (CmdPyramid.wait != 1)
                {
                    if (CmdPyramid.wait == 2)
                    {
                        Command.all.Find("reveal").Use(p, "all");
                        if (p.level.Instant == true)
                        {
                            p.level.Instant = false;
                        }
                    }
                }
                if (p.level.Instant == true)
                {
                    p.level.Instant = false;
                }
                return;
            }
            Player.SendMessage(p, "No such quickcuboid type!");
        }

        public override void Help(Player p)
        {
            Player.SendMessage(p, "/quick [Function] [Block] [Type] - Cuboids the selected function instantly.");
            Player.SendMessage(p, "Functions: cuboid, pyramid, replace, replaceall, replacenot, spheroid.");
            Player.SendMessage(p, "if none is specified, quick cuboid will be used! Shortcut: /q");
        }
    }
}