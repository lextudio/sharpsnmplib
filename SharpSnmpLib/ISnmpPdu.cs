// SNMP PDU interface.
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
        /// Variable bindings.
        /// </summary>
        IList<Variable> Variables 
        { 
            get; 
        }
    }
}
