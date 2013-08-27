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
    public sealed class CmdCompile : Command
    {
        public override string name { get { return "compile"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Admin; } }
        public CmdCompile() { }

        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }
            bool success = false;
            string[] param = message.Split(' ');
            string name = param[0];

            if (param.Length == 1)
            {
                if (File.Exists("extra/commands/source/Cmd" + message + ".cs"))
                {
                    try
                    {
                        success = Scripting.Compile(message);
                    }
                    catch (Exception e)
                    {
                        Server.ErrorLog(e);
                        Player.SendMessage(p, "An exception was thrown during compilation.");
                        return;
                    }
                    if (success)
                    {
                        Player.SendMessage(p, "Compiled successfully.");
                    }
                    else
                    {
                        Player.SendMessage(p, "Compilation error.  Please check compile.log for more information.");
                    }
                    return;
                }
                else
                {
                    Player.SendMessage(p, "file &9Cmd" + message + ".cs " + Server.DefaultColor + "not found!");
                }
            }
            if (param[1] == "vb")
            {
                message = message.Remove(message.Length - 3, 3);
                if (File.Exists("extra/commands/source/Cmd" + message + ".vb"))
                {
                    try
                    {
                        success = ScriptingVB.Compile(name);
                    }
                    catch (Exception e)
                    {
                        Server.ErrorLog(e);
                        Player.SendMessage(p, "An exception was thrown during compilation.");
                        return;
                    }
                    if (success)
                    {
                        Player.SendMessage(p, "Compiled successfully.");
                    }
                    else
                    {
                        Player.SendMessage(p, "Compilation error.  Please check compile.log for more information.");
                    }
                    return;
                }
                else
                {
                    Player.SendMessage(p, "file &9Cmd" + message + ".vb " + Server.DefaultColor + "not found!");
                }
            }

        }

        public override void Help(Player p)
        {
            Player.SendMessage(p, "/compile <class name> - Compiles a command class file into a DLL.");
            Player.SendMessage(p, "/compile <class name> vb - Compiles a command class (that was written in visual basic) file into a DLL.");
            Player.SendMessage(p, "class name: &9Cmd&e<class name>&9.cs");
        }
    }
}
