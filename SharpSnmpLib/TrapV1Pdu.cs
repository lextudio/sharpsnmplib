/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/4/30
 * Time: 21:22
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
    /// Trap v1 PDU.
    /// </summary>
    /// <remarks>represents the PDU of trap v1 message.</remarks>
    public class TrapV1Pdu : ISnmpPdu, ISnmpData
    {
        private byte[] _bytes;    
        private byte[] _raw;
        private ObjectIdentifier _enterprise;
        private IP _agent;
        private Integer32 _generic;
        private Integer32 _specific;
        private TimeTicks _timestamp;
        private Sequence _varbindSection;
        private IList<Variable> _variables;    
      
        /// <summary>
        /// Creates a <see cref="TrapV1Pdu"/> instance with raw bytes.
        /// </summary>
        /// <param name="raw">Raw bytes</param>
        public TrapV1Pdu(byte[] raw)
        {
            _raw = raw;
            MemoryStream m = new MemoryStream(raw);
            _enterprise = (ObjectIdentifier)SnmpDataFactory.CreateSnmpData(m);
            _agent = (IP)SnmpDataFactory.CreateSnmpData(m);
            _generic = (Integer32)SnmpDataFactory.CreateSnmpData(m);
            _specific = (Integer32)SnmpDataFactory.CreateSnmpData(m);
            _timestamp = (TimeTicks)SnmpDataFactory.CreateSnmpData(m);
            _varbindSection = (Sequence)SnmpDataFactory.CreateSnmpData(m);
            _variables = Variable.Transform(_varbindSection);
        }
       
        /// <summary>
        /// Creates a <see cref="TrapV1Pdu"/> instance with PDU elements.
        /// </summary>
        /// <param name="enterprise">Enterprise</param>
        /// <param name="agent">Agent address</param>
        /// <param name="generic">Generic trap type</param>
        /// <param name="specific">Specific trap type</param>
        /// <param name="timestamp">Time stamp</param>
        /// <param name="variables">Variable binds</param>
        [CLSCompliant(false)]
        public TrapV1Pdu(uint[] enterprise, IP agent, Integer32 generic, Integer32 specific, TimeTicks timestamp, IList<Variable> variables)
            : this(new ObjectIdentifier(enterprise), agent, generic, specific, timestamp, variables) 
        {
        }
        
        /// <summary>
        /// Creates a <see cref="TrapV1Pdu"/> instance with PDU elements.
        /// </summary>
        /// <param name="enterprise">Enterprise</param>
        /// <param name="agent">Agent address</param>
        /// <param name="generic">Generic trap type</param>
        /// <param name="specific">Specific trap type</param>
        /// <param name="timestamp">Time stamp</param>
        /// <param name="variables">Variable binds</param>
        public TrapV1Pdu(ObjectIdentifier enterprise, IP agent, Integer32 generic, Integer32 specific, TimeTicks timestamp, IList<Variable> variables)
        {
            _enterprise = enterprise;
            _agent = agent;
            _generic = generic;
            _specific = specific;
            _timestamp = timestamp;
            _varbindSection = Variable.Transform(variables);
            _variables = variables;
            _raw = ByteTool.ParseItems(_enterprise, _agent, _generic, _specific, _timestamp, _varbindSection);
        }   
        
        /// <summary>
        /// Type code.
        /// </summary>
        public SnmpType TypeCode 
        {
            get 
            {
                return SnmpType.TrapV1Pdu;
            }
        }
        
        /// <summary>
        /// To byte format.
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
        
        /// <summary>
        /// To message body.
        /// </summary>
        /// <param name="version">Protocol version</param>
        /// <param name="community">Community name</param>
        /// <returns></returns>
        public ISnmpData ToMessageBody(VersionCode version, string community)
        {
            return ByteTool.PackMessage(version, community, this);
        }      
        
        /// <summary>
        /// Enterprise.
        /// </summary>
        public ObjectIdentifier Enterprise 
        {
            get { return _enterprise; }
        }
        
        /// <summary>
        /// Agent address.
        /// </summary>
        public IP AgentAddress 
        {
            get { return _agent; }
        }
        
        /// <summary>
        /// Generic trap type.
        /// </summary>
        public GenericCode Generic
        {
            get { return (GenericCode)_generic.ToInt32(); }
        }
        
        /// <summary>
        /// Specific trap type.
        /// </summary>
        public int Specific 
        {
            get { return _specific.ToInt32(); }
        }
        
        /// <summary>
        /// Time stamp.
        /// </summary>
        public TimeTicks TimeStamp
        {
            get { return _timestamp; }
        }
        
        /// <summary>
        /// Variable binds.
        /// </summary>
        public IList<Variable> Variables 
        {
            get { return _variables; }
        }

        /// <summary>
        /// Returns a <see cref="String"/> that represents this <see cref="TrapV1Pdu"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "SNMPv1 TRAP PDU: agent address: {0}; time stamp: {1}; enterprise: {2}; generic: {3}; specific: {4}; varbind count: {5}",
                AgentAddress, 
                TimeStamp,
                Enterprise, 
                Generic,
                Specific.ToString(),
                Variables.Count.ToString());
        }
    }
}
