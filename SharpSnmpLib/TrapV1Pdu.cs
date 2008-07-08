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
using System.IO;

namespace Lextm.SharpSnmpLib
{
	/// <summary>
	/// Trap v1 PDU.
	/// </summary>
	/// <remarks>represents the PDU of trap v1 message.</remarks>
	public class TrapV1Pdu: ISnmpPdu, ISnmpData
	{
		byte[] _bytes;	
		byte[] _raw;
		ObjectIdentifier _enterprise;
		IP _agent;
		GenericCode _generic;
		Integer32 _specific;
		TimeTicks _timeStamp;
		Sequence _varbindSection;
		IList<Variable> _variables;	
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
			_generic = (GenericCode)((Integer32)SnmpDataFactory.CreateSnmpData(m)).ToInt32();
			_specific = (Integer32)SnmpDataFactory.CreateSnmpData(m);
			_timeStamp = (TimeTicks)SnmpDataFactory.CreateSnmpData(m);
			_varbindSection = (Sequence)SnmpDataFactory.CreateSnmpData(m);
			_variables = Variable.ConvertFrom(_varbindSection);
		}
				/// <summary>
		/// Creates a <see cref="TrapV1Pdu"/> instance with PDU elements.
		/// </summary>
		/// <param name="enterprise">Enterprise</param>
		/// <param name="agent">Agent address</param>
		/// <param name="generic">Generic trap type</param>
		/// <param name="specific">Specific trap type</param>
		/// <param name="timeStamp">Time stamp</param>
		/// <param name="variables">Variable binds</param>
		[CLSCompliant(false)]
		public TrapV1Pdu(uint[] enterprise,
		                 IP agent,
		                 GenericCode generic,
		                 Integer32 specific,
		                 TimeTicks timeStamp,
		                 IList<Variable> variables)
			: this(new ObjectIdentifier(enterprise), agent, generic, 
			       specific, timeStamp, variables) {}
		/// <summary>
		/// Creates a <see cref="TrapV1Pdu"/> instance with PDU elements.
		/// </summary>
		/// <param name="enterprise">Enterprise</param>
		/// <param name="agent">Agent address</param>
		/// <param name="generic">Generic trap type</param>
		/// <param name="specific">Specific trap type</param>
		/// <param name="timeStamp">Time stamp</param>
		/// <param name="variables">Variable binds</param>
		public TrapV1Pdu(ObjectIdentifier enterprise, 
		                 IP agent,
		                 GenericCode generic,
		                 Integer32 specific,
		                 TimeTicks timeStamp,
		                 IList<Variable> variables)
		{
			_enterprise = enterprise;
			_agent = agent;
			_generic = generic;
			_specific = specific;
			_timeStamp = timeStamp;
			//IMPORTANT: Variable must be converted to varbind section.
			_varbindSection = Variable.ConvertTo(variables);
			_variables = variables;
			_raw = ByteTool.ParseItems(_enterprise, _agent, new Integer32((int)_generic), _specific, _timeStamp, _varbindSection);
		}		
		/// <summary>
		/// Type code.
		/// </summary>
		public SnmpType TypeCode {
			get {
				return SnmpType.TrapV1Pdu;
			}
		}
		/// <summary>
		/// To byte format.
		/// </summary>
		/// <returns></returns>
		public byte[] ToBytes()
		{
			if (_bytes == null) {
				MemoryStream result = new MemoryStream();
				result.WriteByte((byte)TypeCode);
				ByteTool.WriteMultiByteLength(result, _raw.Length); //it seems that trap does not use this function
				result.Write(_raw,0,_raw.Length);
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
			Integer32 ver = new Integer32((int)version);
			OctetString comm = new OctetString(community);
			TrapV1Pdu pdu = this;
			Sequence array = new Sequence(ver, comm, pdu);
			return array;
		}		
		/// <summary>
		/// Enterprise.
		/// </summary>
		public ObjectIdentifier Enterprise {
			get { return _enterprise; }
		}
		/// <summary>
		/// Agent address.
		/// </summary>
		public IP AgentAddress {
			get { return _agent; }
		}
		/// <summary>
		/// Generic trap type.
		/// </summary>
		public GenericCode Generic {
			get { return _generic; }
		}
		/// <summary>
		/// Specific trap type.
		/// </summary>
		public int Specific {
			get { return _specific.ToInt32(); }
		}
		/// <summary>
		/// Time stamp.
		/// </summary>
		public TimeTicks TimeStamp {
			get { return _timeStamp; }
		}
		/// <summary>
		/// Variable binds.
		/// </summary>
        public IList<Variable> Variables {
			get { return _variables; }
		}
	}
}
