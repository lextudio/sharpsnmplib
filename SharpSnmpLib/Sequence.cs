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
using System.Text;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Array type.
    /// </summary>
    /// <remarks>Represents SMIv1 SEQUENCE.</remarks>
    public class Sequence : ISnmpData
    {
        private byte[] _raw;
        private readonly List<ISnmpData> _list = new List<ISnmpData>();

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            foreach (ISnmpData data in _list)
            {
                yield return data;
            }
        }
        
        /// <summary>
        /// Creates an <see cref="Sequence"/> instance with varied <see cref="ISnmpData"/> instances.
        /// </summary>
        /// <param name="items"></param>
        public Sequence(params ISnmpData[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            foreach (ISnmpData data in items)
            {
                if (data != null)
                {
                    _list.Add(data);
                }
            }

            ////_raw = ByteTool.ParseItems(items);
        }
        
        /// <summary>
        /// Creates an <see cref="Sequence"/> instance with varied <see cref="ISnmpData"/> instances.
        /// </summary>
        /// <param name="items"></param>
        public Sequence(IEnumerable items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            IEnumerable<ISnmpData> list = items as IEnumerable<ISnmpData>;
            if (list == null)
            {
                throw new ArgumentException("objects must be IEnumerable<ISnmpData>");
            }

            foreach (ISnmpData data in items)
            {
                if (data != null)
                {
                    _list.Add(data);
                }
            }
            
            ////_raw = ByteTool.ParseItems(items);
        }
        
        /// <summary>
        /// Creates an <see cref="Sequence"/> instance from raw bytes.
        /// </summary>
        /// <param name="raw">Raw bytes</param>
        internal Sequence(byte[] raw) : this(raw.Length, new MemoryStream(raw))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sequence"/> class.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <param name="stream">The stream.</param>
        public Sequence(int length, Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            long original = stream.Position;
            if (length != 0)
            {
                while (stream.Position < original + length)
                {
                    _list.Add(DataFactory.CreateSnmpData(stream));
                }
            }

            ////_raw = ByteTool.ParseItems(_list);
            ////Debug.Assert(length >= _raw.Length, "length not match");
        }

        /// <summary>
        /// <see cref="ISnmpData"/> instances containing in this <see cref="Sequence"/>
        /// </summary>
        [Obsolete("Use indexer directly.")]
        public IList<ISnmpData> Items
        {
            get { return _list; }
        }

        /// <summary>
        /// Data count in this <see cref="Sequence"/>.
        /// </summary>
        public int Count
        {
            get { return _list.Count; }
        }

        /// <summary>
        /// Gets the <see cref="Lextm.SharpSnmpLib.ISnmpData"/> at the specified index.
        /// </summary>
        /// <value></value>
        public ISnmpData this[int index]
        {
            get
            {
                return _list[index];
            }
        }
        
        /// <summary>
        /// Type code.
        /// </summary>
        public SnmpType TypeCode
        {
            get
            {
                return SnmpType.Sequence;
            }
        }
        
        /// <summary>
        /// To byte format.
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            using (MemoryStream result = new MemoryStream())
            {
                AppendBytesTo(result);
                return result.ToArray();
            }
        }

        /// <summary>
        /// Appends the bytes to <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public void AppendBytesTo(Stream stream)
        {
            if (_raw == null)
            {
                _raw = ByteTool.ParseItems(_list);
            }

            ByteTool.AppendBytes(stream, TypeCode, _raw);
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this <see cref="Sequence"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder result = new StringBuilder("SNMP SEQUENCE: ");
            foreach (ISnmpData item in _list)
            {
                result.Append(item).Append("; ");
            }
            
            return result.ToString();
        }
    }
}
