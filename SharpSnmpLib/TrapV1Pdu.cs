/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/4/30
 * Time: 21:22
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System.Collections.Generic;
using System.IO;

namespace SharpSnmpLib
{
	/// <summary>
	/// Description of TrapPduV1.
	/// </summary>
	public class TrapV1Pdu: ISnmpPdu, ISnmpData
	{
		public TrapV1Pdu(byte[] raw)
		{
			_raw = raw;
			MemoryStream m = new MemoryStream(raw);
			_enterprise = (ObjectIdentifier)SnmpDataFactory.CreateSnmpData(m);
			_agent = (IP)SnmpDataFactory.CreateSnmpData(m);
			_generic = (Int)SnmpDataFactory.CreateSnmpData(m);
			_specific = (Int)SnmpDataFactory.CreateSnmpData(m);
			_timeStamp = (Timeticks)SnmpDataFactory.CreateSnmpData(m);
			_varbindSection = (SnmpArray)SnmpDataFactory.CreateSnmpData(m);
			_variables = Variable.ConvertFrom(_varbindSection);
		}
		
		public TrapV1Pdu(ObjectIdentifier enterprise, 
		                 IP agent,
		                 Int generic,
		                 Int specific,
		                 Timeticks timeStamp,
		                 IList<Variable> variables)
		{
			_enterprise = enterprise;
			_agent = agent;
			_generic = generic;
			_specific = specific;
			_timeStamp = timeStamp;
			//TODO: important. Variable must be converted to varbind section.
			_varbindSection = Variable.ConvertTo(variables);
			_variables = variables;
			_raw = ByteTool.ParseItems(_enterprise, _agent, _generic, _specific, _timeStamp, _varbindSection);
		}		
		
		public SnmpType TypeCode {
			get {
				return SnmpType.TrapPDUv1;
			}
		}
		
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
		
		public ISnmpData ToMessageBody(VersionCode version, string community)
		{
			Int ver = new Int((int)version);
			OctetString comm = new OctetString(community);
			TrapV1Pdu pdu = this;
			SnmpArray array = new SnmpArray(ver, comm, pdu);
			return array;
		}
		
		byte[] _bytes;	
		byte[] _raw;
		ObjectIdentifier _enterprise;
		
		public ObjectIdentifier Enterprise {
			get { return _enterprise; }
		}

		IP _agent;
		
		public IP Agent {
			get { return _agent; }
		}

		Int _generic;
		
		public int Generic {
			get { return _generic.ToInt32(); }
		}

		Int _specific;
		
		public int Specific {
			get { return _specific.ToInt32(); }
		}

		Timeticks _timeStamp;
		
		public Timeticks TimeStamp {
			get { return _timeStamp; }
		}

		SnmpArray _varbindSection;
		IList<Variable> _variables;

        public IList<Variable> Variables {
			get { return _variables; }
		}
	}
}
