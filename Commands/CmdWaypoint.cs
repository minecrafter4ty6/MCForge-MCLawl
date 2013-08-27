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
    public sealed class CmdWaypoint : Command
    {
        public override string name { get { return "waypoint"; } }
        public override string shortcut { get { return "wp"; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Builder; } }
        public CmdWaypoint() { }

        public override void Use(Player p, string message)
        {
            if (p == null) { Player.SendMessage(p, "This command can only be used in-game"); return; }
            string[] command = message.ToLower().Split(' ');
            string par0 = String.Empty;
            string par1 = String.Empty;
            try
            {
                par0 = command[0];
                par1 = command[1];
            }
            catch { }
            if (par0.ToLower() == "create" || par0.ToLower() == "new" || par0.ToLower() == "add")
            {
                if (!Player.Waypoint.Exists(par1, p))
                {
                    Player.Waypoint.Create(par1, p);
                    Player.SendMessage(p, "Created waypoint");
                    return;
                }
                else { Player.SendMessage(p, "That waypoint already exists"); return; }
            }
            else if (par0.ToLower() == "goto")
            {
                if (Player.Waypoint.Exists(par1, p))
                {
                    Player.Waypoint.Goto(par1, p);
                    return;
                }
                else { Player.SendMessage(p, "That waypoint doesn't exist"); return; }
            }
            else if (par0.ToLower() == "replace" || par0.ToLower() == "update" || par0.ToLower() == "edit")
            {
                if (Player.Waypoint.Exists(par1, p))
                {
                    Player.Waypoint.Update(par1, p);
                    Player.SendMessage(p, "Updated waypoint");
                    return;
                }
                else { Player.SendMessage(p, "That waypoint doesn't exist"); return; }
            }
            else if (par0.ToLower() == "delete" || par0.ToLower() == "remove")
            {
                if (Player.Waypoint.Exists(par1, p))
                {
                    Player.Waypoint.Remove(par1, p);
                    Player.SendMessage(p, "Deleted waypoint");
                    return;
                }
                else { Player.SendMessage(p, "That waypoint doesn't exist"); return; }
            }
            else if (par0.ToLower() == "list")
            {
                Player.SendMessage(p, "Waypoints:");
                foreach(Player.Waypoint.WP wp in p.Waypoints)
                {
                    if (Level.Find(wp.lvlname) != null)
                    {
                        Player.SendMessage(p, wp.name + ":" + wp.lvlname);
                    }
                }
                return;
            }
            else
            {
                if (Player.Waypoint.Exists(par0, p))
                {
                    Player.Waypoint.Goto(par0, p);
                    Player.SendMessage(p, "Sent you to waypoint");
                    return;
                }
                else { Player.SendMessage(p, "That waypoint or command doesn't exist"); return; }
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/waypoint [create] [name] - Create a new waypoint");
            Player.SendMessage(p, "/waypoint [update] [name] - Update a waypoint");
            Player.SendMessage(p, "/waypoint [remove] [name] - Remove a waypoint");
            Player.SendMessage(p, "/waypoint [list] - Shows a list of waypoints");
            Player.SendMessage(p, "/waypoint [goto] [name] - Goto a waypoint");
            Player.SendMessage(p, "/waypoint [name] - Goto a waypoint");
        }
    }
}