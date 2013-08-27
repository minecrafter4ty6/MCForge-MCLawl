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

namespace MCForge.Utils.Crypto {
    /// <summary>
    /// Interface for Simple Crypto Service
    /// </summary>
    public interface ICryptoService {
        /// <summary>
        /// Gets or sets the number of iterations the hash will go through
        /// </summary>
        int HashIterations { get; set; }

        /// <summary>
        /// Gets or sets the size of salt that will be generated if no Salt was set
        /// </summary>
        int SaltSize { get; set; }

        /// <summary>
        /// Gets or sets the plain text to be hashed
        /// </summary>
        string PlainText { get; set; }

        /// <summary>
        /// Gets the base 64 encoded string of the hashed PlainText
        /// </summary>
        string HashedText { get; }

        /// <summary>
        /// Gets or sets the salt that will be used in computing the HashedText
        /// </summary>
        string Salt { get; }

        /// <summary>
        /// Compute the hash
        /// </summary>
        /// <returns>the computed hash: HashedText</returns>
        string Compute();

        /// <summary>
        /// Compute the hash using default generated salt
        /// </summary>
        /// <param name="textToHash"></param>
        /// <returns></returns>
        string Compute(string textToHash);

        /// <summary>
        /// Compute the hash that will also generate a salt from parameters
        /// </summary>
        /// <param name="textToHash">The text to be hashed</param>
        /// <param name="saltSize">The size of the salt to be generated</param>
        /// <param name="hashIterations"></param>
        /// <returns>the computed hash: HashedText</returns>
        string Compute(string textToHash, int saltSize, int hashIterations);

        /// <summary>
        /// Compute the hash that will utilize the passed salt
        /// </summary>
        /// <param name="textToHash">The text to be hashed</param>
        /// <param name="salt">The salt to be used in the computation</param>
        /// <returns>the computed hash: HashedText</returns>
        string Compute(string textToHash, string salt);

        /// <summary>
        /// Get the time in milliseconds it takes to complete the hash for the iterations
        /// </summary>
        /// <param name="iteration"></param>
        /// <returns></returns>
        int GetElapsedTimeForIteration(int iteration);
    }
}