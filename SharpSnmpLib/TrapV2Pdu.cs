// TRAP message PDU (SNMP version 2 and above).
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
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// TRAP v2 PDU.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Pdu")]
    public sealed class TrapV2Pdu : ISnmpPdu
    {
        private readonly uint[] _timeId = new uint[] { 1, 3, 6, 1, 2, 1, 1, 3, 0 };
        private readonly uint[] _enterpriseId = new uint[] { 1, 3, 6, 1, 6, 3, 1, 1, 4, 1, 0 };
        private readonly Sequence _varbindSection;
        private readonly TimeTicks _time;
        private readonly byte[] _length;
        private byte[] _raw;

        /// <summary>
        /// Creates a <see cref="TrapV2Pdu"/> instance with all content.
        /// </summary>
        /// <param name="requestId">Request ID.</param>
        /// <param name="enterprise">Enterprise.</param>
        /// <param name="time">Time stamp.</param>
        /// <param name="variables">Variables.</param>
        [CLSCompliant(false)]
        public TrapV2Pdu(int requestId, ObjectIdentifier enterprise, uint time, IList<Variable> variables)
        {
            if (enterprise == null)
            {
                throw new ArgumentNullException(nameof(enterprise));
            }

            if (variables == null)
            {
                throw new ArgumentNullException(nameof(variables));
            }

            Enterprise = enterprise;
            RequestId = new Integer32(requestId);
            _time = new TimeTicks(time);
            Variables = variables;
            IList<Variable> full = new List<Variable>(variables);
            full.Insert(0, new Variable(_timeId, _time));
            full.Insert(1, new Variable(_enterpriseId, Enterprise));
            _varbindSection = Variable.Transform(full);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TrapV2Pdu"/> class.
        /// </summary>
        /// <param name="length">The length data.</param>
        /// <param name="stream">The stream.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "temp2")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "temp1")]
        public TrapV2Pdu(Tuple<int, byte[]> length, Stream stream)
        {
            if (length == null)
            {
                throw new ArgumentNullException(nameof(length));
            }

            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
            
            RequestId = (Integer32)DataFactory.CreateSnmpData(stream); // request
#pragma warning disable 168
            var temp1 = (Integer32)DataFactory.CreateSnmpData(stream); // 0
            var temp2 = (Integer32)DataFactory.CreateSnmpData(stream); // 0
#pragma warning restore 168
            _varbindSection = (Sequence)DataFactory.CreateSnmpData(stream);
            Variables = Variable.Transform(_varbindSection); // v[0] is timestamp. v[1] oid, v[2] value.
            _time = (TimeTicks)Variables[0].Data;
            Variables.RemoveAt(0);
            Enterprise = (ObjectIdentifier)Variables[0].Data;
            Variables.RemoveAt(0);
            _length = length.Item2;
        }

        #region ISnmpPdu Members

        /// <summary>
        /// Variables.
        /// </summary>
        public IList<Variable> Variables { get; private set; }

        /// <summary>
        /// Gets the request ID.
        /// </summary>
        /// <value>The request ID.</value>
        public Integer32 RequestId { get; private set; }

        /// <summary>
        /// Gets the error status.
        /// </summary>
        /// <value>The error status.</value>
        public Integer32 ErrorStatus
        {
            get { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Gets the index of the error.
        /// </summary>
        /// <value>The index of the error.</value>
        public Integer32 ErrorIndex
        {
            get { throw new NotSupportedException(); }
        }

        #endregion

        #region ISnmpData Members
        /// <summary>
        /// Type code.
        /// </summary>
        public SnmpType TypeCode
        {
            get { return SnmpType.TrapV2Pdu; }
        }
        
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
            
            if (_raw == null)
            {
                _raw = ByteTool.ParseItems(RequestId, Integer32.Zero, Integer32.Zero, _varbindSection);
            }

            stream.AppendBytes(TypeCode, _length, _raw);
        }

        #endregion

        /// <summary>
        /// Enterprise.
        /// </summary>
        public ObjectIdentifier Enterprise { get; private set; }

        /// <summary>
        /// Time stamp.
        /// </summary>
        [CLSCompliant(false)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "TimeStamp")]
        public uint TimeStamp
        {
            get { return _time.ToUInt32(); }
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this <see cref="TrapV2Pdu"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "TRAP v2 PDU: request ID: {3}; enterprise: {0}; time stamp: {1}; variable count: {2}",
                Enterprise, 
                _time, 
                Variables.Count.ToString(CultureInfo.InvariantCulture),
                RequestId);
        }
    }
}
