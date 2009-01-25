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
        private readonly Integer32 _sequenceNumber;
        private readonly byte[] _raw;
        private readonly Sequence _varbindSection;
        private byte[] _bytes;
        
        /// <summary>
        /// Creates a <see cref="GetRequestPdu"/> with all contents.
        /// </summary>
        /// <param name="errorStatus">Error status</param>
        /// <param name="errorIndex">Error index</param>
        /// <param name="variables">Variables</param>
        public GetRequestPdu(ErrorCode errorStatus, int errorIndex, IList<Variable> variables)
            : this(new Integer32((int)errorStatus), new Integer32(errorIndex), variables)
        {
        }
        
        private GetRequestPdu(Integer32 errorStatus, Integer32 errorIndex, IList<Variable> variables)
        {
            _sequenceNumber = PduCounter.NextCount;
            _errorStatus = errorStatus;
            _errorIndex = errorIndex;
            _variables = variables;
            _varbindSection = Variable.Transform(variables);
            _raw = ByteTool.ParseItems(_sequenceNumber, _errorStatus, _errorIndex, _varbindSection);
        }
        
        /// <summary>
        /// Creates a <see cref="GetRequestPdu"/> with raw bytes.
        /// </summary>
        /// <param name="raw">Raw bytes</param>
        private GetRequestPdu(byte[] raw): this(raw.Length, new MemoryStream(raw))
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetRequestPdu"/> class.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <param name="stream">The stream.</param>
        public GetRequestPdu(int length, Stream stream)
        {
            _sequenceNumber = (Integer32)DataFactory.CreateSnmpData(stream);
            _errorStatus = (Integer32)DataFactory.CreateSnmpData(stream);
            _errorIndex = (Integer32)DataFactory.CreateSnmpData(stream);
            _varbindSection = (Sequence)DataFactory.CreateSnmpData(stream);
            _variables = Variable.Transform(_varbindSection);
            _raw = ByteTool.ParseItems(_sequenceNumber, _errorStatus, _errorIndex, _varbindSection);
            Debug.Assert(length >= _raw.Length);
        }

        internal int SequenceNumber
        {
            get
            {
                return _sequenceNumber.ToInt32();
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
        public ISnmpData ToMessageBody(VersionCode version, OctetString community)
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
        /// Converts to byte format.
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            if (_bytes == null)
            {
                MemoryStream result = new MemoryStream();
                result.WriteByte((byte)TypeCode);
                ByteTool.WritePayloadLength(result, _raw.Length); // it seems that trap does not use this function
                result.Write(_raw, 0, _raw.Length);
                _bytes = result.ToArray();
            }
            
            return _bytes;
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
                _sequenceNumber, 
                _errorStatus, 
                _errorIndex, 
                _variables.Count.ToString(CultureInfo.InvariantCulture));
        }
    }
}
