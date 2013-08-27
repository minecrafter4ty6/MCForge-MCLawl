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
using System.Threading;
namespace MCForge.Commands
{
    public sealed class CmdShutdown : Command
    {
        public override string name { get { return "shutdown"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Admin; } }
        public override void Help(Player p) { Player.SendMessage(p, "/shutdown [time] [message] - Shuts the server down"); }
        public override void Use(Player p, string message)
        {
            int secTime = 10;
            bool shutdown = true;
            string file = "stopShutdown";
            if (File.Exists(file)) { File.Delete(file); }
            if (message == "") { message = "Server is going to shutdown in " + secTime + " seconds"; }
            else
            {
                if (message == "cancel") { File.Create(file).Close(); shutdown = false; message = "Shutdown cancelled"; }
                else
                {
                    if (!message.StartsWith("0"))
                    {
                        string[] split = message.Split(' ');
                        bool isNumber = false;
                        try { secTime = Convert.ToInt32(split[0]); isNumber = true; }
                        catch { secTime = 10; isNumber = false; }
                        if (split.Length > 1) { if (isNumber) { message = message.Substring(1 + split[0].Length); } }
                    }
                    else { Player.SendMessage(p, "Countdown time cannot be zero"); return; }
                }
            }
            if (shutdown)
            {
                Player.GlobalMessage("%4" + message);
                Server.s.Log(message);
                for (int t = secTime; t > 0; t = t - 1)
                {
                    if (!File.Exists(file)) { Player.GlobalMessage("%4Server shutdown in " + t + " seconds"); Server.s.Log("Server shutdown in " + t + " seconds"); Thread.Sleep(1000); }
                    else { File.Delete(file); Player.GlobalMessage("Shutdown cancelled"); Server.s.Log("Shutdown cancelled"); return; }
                }
                if (!File.Exists(file)) { MCForge_.Gui.Program.ExitProgram(false); return; }
                else { File.Delete(file); Player.GlobalMessage("Shutdown cancelled"); Server.s.Log("Shutdown cancelled"); return; }
            }
            return;
        }
    }
}