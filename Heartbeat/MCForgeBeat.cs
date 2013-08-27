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
using System.Collections.Generic;
using System.Linq;
namespace MCForge
{
    internal sealed class MCForgeBeat : IBeat
    {
        public string URL { get { return ServerSettings.HeartbeatAnnounce; } }

        public bool Persistance
        {
            get { return true; }
        }

        public string Prepare()
        {

            string Parameters = "name=" + Heart.EncodeUrl(Server.name) +
                                                     "&users=" + Player.players.Count +
                                                     "&max=" + Server.players +
                                                     "&port=" + Server.port +
                                                     "&version=" + Server.Version +
                                                     "&gcname=" + Heart.EncodeUrl(Server.UseGlobalChat ? Server.GlobalChatNick : "[Disabled]") +
                                                     "&public=" + (Server.pub ? "1" : "0") +
                                                     "&motd=" + Heart.EncodeUrl(Server.motd);

            if (Server.levels != null && Server.levels.Count > 0)
            {
                IEnumerable<string> worlds = from l in Server.levels select l.name;
                Parameters += "&worlds=" + String.Join(", ", worlds.ToArray());
            }
            Parameters += "&hash=" + Server.Hash;

            return Parameters;
        }

        public void OnResponse(string line)
        {
            //Do nothing
        }

    }
}
