using System;
using System.Net.Sockets;
using System.IO;
using System.Collections;
using System.Text;

// ASN.1 BER encoding library by Malcolm Crowe at the University of the West of Scotland
// See http://cis.paisley.ac.uk/crow-ci0
// This is version 0 of the library, please advise me about any bugs
// mailto:malcolm.crowe@paisley.ac.uk

// Restrictions: It is assumed that no encoding has a length greater than 2^31-1.
// UNIVERSAL TYPES
// Some of the more unusual Universal encodings are supported but not fully implemented
// Should you require these types, as an alternative to changing this code
// you can catch the exception that is thrown and examine the contents yourself.
// APPLICATION TYPES
// If you want to handle Application types systematically, you can derive a class from
// Universal, and provide the Creator and Creators methods for your class
// You will see an example of how to do this in the Snmplib
// CONTEXT AND PRIVATE TYPES
// Ad hoc coding can be used for these, as an alterative to derive a class as above.

namespace X690
{
	public enum UniversalType
	{
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
		Sequence=0x10,
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
        IpAddress=0x40,
        Timeticks=0x43
	}
	// all references here are to ITU-X.690-12/97
}
