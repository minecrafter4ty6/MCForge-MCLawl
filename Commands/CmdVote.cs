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
    /// <summary>
    /// This is the command /vote
    /// </summary>
    public sealed class CmdVote : Command
    {
        public override string name { get { return "vote"; } }
        public override string shortcut { get { return "vo"; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdVote() { }

        public override void Use(Player p, string message)
        {
            if (message != "")
            {
                if (!Server.voting)
                {
                    string temp = message.Substring(0, 1) == "%" ? "" : Server.DefaultColor;
                    Server.voting = true;
                    Server.NoVotes = 0;
                    Server.YesVotes = 0;
                    Player.GlobalMessage(" " + c.green + "VOTE: " + temp + message + "(" + c.green + "Yes " + Server.DefaultColor + "/" + c.red + "No" + Server.DefaultColor + ")");
                    System.Threading.Thread.Sleep(15000);
                    Server.voting = false;
                    Player.GlobalMessage("The vote is in! " + c.green + "Y: " + Server.YesVotes + c.red + " N: " + Server.NoVotes);
                    Player.players.ForEach(delegate(Player winners)
                    {
                        winners.voted = false;
                    });
                }
                else
                {
                    p.SendMessage("A vote is in progress!");
                }
            }
            else
            {
                Help(p);
            }
        }
        public override void Help(Player p)
        {
            p.SendMessage("/vote [message] - Obviously starts a vote!");
        }
    }
}