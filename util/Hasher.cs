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
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MCForge.Util {
    internal sealed class PasswordHasher {

        const string FILE_LOCATION = "extra/passwords/{0}.dat";

        internal static byte[] Compute(string salt, string plainText) {
            if ( string.IsNullOrEmpty(salt) ) {
                throw new ArgumentNullException("salt", "fileName is null or empty");
            }

            if ( string.IsNullOrEmpty(plainText) ) {
                throw new ArgumentNullException("plainText", "plainText is null or empty");
            }

            salt = salt.Replace("<", "(");
            salt = salt.Replace(">", ")");
            plainText = plainText.Replace("<", "(");
            plainText = plainText.Replace(">", ")");

            MD5 hash = MD5.Create();

            byte[] textBuffer = Encoding.ASCII.GetBytes(plainText);
            byte[] saltBuffer = Encoding.ASCII.GetBytes(salt);

            byte[] hashedTextBuffer = hash.ComputeHash(textBuffer);
            byte[] hashedSaltBuffer = hash.ComputeHash(saltBuffer);
            return hash.ComputeHash(hashedSaltBuffer.Concat(hashedTextBuffer).ToArray());
        }

        internal static void StoreHash(string salt, string plainText) {

            byte[] doubleHashedSaltBuffer = Encoding.UTF8.GetBytes(Encoding.UTF8.GetString(Compute(salt, plainText)));

            if ( !File.Exists(string.Format(FILE_LOCATION, salt)) )
                using ( var disp = File.Create(string.Format(FILE_LOCATION, salt)) ) ;

            using ( var Writer = File.OpenWrite(string.Format(FILE_LOCATION, salt)) ) {
                Writer.Write(doubleHashedSaltBuffer, 0, doubleHashedSaltBuffer.Length);
            }

        }

        internal static bool MatchesPass(string salt, string plainText) {

            if ( !File.Exists(string.Format(FILE_LOCATION, salt)) )
                return false;

            string hashes = File.ReadAllText(string.Format(FILE_LOCATION, salt));

            if ( hashes.Equals(Encoding.UTF8.GetString(Compute(salt, plainText))) ) {
                return true;
            }


            return false;

        }
    }
}
