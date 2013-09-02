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
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;

namespace MCFork
{
    public static class Permalink
    {
        public static Uri URL;
        public static string UniqueHash
        {
            get
            {
                return GenerateUniqueHash();
            }
        }

        private static string GenerateUniqueHash()
        {
            string macs = "";

            // get network interfaces' physical addresses
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface ni in interfaces)
            {
                PhysicalAddress pa = ni.GetPhysicalAddress();
                macs += pa.ToString();
            }

            // also add the server's current port, so that one machine may run multiple servers
            macs += Server.port.ToString();

            // generate hash
            using (var md5 = new MD5CryptoServiceProvider())
            {
                byte[] originalBytes = Encoding.ASCII.GetBytes(macs);
                byte[] hash = md5.ComputeHash(originalBytes);

                // convert hash to hex string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("X2"));
                }

                // the the final hash as a string
                return sb.ToString();
            }
        }
    }
}
