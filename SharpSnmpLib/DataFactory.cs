using DotNetSnmp.Asn1.Serialization;
using DotNetSnmp.Asn1.SyntaxObjects;
using DotNetSnmp.Protocol.V1;
using DotNetSnmp.Protocol.V2;
using DotNetSnmp.Protocol.V3;
using System.Formats.Asn1;
using System.IO;

namespace Lextm.SharpSnmpLib;

/// <summary>
/// Factory that creates ASN.1 data instances from encoded bytes.
/// </summary>
public static class DataFactory
{
    /// <summary>
    /// Creates ASN.1 data from a full BER-encoded buffer.
    /// </summary>
    public static IAsnSerializable CreateSnmpData(byte[] buffer)
    {
        if (buffer == null)
        {
            throw new ArgumentNullException(nameof(buffer));
        }

        return CreateSnmpData(buffer, 0, buffer.Length);
    }

    /// <summary>
    /// Creates ASN.1 data from a BER-encoded buffer slice.
    /// </summary>
    public static IAsnSerializable CreateSnmpData(byte[] buffer, int index, int count)
    {
        if (buffer == null)
        {
            throw new ArgumentNullException(nameof(buffer));
        }

        if (index < 0 || count < 0 || index + count > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(count));
        }

        if (count == 0)
        {
            throw new SnmpException("empty data buffer");
        }

        if (index == 0 && count == buffer.Length)
        {
            return Parse(buffer);
        }

