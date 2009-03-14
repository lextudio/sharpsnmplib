/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/8/3
 * Time: 15:36
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
    /// INFORM request PDU.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Pdu")]
    public class InformRequestPdu : ISnmpPdu
    {
        private readonly Integer32 _errorStatus;
        private readonly Integer32 _errorIndex;
        private readonly IList<Variable> _variables;
        private readonly Integer32 _requestId;
        private byte[] _raw;
        private readonly Sequence _varbindSection;
        private readonly TimeTicks _time;
        private readonly ObjectIdentifier _enterprise;

        /// <summary>
        /// Creates a <see cref="InformRequestPdu"/> instance with all content.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <param name="enterprise">Enterprise</param>
        /// <param name="time">Time stamp</param>
        /// <param name="variables">Variables</param>
        public InformRequestPdu(Integer32 requestId, ObjectIdentifier enterprise, TimeTicks time, IList<Variable> variables)
        {
            _enterprise = enterprise;
            _requestId = requestId;
            _time = time;
            _variables = variables;
            IList<Variable> full = new List<Variable>(variables);
            full.Insert(0, new Variable(new uint[] { 1, 3, 6, 1, 2, 1, 1, 3, 0 }, _time));
            full.Insert(1, new Variable(new uint[] { 1, 3, 6, 1, 6, 3, 1, 1, 4, 1, 0 }, _enterprise));
            _varbindSection = Variable.Transform(full);
            ////_raw = ByteTool.ParseItems(_seq, new Integer32(0), new Integer32(0), _varbindSection);
        }        

        /// <summary>
        /// Creates a <see cref="InformRequestPdu"/> with raw bytes.
        /// </summary>
        /// <param name="raw">Raw bytes</param>
        internal InformRequestPdu(byte[] raw)
            : this(new MemoryStream(raw))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InformRequestPdu"/> class.
        /// </summary>    
        /// <param name="stream">The stream.</param>
        public InformRequestPdu(Stream stream)
        {
            _requestId = (Integer32)DataFactory.CreateSnmpData(stream);
            _errorStatus = (Integer32)DataFactory.CreateSnmpData(stream);
            _errorIndex = (Integer32)DataFactory.CreateSnmpData(stream);
            _varbindSection = (Sequence)DataFactory.CreateSnmpData(stream);
            _variables = Variable.Transform(_varbindSection);
            _time = (TimeTicks)_variables[0].Data;
            _variables.RemoveAt(0);
            _enterprise = (ObjectIdentifier)_variables[0].Data;
            _variables.RemoveAt(0);
            ////_raw = ByteTool.ParseItems(_seq, _errorStatus, _errorIndex, _varbindSection);
            ////Debug.Assert(length >= _raw.Length, "length not match");
        }
        
        [Obsolete("Use RequestId instead.")]
        internal int SequenceNumber
        {
            get
            {
                return _requestId.ToInt32();
            }
        }
        
        internal int RequestId
        {
            get { return _requestId.ToInt32(); }
        }
        
        internal IList<Variable> AllVariables
        {
            get
            {
                return Variable.Transform(_varbindSection);
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
            get { return SnmpType.InformRequestPdu; }
        }

        /// <summary>
        /// Appends the bytes to <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public void AppendBytesTo(Stream stream)
        {
            if (_raw == null)
            {
                _raw = ByteTool.ParseItems(_requestId, new Integer32(0), new Integer32(0), _varbindSection);
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
        /// Returns a <see cref="string"/> that represents this <see cref="InformRequestPdu"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "INFORM request PDU: seq: {0}; status: {1}; index: {2}; variable count: {3}",
                _requestId,
                _errorStatus,
                _errorIndex,
                _variables.Count.ToString(CultureInfo.InvariantCulture));
        }
    }
}
