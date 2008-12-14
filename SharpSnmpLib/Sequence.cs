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
        private IList<ISnmpData> _list = new List<ISnmpData>();
        
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

            foreach (ISnmpData item in items)
            {
                _list.Add(item);
            }
            
            _raw = ByteTool.ParseItems(items);
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

            if (!(items is IEnumerable<ISnmpData>))
            {
                throw new ArgumentException("objects must be IEnumerable<ISnmpData>");
            }
            
            foreach (ISnmpData item in items)
            {
                _list.Add(item);
            }
            
            _raw = ByteTool.ParseItems(items);
        }
        
        /// <summary>
        /// Creates an <see cref="Sequence"/> instance from raw bytes.
        /// </summary>
        /// <param name="raw">Raw bytes</param>
        public Sequence(byte[] raw)
        {
            if (raw == null)
            {
                throw new ArgumentNullException("raw");
            }

            _raw = raw;
            if (raw.Length != 0)
            {
                MemoryStream m = new MemoryStream(raw);
                while (m.Position < raw.Length)
                {
                    _list.Add(SnmpDataFactory.CreateSnmpData(m));
                }
            }
        }
        
        /// <summary>
        /// <see cref="ISnmpData"/> instances containing in this <see cref="Sequence"/>
        /// </summary>
        public IList<ISnmpData> Items
        {
            get
            {
                return _list;
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
            MemoryStream result = new MemoryStream();
            result.WriteByte((byte)TypeCode);
            ByteTool.WritePayloadLength(result, _raw.Length); // it seems that trap does not use this function
            result.Write(_raw, 0, _raw.Length);
            return result.ToArray();
        }
        
        /// <summary>
        /// Returns a <see cref="String"/> that represents this <see cref="Sequence"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder result = new StringBuilder("SNMP SEQUENCE: ");
            foreach (ISnmpData item in Items)
            {
                result.Append(item + "; ");
            }
            
            return result.ToString();
        }
    }
}
