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
using System.Linq;
namespace MCForge.Commands
{
    public sealed class CmdChangeLog : Command
    {
        public override string name { get { return "changelog"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
        public override void Use(Player p, string message)
        {
            if (!File.Exists("changelog.txt"))
            {
                Player.SendMessage(p, "Unable to find changelog");
                return;
            }

            // Read the changelog but stop reading if it encounters a blank line
            // This is done so that a player will only see the latest changes even if multiple version info exists in the changelog
            // Because of this, its really important that blank lines are ONLY used to separate different versions
            string[] strArray = File.ReadAllLines("changelog.txt").TakeWhile(s => !String.IsNullOrEmpty(s.Trim())).ToArray();
            if (message == "")
            {
                for (int j = 0; j < strArray.Length; j++)
                {
                    Player.SendMessage(p, strArray[j]);
                }
            }
            else
            {
                string[] split = message.Split(' ');
                if(split.Length != 1)
                {
                    Help(p);
                    return;
                }

                if (split[0] == "all")
                {
                    if ((int)p.group.Permission < CommandOtherPerms.GetPerm(this))
                    {
                        Player.SendMessage(p, "You must be at least " + Group.findPermInt(CommandOtherPerms.GetPerm(this)).name + " to send the changelog to all players.");
                        return;
                    }
                    for (int k = 0; k < strArray.Length; k++)
                    {
                        Player.GlobalMessage(strArray[k]);
                    }
                    
                    return;
                }
                else
                {
                    Player player = Player.Find(split[0]);
                    if (player == null)
                    {
                        Player.SendMessage(p, "Could not find player \"" + split[0] + "\"!");
                        return;
                    }

                    Player.SendMessage(player, "Changelog:");

                    for (int l = 0; l < strArray.Length; l++)
                    {
                        Player.SendMessage(player, strArray[l]);
                    }
                    
                    Player.SendMessage(p, "The Changelog was successfully sent to " + player.name + ".");
                    
                    return;
                }
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/changelog - View the most recent changelog!!");
            Player.SendMessage(p, "/changelog <player> - Sends the most recent changelog to <player>!!");
            Player.SendMessage(p, "/changelog all - Sends the most recent changelog to everyone!!");
        }
    }
}