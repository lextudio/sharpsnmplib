using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// GET request PDU.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Pdu")]
    public class GetRequestPdu : ISnmpPdu
    {
        private readonly Integer32 _errorStatus;
        private readonly Integer32 _errorIndex;
        private readonly IList<Variable> _variables;
        private readonly Integer32 _requestId;
        private byte[] _raw;
        private readonly Sequence _varbindSection;

        /// <summary>
        /// Creates a <see cref="GetRequestPdu"/> with all contents.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <param name="errorStatus">Error status</param>
        /// <param name="errorIndex">Error index</param>
        /// <param name="variables">Variables</param>
        public GetRequestPdu(int requestId, ErrorCode errorStatus, int errorIndex, IList<Variable> variables)
            : this(new Integer32(requestId), new Integer32((int)errorStatus), new Integer32(errorIndex), variables)
        {
        }
        
        private GetRequestPdu(Integer32 requestId, Integer32 errorStatus, Integer32 errorIndex, IList<Variable> variables)
        {
            _requestId = requestId;
            _errorStatus = errorStatus;
            _errorIndex = errorIndex;
            _variables = variables;
            _varbindSection = Variable.Transform(variables);
            ////_raw = ByteTool.ParseItems(_sequenceNumber, _errorStatus, _errorIndex, _varbindSection);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetRequestPdu"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public GetRequestPdu(Stream stream)
        {
            _requestId = (Integer32)DataFactory.CreateSnmpData(stream);
            _errorStatus = (Integer32)DataFactory.CreateSnmpData(stream);
            _errorIndex = (Integer32)DataFactory.CreateSnmpData(stream);
            _varbindSection = (Sequence)DataFactory.CreateSnmpData(stream);
            _variables = Variable.Transform(_varbindSection);
            ////_raw = ByteTool.ParseItems(_sequenceNumber, _errorStatus, _errorIndex, _varbindSection);
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
            get { return SnmpType.GetRequestPdu; }
        }

        /// <summary>
        /// Appends the bytes to <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public void AppendBytesTo(Stream stream)
        {
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
        
        /// <summary>
        /// Returns a <see cref="string"/> that represents this <see cref="GetRequestPdu"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "GET request PDU: seq: {0}; status: {1}; index: {2}; variable count: {3}",
                _requestId, 
                _errorStatus, 
                _errorIndex, 
                _variables.Count.ToString(CultureInfo.InvariantCulture));
        }
    }
}
