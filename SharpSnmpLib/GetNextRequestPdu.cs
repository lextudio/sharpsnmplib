/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/11
 * Time: 12:35
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Diagnostics;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// GETNEXT request PDU.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Pdu")]
    public class GetNextRequestPdu : ISnmpPdu
    {
        private readonly Integer32 _errorStatus;
        private readonly Integer32 _errorIndex;
        private readonly IList<Variable> _variables;
        private readonly Integer32 _seq;
        private readonly byte[] _raw;
        private readonly Sequence _varbindSection;
        private byte[] _bytes;     
        
        /// <summary>
        /// Creates a <see cref="GetNextRequestPdu"/> with all contents.
        /// </summary>
        /// <param name="errorStatus">Error status</param>
        /// <param name="errorIndex">Error index</param>
        /// <param name="variables">Variables</param>
        public GetNextRequestPdu(ErrorCode errorStatus, int errorIndex, IList<Variable> variables) 
            : this(new Integer32((int)errorStatus), new Integer32(errorIndex), variables) 
        {
        }
        
        private GetNextRequestPdu(Integer32 errorStatus, Integer32 errorIndex, IList<Variable> variables)
        {
            _seq = PduCounter.NextCount;                      
            _errorStatus = errorStatus; 
            _errorIndex = errorIndex;
            _variables = variables;
            _varbindSection = Variable.Transform(variables);
            _raw = ByteTool.ParseItems(_seq, _errorStatus, _errorIndex, _varbindSection);
        }
        
        /// <summary>
        /// Creates a <see cref="GetNextRequestPdu"/> with raw bytes.
        /// </summary>
        /// <param name="raw">Raw bytes</param>
        private GetNextRequestPdu(byte[] raw): this(raw.Length, new MemoryStream(raw))
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetNextRequestPdu"/> class.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <param name="stream">The stream.</param>
        public GetNextRequestPdu(int length, Stream stream)
        {
            _seq = (Integer32)DataFactory.CreateSnmpData(stream);
            _errorStatus = (Integer32)DataFactory.CreateSnmpData(stream);
            _errorIndex = (Integer32)DataFactory.CreateSnmpData(stream);
            _varbindSection = (Sequence)DataFactory.CreateSnmpData(stream);
            _variables = Variable.Transform(_varbindSection);
            _raw = ByteTool.ParseItems(_seq, _errorStatus, _errorIndex, _varbindSection);
            Debug.Assert(length >= _raw.Length);
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
            get { return SnmpType.GetNextRequestPdu; }
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
        /// Returns a <see cref="string"/> that represents this <see cref="GetNextRequestPdu"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "GET NEXT request PDU: seq: {0}; status: {1}; index: {2}; variable count: {3}",
                _seq, 
                _errorStatus, 
                _errorIndex, 
                _variables.Count.ToString(CultureInfo.InvariantCulture));
        }
    }
}

