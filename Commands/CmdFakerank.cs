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
   public sealed class CmdFakeRank : Command
   {
      public override string name { get { return "fakerank"; } }
      public override string shortcut { get { return "frk"; } } 
      public override string type { get { return "other"; } }
      public override bool museumUsable { get { return true; } }
      public override void Help(Player p)
      {
         Player.SendMessage(p, "/fakerank <name> <rank> - Sends a fake rank change message.");
      }
      public override LevelPermission defaultRank { get { return LevelPermission.Admin; } }
      public override void Use(Player p, string message)
      {
	  
         if (message == ""){Help(p); return;}
		 
         Player plr = Player.Find(message.Split (' ')[0]);
         Group grp = Group.Find(message.Split (' ')[1]);
		 
         if (plr == null)
         {
            Player.SendMessage(p, Server.DefaultColor + "Player not found!");
            return;
         }
         if (grp == null)
         {
             Player.SendMessage(p, Server.DefaultColor + "No rank entered.");
             return;
         }
         if (Group.GroupList.Contains(grp))
         {
             
             if (grp.name == "banned")
             {
                 Player.GlobalMessage(plr.color + plr.name + Server.DefaultColor + " is now &8banned" + Server.DefaultColor + "!");
             }
             else
             {
                 Player.GlobalMessage(plr.color + plr.name + Server.DefaultColor + "'s rank was set to " + grp.color + grp.name + Server.DefaultColor + ".");
                 Player.GlobalMessage("&6Congratulations!");
             }
         }
   
         else
         {
             Player.SendMessage(p, Server.DefaultColor + "Invalid Rank Entered!");
             return;
         }

      }
   }
}
