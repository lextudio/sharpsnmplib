// Malformed PDU type.
// Copyright (C) 2010 Lex Li.
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;
using System.Collections.Generic;
using System.IO;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Malformed PDU class. Many things are not implemented as they are not in use.
    /// </summary>
    internal class MalformedPdu : ISnmpPdu
    {
        private static readonly ISnmpPdu Pdu = new MalformedPdu();

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static ISnmpPdu Instance
        {
            get { return Pdu; }
        }

        /// <summary>
        /// Type code.
        /// </summary>
        /// <value>Returns <see cref="SnmpType"/> unknown type.</value>
        public SnmpType TypeCode
        {
            get { return SnmpType.Unknown; }
        }

        /// <summary>
        /// Appends the bytes to <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public void AppendBytesTo(Stream stream)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Converts to the bytes.
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the request ID.
        /// </summary>
        /// <value>The request ID.</value>
        public Integer32 RequestId
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets the error status.
        /// </summary>
        /// <value>The error status.</value>
        public Integer32 ErrorStatus
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets the index of the error.
        /// </summary>
        /// <value>The index of the error.</value>
        public Integer32 ErrorIndex
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Converts the PDU to index complete message.
        /// </summary>
        /// <param name="version">Protocol version.</param>
        /// <param name="community">Community name.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "version")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "community")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public Sequence ToMessageBody(VersionCode version, OctetString community)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Variable bindings.
        /// </summary>
        /// <value>Returns an empty list.</value>
        public IList<Variable> Variables
        {
            // as we cannot extract PDU data in such cases, only an empty list can be returned here.
            get { return new List<Variable>(0); }
        }
    }
}
