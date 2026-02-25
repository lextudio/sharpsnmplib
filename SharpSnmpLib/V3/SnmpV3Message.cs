using DotNetSnmp.Asn1.Serialization;
using DotNetSnmp.Common.Definitions;
using DotNetSnmp.Protocol.V3.Security;
using System.Formats.Asn1;

namespace DotNetSnmp.Protocol.V3
{
    public class SnmpV3Message : ISnmpMessage
    {
        public VersionCode ProtocolVersion => VersionCode.V3;

        public required HeaderData Header { get; set; }

    public required UsmSecurityParameters SecurityParameters { get; set; }

public ReadOnlyMemory<byte> EncryptedScopedPdu { get; set; }

public IScope? Scope { get; set; }

public void WriteTo(AsnWriter writer)
{
    using (_ = writer.PushSequence())
    {
        // version
        writer.WriteInteger((int)VersionCode.V3);

        Header.WriteTo(writer);

        SecurityParameters.WriteTo(writer);

        // Write either the encrypted data or the ScopedPdu based on privacy flag
        if (Header.MsgFlags.HasFlag(MsgFlags.Priv))
        {
            if (EncryptedScopedPdu.IsEmpty)
            {
                throw new InvalidOperationException("Encrypted scoped PDU is required when privacy flag is set");
            }

            writer.WriteOctetString(EncryptedScopedPdu.Span);
        }
        else
        {
            if (Scope == null)
            {
                throw new InvalidOperationException("Scoped PDU is required when privacy flag is not set");
            }

            Scope.WriteTo(writer);
        }
    }
}

public static SnmpV3Message ReadFrom(AsnReader reader)
{
    // Message ::= SEQUENCE
    var rootSeq = reader.ReadSequence(expectedTag: Asn1Tag.Sequence);

    // version INTEGER
    if (rootSeq.TryReadInt32(out int messageVersion) == false)
    {
        throw new SnmpDecodeException(
            "Cannot read 'version' number");
    }

    var version = (VersionCode)messageVersion;

    if (version != VersionCode.V3)
    {
        throw new ArgumentNullException(
            $"expected version: 3 found: {version}");
    }

    var globalData = HeaderData.ReadFrom(rootSeq);

    var usmSecurityParams = UsmSecurityParameters.ReadFrom(rootSeq);

    var msg = new SnmpV3Message
    {
        Header = globalData,
        SecurityParameters = usmSecurityParams
    };

    if (globalData.MsgFlags.HasFlag(MsgFlags.Priv))
    {
        msg.EncryptedScopedPdu = rootSeq.ReadOctetString();
    }
    else
    {
        msg.Scope = V3.Scope.ReadFrom(rootSeq);
    }

    return msg;
}
    }
}
