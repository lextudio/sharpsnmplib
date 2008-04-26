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
        string _community;
        public string Community
        {
            get
            {
                return _community;
            }
        }
        uint[] _enterprise;
        public uint[] Enterprise
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
 
		TrapMessage(MemoryStream m)
		{
            int first = m.ReadByte();
            int packetLength = m.ReadByte();
            int packetHeader_2 = m.ReadByte();
            int packetHeader_1 = m.ReadByte();
            int packetHeader_0 = m.ReadByte();
            Universal tag = new Universal(m);
            _community = tag.Value.ToString();
            int pduType = m.ReadByte();
            int pduLength = m.ReadByte();
            Universal enterpriseOID = new Universal(m);
            _enterprise = (uint[])enterpriseOID.Value;// change to uint[] OID form
            int IPflag = m.ReadByte();
            int IPversion = m.ReadByte();
            byte[] ip = new byte[4];
            m.Read(ip, 0, 4);
            _ip = new System.Net.IPAddress(ip);
            int separator = m.ReadByte();
            int separator1 = m.ReadByte();
            _generic = m.ReadByte();
            int separator2 = m.ReadByte();
            int separator3 = m.ReadByte();
            _specific = m.ReadByte();
            Universal timeStamp = new Universal(m);
            string time = timeStamp.Value.ToString();
            int separator4 = m.ReadByte();
            int varbindSectionLength = m.ReadByte();
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
		
		public static TrapMessage Parse(byte[] buffer, int length)
		{
			MemoryStream m = new MemoryStream(buffer, 0, length, false);
			TrapMessage result = new TrapMessage(m);
			return result;
		}
		
		public static TrapMessage Parse(byte[] buffer)
		{
			return Parse(buffer, buffer.Length);
		}
	}
	
	public class Variable
	{
		Universal _oid;
		Universal _data;
		
		public Variable(Universal varbind)
		{
            Universal[] content = (Universal[])varbind.Value;
			_oid = content[0];
			_data = content[1];
		}
		
		public Universal Id
		{
			get
			{
				return _oid;				
			}
		}
		
		public Universal Data
		{
			get
			{
				return _data;
			}
		}
	}
}