        return Parse(new ReadOnlyMemory<byte>(buffer, index, count));
    }

    /// <summary>
    /// Creates ASN.1 data from a stream containing one BER value.
    /// </summary>
    public static IAsnSerializable CreateSnmpData(Stream stream)
    {
        if (stream == null)
        {
            throw new ArgumentNullException(nameof(stream));
        }

        using var memory = new MemoryStream();
        stream.CopyTo(memory);
        var payload = memory.ToArray();
        if (payload.Length == 0)
        {
            throw new SnmpException("empty data stream");
        }

        return Parse(payload);
    }

    /// <summary>
    /// Creates ASN.1 data from type + payload stream (legacy overload).
    /// </summary>
    public static IAsnSerializable CreateSnmpData(int type, Stream stream)
    {
        if (stream == null)
        {
            throw new ArgumentNullException(nameof(stream));
        }

        using var memory = new MemoryStream();
        memory.WriteByte((byte)type);
        stream.CopyTo(memory);
        var payload = memory.ToArray();
        return Parse(payload);
    }

    private static IAsnSerializable Parse(byte[] payload)
    {
        return Parse(new ReadOnlyMemory<byte>(payload));
    }

    private static IAsnSerializable Parse(ReadOnlyMemory<byte> payload)
    {
        try
        {
            // Legacy DataFactory parsed a single BER value from stream and ignored any trailing bytes.
            // Keep that behavior so decrypted scoped PDUs with cipher padding remain parseable.
            var trimmedPayload = TrimToSingleBerValue(payload);
            var reader = new AsnReader(trimmedPayload, AsnEncodingRules.BER);
            var tag = reader.PeekTag();

            if (tag.HasSameClassAndValue(Asn1Tag.Integer))
            {
                return Integer32.ReadFrom(reader);
            }

            if (tag.HasSameClassAndValue(Asn1Tag.PrimitiveOctetString))
            {
                return OctetString.ReadFrom(reader);
            }

            if (tag.HasSameClassAndValue(Asn1Tag.Null))
            {
                reader.ReadNull();
                return new Null();
            }

            if (tag.HasSameClassAndValue(Asn1Tag.ObjectIdentifier))
            {
                return ObjectIdentifier.ReadFrom(reader);
            }

            if (tag.HasSameClassAndValue(AsnTypes.IpAddress))
            {
                return IP.ReadFrom(reader);
            }

            if (tag.HasSameClassAndValue(AsnTypes.Counter32))
            {
                reader.TryReadUInt32(out var value, expectedTag: AsnTypes.Counter32);
                return new Counter32(value);
            }

            if (tag.HasSameClassAndValue(AsnTypes.Gauge32))
            {
                reader.TryReadUInt32(out var value, expectedTag: AsnTypes.Gauge32);
                return new Gauge32(value);
            }

            if (tag.HasSameClassAndValue(AsnTypes.TimeTicks))
            {
                return TimeTicks.ReadFrom(reader);
            }

            if (tag.HasSameClassAndValue(AsnTypes.Counter64))
            {
                return Counter64.ReadFrom(reader);
            }

            if (tag.HasSameClassAndValue(AsnTypes.Opaque))
            {
                return new Opaque(reader.ReadOctetString(expectedTag: AsnTypes.Opaque));
            }

            if (tag.HasSameClassAndValue(SnmpAsnTags.NoSuchObject))
            {
                reader.ReadNull(SnmpAsnTags.NoSuchObject);
                return new NoSuchObject();
            }

            if (tag.HasSameClassAndValue(SnmpAsnTags.NoSuchInstance))
            {
                reader.ReadNull(SnmpAsnTags.NoSuchInstance);
                return new NoSuchInstance();
            }

            if (tag.HasSameClassAndValue(SnmpAsnTags.EndOfMibView))
            {
                reader.ReadNull(SnmpAsnTags.EndOfMibView);
                return new EndOfMibView();
            }

            if (tag.HasSameClassAndValue(SnmpAsnTags.GetMsg))
            {
                return GetRequestPdu.ReadFrom(reader);
            }

            if (tag.HasSameClassAndValue(SnmpAsnTags.GetNextMsg))
            {
                return GetNextRequestPdu.ReadFrom(reader);
            }

            if (tag.HasSameClassAndValue(SnmpAsnTags.GetResponseMsg))
            {
                return ResponsePdu.ReadFrom(reader);
            }

            if (tag.HasSameClassAndValue(SnmpAsnTags.SetMsg))
            {
                return SetRequestPdu.ReadFrom(reader);
            }

            if (tag.HasSameClassAndValue(SnmpAsnTags.BulkMsg))
            {
                return GetBulkRequestPdu.ReadFrom(reader);
            }

            if (tag.HasSameClassAndValue(SnmpAsnTags.InformMsg))
            {
                return InformRequestPdu.ReadFrom(reader);
            }

            if (tag.HasSameClassAndValue(SnmpAsnTags.Trap2Msg))
            {
                return TrapV2Pdu.ReadFrom(reader);
            }

            if (tag.HasSameClassAndValue(SnmpAsnTags.ReportMsg))
            {
                return ReportPdu.ReadFrom(reader);
            }

            if (tag.HasSameClassAndValue(Asn1Tag.Sequence))
            {
                var sequenceKind = ClassifySequence(trimmedPayload);
                if (sequenceKind == SequenceKind.Scope)
                {
                    try
                    {
                        return Scope.ReadFrom(new AsnReader(trimmedPayload, AsnEncodingRules.BER));
                    }
                    catch
                    {
                        return new EncodedSequence(trimmedPayload);
                    }
                }

                if (sequenceKind == SequenceKind.VarBindList)
                {
                    try
                    {
                        return VarBindList.ReadFrom(new AsnReader(trimmedPayload, AsnEncodingRules.BER));
                    }
                    catch
                    {
                        return new EncodedSequence(trimmedPayload);
                    }
                }

                return new EncodedSequence(trimmedPayload);
            }
        }
        catch (Exception ex) when (ex is not SnmpException)
        {
            throw new SnmpException("data construction exception", ex);
        }

        throw new SnmpException("unsupported data type");
    }

    private sealed class EncodedSequence : IAsnSerializable
    {
        private readonly ReadOnlyMemory<byte> _encoded;

        public EncodedSequence(ReadOnlyMemory<byte> encoded)
        {
            _encoded = encoded;
        }

        public SnmpType TypeCode => SnmpType.Sequence;

        public void WriteTo(AsnWriter writer)
        {
            writer.WriteEncodedValue(_encoded.Span);
        }
    }

    private enum SequenceKind
    {
        Unknown = 0,
        Scope = 1,
        VarBindList = 2,
    }

    private static SequenceKind ClassifySequence(ReadOnlyMemory<byte> payload)
    {
        try
        {
            var reader = new AsnReader(payload, AsnEncodingRules.BER);
            var sequence = reader.ReadSequence();
            if (!sequence.HasData)
            {
                return SequenceKind.Unknown;
            }

            var firstTag = sequence.PeekTag();
            if (firstTag.HasSameClassAndValue(Asn1Tag.Sequence))
            {
                return SequenceKind.VarBindList;
            }

            if (!firstTag.HasSameClassAndValue(Asn1Tag.PrimitiveOctetString))
            {
                return SequenceKind.Unknown;
            }

            // Scope starts with contextEngineId + contextName (octet strings),
            // followed by a context-specific constructed PDU.
            sequence.ReadOctetString();
            if (!sequence.HasData || !sequence.PeekTag().HasSameClassAndValue(Asn1Tag.PrimitiveOctetString))
            {
                return SequenceKind.Unknown;
            }

            sequence.ReadOctetString();
            if (!sequence.HasData)
            {
                return SequenceKind.Unknown;
            }

            var pduTag = sequence.PeekTag();
            return pduTag.TagClass == TagClass.ContextSpecific && pduTag.IsConstructed
                ? SequenceKind.Scope
                : SequenceKind.Unknown;
        }
        catch
        {
            return SequenceKind.Unknown;
        }
    }

    private static ReadOnlyMemory<byte> TrimToSingleBerValue(ReadOnlyMemory<byte> payload)
    {
        var span = payload.Span;

        if (span.Length < 2)
        {
            throw new SnmpException("invalid BER data");
        }

        var offset = 1; // tag octet

        // High-tag-number form.
        if ((span[0] & 0x1F) == 0x1F)
        {
            while (true)
            {
                if (offset >= span.Length)
                {
                    throw new SnmpException("invalid BER data");
                }

                var octet = span[offset++];
                if ((octet & 0x80) == 0)
                {
                    break;
                }
            }
        }

        if (offset >= span.Length)
        {
            throw new SnmpException("invalid BER data");
        }

        var firstLengthOctet = span[offset++];
        long contentLength;
        if ((firstLengthOctet & 0x80) == 0)
        {
            contentLength = firstLengthOctet;
        }
        else
        {
            var lengthOctetCount = firstLengthOctet & 0x7F;
            if (lengthOctetCount == 0 || offset + lengthOctetCount > span.Length)
            {
                throw new SnmpException("invalid BER length");
            }

            contentLength = 0;
            for (var i = 0; i < lengthOctetCount; i++)
            {
                contentLength = (contentLength << 8) | span[offset++];
            }
        }

        var totalLength = offset + contentLength;
        if (totalLength <= 0 || totalLength > span.Length)
        {
            throw new SnmpException("invalid BER length");
        }

        if (totalLength == span.Length)
        {
            return payload;
        }

        if (totalLength > int.MaxValue)
        {
            throw new SnmpException("BER value too large");
        }

        return payload.Slice(0, (int)totalLength);
    }
}
