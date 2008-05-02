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

namespace SharpSnmpLib
{
	/// <summary>
	/// Description of SnmpArray.
	/// </summary>
	public class SnmpArray: ISnmpData
	{
		public SnmpArray(params ISnmpData[] items)
		{
			foreach (ISnmpData item in items)
			{
				_list.Add(item);
			}
			_raw = ByteTool.ParseItems(items);
		}
		
		public SnmpArray(IEnumerable items)
		{
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
		
		public SnmpArray(byte[] raw)
		{
			_raw = raw;
			if (raw.Length != 0) {
				MemoryStream m = new MemoryStream(raw);
				while (m.Position < raw.Length)
				{
					_list.Add(SnmpDataFactory.CreateSnmpData(m));
				}
			}
		}
		
		byte[] _raw;
		
		IList<ISnmpData> _list = new List<ISnmpData>();
		
		public IList<ISnmpData> Items
		{
			get
			{
				return _list;
			}
		}
		
		public SnmpType TypeCode {
			get {
				return SnmpType.Array;
			}
		}
		
		byte[] _bytes;
		
		public byte[] ToBytes()
		{
			if (null == _bytes) {
				MemoryStream result = new MemoryStream();
				result.WriteByte((byte)TypeCode);
				ByteTool.WriteMultiByteLength(result, _raw.Length); //it seems that trap does not use this function
				result.Write(_raw,0,_raw.Length);
				_bytes = result.ToArray();
			}
			return _bytes;
		}
	}
}
