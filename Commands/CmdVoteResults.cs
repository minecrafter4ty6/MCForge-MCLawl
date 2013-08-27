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
    /// This is the command /voteresults
    /// use /help voteresults in-game for more info
    /// </summary>
    public sealed class CmdVoteResults : Command
    {
        public override string name { get { return "voteresults"; } }
        public override string shortcut { get { return "vr"; } }
        public override string type { get { return ""; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
        public CmdVoteResults() { }
        public override void Use(Player p, string message)
        {
            Player who = null;
            if (message == "") { who = p; message = p.name; } else { who = Player.Find(message); }
            if (Server.YesVotes >= 1 || Server.NoVotes >= 1)
            {
                p.SendMessage(c.green + "Y: " + Server.YesVotes + c.red + " N: " + Server.NoVotes);
                return;
            }
            else
            {
                p.SendMessage("There hasn't been a vote yet!");
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/voteresults - see the results of the last vote!");
        }
    }
}