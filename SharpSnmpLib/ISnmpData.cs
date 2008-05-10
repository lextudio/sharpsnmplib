/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/4/28
 * Time: 11:48
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;

namespace Lextm.SharpSnmpLib
{
	/// <summary>
	/// SNMP data entity.
	/// </summary>
	public interface ISnmpData
	{
		/// <summary>
		/// Type code
		/// </summary>
		SnmpType TypeCode
		{
			get;
		}
		/// <summary>
		/// To byte format.
		/// </summary>
		/// <returns></returns>
		/// <remarks>The byte format is used to construct PDU.</remarks>
		byte[] ToBytes();
	}
}
