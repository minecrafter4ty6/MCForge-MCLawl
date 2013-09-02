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
using System.IO;
using System.Net;

namespace MCFork
{
	public sealed class OmniBan
	{
		public List<string> bans;
		public string kickMsg;


		public OmniBan()
		{
			bans = new List<string>();
			kickMsg = "You are Omnibanned!
		}

		public void Load(bool web)
		{
			if (web)
			{
				try
				{
					string data = "";
					try
					{
						using (WebClient WC = new WebClient())
							data = WC.DownloadString("http://mcforge.net/serverdata/omnibans.txt").ToLower();
					}
					catch { Load(false); return; }

					bans.Clear();
					bans.AddRange(data.Split(';'));
					Save();
				}
				catch (Exception e) { Server.ErrorLog(e); }
			}
			else
			{
				if (!File.Exists("text/omniban.txt")) return;

				try
				{
					foreach (string line in File.ReadAllLines("text/omniban.txt"))
						if (!String.IsNullOrEmpty(line)) bans.Add(line.ToLower());
				}
				catch (Exception e) { Server.ErrorLog(e); }
			}
		}

		public void Save()
		{
			try
			{
				File.Create("text/omniban.txt").Dispose();
				using (StreamWriter SW = File.CreateText("text/omniban.txt"))
					foreach (string ban in bans)
						SW.WriteLine(ban);
			}
			catch (Exception e) { Server.ErrorLog(e); }
		}

		public void KickAll()
		{
			try
			{
				kickall:
				foreach (Player p in Player.players)
					if (CheckPlayer(p))
					{
						p.Kick(kickMsg);
						goto kickall;
					}
			}
			catch (Exception e) { Server.ErrorLog(e); }
		}

		public bool CheckPlayer(Player p)
		{
			return p.name.ToLower() != Server.server_owner.ToLower() && !Player.IPInPrivateRange(p.ip) && (bans.Contains(p.name.ToLower()) || bans.Contains(p.ip)) && !Server.IgnoreOmnibans;
		}
	}
}
