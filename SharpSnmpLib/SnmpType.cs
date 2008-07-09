
using System;
using System.Net.Sockets;
using System.IO;
using System.Collections;
using System.Text;
using System.Net;
using System.Diagnostics.CodeAnalysis;

// SNMP library for .NET by Malcolm Crowe at University of the West of Scotland
// http://cis.paisley.ac.uk/crow-ci0/
// This is version 0 of the library. Email bugs to
// mailto:malcolm.crowe@paisley.ac.uk

// Getting Started
// The simplest way to get an SNMP value from index host is
// ManagerItem mi = new ManagerItem(
//								new ManagerSession(hostname,"public"),
//								"1.3.6.1.2.1.1.4.0");
// Then the actual OID is mi.Name and the value is in mi.Value.ToString().

// TODO: Tables, lists of bindings
//		 Friendly strings derived from MIBs

[module: SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Scope = "member", Target = "SharpSnmpLib.SnmpType.#UTF8String")]
#pragma warning disable 1591
namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// SNMP type code. The values are tag values for SNMP types.
    /// </summary>
	public enum SnmpType // RFC1213 subset of ASN.1
	{ 
		EndMarker = 0x00,
		/// <summary>
		/// Boolean type.
		/// </summary>
		Boolean = 0x01,
		/// <summary>
		/// INTEGER type. (SMIv1, SMIv2)
		/// </summary>
		Integer32=0x02, // X690.Int
		/// <summary>
		/// BITS type. (SMIv2)
		/// </summary>
		BitString=0x03,  // X690.BitSet//TODO: verify if this is BITS.
		/// <summary>
		/// OCTET STRING type.
		/// </summary>
		OctetString=0x04, // X690.OctetString
		/// <summary>
		/// NULL type. (SMIv1)
		/// </summary>
		Null=0x05,
		/// <summary>
		/// OBJECT IDENTIFIER type. (SMIv1)
		/// </summary>
		ObjectIdentifier=0x06, // uint[]
		ObjectDescriptor=0x07,
		ExternalInstance=0x08,
		/// <summary>
		/// Real type.
		/// </summary>
		Real=0x09,  // X690.Real
		Enumerated=0x0a,
		EmbeddedPDV=0x0b,
		UTF8String=0x0c,
		RelativeOID=0x0d,
		Reserved1=0x0e,
		Reserved2=0x0f,
		SequenceTagNumber=0x10,//X690.Sequence (this is in fact the tag number for SEQUENCE)
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
		/// <summary>
		/// RFC1213 sequence for whole SNMP packet beginning
		/// </summary>
		Sequence=0x30,  // RFC1213 sequence for whole SNMP packet beginning
		/// <summary>
		/// IpAddress type. (SMIv1)
		/// </summary>
		IPAddress=0x40,
		/// <summary>
		/// Counter32 type. (SMIv1, SMIv2)
		/// </summary>
		Counter32=0x41,
		/// <summary>
		/// Gauge32 type. (SMIv1, SMIv2)
		/// </summary>
		Gauge32=0x42,
		/// <summary>
		/// TimeTicks type. (SMIv1)
		/// </summary>
        TimeTicks=0x43,
        /// <summary>
        /// Opaque type. (SMIv1)
        /// </summary>
        Opaque=0x44,
        /// <summary>
        /// Network Address. (SMIv1)
        /// </summary>
		NetAddress=0x45,
		/// <summary>
		/// Counter64 type. (SMIv2)
		/// </summary>
        Counter64=0x46,
        /// <summary>
        /// Unsigned32 type. (SMIv2)
        /// </summary>
        UInt32=0x47,
        /// <summary>
        /// No such object exception.
        /// </summary>
        NoSuchObject=0x80,
        /// <summary>
        /// No such instance exception.
        /// </summary>
        NoSuchInstance=0x81,
        /// <summary>
        /// End of MIB view exception.
        /// </summary>
        EndOfMibView=0x82,        
        /// <summary>
        /// Get request PDU.
        /// </summary>
        GetRequestPdu=0xA0,
        /// <summary>
        /// Get Next request PDU.
        /// </summary>
		GetNextRequestPdu=0xA1,
		/// <summary>
		/// Get response PDU.
		/// </summary>
		GetResponsePdu=0xA2,
		/// <summary>
		/// Set request PDU.
		/// </summary>
		SetRequestPdu=0xA3,
		/// <summary>
		/// Trap v1 PDU.
		/// </summary>
		TrapV1Pdu=0xA4,
		/// <summary>
		/// Get Bulk PDU.
		/// </summary>
		GetBulkRequestPdu=0xA5,
		/// <summary>
		/// Inform PDU.
		/// </summary>
		InformRequestPdu=0xA6,
		/// <summary>
		/// Trap v2 PDU.
		/// </summary>
		TrapV2Pdu=0xA7,
		/// <summary>
		/// Report PDU. SNMP v3.
		/// </summary>
		ReportPdu=0xA8
	}
}
#pragma warning restore 1591