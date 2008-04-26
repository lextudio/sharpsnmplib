
using System;
using System.Net.Sockets;
using System.IO;
using System.Collections;
using System.Text;
using System.Net;
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
		public ManagerSession(string a, string c)
		{
			agentAddress = a;
			agentCommunity = c;
			IPAddress host = Dns.GetHostEntry(a).AddressList[0];
			agent = new IPEndPoint(host,161);
		}
		public void Close() { udp.Close(); }
		public Universal VarBind(uint[] oid)
		{
			return VarBind(oid, Universal.Null);
		}
		public Universal VarBind(uint[] oid, Universal val)
		{
			return new Universal(new Universal(oid),val);
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
		public Universal[] Get(params Universal[] vbinds)
		{
			SnmpBER mess = new SnmpBER(SnmpType.Sequence,
				new Universal(0), // version-1
				new Universal(agentCommunity),
				PDU(SnmpType.GetRequestPDU,vbinds));
			MemoryStream m = new MemoryStream();
			mess.Send(m);
			byte[] bytes = m.ToArray();
			udp.Send(bytes,bytes.Length,agent);
			IPEndPoint from = new IPEndPoint(IPAddress.Any,0);
	            IAsyncResult result = udp.BeginReceive(null, this);
	            result.AsyncWaitHandle.WaitOne(100, false);
	            if (!result.IsCompleted)
	                return null;
	            bytes = udp.EndReceive(result, ref from);
	            m = new MemoryStream(bytes, false);
			mess = new SnmpBER(m);
			Universal pdu = mess[2];
			Universal vbindlist = pdu[3];
			return (Universal[])vbindlist.Value;
		}
		public void Set(params Universal[] vbinds)
		{
			SnmpBER mess = new SnmpBER(SnmpType.Sequence,
				new Universal(0), // version-1
				new Universal(agentCommunity),
				PDU(SnmpType.SetRequestPDU,vbinds));
			MemoryStream m = new MemoryStream();
			mess.Send(m);
			byte[] bytes = m.ToArray();
			udp.Send(bytes,bytes.Length,agent);
		}
		public bool GetNext(ref Universal vbind)
		{
			SnmpBER mess = new SnmpBER(SnmpType.Sequence,
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
	            result.AsyncWaitHandle.WaitOne(100, false);
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
