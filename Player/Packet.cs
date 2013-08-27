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
using System.Net;
using System.Text;
namespace MCForge
{
    public sealed class Packet
    {
        public List<byte> totalData;
        Encoding encoding;
        bool LittleEndian;

        public Packet(Encoding e, bool LittleEndian)
        {
            totalData = new List<byte>();
            encoding = e;
            this.LittleEndian = LittleEndian;
        }

        public void addData(byte b)
        {
            totalData.Add(b);
        }
        public void addData(string b)
        {
            b = b.PadLeft(b.Length);
            byte[] array = encoding.GetBytes(b);
            addData((int)b.Length);
            for (int i = 0; i < array.Count(); i++)
                totalData.Add(array[i]);
        }
        public void addData(long b)
        {
            if (!LittleEndian)
                b = IPAddress.HostToNetworkOrder(b);
            byte[] array = BitConverter.GetBytes(b);
            for (int i = 0; i < array.Count(); i++)
                totalData.Add(array[i]);
        }
        public void addData(int b)
        {
            if (!LittleEndian)
                b = IPAddress.HostToNetworkOrder(b);
            byte[] array = BitConverter.GetBytes(b);
            for (int i = 0; i < array.Count(); i++)
                totalData.Add(array[i]);
        }
        public void addData(short b)
        {
            if (!LittleEndian)
                b = IPAddress.HostToNetworkOrder(b);
            byte[] array = BitConverter.GetBytes(b);
            for (int i = 0; i < array.Count(); i++)
                totalData.Add(array[i]);
        }
        public void addData(bool b)
        {
            totalData.Add((byte)(b ? 0x1 : 0x0));
        }
        public byte[] getData()
        {
            return totalData.ToArray();
        }

    }
}
