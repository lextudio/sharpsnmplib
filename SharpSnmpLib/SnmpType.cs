using System.Runtime.Serialization;

namespace Lextm.SharpSnmpLib;

/// <summary>
/// SNMP type code. The values are tag values for SNMP types.
/// </summary>
[DataContract]
public enum SnmpType
{
    EndMarker = 0x00,
    Integer32 = 0x02,
    OctetString = 0x04,
    Null = 0x05,
    ObjectIdentifier = 0x06,
    Sequence = 0x30,
    IPAddress = 0x40,
    Counter32 = 0x41,
    Gauge32 = 0x42,
    TimeTicks = 0x43,
    Opaque = 0x44,
    NetAddress = 0x45,
    Counter64 = 0x46,
    Unsigned32 = 0x47,
    NoSuchObject = 0x80,
    NoSuchInstance = 0x81,
    EndOfMibView = 0x82,
    GetRequestPdu = 0xA0,
    GetNextRequestPdu = 0xA1,
    ResponsePdu = 0xA2,
    SetRequestPdu = 0xA3,
    TrapV1Pdu = 0xA4,
    GetBulkRequestPdu = 0xA5,
    InformRequestPdu = 0xA6,
    TrapV2Pdu = 0xA7,
    ReportPdu = 0xA8,
    Unknown = 0xFFFF
}
