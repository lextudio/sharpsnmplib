// GET NEXT request message PDU.
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

/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/11
 * Time: 12:35
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
    /// GETNEXT request PDU.
    /// </summary>
    public sealed class GetNextRequestPdu : ISnmpPdu
    {
        private readonly Sequence _varbindSection;
        private readonly byte[]? _length;
        private byte[]? _raw;

        /// <summary>
        /// Creates a <see cref="GetNextRequestPdu"/> with all contents.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <param name="variables">Variables</param>
        public GetNextRequestPdu(int requestId, IList<Variable> variables)
        {
            RequestId = new Integer32(requestId);
            ErrorStatus = Integer32.Zero;
            ErrorIndex = Integer32.Zero;
            Variables = variables ?? throw new ArgumentNullException(nameof(variables));
            _varbindSection = Variable.Transform(variables);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetNextRequestPdu"/> class.
        /// </summary>
        /// <param name="length">The length data.</param>
        /// <param name="stream">The stream.</param>
        public GetNextRequestPdu(Tuple<int, byte[]> length, Stream stream)
        {
            if (length == null)
            {
                throw new ArgumentNullException(nameof(length));
            }

            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            RequestId = (Integer32)DataFactory.CreateSnmpData(stream);
            ErrorStatus = (Integer32)DataFactory.CreateSnmpData(stream);
            ErrorIndex = (Integer32)DataFactory.CreateSnmpData(stream);
            _varbindSection = (Sequence)DataFactory.CreateSnmpData(stream);
            Variables = Variable.Transform(_varbindSection);
            _length = length.Item2;
        }

        /// <summary>
        /// Gets the request ID.
        /// </summary>
        /// <value>The request ID.</value>
        public Integer32 RequestId { get; }

        /// <summary>
        /// Gets the error status.
        /// </summary>
        /// <value>The error status.</value>
        public Integer32 ErrorStatus { get; }

        /// <summary>
        /// Gets the index of the error.
        /// </summary>
        /// <value>The index of the error.</value>
        public Integer32 ErrorIndex { get; }

        /// <summary>
        /// Variables.
        /// </summary>
        public IList<Variable> Variables { get; }

        #region ISnmpData Members
        /// <summary>
        /// Type code.
        /// </summary>
        public SnmpType TypeCode => SnmpType.GetNextRequestPdu;

        /// <summary>
        /// Appends the bytes to <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public void AppendBytesTo(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            _raw ??= ByteTool.ParseItems(RequestId, ErrorStatus, ErrorIndex, _varbindSection);
            stream.AppendBytes(TypeCode, _length, _raw);
        }

        #endregion
        /// <summary>
        /// Returns a <see cref="string"/> that represents this <see cref="GetNextRequestPdu"/>.
        /// </summary>
        /// <returns>A <see cref="string"/> that represents this <see cref="GetNextRequestPdu"/>.</returns>
        public override string ToString()
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "GET NEXT request PDU: seq: {0}; status: {1}; index: {2}; variable count: {3}",
                RequestId,
                ErrorStatus,
                ErrorIndex,
                Variables.Count.ToString(CultureInfo.InvariantCulture));
        }
    }
}
