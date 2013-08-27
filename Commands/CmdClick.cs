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
    public sealed class CmdClick : Command
    {
        public override string name { get { return "click"; } }
        public override string shortcut { get { return "x"; } }
        public override string type { get { return "build"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }
        public CmdClick() { }

        public override void Use(Player p, string message)
        {
            if (p == null)
            {
                Player.SendMessage(p, "This command can only be used in-game");
                return;
            }
            string[] parameters = message.Split(' ');
            ushort[] click = p.lastClick;

            if (message.IndexOf(' ') != -1)
            {
                if (parameters.Length != 3)
                {
                    Help(p);
                    return;
                }
                else
                {
                    for (int value = 0; value < 3; value++)
                    {
                        if (parameters[value].ToLower() == "x" || parameters[value].ToLower() == "y" || parameters[value].ToLower() == "z")
                            click[value] = p.lastClick[value];
                        else if (isValid(parameters[value], value, p))
                            click[value] = ushort.Parse(parameters[value]);
                        else
                        {
                            Player.SendMessage(p, "\"" + parameters[value] + "\" was not valid");
                            return;
                        }
                    }
                }
            }

            p.lastCMD = "click";
            p.manualChange(click[0], click[1], click[2], 0, Block.rock);
            Player.SendMessage(p, "Clicked &b(" + click[0] + ", " + click[1] + ", " + click[2] + ")");
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/click [x y z] - Fakes a click");
            Player.SendMessage(p, "If no xyz is given, it uses the last place clicked");
            Player.SendMessage(p, "/click 200 y 200 will cause it to click at 200x, last y and 200z");
        }

        private bool isValid(string message, int dimension, Player p)
        {
            ushort testValue;
            try {
                testValue = ushort.Parse(message);
            } catch { return false; }

            if (testValue < 0)
                return false;

            if (testValue >= p.level.width && dimension == 0) return false;
            else if (testValue >= p.level.depth && dimension == 1) return false;
            else if (testValue >= p.level.height && dimension == 2) return false;

            return true;
        }
    }
}