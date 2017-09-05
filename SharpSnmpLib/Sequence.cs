// SEQUENCE data type.
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

/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/4/30
 * Time: 20:53
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Array type.
    /// </summary>
    /// <remarks>Represents SMIv1 SEQUENCE.</remarks>
    public sealed class Sequence : ISnmpData
    {
        private readonly List<ISnmpData> _list = new List<ISnmpData>();
        private readonly byte[] _length;
        private byte[] _buffer;

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        /// <summary>
        /// Creates an <see cref="Sequence"/> instance with varied <see cref="ISnmpData"/> instances.
        /// </summary>
        /// <param name="length">The length bytes.</param>
        /// <param name="items">The items.</param>
        public Sequence(byte[] length, params ISnmpData[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            foreach (var data in items.Where(data => data != null))
            {
                _list.Add(data);
            }

            _length = length;
        }

        /// <summary>
        /// Creates an <see cref="Sequence"/> instance with varied <see cref="ISnmpData"/> instances.
        /// </summary>
        /// <param name="items"></param>
        public Sequence(IEnumerable<ISnmpData> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            foreach (var data in items.Where(data => data != null))
            {
                _list.Add(data);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sequence"/> class.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <param name="stream">The stream.</param>
        public Sequence(Tuple<int, byte[]> length, Stream stream)
        {
            if (length == null)
            {
                throw new ArgumentNullException(nameof(length));
            }

            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            _length = length.Item2;
            if (length.Item1 == 0)
            {
                return;
            }

            var original = stream.Position;
            while (stream.Position < original + length.Item1)
            {
                _list.Add(DataFactory.CreateSnmpData(stream));
            }
        }

        /// <summary>
        /// Item count in this <see cref="Sequence"/>.
        /// </summary>
        public int Length
        {
            get { return _list.Count; }
        }

        /// <summary>
        /// Gets the <see cref="Lextm.SharpSnmpLib.ISnmpData"/> at the specified index.
        /// </summary>
        /// <value></value>
        public ISnmpData this[int index]
        {
            get { return _list[index]; }
        }
        
        /// <summary>
        /// Type code.
        /// </summary>
        public SnmpType TypeCode
        {
            get { return SnmpType.Sequence; }
        }

        /// <summary>
        /// Appends the bytes to <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public void AppendBytesTo(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (_buffer == null)
            {
                _buffer = ByteTool.ParseItems(_list);
            }

            stream.AppendBytes(TypeCode, _length, _buffer);
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this <see cref="Sequence"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var result = new StringBuilder("SNMP SEQUENCE: ");
            foreach (var item in _list)
            {
                result.Append(item).Append("; ");
            }
            
            return result.ToString();
        }

        /// <summary>
        /// Gets the length bytes.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        public byte[] GetLengthBytes()
        {
            return _length;
        }
    }
}
