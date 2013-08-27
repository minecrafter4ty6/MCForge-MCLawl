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

using System.Collections.Generic;
using System.IO;

namespace MCForge
{
	/// <summary>
	/// Description of MojangAccount.
	/// </summary>
	public sealed class MojangAccount
	{
		static Dictionary<string, int> users = new Dictionary<string, int>();
		public static bool HasID(string truename) 
		{
			return GetID(truename) != -1;
		}
		
		public static int GetID(string truename)
		{
			if (users.ContainsKey(truename))
				return users[truename];
			return -1;
		}
		
		public static void AddUser(string truename) 
		{
			int i = users.Count;
			users.Add(truename, i);
			Save();
		}
		
		public static void Save() 
		{
			string[] lines = new string[users.Count];
			int i = 0; //because fuck forloops
			foreach (string s in users.Keys)
			{
				lines[i] = s + ":" + users[s];
				i++;
			}
			File.WriteAllLines("extra/mojang.dat", lines);
			lines = null;
		}
		
		public static void Load()
		{
			if (!File.Exists("extra/mojang.dat")) {
				File.Create("extra/mojang.dat");
				return;
			}
			string[] lines = File.ReadAllLines("extra/mojang.dat");
			foreach (string s in lines) 
			{
				int id = int.Parse(s.Split(':')[1]);
				string user = s.Split(':')[0];
				users.Add(user, id);
			}
			lines = null;
		}
	}
}
