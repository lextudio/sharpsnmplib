using System;
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
        /// Gets the request ID.
        /// </summary>
        /// <value>The request ID.</value>
        Integer32 RequestId { get; }

        /// <summary>
        /// Gets the error status.
        /// </summary>
        /// <value>The error status.</value>
        Integer32 ErrorStatus { get; }

        /// <summary>
        /// Gets the index of the error.
        /// </summary>
        /// <value>The index of the error.</value>
        Integer32 ErrorIndex { get; }

        /// <summary>
        /// Converts the PDU to index complete message.
        /// </summary>
        /// <param name="version">Protocol version.</param>
        /// <param name="community">Community name.</param>
        /// <returns></returns>
        [Obsolete("Use ByteTool.PackMessage instead")]
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
