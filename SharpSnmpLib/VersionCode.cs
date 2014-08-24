// SNMP version code enum.
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

namespace Lextm.SharpSnmpLib
{
    using System;
    using System.Runtime.Serialization;
    
    /// <summary>
    /// Protocol version code.
    /// </summary>
    [DataContract]
    public enum VersionCode
    {
        /// <summary>
        /// SNMP v1.
        /// </summary>
        V1 = 0,
        
        /// <summary>
        /// SNMP v2 classic.
        /// </summary>
        V2 = 1,
        
        /// <summary>
        /// SNMP v2u is obsolete.
        /// </summary>
        [Obsolete("This version of SNMP is obsolete and replaced by v3.")]
        V2U = 2,

        /// <summary>
        /// SNMP v3.
        /// </summary>
        V3 = 3
    }
}