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
    public interface ISnmpMessage
    {
        /// <summary>
        /// PDU section.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Pdu")]
        ISnmpPdu Pdu 
        {
            get;
        }

        SecurityParameters Parameters { get; }

        Scope Scope { get; }
        /// <summary>
        /// Converts to the bytes.
        /// </summary>
        /// <returns></returns>
        byte[] ToBytes();
    }
}
