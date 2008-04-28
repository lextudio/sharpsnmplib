
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
	public enum SnmpType // RFC1213 subset of ASN.1
	{ 
		//TODO: remove this
//		EndMarker = 0x00,
//		Boolean = 0x01,
//		Integer=0x02, 
//		UInt32=0x47, 
//		BitString=0x03,  // internally BitSet
//		OctetString=0x04, // internally string
//		ObjectIdentifier=0x06, // internally uint[]
//		Null=0x05,
//		Sequence=0x30, // Array
//		Counter32=0x41,
//		Counter64=0x46,
//		Gauge=0x42,
//		TimeTicks=0x43,
//		IPAddress=0x40, // byte[]
//		Opaque=0x44,
//		NetAddress=0x45,
//		GetRequestPDU=0xA0,
//		GetNextRequestPDU=0xA1,
//		GetResponsePDU=0xA2,
//		SetRequestPDU=0xA3,
//		TrapPDUv1=0xA4,
//		TrapPDUv2=0xA7,
//		GetBulkRequest=0xA5,
//		InformRequest=0xA6
		EndMarker = 0x00,
		Boolean = 0x01,
		Integer=0x02, // X690.Integer
		BitString=0x03,  // X690.BitSet
		OctetString=0x04, // X690.OctetString
		Null=0x05,
		ObjectIdentifier=0x06, // uint[]
		ObjectDescriptor=0x7,
		ExternalInstance=0x8,
		Real=0x9,  // X690.Real
		Enumerated=0xa,
		EmbeddedPDV=0xb,
		UTF8String=0xc,
		RelativeOID=0xd,
		Reserved1=0xe,
		Reserved2=0xf,
		Sequence=0x10,//X690.Sequence
		Set=0x11,
		NumericString=0x12,
		PrintableString=0x13,
		T61String=0x14,
		VideoTextString=0x15,
		IA5String=0x16,
		UTCTime=0x17,
		GeneralizedTime=0x18,
		GrahicString=0x19,
		VisibleString=0x1a,
		GeneralString=0x1b,
		UniversalString=0x1c,
		CharacterString=0x1d,
		BMPString=0x1e,
		Array=0x30,  // RFC1213 sequence for whole SNMP packet beginning
		IpAddress=0x40,
		Counter32=0x41,
		Gauge=0x42,
        Timeticks=0x43,
        Opaque=0x44,
		NetAddress=0x45,
        Counter64=0x46,
        UInt32=0x47,
        GetRequestPDU=0xA0,
		GetNextRequestPDU=0xA1,
		GetResponsePDU=0xA2,
		SetRequestPDU=0xA3,
		TrapPDUv1=0xA4,
		TrapPDUv2=0xA7,
		GetBulkRequest=0xA5,
		InformRequest=0xA6
	}
}
