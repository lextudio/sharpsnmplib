
using SharpSnmpLib;
using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using X690;

// SNMP library for .NET by Malcolm Crowe at University of the West of Scotland
// http://cis.paisley.ac.uk/crow-ci0/
// This is version 0 of the library. Email bugs to
// mailto:malcolm.crowe@paisley.ac.uk

// Getting Started
// The simplest way to get an SNMP value from a host is
// ManagerItem mi = new ManagerItem(
//								new ManagerSession(hostname,"public"),
//								"1.3.6.1.2.1.1.4.0");
// Then the actual OID is mi.Name and the value is in mi.Value.ToString().

// TODO: Tables, lists of bindings
//		 Friendly strings derived from MIBs

namespace Snmp
{
	public class ManagerSession 
	{
		public string agentAddress;
		public string agentCommunity;
		IPEndPoint agent; 
		int seq = 0;
		UdpClient udp = new UdpClient();

        public ManagerSession(string a, string c) : this(IPAddress.Parse(a), c) { }

        public ManagerSession(IPAddress ip, string c)
        {
            agentAddress = ip.ToString();
            agentCommunity = c;
            //IPAddress host = Dns.GetHostEntry(a).AddressList[0];
            agent = new IPEndPoint(ip, 161);
        }
		public void Close() { udp.Close(); }
		public Universal VarBind(uint[] oid)
		{
			return VarBind(new ObjectIdentifier(oid));
		}
		
		public Universal VarBind(ObjectIdentifier oid)
		{
			return VarBind(oid, Universal.Null);
		}
			
		public Universal VarBind(uint[] oid, Universal val)
		{
			return VarBind(new ObjectIdentifier(oid), val);//new Universal(new Universal(oid),val);
		}
		
		public Universal VarBind(ObjectIdentifier oid, Universal val)
		{
			return new Universal(new Universal(oid), val);
		}
		
		public Universal PDU(SnmpType t,params Universal[] vbinds)
		{
			seq += 10;
			return new SnmpBER(t,
				new Universal(seq),
				new Universal(0), // errorStatus
				new Universal(0), // errorIndex
				new Universal(vbinds));
		}
		
		public byte[] GetBytes(params Universal[] vbinds)
		{
			SnmpBER mess = new SnmpBER(SnmpType.Array,
				new Universal(0), // version-1
				new Universal(agentCommunity),
				PDU(SnmpType.GetRequestPDU,vbinds));
			MemoryStream m = new MemoryStream();
			mess.Send(m);
			return m.ToArray();
		}
		
		public Universal[] Get(params Universal[] vbinds)
		{
			SnmpBER mess = new SnmpBER(SnmpType.Array,
				new Universal(0), // version-1
				new Universal(agentCommunity),
				PDU(SnmpType.GetRequestPDU,vbinds));
			MemoryStream m = new MemoryStream();
			mess.Send(m);
			byte[] bytes = m.ToArray();
			udp.Send(bytes,bytes.Length,agent);
			IPEndPoint from = new IPEndPoint(IPAddress.Any,0);
	            IAsyncResult result = udp.BeginReceive(null, this);
	            result.AsyncWaitHandle.WaitOne(_timeout, false);
	            if (!result.IsCompleted)
	                throw new SharpSnmpException("timeout");
	            bytes = udp.EndReceive(result, ref from);
	            m = new MemoryStream(bytes, false);
			mess = new SnmpBER(m);
			Universal pdu = mess[2];
			Universal vbindlist = pdu[3];
			return (Universal[])vbindlist.Value;
		}

        int _timeout = 5000;

        public int Timeout
        {
            get
            {
                return _timeout;
            }
            set
            {
                _timeout = value;
            }
        }

		public void Set(params Universal[] vbinds)
		{
			SnmpBER mess = new SnmpBER(SnmpType.Array,
				new Universal(0), // version-1
				new Universal(agentCommunity),
				PDU(SnmpType.SetRequestPDU,vbinds));
			MemoryStream m = new MemoryStream();
			mess.Send(m);
			byte[] bytes = m.ToArray();
			udp.Send(bytes,bytes.Length,agent);
				IPEndPoint from = new IPEndPoint(IPAddress.Any,0);
	            IAsyncResult result = udp.BeginReceive(null, this);
	            result.AsyncWaitHandle.WaitOne(_timeout, false);
	            if (!result.IsCompleted)
	                throw new SharpSnmpException("timeout");
	            bytes = udp.EndReceive(result, ref from);
	            m = new MemoryStream(bytes, false);
			mess = new SnmpBER(m);
			Universal pdu = mess[2];
			Universal vbindlist = pdu[3];
		}
		
		public bool GetNext(ref Universal vbind)
		{
			SnmpBER mess = new SnmpBER(SnmpType.Array,
				new Universal(0), // version-1
				new Universal(agentCommunity),
				PDU(SnmpType.GetNextRequestPDU,vbind));
			MemoryStream m = new MemoryStream();
			mess.Send(m);
			byte[] bytes = m.ToArray();
			IPAddress host = Dns.GetHostEntry(agentAddress).AddressList[0];
			agent = new IPEndPoint(host,161);
			udp.Send(bytes,bytes.Length,agent);
			IPEndPoint from = new IPEndPoint(IPAddress.Any,0);
	            IAsyncResult result = udp.BeginReceive(null, this);
	            result.AsyncWaitHandle.WaitOne(_timeout, false);
	            if (!result.IsCompleted)
	                return false;
	            bytes = udp.EndReceive(result, ref from);
	            m = new MemoryStream(bytes, false);
			mess = new SnmpBER(m);
			Universal pdu = mess[2];
			if (((int)(X690.Integer)pdu[1].Value)!=0) // errorStatus
				return false;
			Universal vbindlist = pdu[3];
			vbind = ((Universal[])vbindlist.Value)[0];
			return true;
		}
	}
}
