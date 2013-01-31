// Message parameters class.
// Copyright (C) 2008-2010 Malcolm Crowe, Lex Li, and other contributors.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

using System;
using System.Net;

namespace Lextm.SharpSnmpLib.Messaging
{
    internal sealed class MessageParams
    {
        private readonly byte[] _bytes;

        public MessageParams(byte[] bytes, int number, EndPoint sender)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }
            
            if (sender == null)
            {
                throw new ArgumentNullException("sender");
            }
            
            _bytes = bytes;
            Number = number;
            Sender = (IPEndPoint)sender;
        }

        public byte[] GetBytes()
        {
           return _bytes;
        }

        public IPEndPoint Sender { get; private set; }

        public int Number { get; private set; }
    }
}