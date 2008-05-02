/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/4/25
 * Time: 20:30
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using System.IO;

namespace SharpSnmpLib
{
	public class Variable
	{
		ObjectIdentifier _oid;
		ISnmpData _data;

		public Variable(ObjectIdentifier oid)
		{
			_oid = oid;
			_data = new Null();
		}
		
		public Variable(ObjectIdentifier oid, ISnmpData data)
		{
			_oid = oid;
			_data = data;
		}
		
		public ObjectIdentifier Id
		{
			get
			{
				return _oid;
			}
		}
		
		public ISnmpData Data
		{
			get
			{
				return _data;
			}
		}
		
        /// <summary>
        /// Converts varbind section to variable binds list.
        /// </summary>
        /// <param name="varbindSection"></param>
        /// <returns></returns>
		internal static IList<Variable> ConvertFrom(SnmpArray varbindSection)
		{
			IList<Variable> result = new List<Variable>(varbindSection.Items.Count);
			foreach (ISnmpData item in varbindSection.Items)
			{
				if (item.TypeCode != SnmpType.Array)
				{
					throw new ArgumentException("wrong varbind section data");
				}
				SnmpArray varbind = item as SnmpArray;
				if (null != varbind)
				{
					if (varbind.Items.Count != 2 || varbind.Items[0].TypeCode != SnmpType.ObjectIdentifier)
					{
						throw new ArgumentException("wrong varbind data");
					}
					result.Add(new Variable((ObjectIdentifier)varbind.Items[0], varbind.Items[1]));
				}
				else
				{
					Variable v = item as Variable;
					if (null != v) {
						result.Add(v);
					}
				}
			}
			return result;
		}
		/// <summary>
		/// Converts variable binds to varbind section.
		/// </summary>
		/// <param name="variables"></param>
		/// <returns></returns>
        internal static SnmpArray ConvertTo(IList<Variable> variables)
        {
        	IList<ISnmpData> varbinds = new List<ISnmpData>(2*variables.Count);
        	foreach (Variable v in variables)
        	{
        		varbinds.Add(new SnmpArray(v.Id, v.Data));
        	}
            SnmpArray result = new SnmpArray(varbinds);
            return result;
        }
    }
}
