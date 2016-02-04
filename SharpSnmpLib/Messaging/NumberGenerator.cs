// PDU counter class.
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

namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// A counter that generates ID.
    /// </summary>
    /// <remarks>The request ID is used to identifier sessions.</remarks>
    public sealed class NumberGenerator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NumberGenerator"/> class.
        /// </summary>
        /// <param name="min">The min.</param>
        /// <param name="max">The max.</param>
        public NumberGenerator(int min, int max)
        {
            _min = min;
            _max = max;
            _salt = new Random().Next(_min, _max);
        }

        /// <summary>
        /// Returns next ID. This method is thread-safe within this instance of NumberGenerator.
        /// </summary>
        public int NextId
        {
            get
            {
                lock (_root)
                {
                    if (_salt == _max)
                    {
                        _salt = _min;
                    }
                    else
                    {
                        _salt++;
                    }
                    return _salt;
                }
            }
        }

        private readonly object _root = new object();
        private int _salt;
        private readonly int _min;
        private readonly int _max;

        internal void SetSalt(int value)
        {
            _salt = value;
        }
    }
}
