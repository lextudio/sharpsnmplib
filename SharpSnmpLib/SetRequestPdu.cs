// SET request PDU.
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
 * Time: 20:51
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
    /// SET request PDU.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Pdu")]
    public class SetRequestPdu : ISnmpPdu
    {
        private readonly Integer32 _errorStatus;
        private readonly Integer32 _errorIndex;
        private readonly IList<Variable> _variables;
        private readonly Integer32 _requestId;
        private byte[] _raw;
        private readonly Sequence _varbindSection;

        /// <summary>
        /// Creates a <see cref="SetRequestPdu"/> instance with all contents.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <param name="errorStatus">Error status</param>
        /// <param name="errorIndex">Error index</param>
        /// <param name="variables">Variables</param>
        public SetRequestPdu(int requestId, ErrorCode errorStatus, int errorIndex, IList<Variable> variables)
            : this(new Integer32(requestId), new Integer32((int)errorStatus), new Integer32(errorIndex), variables)
        {
        }

        /// <summary>
        /// Creates a <see cref="SetRequestPdu"/> instance with all contents.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <param name="errorStatus">Error status</param>
        /// <param name="errorIndex">Error index</param>
        /// <param name="variables">Variables</param>
        private SetRequestPdu(Integer32 requestId, Integer32 errorStatus, Integer32 errorIndex, IList<Variable> variables)
        {
            _requestId = requestId;
            _errorStatus = errorStatus;
            _errorIndex = errorIndex;
            _variables = variables;
            _varbindSection = Variable.Transform(_variables);
            ////_raw = ByteTool.ParseItems(_sequenceNumber, _errorStatus, _errorIndex, _varbindSection);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SetRequestPdu"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public SetRequestPdu(Stream stream)
        {
            _requestId = (Integer32)DataFactory.CreateSnmpData(stream);
            _errorStatus = (Integer32)DataFactory.CreateSnmpData(stream);
            _errorIndex = (Integer32)DataFactory.CreateSnmpData(stream);
            _varbindSection = (Sequence)DataFactory.CreateSnmpData(stream);
            _variables = Variable.Transform(_varbindSection);
            ////_raw = ByteTool.ParseItems(_sequenceNumber, _errorStatus, _errorIndex, _varbindSection);
            ////Debug.Assert(length >= _raw.Length, "length not match");
        }
        
        /// <summary>
        /// Returns a <see cref="string"/> that represents this <see cref="SetRequestPdu"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "SET request PDU: seq: {0}; status: {1}; index: {2}; variable count: {3}",
                _requestId,
                _errorStatus,
                _errorIndex,
                _variables.Count.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Gets the request ID.
        /// </summary>
        /// <value>The request ID.</value>
        public Integer32 RequestId
        {
            get { return _requestId; }
        }

        /// <summary>
        /// Gets the error status.
        /// </summary>
        /// <value>The error status.</value>
        public Integer32 ErrorStatus
        {
            get { return _errorStatus; }
        }

        /// <summary>
        /// Gets the index of the error.
        /// </summary>
        /// <value>The index of the error.</value>
        public Integer32 ErrorIndex
        {
            get { return _errorIndex; }
        }
        
        /// <summary>
        /// Variables.
        /// </summary>
        public IList<Variable> Variables
        {
            get
            {
                return _variables;
            }
        }

        #region ISnmpPdu Members
        /// <summary>
        /// Converts to message body.
        /// </summary>
        /// <param name="version">Prtocol version</param>
        /// <param name="community">Community name</param>
        /// <returns></returns>
        [Obsolete("Use ByteTool.PackMessage instead")]
        public Sequence ToMessageBody(VersionCode version, OctetString community)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ISnmpData Members
        /// <summary>
        /// Type code.
        /// </summary>
        public SnmpType TypeCode
        {
            get { return SnmpType.SetRequestPdu; }
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
                _raw = ByteTool.ParseItems(_requestId, _errorStatus, _errorIndex, _varbindSection);
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

        #endregion
    }
}
