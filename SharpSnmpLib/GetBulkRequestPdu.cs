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
using System.Text;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// GETBULK request PDU.
    /// </summary>
    public class GetBulkRequestPdu : ISnmpPdu
    {
        private Integer32 _nonRepeaters;
        private Integer32 _maxRepetitions;
        private IList<Variable> _variables;
        private Integer32 _seq;
        private byte[] _raw;
        private Sequence _varbindSection;
        private byte[] _bytes;
        
        /// <summary>
        /// Creates a <see cref="GetBulkRequestPdu"/> with all contents.
        /// </summary>
        /// <param name="nonRepeaters">Non-repeaters.</param>
        /// <param name="maxRepetitions">Max repetitions.</param>
        /// <param name="variables">Variables.</param>
        public GetBulkRequestPdu(int nonRepeaters, int maxRepetitions, IList<Variable> variables)
            : this(new Integer32(nonRepeaters), new Integer32(maxRepetitions), variables)
        {
        }
        
        private GetBulkRequestPdu(Integer32 nonRepeaters, Integer32 maxRepetitions, IList<Variable> variables)
        {
            _seq = PduCounter.NextCount;
            _nonRepeaters = nonRepeaters;
            _maxRepetitions = maxRepetitions;
            _variables = variables;
            _varbindSection = Variable.Transform(variables);
            _raw = ByteTool.ParseItems(_seq, _nonRepeaters, _maxRepetitions, _varbindSection);
        }
        
        /// <summary>
        /// Creates a <see cref="GetBulkRequestPdu"/> with raw bytes.
        /// </summary>
        /// <param name="raw">Raw bytes</param>
        public GetBulkRequestPdu(byte[] raw)
        {
            _raw = raw;
            MemoryStream m = new MemoryStream(raw);
            _seq = (Integer32)SnmpDataFactory.CreateSnmpData(m);
            _nonRepeaters = (Integer32)SnmpDataFactory.CreateSnmpData(m);
            _maxRepetitions = (Integer32)SnmpDataFactory.CreateSnmpData(m);
            _varbindSection = (Sequence)SnmpDataFactory.CreateSnmpData(m);
            _variables = Variable.Transform(_varbindSection);
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
            get { return SnmpType.GetBulkRequestPdu; }
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
        /// Returns a <see cref="String"/> that represents this <see cref="GetBulkRequestPdu"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "GETBULK request PDU: seq: {0}; non-repeaters: {1}; max-repetitions: {2}; variable count: {3}",
                _seq, 
                _nonRepeaters, 
                _maxRepetitions, 
                _variables.Count.ToString(CultureInfo.InvariantCulture));
        }
    }
}