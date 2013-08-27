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
using MCForge.SQL;
namespace MCForge.Commands
{
    public sealed class CmdTopTen : Command
    {
        public override string name { get { return "topten"; } }
        public override string shortcut { get { return "10"; } }
        public override string type { get { return "information"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }
        public CmdTopTen() { }

        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }

            if (String.Compare(message, "1", true) == 0)
            {
                DataTable playerDb = Database.fillData("SELECT distinct name, totallogin FROM Players order by totallogin desc limit 10");

                Player.SendMessage(p, "TOP TEN NUMBER OF LOGINS:");
                for (int i = 0; i < playerDb.Rows.Count; i++)
                {
                    Player.SendMessage(p, (i + 1) + ") " + playerDb.Rows[i]["Name"] + " - [" + playerDb.Rows[i]["TotalLogin"] + "]");
                }

                playerDb.Dispose();
            }
            if (String.Compare(message, "2", true) == 0)
            {
                DataTable playerDb = Database.fillData("SELECT distinct name, totaldeaths FROM Players order by totaldeaths desc limit 10");

                Player.SendMessage(p, "TOP TEN NUMBER OF DEATHS:");
                for (int i = 0; i < playerDb.Rows.Count; i++)
                {
                    Player.SendMessage(p, (i + 1) + ") " + playerDb.Rows[i]["Name"] + " - [" + playerDb.Rows[i]["TotalDeaths"] + "]");
                }

                playerDb.Dispose();
            }
            if (String.Compare(message, "3", true) == 0)
            {
                DataTable playerDb = Database.fillData("SELECT distinct player, money FROM Economy order by money desc limit 10");

                Player.SendMessage(p, "TOP TEN AMOUNTS OF MONEY:");
                for (int i = 0; i < playerDb.Rows.Count; i++)
                {
                    Player.SendMessage(p, (i + 1) + ") " + playerDb.Rows[i]["player"] + " - [" + playerDb.Rows[i]["money"] + "]");
                }

                playerDb.Dispose();
            }
            if (String.Compare(message, "4", true) == 0)
            {
                DataTable playerDb = Database.fillData("SELECT distinct name, firstlogin FROM Players order by firstlogin asc limit 10");

                Player.SendMessage(p, "FIRST PLAYERS:");
                for (int i = 0; i < playerDb.Rows.Count; i++)
                {
                    Player.SendMessage(p, (i + 1) + ") " + playerDb.Rows[i]["Name"] + " - [" + playerDb.Rows[i]["firstlogin"] + "]");
                }

                playerDb.Dispose();
            }
            if (String.Compare(message, "5", true) == 0)
            {
                DataTable playerDb = Database.fillData("SELECT distinct name, lastlogin  FROM Players order by lastlogin desc limit 10");

                Player.SendMessage(p, "MOST RECENT PLAYERS:");
                for (int i = 0; i < playerDb.Rows.Count; i++)
                {
                    Player.SendMessage(p, (i + 1) + ") " + playerDb.Rows[i]["Name"] + " - [" + playerDb.Rows[i]["lastlogin"] + "]");
                }

                playerDb.Dispose();
            }
            if (String.Compare(message, "6", true) == 0)
            {
                DataTable playerDb = Database.fillData("SELECT distinct name, totalblocks FROM Players order by totalblocks desc limit 10");

                Player.SendMessage(p, "TOP TEN NUMBER OF BLOCKS MODIFIED:");
                for (int i = 0; i < playerDb.Rows.Count; i++)
                {
                    Player.SendMessage(p, (i + 1) + ") " + playerDb.Rows[i]["Name"] + " - [" + playerDb.Rows[i]["TotalBlocks"] + "]");
                }

                playerDb.Dispose();
            }
            if (String.Compare(message, "7", true) == 0)
            {
                DataTable playerDb = Database.fillData("SELECT distinct name, totalkicked FROM Players order by totalkicked desc limit 10");

                Player.SendMessage(p, "TOP TEN NUMBER OF KICKS:");
                for (int i = 0; i < playerDb.Rows.Count; i++)
                {
                    Player.SendMessage(p, (i + 1) + ") " + playerDb.Rows[i]["Name"] + " - [" + playerDb.Rows[i]["TotalKicked"] + "]");
                }

                playerDb.Dispose();
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "%2/topten [#] - Prints a particular top ten list");
            Player.SendMessage(p, "1) Number of Logins");
            Player.SendMessage(p, "2) Number of Deaths");
            Player.SendMessage(p, "3) Money");
            Player.SendMessage(p, "4) First Players");
            Player.SendMessage(p, "5) Recent Players");
            Player.SendMessage(p, "6) Blocks Modified");
            Player.SendMessage(p, "7) Number of Kicks");
        }
    }
}