/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/1
 * Time: 11:17
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// SNMP message.
    /// </summary>
    public interface ISnmpMessage : ISnmpData
    {
        /// <summary>
        /// PDU section.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Pdu")]
        ISnmpPdu Pdu 
        {
            get;
        }
    }
}
