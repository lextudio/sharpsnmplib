// Definition type enum.
// Copyright (C) 2008-2010 Malcolm Crowe, Lex Li, and other contributors.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

using System;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Definition type.
    /// </summary>
    [Serializable]
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
