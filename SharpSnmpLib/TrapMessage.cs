/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/4/25
 * Time: 20:30
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using Snmp;
using System.IO;
using X690;
using System.Net;
using System.Collections.Generic;

namespace SharpSnmpLib
{
	/// <summary>
	/// Description of Trap.
	/// </summary>
	public class TrapMessage
	{
        public override string ToString()
        {
            return string.Format("SNMPv1 trap: agent address: {0}; time stamp: {1}; community: {2}; enterprise: {3}; generic: {4}; specific: {5}; varbind count: {6}",
                AgentAddress, TimeStamp, Community, Enterprise, GenericId, SpecificId, Variables.Count);
        }
        int _time;
        public int TimeStamp
        {
            get
            {
                return _time;
            }
        }
        string _community;
        public string Community
        {
            get
            {
                return _community;
            }
        }
        ObjectIdentifier _enterprise;
        public ObjectIdentifier Enterprise
        {
            get
            {
                return _enterprise;
            }
        }
        IPAddress _ip;
        public IPAddress AgentAddress
        {
            get
            {
                return _ip;
            }
        }
        int _generic;
        public int GenericId
        {
            get
            {
                return _generic;
            }
        }
        int _specific;
        public int SpecificId
        {
            get
            {
                return _specific;
            }
        }
        IList<Variable> _varbinds;
        public IList<Variable> Variables
        {
            get
            {
                return _varbinds;
            }
        }
        ProtocolVersion _version;
        public ProtocolVersion Version
        {
        	get {
        		return _version;
        	}
        }
 
		public TrapMessage(MemoryStream m)
		{
			if ((int)SnmpType.Array != m.ReadByte()) {
				throw new ArgumentException("not a trap message");
			}
            int packetLength = getMultiByteLength(m);
            Universal protocol = new Universal(m);
            int version = (Integer)protocol.Value;
            _version = (ProtocolVersion)version;
            Universal community = new Universal(m);
            _community = community.Value.ToString();
            int pduType = m.ReadByte();
            int pduLength = getMultiByteLength(m);
            Universal enterpriseOID = new Universal(m);
            _enterprise = (ObjectIdentifier)enterpriseOID.Value;
            Universal ip = new Universal(m);
            _ip = ((IpAddress)ip.Value).ToIPAddress();
            Universal generic = new Universal(m);
            _generic = (Integer)generic.Value;
            Universal specific = new Universal(m);
            _specific = (Integer)specific.Value;
            Universal timeStamp = new Universal(m);
            _time = (Integer)timeStamp.Value;
            int separator4 = m.ReadByte();
            int varbindSectionLength = getMultiByteLength(m);
            long current = m.Position;
            _varbinds = new List<Variable>();
            if (varbindSectionLength != 0)
            {
                // parse varbinds
                do {
	                Universal varbind = new Universal(m);
	                _varbinds.Add(new Variable(varbind));
                } while ((m.Position - current) < varbindSectionLength);
            }
		}

        private static int getMultiByteLength(MemoryStream m)
        {
            int current = m.ReadByte();
            return ReadLength(m, (byte)current);
        }
        // copied from universal
        static int ReadLength(Stream s, byte x) // x is initial octet
        {
            if ((x & 0x80) == 0)
                return (int)x;
            int u = 0;
            int n = (int)(x & 0x7f);
            for (int j = 0; j < n; j++)
            {
                x = ReadByte(s);
                u = (u << 8) + (int)x;
            }
            return u;
        }
        //copied from universal
        static byte ReadByte(Stream s)
        {
            int n = s.ReadByte();
            if (n == -1)
                throw (new Exception("BER end of file"));
            return (byte)n;
        }

		public TrapMessage(byte[] buffer, int length): this(new MemoryStream(buffer, 0, length, false))
		{
		}
		
		public TrapMessage(byte[] buffer): this(buffer, buffer.Length)
		{
		}
	}
	

}
