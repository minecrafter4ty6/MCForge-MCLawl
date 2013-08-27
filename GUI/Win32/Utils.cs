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
using System.Text;
using System.Drawing;

namespace MCForge.Gui.Utils {
    public class Utilities {

        /// <summary>
        /// Gets a color from a char.
        /// </summary>
        /// <param name="c">The char.</param>
        /// <returns>A color, that can be null</returns>
        public static Color? GetDimColorFromChar( char c ) {
            switch ( c ) {
                case '0': return Color.Black;
                case '1': return Color.FromArgb( 255, 0, 0, 161 );
                case '2': return Color.FromArgb( 255, 0, 161, 0 );
                case '3': return Color.FromArgb( 255, 0, 161, 161 );
                case '4': return Color.FromArgb( 255, 161, 0, 0 );
                case '5': return Color.FromArgb( 255, 161, 0, 161 );
                case '6': return Color.FromArgb( 255, 161, 161, 0 );
                case '7': return Color.FromArgb( 255, 161, 161, 161 );
                case '8': return Color.FromArgb( 255, 34, 34, 34 );
                case '9': return Color.FromArgb( 255, 34, 34, 225 );
                case 'a': return Color.FromArgb( 255, 34, 225, 34 );
                case 'b': return Color.FromArgb( 255, 34, 225, 225 );
                case 'c': return Color.FromArgb( 255, 225, 34, 34 );
                case 'd': return Color.FromArgb( 255, 225, 34, 225 );
                case 'e': return Color.FromArgb( 255, 225, 225, 34 );
                case 'f': return Color.Black;
                default: return null;
            }
        }

        public static int HexIntFromChar( char hexChar ) {
            hexChar = char.ToUpper( hexChar );  // may not be necessary

            return ( int ) hexChar < ( int ) 'A' ?
                ( ( int ) hexChar - ( int ) '0' ) :
                10 + ( ( int ) hexChar - ( int ) 'A' );
        }

        public static char HexCharFromInt( int hexInt ) {
            return char.Parse(hexInt.ToString());
        }

    }
}
