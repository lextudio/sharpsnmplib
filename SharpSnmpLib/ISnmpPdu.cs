using System.Collections.Generic;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// SNMP PDU.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Pdu")]
    public interface ISnmpPdu : ISnmpData
    {
        /// <summary>
        /// Converts the PDU to index complete message.
        /// </summary>
        /// <param name="version"></param>
        /// <param name="community"></param>
        /// <returns></returns>
        Sequence ToMessageBody(VersionCode version, OctetString community);
        
        /// <summary>
        /// Variable bindings.
        /// </summary>
        IList<Variable> Variables 
        { 
            get; 
        }
    }
}
