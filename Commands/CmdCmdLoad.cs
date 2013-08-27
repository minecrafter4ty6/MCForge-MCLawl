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

namespace MCForge.Commands
{
    public sealed class CmdCmdLoad : Command
    {
        public override string name { get { return "cmdload"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Nobody; } }

        public override void Use(Player p, string message)
        {

            if (Command.all.Contains(message.Split(' ')[0]))
            {
                Player.SendMessage(p, "That command is already loaded!");
                return;
            }

            string[] param = message.Split(' ');
            string name = "Cmd" + param[0];


            if (param.Length == 1)
            {
                string error = Scripting.Load(name);
                if (error != null)
                {
                    Player.SendMessage(p, error);
                    return;
                }
                GrpCommands.fillRanks();
                Player.SendMessage(p, "Command was successfully loaded.");
                return;
            }
            if (param[1] == "vb")
            {

                string error = ScriptingVB.Load(name);
                if (error != null)
                {
                    Player.SendMessage(p, error);
                    return;
                }
                GrpCommands.fillRanks();
                Player.SendMessage(p, "Command was successfully loaded.");
                return;
            }
        }


        public override void Help(Player p)
        {
            Player.SendMessage(p, "/cmdload <command name> - Loads a C# command into the server for use.");
            Player.SendMessage(p, "/cmdload <command name> vb - Loads a Visual basic command into the server for use.");
        }
    }
}
