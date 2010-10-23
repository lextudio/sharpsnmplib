// GET response message PDU.
// Copyright (C) 2008-2010 Malcolm Crowe, Lex Li, and other contributors.
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

/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/1
 * Time: 18:10
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// GET response PDU.
    /// </summary>
    [Obsolete("Please use ResponsePdu.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Pdu")]
    public class GetResponsePdu : ISnmpPdu
    {
        private readonly Sequence _varbindSection;
        private byte[] _raw;

        /// <summary>
        /// Creates a <see cref="GetResponsePdu"/> with all contents.
        /// </summary>
        /// <param name="requestId">The request ID.</param>
        /// <param name="errorStatus">Error status.</param>
        /// <param name="errorIndex">Error index.</param>
        /// <param name="variables">Variables.</param>
        public GetResponsePdu(int requestId, ErrorCode errorStatus, int errorIndex, IList<Variable> variables)
        {
            RequestId = new Integer32(requestId);
            ErrorStatus = new Integer32((int)errorStatus);
            ErrorIndex = new Integer32(errorIndex);
            Variables = variables;
            _varbindSection = Variable.Transform(variables);
            ////_raw = ByteTool.ParseItems(_sequenceNumber, _errorStatus, _errorIndex, _varbindSection);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetResponsePdu"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public GetResponsePdu(Stream stream)
        {
            RequestId = (Integer32)DataFactory.CreateSnmpData(stream);
            ErrorStatus = (Integer32)DataFactory.CreateSnmpData(stream);
            ErrorIndex = (Integer32)DataFactory.CreateSnmpData(stream);
            _varbindSection = (Sequence)DataFactory.CreateSnmpData(stream);
            Variables = Variable.Transform(_varbindSection);
        }

        /// <summary>
        /// Gets the request ID.
        /// </summary>
        /// <value>The request ID.</value>
        public Integer32 RequestId { get; private set; }

        /// <summary>
        /// Error status.
        /// </summary>
        public Integer32 ErrorStatus { get; private set; }

        /// <summary>
        /// Error index.
        /// </summary>
        public Integer32 ErrorIndex { get; private set; }

        /// <summary>
        /// Variables.
        /// </summary>
        public IList<Variable> Variables { get; private set; }

        /// <summary>
        /// Type code.
        /// </summary>
        public SnmpType TypeCode
        {
            get
            {
                return SnmpType.GetResponsePdu;
            }
        }

        /// <summary>
        /// Appends the bytes to <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public void AppendBytesTo(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            
            if (_raw == null)
            {
                _raw = ByteTool.ParseItems(RequestId, ErrorStatus, ErrorIndex, _varbindSection);
            }

            ByteTool.AppendBytes(stream, TypeCode, _raw);
        }

        /// <summary>
        /// Converts to byte format.
        /// </summary>
        /// <returns></returns>
        [Obsolete("Use AppendBytesTo instead.")]
        public byte[] ToBytes()
        {
            using (MemoryStream result = new MemoryStream())
            {
                AppendBytesTo(result);
                return result.ToArray();
            }
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this <see cref="GetResponsePdu"/>/
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "GET response PDU: seq: {0}; status: {1}; index: {2}; variable count: {3}",
                RequestId, 
                ErrorStatus, 
                ErrorIndex, 
                Variables.Count.ToString(CultureInfo.InvariantCulture));
        }
    }
}
