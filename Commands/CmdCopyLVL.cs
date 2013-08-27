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
    public class CmdCopyLVL : Command
    {
        public override string name { get { return "copylvl"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "build"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdCopyLVL() { }

        public override void Use(Player p, string message)
        {
            string msg1 = String.Empty;
            string msg2 = String.Empty;
            try
            {
                msg1 = message.Split(' ')[0];
                msg2 = message.Split(' ')[1];
            }
            catch { }
              
            if (!p.group.CanExecute(Command.all.Find("newlvl")))
            {
                Player.SendMessage(p, "You cannot use this command, unless you can use /newlvl!");
                return;
            }
            int number = message.Split(' ').Length;
            if (message == "")
            {
                Help(p);
                return;
            }
            if (number < 2)
            {
                Player.SendMessage(p, "You did not specify the level it would be copied to as!");
                return;
            }
            try {
                File.Copy("levels/" + msg1 + ".lvl", "levels/" + msg2 + ".lvl");
                File.Copy("levels/level properties/" + msg1 + ".properties", "levels/level properties/" + msg1 + ".properties", false);

            } catch (System.IO.FileNotFoundException) {
                Player.SendMessage(p, msg2 + " does not exist!");
                return;

            } catch (System.IO.IOException) {
                Player.SendMessage(p, "The level, &c" + msg2 + " &e already exists!");
                return;

            } catch (System.ArgumentException) {
                Player.SendMessage(p, "One or both level names are either invalid, or corrupt.");
                return;

            }
            Player.SendMessage(p, "The level, &c" + msg1 + " &ehas been copied to &c" + msg2 + "!");
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/copylvl [Level] [Copied Level] - Makes a copy of [Level] called [Copied Level].");
        }
    }
}