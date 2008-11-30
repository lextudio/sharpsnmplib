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
using System.Text;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// INFORM request PDU.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Pdu")]
    public class InformRequestPdu : ISnmpPdu
    {
        private Integer32 _errorStatus;
        private Integer32 _errorIndex;
        private IList<Variable> _variables;
        private Integer32 _seq;
        private byte[] _raw;
        private Sequence _varbindSection;
        private byte[] _bytes;
        private TimeTicks _time;
        private ObjectIdentifier _enterprise;
        
        /// <summary>
        /// Creates a <see cref="InformRequestPdu"/> instance with all content.
        /// </summary>
        /// <param name="enterprise">Enterprise</param>
        /// <param name="time">Time stamp</param>
        /// <param name="variables">Variables</param>
        public InformRequestPdu(ObjectIdentifier enterprise, TimeTicks time, IList<Variable> variables)
        {
            _enterprise = enterprise;
            _seq = PduCounter.NextCount;
            _time = time;
            _variables = variables;
            IList<Variable> full = new List<Variable>(variables);
            full.Insert(0, new Variable(new uint[] { 1, 3, 6, 1, 2, 1, 1, 3, 0 }, _time));
            full.Insert(1, new Variable(new uint[] { 1, 3, 6, 1, 6, 3, 1, 1, 4, 1, 0 }, _enterprise));
            _varbindSection = Variable.Transform(full);
            _raw = ByteTool.ParseItems(_seq, new Integer32(0), new Integer32(0), _varbindSection);
        }        

        /// <summary>
        /// Creates a <see cref="InformRequestPdu"/> with raw bytes.
        /// </summary>
        /// <param name="raw">Raw bytes</param>
        public InformRequestPdu(byte[] raw)
        {
            _raw = raw;
            MemoryStream m = new MemoryStream(raw);
            _seq = (Integer32)SnmpDataFactory.CreateSnmpData(m);
            _errorStatus = (Integer32)SnmpDataFactory.CreateSnmpData(m);
            _errorIndex = (Integer32)SnmpDataFactory.CreateSnmpData(m);
            _varbindSection = (Sequence)SnmpDataFactory.CreateSnmpData(m);
            _variables = Variable.Transform(_varbindSection);
            _time = (TimeTicks)_variables[0].Data;
            _variables.RemoveAt(0);
            _enterprise = (ObjectIdentifier)_variables[0].Data;
            _variables.RemoveAt(0);
        }
        
        internal int SequenceNumber
        {
            get
            {
                return _seq.ToInt32();
            }
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
            get { return SnmpType.InformRequestPdu; }
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
        /// Returns a <see cref="String"/> that represents this <see cref="InformRequestPdu"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "INFORM request PDU: seq: {0}; status: {1}; index: {2}; variable count: {3}",
                _seq,
                _errorStatus,
                _errorIndex,
                _variables.Count.ToString(CultureInfo.InvariantCulture));
        }
    }
}
