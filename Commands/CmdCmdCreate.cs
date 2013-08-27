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
    public sealed class CmdCmdCreate : Command
    {
        public override string name { get { return "cmdcreate"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Admin; } }
        public CmdCmdCreate() { }

        public override void Use(Player p, string message)
        {

            if (message == "")
            {
                Help(p);
                return;
            }
            string[] name = message.Split(' ');


            if (name.Length == 1)
            {
                if (File.Exists("extra/commands/source/Cmd" + message + ".cs")) { p.SendMessage("File Cmd" + message + ".cs already exists.  Choose another name."); return; }
                try
                {
                    Scripting.CreateNew(message);
                }
                catch (Exception e)
                {
                    Server.ErrorLog(e);
                    Player.SendMessage(p, "An error occurred creating the class file.");
                    return;
                }
                Player.SendMessage(p, "Successfully created a new command class.");
                return;
            }

            if (name[1] == "vb")
            {
                if (File.Exists("extra/commands/source/Cmd" + name[0] + ".vb")) { p.SendMessage("File Cmd" + name[0] + ".vb already exists.  Choose another name."); return; }
                try
                {
                    ScriptingVB.CreateNew(name[0]);
                }
                catch (Exception e)
                {
                    Server.ErrorLog(e);
                    Player.SendMessage(p, "An error occurred creating the class file.");
                    return;
                }
                Player.SendMessage(p, "Successfully created a new vb command class.");
                return;
            }
            // else if (name.Length > 2) { Help(p); return; }

        }

        public override void Help(Player p)
        {
            Player.SendMessage(p, "/cmdcreate <message> - Creates a dummy command class named Cmd<Message> from which you can make a new command.");
            Player.SendMessage(p, "Or use \"/cmdcreate <name of command> vb\" to create a dummy class in visual basic");
        }
    }
}
