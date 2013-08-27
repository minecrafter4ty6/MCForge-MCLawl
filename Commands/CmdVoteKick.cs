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
    public sealed class CmdVoteKick : Command
    {
        public override string name { get { return "votekick"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdVoteKick() { }

        public override void Use(Player p, string message)
        {
            if (p == null) { Player.SendMessage(p, "This command can only be used in-game!"); return; }
            if (message == "" || message.IndexOf(' ') != -1) { Help(p); return; }

            if (Server.voteKickInProgress == true) { p.SendMessage("Please wait for the current vote to finish!"); return; }

            Player who = Player.Find(message);
            if (who == null)
            {
                Player.SendMessage(p, "Could not find player specified!");
                return;
            }

            if (who.group.Permission >= p.group.Permission)
            {
                Player.GlobalChat(p, p.color + p.name + " " + Server.DefaultColor + "tried to votekick " + who.color + who.name + " " + Server.DefaultColor + "but failed!", false);
                return;
            }

            Player.GlobalMessageOps(p.color + p.name + Server.DefaultColor + " used &a/votekick");
            Player.GlobalMessage("&9A vote to kick " + who.color + who.name + " " + Server.DefaultColor + "has been called!");
            Player.GlobalMessage("&9Type &aY " + Server.DefaultColor + "or &cN " + Server.DefaultColor + "to vote.");

            // 1/3rd of the players must vote or nothing happens
            // Keep it at 0 to disable min number of votes
            Server.voteKickVotesNeeded = 3; //(int)(Player.players.Count / 3) + 1;
            Server.voteKickInProgress = true;

            System.Timers.Timer voteTimer = new System.Timers.Timer(30000);

            voteTimer.Elapsed += delegate
            {
                voteTimer.Stop();

                Server.voteKickInProgress = false;

                int votesYes = 0;
                int votesNo = 0;

                Player.players.ForEach(delegate(Player pl)
                {
                    // Tally the votes
                    if (pl.voteKickChoice == VoteKickChoice.Yes) votesYes++;
                    if (pl.voteKickChoice == VoteKickChoice.No) votesNo++;
                    // Reset their choice
                    pl.voteKickChoice = VoteKickChoice.HasntVoted;
                });

                int netVotesYes = votesYes - votesNo;

                // Should we also send this to players?
                Player.GlobalMessageOps("Vote Ended.  Results: &aY: " + votesYes + " &cN: " + votesNo);
                Server.s.Log("VoteKick results for " + who.name + ": " + votesYes + " yes and " + votesNo + " no votes.");

                if (votesYes + votesNo < Server.voteKickVotesNeeded)
                {
                    Player.GlobalMessage("Not enough votes were made. " + who.color + who.name + " " + Server.DefaultColor + "shall remain!");
                }
                else if (netVotesYes > 0)
                {
                    Player.GlobalMessage("The people have spoken, " + who.color + who.name + " " + Server.DefaultColor + "is gone!");
                    who.Kick("Vote-Kick: The people have spoken!");
                }
                else
                {
                    Player.GlobalMessage(who.color + who.name + " " + Server.DefaultColor + "shall remain!");
                }

                voteTimer.Dispose();
            };

            voteTimer.Start();
        }
        public override void Help(Player p)
        {
            p.SendMessage("/votekick <player> - Calls a 30sec vote to kick <player>");
        }
    }
}
