// GET BULK request message PDU.
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
 * Date: 2008/8/3
 * Time: 15:14
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
    /// GETBULK request PDU.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Pdu")]
    public class GetBulkRequestPdu : ISnmpPdu
    {
        private readonly Integer32 _nonRepeaters;
        private readonly Integer32 _maxRepetitions;
        private readonly IList<Variable> _variables;
        private readonly Integer32 _requestId;
        private byte[] _raw;
        private readonly Sequence _varbindSection;

        /// <summary>
        /// Creates a <see cref="GetBulkRequestPdu"/> with all contents.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <param name="nonRepeaters">Non-repeaters.</param>
        /// <param name="maxRepetitions">Max repetitions.</param>
        /// <param name="variables">Variables.</param>
        public GetBulkRequestPdu(int requestId, int nonRepeaters, int maxRepetitions, IList<Variable> variables)
            : this(new Integer32(requestId), new Integer32(nonRepeaters), new Integer32(maxRepetitions), variables)
        {
        }
        
        private GetBulkRequestPdu(Integer32 requestId, Integer32 nonRepeaters, Integer32 maxRepetitions, IList<Variable> variables)
        {
            _requestId = requestId;
            _nonRepeaters = nonRepeaters;
            _maxRepetitions = maxRepetitions;
            _variables = variables;
            _varbindSection = Variable.Transform(variables);
            ////_raw = ByteTool.ParseItems(_seq, _nonRepeaters, _maxRepetitions, _varbindSection);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetBulkRequestPdu"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public GetBulkRequestPdu(Stream stream)
        {
            _requestId = (Integer32)DataFactory.CreateSnmpData(stream);
            _nonRepeaters = (Integer32)DataFactory.CreateSnmpData(stream);
            _maxRepetitions = (Integer32)DataFactory.CreateSnmpData(stream);
            _varbindSection = (Sequence)DataFactory.CreateSnmpData(stream);
            _variables = Variable.Transform(_varbindSection);
            ////_raw = ByteTool.ParseItems(_seq, _nonRepeaters, _maxRepetitions, _varbindSection);
            ////Debug.Assert(length >= _raw.Length, "length not match");
        }

        /// <summary>
        /// Gets the request ID.
        /// </summary>
        /// <value>The request ID.</value>
        public Integer32 RequestId
        {
            get
            {
                return _requestId;
            }
        }

        /// <summary>
        /// Gets the error status.
        /// </summary>
        /// <value>The error status.</value>
        public Integer32 ErrorStatus
        {
            get { return _nonRepeaters; }
        }

        /// <summary>
        /// Gets the index of the error.
        /// </summary>
        /// <value>The index of the error.</value>
        public Integer32 ErrorIndex
        {
            get { return _maxRepetitions; }
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

        #endregion

        #region ISnmpData Members
        /// <summary>
        /// Type code.
        /// </summary>
        public SnmpType TypeCode
        {
            get { return SnmpType.GetBulkRequestPdu; }
        }

        /// <summary>
        /// Appends the bytes to <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public void AppendBytesTo(Stream stream)
        {
            if (_raw == null)
            {
                _raw = ByteTool.ParseItems(_requestId, _nonRepeaters, _maxRepetitions, _varbindSection);
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
        
        /// <summary>
        /// Returns a <see cref="string"/> that represents this <see cref="GetBulkRequestPdu"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "GETBULK request PDU: seq: {0}; non-repeaters: {1}; max-repetitions: {2}; variable count: {3}",
                _requestId, 
                _nonRepeaters, 
                _maxRepetitions, 
                _variables.Count.ToString(CultureInfo.InvariantCulture));
        }
    }
}
