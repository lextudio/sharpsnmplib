/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/4/28
 * Time: 11:48
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using Snmp;
using System;

namespace SharpSnmpLib
{
	/// <summary>
	/// Description of IDataType.
	/// </summary>
	public interface ISnmpData
	{
		SnmpType DataType
		{
			get;
		}
	}
}
