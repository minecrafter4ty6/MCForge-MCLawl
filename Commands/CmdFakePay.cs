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
    public sealed class CmdFakePay : Command
    {
        public override string name { get { return "fakepay"; } }
        public override string shortcut { get { return "fpay"; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/fakepay <name> <amount> - Sends a fake give change message.");
        }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }
			
			var split = message.Split(' ');
			
            Player who = Player.Find(split[0]);
			if (who == null)
            {
                Player.SendMessage(p, Server.DefaultColor + "Player not found!");
                return;
            }
			
			int amount = 0;
			try
			{
	            amount = int.Parse(split[1]);
			}
			catch/* (Exception ex)*/
			{
				Player.SendMessage(p, "How much do you want to fakepay them?");
				return;
			}
			
            if (amount < 0)
            {
                Player.SendMessage(p, Server.DefaultColor + "You can't fakepay a negative amount.");
                return;
            }
			
            if (amount >= 16777215)
            {
                Player.SendMessage(p, "Whoa, that's too much money. Try less than 16777215.");
                return;
            }

            Player.GlobalMessage(who.color + who.prefix + who.name + Server.DefaultColor + " was given " + amount + " " + Server.moneys);

        }
    }
}

