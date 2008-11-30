using System;
using System.Collections.Generic;
using System.Text;

namespace Lextm.SharpSnmpLib.Mib
{
    /// <summary>
    /// Definition type.
    /// </summary>
    public enum DefinitionType
    {
        /// <summary>
        /// Unknown type.
        /// </summary>
        Unknown = 0, // agentcapability, modulecompliance, moduleidentity, notificationgroup, notificationtype, objectgroup, objecttype
       
        /// <summary>
        /// OID value assignment.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Oid")]
        OidValueAssignment = 1, // such as iso
      
        /// <summary>
        /// Scalar OID.
        /// </summary>
        Scalar = 2, // sysUpTime
     
        /// <summary>
        /// Table OID.
        /// </summary>
        Table = 3,
     
        /// <summary>
        /// Table entry OID.
        /// </summary>
        Entry = 4,
      
        /// <summary>
        /// Table column OID.
        /// </summary>
        Column = 5
    }
}
