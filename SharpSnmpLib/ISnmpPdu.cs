using System;
using System.Collections.Generic;
using System.Text;

namespace Lextm.SharpSnmpLib
{
	/// <summary>
	/// SNMP PDU.
	/// </summary>
    public interface ISnmpPdu: ISnmpData
    {
    	/// <summary>
    	/// Converts the PDU to index complete message.
    	/// </summary>
    	/// <param name="version"></param>
    	/// <param name="community"></param>
    	/// <returns></returns>
        ISnmpData ToMessageBody(VersionCode version, string community);
        /// <summary>
        /// Variable bindings.
        /// </summary>
        IList<Variable> Variables { get; }
    }
}
