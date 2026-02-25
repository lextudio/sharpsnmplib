using DotNetSnmp.Asn1.Serialization;
using DotNetSnmp.Asn1.SyntaxObjects;
using System.Formats.Asn1;
using System.Text.RegularExpressions;

namespace DotNetSnmp.Utils
{
    public record AsnMember
    {
        private const string Reset = "\u001b[0m";
        private const string Gray = $"\u001b[38;2;65;65;65m";

        public Asn1Tag Tag { get; init; }

        public int HeaderSize { get; init; }

        public int ContentSize { get; init; }

        public int Offset { get; init; }

        public string TagLabel { get; set; } = string.Empty;

        public string ValueLabel { get; set; } = string.Empty;

        public string TagColor { get; set; } = string.Empty;

        public string ValueColor { get; set; } = string.Empty;

        public int IndentLevel { get; set; }

        public int BerHeaderLen { get; internal set; }

        public int ContentLen { get; internal set; }

        public int OffsetEnd => Offset + HeaderSize + ContentSize;

        public override string ToString()
        {
            var indent = IndentLevel > 0
                ? Enumerable.Repeat(" ", IndentLevel).Aggregate((a, b) => a + b)
                : string.Empty;

            var tag = TagColor + TagLabel + Reset;
            var value = ValueColor + ValueLabel + Reset;
            var size = Gray + $"({ContentLen} bytes)" + Reset;

            return $"{indent}{tag}: {size} {value}";
        }
    }

    public static class BERUtils
    {
        private const string BrightGreen = "\u001b[32;1m";
        private const string BrightBlue = "\u001b[34;1m";
        private const string BrightMagenta = "\u001b[35;1m";
        private const string Green = "\u001b[32m";
        private const string Yellow = "\u001b[33m";

        private static Regex _isHexString = new(@"\A\b[0-9a-fA-F]+\b\Z");

