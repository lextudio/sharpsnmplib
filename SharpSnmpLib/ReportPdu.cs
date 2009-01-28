/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/8/3
 * Time: 15:37
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Report PDU.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Pdu")]
    public class ReportPdu : ISnmpPdu
    {
        private readonly Integer32 _errorStatus;
        private readonly Integer32 _errorIndex;
        private readonly IList<Variable> _variables;
        private readonly Integer32 _seq;
        private readonly byte[] _raw;
        private readonly Sequence _varbindSection;

        /// <summary>
        /// Creates a <see cref="ReportPdu"/> with all contents.
        /// </summary>
        /// <param name="errorStatus">Error status</param>
        /// <param name="errorIndex">Error index</param>
        /// <param name="variables">Variables</param>
        public ReportPdu(ErrorCode errorStatus, int errorIndex, IList<Variable> variables)
            : this(new Integer32((int)errorStatus), new Integer32(errorIndex), variables)
        {
        }
        
        private ReportPdu(Integer32 errorStatus, Integer32 errorIndex, IList<Variable> variables)
        {
            _seq = PduCounter.NextCount;
            _errorStatus = errorStatus;
            _errorIndex = errorIndex;
            _variables = variables;
            _varbindSection = Variable.Transform(variables);
            _raw = ByteTool.ParseItems(_seq, _errorStatus, _errorIndex, _varbindSection);
        }

        internal int SequenceNumber
        {
            get
            {
                return _seq.ToInt32();
            }
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
        /// Initializes a new instance of the <see cref="ReportPdu"/> class.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <param name="stream">The stream.</param>
        public ReportPdu(int length, Stream stream)
        {
            _seq = (Integer32)DataFactory.CreateSnmpData(stream);
            _errorStatus = (Integer32)DataFactory.CreateSnmpData(stream);
            _errorIndex = (Integer32)DataFactory.CreateSnmpData(stream);
            _varbindSection = (Sequence)DataFactory.CreateSnmpData(stream);
            _variables = Variable.Transform(_varbindSection);
            _raw = ByteTool.ParseItems(_seq, _errorStatus, _errorIndex, _varbindSection);
            Debug.Assert(length >= _raw.Length, "length not match");
        }

        /// <summary>
        /// Converts to message body.
        /// </summary>
        /// <param name="version">Protocol version</param>
        /// <param name="community">Community name</param>
        /// <returns></returns>
        public Sequence ToMessageBody(VersionCode version, OctetString community)
        {
            return ByteTool.PackMessage(version, community, this);
        }

        #endregion

        #region ISnmpData Members
        /// <summary>
        /// Type code.
        /// </summary>
        public SnmpType TypeCode
        {
            get { return SnmpType.ReportPdu; }
        }

        /// <summary>
        /// Appends the bytes to <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public void AppendBytesTo(Stream stream)
        {
            ByteTool.AppendBytes(stream, TypeCode, _raw);
        }

        /// <summary>
        /// Converts to byte format.
        /// </summary>
        /// <returns></returns>
        [Obsolete("Use AppendBytesTo instead.")]
        public byte[] ToBytes()
        {
            MemoryStream result = new MemoryStream();
            AppendBytesTo(result);
            return result.ToArray();
        }

        #endregion
        
        /// <summary>
        /// Returns a <see cref="string"/> that represents this <see cref="ReportPdu"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "REPORT PDU: seq: {0}; status: {1}; index: {2}; variable count: {3}",
                _seq, 
                _errorStatus, 
                _errorIndex, 
                _variables.Count.ToString(CultureInfo.InvariantCulture));
        }
    }
}
