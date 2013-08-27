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
    public sealed class CmdCmdSet : Command
    {
        public override string name { get { return "cmdset"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Admin; } }
        public CmdCmdSet() { }

        public override void Use(Player p, string message)
        {
            if (message == "" || message.IndexOf(' ') == -1) { Help(p); return; }

            string foundBlah = Command.all.FindShort(message.Split(' ')[0]);

            Command foundCmd;
            if (foundBlah == "") foundCmd = Command.all.Find(message.Split(' ')[0]);
            else foundCmd = Command.all.Find(foundBlah);

            if (foundCmd == null) { Player.SendMessage(p, "Could not find command entered"); return; }
            if (p != null && !p.group.CanExecute(foundCmd)) { Player.SendMessage(p, "This command is higher than your rank."); return; }

            LevelPermission newPerm = Level.PermissionFromName(message.Split(' ')[1]);
            if (newPerm == LevelPermission.Null) { Player.SendMessage(p, "Could not find rank specified"); return; }
            if (p != null && newPerm > p.group.Permission) { Player.SendMessage(p, "Cannot set to a rank higher than yourself."); return; }

            GrpCommands.rankAllowance newCmd = GrpCommands.allowedCommands.Find(rA => rA.commandName == foundCmd.name);
            newCmd.lowestRank = newPerm;
            GrpCommands.allowedCommands[GrpCommands.allowedCommands.FindIndex(rA => rA.commandName == foundCmd.name)] = newCmd;

            GrpCommands.Save(GrpCommands.allowedCommands);
            GrpCommands.fillRanks();
            Player.GlobalMessage("&d" + foundCmd.name + Server.DefaultColor + "'s permission was changed to " + Level.PermissionToName(newPerm));
            //if (p == null) ; // this is useless?
            //{
                Player.SendMessage(p, foundCmd.name + "'s permission was changed to " + Level.PermissionToName(newPerm));
                return;
            //}
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/cmdset [cmd] [rank] - Changes [cmd] rank to [rank]");
            Player.SendMessage(p, "Only commands you can use can be modified");
            Player.SendMessage(p, "Available ranks: " + Group.concatList());
        }
    }
}