        public static IList<AsnMember> Dump(ReadOnlyMemory<byte> berEncodedBytes, bool colorize = true)
        {
            var result = new List<AsnMember>();

            var stack = new Stack<(AsnReader, int)>();

            stack.Push((new AsnReader(
                berEncodedBytes,
                AsnEncodingRules.BER), 0));

            var offset = 0;

            void AddMember(Asn1Tag tag, string tagName, object tagValue, int indent,
                (int HeaderLen, int ContentLen) len, bool opaqueOctetString = false)
            {
                if (colorize == false)
                {
                    Console.WriteLine($"    {indent}{tagName}: {tagValue}");
                }
                else
                {
                    var valColor = BrightGreen;
                    var _tagValue = tagValue?.ToString() ?? "NULL";

                    if (tag == Asn1Tag.Integer
                        || tag == AsnTypes.Counter32
                        || tag == AsnTypes.Counter64
                        || tag == AsnTypes.Unsigned32
                        || tag == AsnTypes.Unsigned64)
                    {
                        valColor = Yellow;
                    }
                    else if (tag == Asn1Tag.ConstructedOctetString
                        || tag == Asn1Tag.PrimitiveOctetString)
                    {
                        if (_isHexString.IsMatch(_tagValue))
                        {
                            valColor = Green;
                        }
                    }
                    else if (tag == Asn1Tag.ObjectIdentifier)
                    {
                        valColor = BrightMagenta;
                    }

                    result.Add(new()
                    {
                        ContentSize = len.ContentLen,
                        HeaderSize = len.HeaderLen,
                        Offset = offset,
                        Tag = tag,
                        TagLabel = tagName,
                        ValueLabel = _tagValue,
                        TagColor = BrightBlue,
                        ValueColor = valColor,
                        BerHeaderLen = len.HeaderLen,
                        ContentLen = len.ContentLen,
                        IndentLevel = indent
                    });

                    if (tag == Asn1Tag.Sequence
                        || ((tag == Asn1Tag.ConstructedOctetString || tag == Asn1Tag.PrimitiveOctetString)
                            && opaqueOctetString)
                        || tagName == "PDU")
                    {
                        offset += len.HeaderLen;
                    }
                    else
                    {
                        offset += len.ContentLen + len.HeaderLen;
                    }
                }
            }

            while (stack.Count > 0)
            {
                var (reader, indentLevel) = stack.Pop();

                while (reader.HasData)
                {
                    var tag = reader.PeekTag();
                    var ber = reader.PeekEncodedValue();
                    var val = reader.PeekContentBytes();
                    var hs = ber.Length - val.Length;
                    var size = (hs, val.Length);

                    var tagName = tag.ToString();
                    object tagValue = string.Empty;

                    if (tag == Asn1Tag.Sequence)
                    {
                        stack.Push((reader, indentLevel));
                        stack.Push((
                            reader.ReadSequence(),
                            indentLevel + 2));
                        AddMember(tag, "Sequence", tagValue, indentLevel, size);
                        break;
                    }
                    else if (tag == SnmpAsnTags.GetMsg
                        || tag == SnmpAsnTags.GetResponseMsg)
                    {
                        stack.Push((reader, indentLevel));
                        stack.Push((
                            reader.ReadSequence(tag),
                            indentLevel + 2));
                        AddMember(tag, "PDU", tagValue, indentLevel, size);
                        break;
                    }
                    else if (tag == Asn1Tag.Boolean)
                    {
                        tagValue = reader.ReadBoolean();
                    }
                    else if (tag == Asn1Tag.GeneralizedTime)
                    {
                        tagValue = reader.ReadGeneralizedTime();
                    }
                    else if (tag == Asn1Tag.Integer)
                    {
                        tagValue = reader.ReadInteger();
                    }
                    else if (tag == Asn1Tag.Null)
                    {
                        reader.ReadNull();
                        tagValue = "NULL";
                    }
                    else if (tag == Asn1Tag.ObjectIdentifier)
                    {
                        tagValue = reader.ReadObjectIdentifier();
                    }
                    else if (tag == Asn1Tag.ConstructedOctetString
                        || tag == Asn1Tag.PrimitiveOctetString)
                    {
                        try
                        {
                            var octetStringReader = new AsnReader(val, AsnEncodingRules.BER);
                            var maybeTag = octetStringReader.PeekTag();
                            if (maybeTag == Asn1Tag.Sequence)
                            {
                                var opaque = reader.ReadOctetString();
                                AddMember(tag, "OctetString", tagValue, indentLevel, size, true);
                                var nestedSeqReader = octetStringReader.ReadSequence();
                                stack.Push((reader, indentLevel));
                                stack.Push((nestedSeqReader, indentLevel + 4));
                                AddMember(Asn1Tag.Sequence, "Sequence", tagValue, indentLevel + 2, size);
                                break;
                            }
                        }
                        catch
                        {
                        }

                        tagValue = OctetString.ReadFrom(reader)
                            .ToString()
                            .Split(":")[1]
                            .Replace(" ", "");
                    }
                    else if (tag == Asn1Tag.SetOf)
                    {
                        reader = reader.ReadSetOf();
                    }
                    else if (tag == Asn1Tag.UtcTime)
                    {
                        tagValue = reader.ReadUtcTime();
                    }
                    else if (tag == AsnTypes.Counter32)
                    {
                        reader.TryReadUInt32(out var uint32, AsnTypes.Counter32);
                        tagValue = uint32;
                    }
                    else if (tag == AsnTypes.Gauge32)
                    {
                        reader.TryReadUInt32(out var uint32, AsnTypes.Gauge32);
                        tagValue = uint32;
                    }
                    else if (tag == AsnTypes.IpAddress)
                    {
                        reader.ReadOctetString(AsnTypes.IpAddress);
                    }
                    else if (tag == AsnTypes.TimeTicks)
                    {
                        reader.TryReadUInt32(out var uint32, AsnTypes.TimeTicks);
                        tagValue = uint32;
                    }
                    else if (tag == AsnTypes.Unsigned32)
                    {
                        reader.TryReadUInt32(out var uint32, AsnTypes.Unsigned32);
                        tagValue = uint32;
                    }
                    else if (tag == Asn1Tag.ObjectIdentifier)
                    {
                        tagValue = reader.ReadObjectIdentifier();
                    }
                    else if (tag == SnmpAsnTags.NoSuchObject)
                    {
                        reader.ReadNull();
                    }
                    else if (tag == SnmpAsnTags.NoSuchInstance)
                    {
                        reader.ReadNull();
                    }
                    else if (tag == SnmpAsnTags.EndOfMibView)
                    {
                        reader.ReadNull();
                    }
                    else if (tag == AsnTypes.Counter64)
                    {
                        var c64 = Counter64.ReadFrom(reader);
                        tagValue = c64.Value;
                    }
                    else
                    {
                        Console.WriteLine("???");
                    }

                    AddMember(tag, tagName, tagValue, indentLevel, size);
                }
            }
            return result;
        }
    }
}
