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
using System.Diagnostics;
namespace MCForge.Commands
{
    public sealed class CmdServerReport : Command
    {
        public override string name { get { return "serverreport"; } }
        public override string shortcut { get { return "sr"; } }
        public override string type { get { return "information"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Admin; } }
        public CmdServerReport() { }

        public override void Use(Player p, string message)
        {
            if (Server.PCCounter == null)
            {
                Player.SendMessage(p, "Starting PCCounter...one second");
                Server.PCCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                Server.PCCounter.BeginInit();
                Server.PCCounter.NextValue();
            }
            if (Server.ProcessCounter == null)
            {
                Player.SendMessage(p, "Starting ProcessCounter...one second");
                Server.ProcessCounter = new PerformanceCounter("Process", "% Processor Time", Process.GetCurrentProcess().ProcessName);
                Server.ProcessCounter.BeginInit();
                Server.ProcessCounter.NextValue();
            }

            TimeSpan tp = Process.GetCurrentProcess().TotalProcessorTime;
            TimeSpan up = (DateTime.Now - Process.GetCurrentProcess().StartTime);

            //To get actual CPU% is OS dependant
            string ProcessorUsage = "CPU Usage (Processes : All Processes):" + Server.ProcessCounter.NextValue() + " : " + Server.PCCounter.NextValue();
            //Alternative Average?
            //string ProcessorUsage = "CPU Usage is Not Implemented: So here is ProcessUsageTime/ProcessTotalTime:"+String.Format("00.00",(((tp.Ticks/up.Ticks))*100))+"%";
            //reports Private Bytes because it is what the process has reserved for itself and is unsharable
            string MemoryUsage = "Memory Usage: " + Math.Round((double)Process.GetCurrentProcess().PrivateMemorySize64 / 1048576).ToString() + " Megabytes";
            string Uptime = "Uptime: " + up.Days + " Days " + up.Hours + " Hours " + up.Minutes + " Minutes " + up.Seconds + " Seconds";
            string Threads = "Threads: " + Process.GetCurrentProcess().Threads.Count;
            Player.SendMessage(p, Uptime);
            Player.SendMessage(p, MemoryUsage);
            Player.SendMessage(p, ProcessorUsage);
            Player.SendMessage(p, Threads);

        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/serverreport - Get server CPU%, RAM usage, and uptime.");
        }
    }
}
