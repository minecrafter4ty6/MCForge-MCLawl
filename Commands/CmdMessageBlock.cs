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
using System.Data;
using System.Text.RegularExpressions;
using MCForge.SQL;
namespace MCForge.Commands
{
    public sealed class CmdMessageBlock : Command
    {
        public override string name { get { return "mb"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "build"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.AdvBuilder; } }
        public CmdMessageBlock() { }

        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }

            CatchPos cpos;
            cpos.message = "";

            try
            {
                switch (message.Split(' ')[0])
                {
                    case "air": cpos.type = Block.MsgAir; break;
                    case "water": cpos.type = Block.MsgWater; break;
                    case "lava": cpos.type = Block.MsgLava; break;
                    case "black": cpos.type = Block.MsgBlack; break;
                    case "white": cpos.type = Block.MsgWhite; break;
                    case "show": showMBs(p); return;
                    default: cpos.type = Block.MsgWhite; cpos.message = message; break;
                }
            }
            catch { cpos.type = Block.MsgWhite; cpos.message = message; }

            string current = "";
            string cmdparts = "";

            /*Fix by alecdent*/
            try
            {
                foreach (var com in Command.all.commands)
                {
                    if (com.type.Contains("mod"))
                    {
                        current = "/" + com.name;

                        cmdparts = message.Split(' ')[0].ToLower().ToString();
                        if (cmdparts[0] == '/')
                        {
                            if (current == cmdparts.ToLower())
                            {
                                p.SendMessage("You can't use that command in your messageblock!");
                                return;
                            }
                        }

                        cmdparts = message.Split(' ')[1].ToLower().ToString();
                        if (cmdparts[0] == '/')
                        {
                            if (current == cmdparts.ToLower())
                            {
                                p.SendMessage("You can't use that command in your messageblock!");
                                return;
                            }
                        }
                        if (com.shortcut != "")
                        {
                            current = "/" + com.name;

                            cmdparts = message.Split(' ')[0].ToLower().ToString();
                            if (cmdparts[0] == '/')
                            {
                                if (current == cmdparts.ToLower())
                                {
                                    p.SendMessage("You can't use that command in your messageblock!");
                                    return;
                                }
                            }

                            cmdparts = message.Split(' ')[1].ToLower().ToString();
                            if (cmdparts[0] == '/')
                            {
                                if (current == cmdparts.ToLower())
                                {
                                    p.SendMessage("You can't use that command in your messageblock!");
                                    return;
                                }
                            }
                        }
                    }
                }
            }
            catch { }

            if (cpos.message == "") cpos.message = message.Substring(message.IndexOf(' ') + 1);
            p.blockchangeObject = cpos;

            Player.SendMessage(p, "Place where you wish the message block to go."); p.ClearBlockchange();
            p.Blockchange += new Player.BlockchangeEventHandler(Blockchange1);
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/mb [block] [message] - Places a message in your next block.");
            Player.SendMessage(p, "Valid blocks: white, black, air, water, lava");
            Player.SendMessage(p, "/mb show shows or hides MBs");
        }

        public void Blockchange1(Player p, ushort x, ushort y, ushort z, byte type)
        {
            p.ClearBlockchange();
            CatchPos cpos = (CatchPos)p.blockchangeObject;

            cpos.message = cpos.message.Replace("'", "\\'");

            if (!Regex.IsMatch(cpos.message.ToLower(), @".*%([0-9]|[a-f]|[k-r])%([0-9]|[a-f]|[k-r])%([0-9]|[a-f]|[k-r])"))
            {
                if (Regex.IsMatch(cpos.message.ToLower(), @".*%([0-9]|[a-f]|[k-r])(.+?).*"))
                {
                    Regex rg = new Regex(@"%([0-9]|[a-f]|[k-r])(.+?)");
                    MatchCollection mc = rg.Matches(cpos.message.ToLower());
                    if (mc.Count > 0)
                    {
                        Match ma = mc[0];
                        GroupCollection gc = ma.Groups;
                        cpos.message.Replace("%" + gc[1].ToString().Substring(1), "&" + gc[1].ToString().Substring(1));
                    }
                }
            }
            //safe against SQL injections because no user input is given here
            DataTable Messages = Database.fillData("SELECT * FROM `Messages" + p.level.name + "` WHERE X=" + (int)x + " AND Y=" + (int)y + " AND Z=" + (int)z);
            Database.AddParams("@Message", cpos.message);
            if (Messages.Rows.Count == 0)
            {
                Database.executeQuery("INSERT INTO `Messages" + p.level.name + "` (X, Y, Z, Message) VALUES (" + (int)x + ", " + (int)y + ", " + (int)z + ", @Message)");
            }
            else
            {
                Database.executeQuery("UPDATE `Messages" + p.level.name + "` SET Message=@Message WHERE X=" + (int)x + " AND Y=" + (int)y + " AND Z=" + (int)z);
            }
            Messages.Dispose();
            Player.SendMessage(p, "Message block placed.");
            p.level.Blockchange(p, x, y, z, cpos.type);
            p.SendBlockchange(x, y, z, cpos.type);

            if (p.staticCommands) p.Blockchange += new Player.BlockchangeEventHandler(Blockchange1);
        }

        struct CatchPos { public string message; public byte type; }

        public void showMBs(Player p)
        {
            try
            {
                p.showMBs = !p.showMBs;
                //safe against SQL injections because no user input is given here
                using (DataTable Messages = Database.fillData("SELECT * FROM `Messages" + p.level.name + "`"))
                {
                    int i;

                    if (p.showMBs)
                    {
                        for (i = 0; i < Messages.Rows.Count; i++)
                            p.SendBlockchange(ushort.Parse(Messages.Rows[i]["X"].ToString()), ushort.Parse(Messages.Rows[i]["Y"].ToString()), ushort.Parse(Messages.Rows[i]["Z"].ToString()), Block.MsgWhite);
                        Player.SendMessage(p, "Now showing &a" + i.ToString() + Server.DefaultColor + " MBs.");
                    }
                    else
                    {
                        for (i = 0; i < Messages.Rows.Count; i++)
                            p.SendBlockchange(ushort.Parse(Messages.Rows[i]["X"].ToString()), ushort.Parse(Messages.Rows[i]["Y"].ToString()), ushort.Parse(Messages.Rows[i]["Z"].ToString()), p.level.GetTile(ushort.Parse(Messages.Rows[i]["X"].ToString()), ushort.Parse(Messages.Rows[i]["Y"].ToString()), ushort.Parse(Messages.Rows[i]["Z"].ToString())));
                        Player.SendMessage(p, "Now hiding MBs.");
                    }
                }
            }
            catch (Exception e)
            {
                Server.ErrorLog(e);
            }
        }
    }
}