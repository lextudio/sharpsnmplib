// Salt generator.
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
using System.Linq;

namespace Lextm.SharpSnmpLib.Security
{
    /// <summary>
    /// Salt generator.
    /// </summary>
    public sealed class SaltGenerator
    {
        private readonly object _root = new object();
        private long _salt = Convert.ToInt64(new Random().Next(1, int.MaxValue));
        
        internal void SetSalt(long salt)
        {
            _salt = salt;
        }

        /// <summary>
        /// Get next salt <see cref="Int64"/> value. Used internally to encrypt data.
        /// </summary>
        /// <returns>Random <see cref="Int64"/> value</returns>
        internal long NextSalt
        {
            get
            {
                lock (_root)
                {
                    if (_salt == long.MaxValue)
                    {
                        _salt = 1;
                    }
                    else
                    {
                        _salt++;
                    }

                    return _salt;
                }
            }
        }

        /// <summary>
        /// Gets salt bytes.
        /// </summary>
        /// <returns></returns>
        public byte[] GetSaltBytes()
        {
            return BitConverter.GetBytes(NextSalt).Reverse().ToArray();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "Salt generator";
        }
    }
}
