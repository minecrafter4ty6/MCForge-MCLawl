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
namespace MCForge
{
    public sealed class MinecraftBeat : IBeat
    {
        public string URL
        {
            get
            {
                return "https://minecraft.net/heartbeat.jsp";
            }
        }

        public bool Persistance
        {
            get { return true; }
        }

        public string Prepare()
        {
            return "&port=" + Server.port +
                "&max=" + Server.players +
                "&name=" + Heart.EncodeUrl(Server.name) +
                "&public=" + Server.pub +
                "&version=7" +
                "&salt=" + Server.salt +
                "&users=" + Player.players.Count;
        }


        public void OnResponse(string line)
        {

            // Only run the code below if we receive a response
            if (!String.IsNullOrEmpty(line.Trim()))
            {
                string newHash = line.Substring(line.LastIndexOf('/') + 1);

                // Run this code if we don't already have a hash or if the hash has changed
                if (String.IsNullOrEmpty(Server.Hash) || !newHash.Equals(Server.Hash))
                {
                    Server.Hash = newHash;
                    Server.URL = line;
                    Server.s.UpdateUrl(Server.URL);
                    File.WriteAllText("text/externalurl.txt", Server.URL);
                    Server.s.Log("URL found: " + Server.URL);
                }
            }
        }
    }
}
