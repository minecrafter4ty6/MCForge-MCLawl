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
    public static class VIP
    {
        public static readonly string file = "text/vips.txt"; //this file is never even created anywhere..

        public static void Add(string name)
        {
            if (!File.Exists(file)) return;
            name = name.Trim().ToLower();
            List<string> list = new List<string>(File.ReadAllLines(file));
            list.Add(name);
            File.WriteAllLines(file, list.ToArray());
        }
        /// <summary>
        /// Adds a Player to VIP's
        /// </summary>
        /// <param name="p">the Player to add</param>
        public static void Add(Player p)
        {
            Add(p.name);
        }

        public static void Remove(string name)
        {
            if (!File.Exists(file)) return;
            name = name.Trim().ToLower();
            List<string> list = new List<string>();
            foreach (string line in File.ReadAllLines(file))
                if (line.Trim().ToLower() != name)
                    list.Add(line);
            File.WriteAllLines(file, list.ToArray());
        }
        public static void Remove(Player p)
        {
            Remove(p.name);
        }

        public static bool Find(string name)
        {
            if (!File.Exists(file)) return false;
            name = name.Trim().ToLower();
            foreach (string line in File.ReadAllLines(file))
                if (line.Trim().ToLower() == name)
                    return true;
            return false;
        }
        public static bool Find(Player p)
        {
            return Find(p.name);
        }

        public static List<string> GetAll()
        {
            if (File.Exists(file))
                return new List<string>(File.ReadAllLines(file));
            return new List<string>();
        }
    }
}